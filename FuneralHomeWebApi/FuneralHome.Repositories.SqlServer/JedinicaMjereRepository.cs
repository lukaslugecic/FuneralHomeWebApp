using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class JedinicaMjereRepository : IJedinicaMjereRepository
{
    private readonly FuneralHomeContext _dbContext;

    public JedinicaMjereRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(JedinicaMjere model)
    {
        try
        {
            return _dbContext.JedinicaMjere
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
            return _dbContext.JedinicaMjere
                             .AsNoTracking()
                             .FirstOrDefault(v => v.IdJednicaMjere.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<JedinicaMjere> Get(int id)
    {
        try
        {
            var jedinicaMjere = _dbContext.JedinicaMjere
                                 .AsNoTracking()
                                 .FirstOrDefault(v => v.IdJednicaMjere.Equals(id))?
                                 .ToDomain();

            return jedinicaMjere is not null
            ? Results.OnSuccess(jedinicaMjere)
                : Results.OnFailure<JedinicaMjere>($"No type with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<JedinicaMjere>(e);
        }

    }

    public Result<IEnumerable<JedinicaMjere>> GetAll()
    {
        try
        {
            var jedinicaMjere =
                _dbContext.JedinicaMjere
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(jedinicaMjere);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<JedinicaMjere>>(e);
        }
    }



    public Result Insert(JedinicaMjere model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.JedinicaMjere.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.JedinicaMjere
                          .AsNoTracking()
                          .FirstOrDefault(cv => cv.IdJednicaMjere.Equals(id));
            if (model is not null)
            {
                _dbContext.JedinicaMjere.Remove(model);

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

    public Result Update(JedinicaMjere model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.JedinicaMjere.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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