using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class UrnaRepository : IUrnaRepository
{
    private readonly FuneralHomeContext _dbContext;

    public UrnaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Urna model)
    {
        try
        {
            return _dbContext.Urna
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
            return _dbContext.Urna
                             .AsNoTracking()
                             .FirstOrDefault(u => u.Id.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<Urna> Get(int id)
    {
        try
        {
            var model = _dbContext.Urna
                                 .AsNoTracking()
                                 .FirstOrDefault(u => u.Id.Equals(id))?
                                 .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Urna>($"No urn with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<Urna>(e);
        }

    }

    public Result<IEnumerable<Urna>> GetAll()
    {
        try
        {
            var models =
                _dbContext.Urna
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Urna>>(e);
        }
    }


    public Result Insert(Urna model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Urna.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Urna
                          .AsNoTracking()
                          .FirstOrDefault(o => o.Id.Equals(id));
            if (model is not null)
            {
                _dbContext.Urna.Remove(model);

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

    public Result Update(Urna model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Urna.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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
}