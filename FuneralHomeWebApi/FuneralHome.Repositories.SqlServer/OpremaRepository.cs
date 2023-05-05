using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class OpremaRepository : IOpremaRepository
{
    private readonly FuneralHomeContext _dbContext;

    public OpremaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Oprema model)
    {
        try
        {
            return _dbContext.Oprema
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
            var model = _dbContext.Oprema
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOprema.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Oprema> Get(int id)
    {
        try
        {
            var model = _dbContext.Oprema
                          .Include(o => o.VrstaOpreme)
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOprema.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Oprema>($"No equipment with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Oprema>(e);
        }
    }

    public Result<Oprema> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.Oprema
                          .Include(o => o.VrstaOpreme)
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOprema.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Oprema>();
        }
        catch (Exception e)
        {
            return Results.OnException<Oprema>(e);
        }
    }

    public Result<IEnumerable<Oprema>> GetAll()
    {
        try
        {
            var models = _dbContext.Oprema
                           .Include(o => o.VrstaOpreme)
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Oprema>>(e);
        }
    }


    public Result<IEnumerable<Oprema>> GetAllByType(int id)
    {
        try
        {
            var models = _dbContext.Oprema
                           .Include(o => o.VrstaOpreme)
                           .Where(o => o.VrstaOpremeId.Equals(id))
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Oprema>>(e);
        }
    }

    public Result<IEnumerable<Oprema>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.Oprema
                           .Include(o => o.VrstaOpreme)
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Oprema>>(e);
        }
    }

    public Result Insert(Oprema model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Oprema.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Oprema
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOprema.Equals(id));

            if (model is not null)
            {
                _dbContext.Oprema.Remove(model);

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

    public Result Update(Oprema model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Oprema.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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

    public Result IncreaseZaliha(Oprema model, int kolicina)
    {
        try
        {
            var dbModel = model.ToDbModel();
            dbModel.ZalihaOpreme += kolicina;
            // detach
            if (_dbContext.Oprema.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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

    public Result DecreaseZaliha(Oprema model, int kolicina)
    {
        try
        {
            var dbModel = model.ToDbModel();
            dbModel.ZalihaOpreme -= kolicina;
            if (dbModel.ZalihaOpreme < 0)
            {
                return Results.OnFailure();
            }
            // detach
            if (_dbContext.Oprema.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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


    public Result UpdateAggregate(Oprema model)
    {
        return Results.OnFailure();
    }
    
}