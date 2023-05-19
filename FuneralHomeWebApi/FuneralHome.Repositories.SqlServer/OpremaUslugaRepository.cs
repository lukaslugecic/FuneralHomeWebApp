using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class OpremaUslugaRepository : IOpremaUslugaRepository
{
    private readonly FuneralHomeContext _dbContext;

    public OpremaUslugaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(OpremaUsluga model)
    {
        try
        {
            return _dbContext.OpremaUsluga
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
            var model = _dbContext.OpremaUsluga
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOpremaUsluga.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<OpremaUsluga> Get(int id)
    {
        try
        {
            var model = _dbContext.OpremaUsluga
                          .Include(o => o.VrstaOpremeUsluge)
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOpremaUsluga.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<OpremaUsluga>($"No equipment or service with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<OpremaUsluga>(e);
        }
    }

    public Result<OpremaUsluga> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.OpremaUsluga
                          .Include(o => o.VrstaOpremeUsluge)
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOpremaUsluga.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<OpremaUsluga>();
        }
        catch (Exception e)
        {
            return Results.OnException<OpremaUsluga>(e);
        }
    }

    public Result<IEnumerable<OpremaUsluga>> GetAll()
    {
        try
        {
            var models = _dbContext.OpremaUsluga
                           .Include(o => o.VrstaOpremeUsluge)
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<OpremaUsluga>>(e);
        }
    }


    public Result<IEnumerable<OpremaUsluga>> GetAllByType(int id)
    {
        try
        {
            var models = _dbContext.OpremaUsluga
                           .Include(o => o.VrstaOpremeUsluge)
                           .Where(o => o.VrstaOpremeUslugeId.Equals(id))
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<OpremaUsluga>>(e);
        }
    }

    public Result<IEnumerable<OpremaUsluga>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.OpremaUsluga
                           .Include(o => o.VrstaOpremeUsluge)
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<OpremaUsluga>>(e);
        }
    }

    public Result Insert(OpremaUsluga model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.OpremaUsluga.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.OpremaUsluga
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOpremaUsluga.Equals(id));

            if (model is not null)
            {
                _dbContext.OpremaUsluga.Remove(model);

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

    public Result Update(OpremaUsluga model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.OpremaUsluga.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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

    public Result IncreaseZaliha(OpremaUsluga model, int kolicina)
    {
        try
        {
            var dbModel = model.ToDbModel();
            dbModel.Zaliha += kolicina;
            // detach
            if (_dbContext.OpremaUsluga.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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

    public Result DecreaseZaliha(OpremaUsluga model, int kolicina)
    {
        try
        {
            var dbModel = model.ToDbModel();
            dbModel.Zaliha -= kolicina;
            if (dbModel.Zaliha < 0)
            {
                return Results.OnFailure();
            }
            // detach
            if (_dbContext.OpremaUsluga.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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


    public Result UpdateAggregate(OpremaUsluga model)
    {
        return Results.OnFailure();
    }
    
}