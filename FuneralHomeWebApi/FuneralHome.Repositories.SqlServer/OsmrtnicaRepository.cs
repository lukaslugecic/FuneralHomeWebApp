using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class OsmrtnicaRepository : IOsmrtnicaRepository
{
    private readonly FuneralHomeContext _dbContext;

    public OsmrtnicaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Osmrtnica model)
    {
        try
        {
            return _dbContext.Osmrtnica
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
            return _dbContext.Osmrtnica
                             .AsNoTracking()
                             .FirstOrDefault(v => v.IdOsmrtnica.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<Osmrtnica> Get(int id)
    {
        try
        {
            var osmrtnica = _dbContext.Osmrtnica
                             .AsNoTracking()
                            .FirstOrDefault(v => v.IdOsmrtnica.Equals(id))?
                            .ToDomain();

            return osmrtnica is not null
            ? Results.OnSuccess(osmrtnica)
                : Results.OnFailure<Osmrtnica>($"No obituary with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<Osmrtnica>(e);
        }

    }

    public Result<IEnumerable<Osmrtnica>> GetAll()
    {
        try
        {
            var osmrtnica =
                _dbContext.Osmrtnica
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(osmrtnica);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Osmrtnica>>(e);
        }
    }


    public Result Insert(Osmrtnica model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Osmrtnica.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Osmrtnica
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOsmrtnica.Equals(id));
            if (model is not null)
            {
                _dbContext.Osmrtnica.Remove(model);

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

    public Result Update(Osmrtnica model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Osmrtnica.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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