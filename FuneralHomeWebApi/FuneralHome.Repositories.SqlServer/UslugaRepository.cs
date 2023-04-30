using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class UslugaRepository : IUslugaRepository
{
    private readonly FuneralHomeContext _dbContext;

    public UslugaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Usluga model)
    {
        try
        {
            return _dbContext.Usluga
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
            var model = _dbContext.Usluga
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdUsluga.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Usluga> Get(int id)
    {
        try
        {
            var model = _dbContext.Usluga
                          .Include(o => o.VrstaUsluge)
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdUsluga.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Usluga>($"No service with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Usluga>(e);
        }
    }

    public Result<Usluga> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.Usluga
                          .Include(o => o.VrstaUsluge)
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdUsluga.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Usluga>();
        }
        catch (Exception e)
        {
            return Results.OnException<Usluga>(e);
        }
    }

    public Result<IEnumerable<Usluga>> GetAll()
    {
        try
        {
            var models = _dbContext.Usluga
                           .Include(o => o.VrstaUsluge)
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Usluga>>(e);
        }
    }

    public Result<IEnumerable<Usluga>> GetAllByType(int id)
    {
        try
        {
            var models = _dbContext.Usluga
                           .Include(o => o.VrstaUsluge)
                           .Where(o=> o.VrstaUslugeId.Equals(id))
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Usluga>>(e);
        }
    }

    public Result<IEnumerable<Usluga>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.Usluga
                           .Include(o => o.VrstaUsluge)
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Usluga>>(e);
        }
    }

    public Result Insert(Usluga model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Usluga.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Usluga
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdUsluga.Equals(id));

            if (model is not null)
            {
                _dbContext.Usluga.Remove(model);

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

    public Result Update(Usluga model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Usluga.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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


    
    public Result UpdateAggregate(Usluga model)
    {
       return Results.OnFailure();
    }
    
}