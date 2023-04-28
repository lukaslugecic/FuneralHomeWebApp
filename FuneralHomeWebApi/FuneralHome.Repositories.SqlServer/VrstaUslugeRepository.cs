using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class VrstaUslugeRepository : IVrstaUslugeRepository
{
    private readonly FuneralHomeContext _dbContext;

    public VrstaUslugeRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(VrstaUsluge model)
    {
        try
        {
            return _dbContext.VrstaUsluge
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
            return _dbContext.VrstaUsluge
                             .AsNoTracking()
                             .FirstOrDefault(v => v.IdVrstaUsluge.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<VrstaUsluge> Get(int id)
    {
        try
        {
            var vrsta = _dbContext.VrstaUsluge
                                 .AsNoTracking()
                                 .FirstOrDefault(v => v.IdVrstaUsluge.Equals(id))?
                                 .ToDomain();

            return vrsta is not null
            ? Results.OnSuccess(vrsta)
                : Results.OnFailure<VrstaUsluge>($"No type with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<VrstaUsluge>(e);
        }

    }

    public Result<IEnumerable<VrstaUsluge>> GetAll()
    {
        try
        {
            var vrsta =
                _dbContext.VrstaUsluge
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(vrsta);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<VrstaUsluge>>(e);
        }
    }


    public Result Insert(VrstaUsluge model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.VrstaUsluge.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.VrstaUsluge
                          .AsNoTracking()
                          .FirstOrDefault(cv => cv.IdVrstaUsluge.Equals(id));
            if (model is not null)
            {
                _dbContext.VrstaUsluge.Remove(model);

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

    public Result Update(VrstaUsluge model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.VrstaUsluge.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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