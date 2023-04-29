using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class OglasRepository : IOglasRepository
{
    private readonly FuneralHomeContext _dbContext;

    public OglasRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Oglas model)
    {
        try
        {
            return _dbContext.Oglas
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
            var model = _dbContext.Oglas
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOglas.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Oglas> Get(int id)
    {
        try
        {
            var model = _dbContext.Oglas
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOglas.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Oglas>($"No announcement with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Oglas>(e);
        }
    }

    public Result<Oglas> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.Oglas
                          .Include(o => o.Osmrtnica)
                          .Include(o => o.SmrtniSlucaj)
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOglas.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Oglas>();
        }
        catch (Exception e)
        {
            return Results.OnException<Oglas>(e);
        }
    }

    public Result<IEnumerable<Oglas>> GetAll()
    {
        try
        {
            var models = _dbContext.Oglas
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Oglas>>(e);
        }
    }

    public Result<IEnumerable<Oglas>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.Oglas
                           .Include(o => o.Osmrtnica)
                           .Include(o => o.SmrtniSlucaj)
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Oglas>>(e);
        }
    }

    public Result Insert(Oglas model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Oglas.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Oglas
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOglas.Equals(id));

            if (model is not null)
            {
                _dbContext.Oglas.Remove(model);

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

    public Result Update(Oglas model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Oglas.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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


    
    public Result UpdateAggregate(Oglas model)
    {
        return Results.OnFailure();
    }
    
}