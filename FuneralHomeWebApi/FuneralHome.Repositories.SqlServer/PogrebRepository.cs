using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class PogrebRepository : IPogrebRepository
{
    private readonly FuneralHomeContext _dbContext;

    public PogrebRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Pogreb model)
    {
        try
        {
            return _dbContext.Pogreb
                     .AsNoTracking()
                     .Contains(model.ToDbModel());
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Exists(int id)
    {
        try
        {
            var model = _dbContext.Pogreb
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdPogreb.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Pogreb> Get(int id)
    {
        try
        {
            var model = _dbContext.Pogreb
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdPogreb.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Pogreb>($"No funeral with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Pogreb>(e);
        }
    }

    public Result<Pogreb> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.Pogreb
                          .Include(p => p.SmrtniSlucaj)
                          .Include(p => p.PogrebOprema)
                          .Include(p => p.Usluga) // ????
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdPogreb.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Pogreb>();
        }
        catch (Exception e)
        {
            return Results.OnException<Pogreb>(e);
        }
    }

    public Result<IEnumerable<Pogreb>> GetAll()
    {
        try
        {
            var models = _dbContext.Pogreb
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Pogreb>>(e);
        }
    }

    public Result<IEnumerable<Pogreb>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.Pogreb
                            .Include(p => p.SmrtniSlucaj)
                            .Include(p => p.PogrebOprema)
                            .Include(p => p.Usluga) // ????
                            .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Pogreb>>(e);
        }
    }

    public Result Insert(Pogreb model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Pogreb.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                var isSuccess = _dbContext.SaveChanges() > 0;

                // every Add attaches the entity object and EF begins tracking
                // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
                _dbContext.Entry(dbModel).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                return isSuccess
                    ? Results.OnSuccess()
                    : Results.OnFailure();
            }

            return Results.OnFailure();
        }
        catch (Exception e)
        {
            return Results.OnException(e);
        }
    }

    public Result Remove(int id)
    {
        try
        {
            var model = _dbContext.Pogreb
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdPogreb.Equals(id));

            if (model is not null)
            {
                _dbContext.Pogreb.Remove(model);

                return _dbContext.SaveChanges() > 0
                    ? Results.OnSuccess()
                    : Results.OnFailure();
            }
            return Results.OnFailure();
        }
        catch (Exception e)
        {
            return Results.OnException(e);
        }
    }

    public Result Update(Pogreb model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Pogreb.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                var isSuccess = _dbContext.SaveChanges() > 0;

                // every Update attaches the entity object and EF begins tracking
                // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
                _dbContext.Entry(dbModel).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

                return isSuccess
                    ? Results.OnSuccess()
                    : Results.OnFailure();
            }

            return Results.OnFailure();
        }
        catch (Exception e)
        {
            return Results.OnException(e);
        }
    }


    /*
    public Result UpdateAggregate(Korisnik model)
    {}
    */
}