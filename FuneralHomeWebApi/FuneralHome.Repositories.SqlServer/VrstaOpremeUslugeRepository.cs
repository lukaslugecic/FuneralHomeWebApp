using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class VrstaOpremeUslugeRepository : IVrstaOpremeUslugeRepository
{
    private readonly FuneralHomeContext _dbContext;

    public VrstaOpremeUslugeRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(VrstaOpremeUsluge model)
    {
        try
        {
            return _dbContext.VrstaOpremeUsluge
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
            return _dbContext.VrstaOpremeUsluge
                             .AsNoTracking()
                             .FirstOrDefault(v => v.IdVrstaOpremeUsluge.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<VrstaOpremeUsluge> Get(int id)
    {
        try
        {
            var vrsta = _dbContext.VrstaOpremeUsluge
                                  .Include(v => v.JedinicaMjere)
                                  .AsNoTracking()
                                  .FirstOrDefault(v => v.IdVrstaOpremeUsluge.Equals(id))?
                                  .ToDomain();

            return vrsta is not null
            ? Results.OnSuccess(vrsta)
                : Results.OnFailure<VrstaOpremeUsluge>($"No type with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<VrstaOpremeUsluge>(e);
        }

    }

    public Result<IEnumerable<VrstaOpremeUsluge>> GetAll()
    {
        try
        {
            var vrsta =
                _dbContext.VrstaOpremeUsluge
                          .Include(v => v.JedinicaMjere)
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(vrsta);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<VrstaOpremeUsluge>>(e);
        }
    }

    public Result<IEnumerable<VrstaOpremeUsluge>> GetAllOprema()
    {
        try
        {
            var vrsta =
                _dbContext.VrstaOpremeUsluge
                          .Include(v => v.JedinicaMjere)
                          .Where(v => v.JeOprema == true)
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(vrsta);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<VrstaOpremeUsluge>>(e);
        }
    }

    public Result<IEnumerable<VrstaOpremeUsluge>> GetAllUsluge()
    {
        try
        {
            var vrsta =
                _dbContext.VrstaOpremeUsluge
                          .Include(v => v.JedinicaMjere)
                          .Where(v => v.JeOprema == false)
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(vrsta);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<VrstaOpremeUsluge>>(e);
        }
    }


    public Result Insert(VrstaOpremeUsluge model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.VrstaOpremeUsluge.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.VrstaOpremeUsluge
                          .AsNoTracking()
                          .FirstOrDefault(cv => cv.IdVrstaOpremeUsluge.Equals(id));
            if (model is not null)
            {
                _dbContext.VrstaOpremeUsluge.Remove(model);

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

    public Result Update(VrstaOpremeUsluge model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.VrstaOpremeUsluge.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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