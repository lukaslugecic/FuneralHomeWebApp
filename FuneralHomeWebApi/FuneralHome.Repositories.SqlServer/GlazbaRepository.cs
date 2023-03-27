using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class GlazbaRepository : IGlazbaRepository
{
    private readonly FuneralHomeContext _dbContext;

    public GlazbaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Glazba model)
    {
        try
        {
            return _dbContext.Glazba
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
            return _dbContext.Glazba
                             .AsNoTracking()
                             .FirstOrDefault(gl => gl.Id.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<Glazba> Get(int id)
    {
        try
        {
            var glazba = _dbContext.Glazba
                                 .AsNoTracking()
                                 .FirstOrDefault(gl => gl.Id.Equals(id))?
                                 .ToDomain();

            return glazba is not null
            ? Results.OnSuccess(glazba)
                : Results.OnFailure<Glazba>($"No music with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<Glazba>(e);
        }

    }

    public Result<IEnumerable<Glazba>> GetAll()
    {
        try
        {
            var glazba =
                _dbContext.Glazba
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(glazba);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Glazba>>(e);
        }
    }


    public Result Insert(Glazba model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Glazba.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Glazba
                          .AsNoTracking()
                          .FirstOrDefault(gl => gl.Id.Equals(id));
            if (model is not null)
            {
                _dbContext.Glazba.Remove(model);

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

    public Result Update(Glazba model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Glazba.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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