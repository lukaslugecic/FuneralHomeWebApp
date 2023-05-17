using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class VrstaOpremeRepository : IVrstaOpremeRepository
{
    private readonly FuneralHomeContext _dbContext;

    public VrstaOpremeRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(VrstaOpremeUsluge model)
    {
        try
        {
            return _dbContext.VrstaOpreme
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
            return _dbContext.VrstaOpreme
                             .AsNoTracking()
                             .FirstOrDefault(v => v.IdVrstaOpreme.Equals(id)) != null;
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
            var vrsta = _dbContext.VrstaOpreme
                                 .AsNoTracking()
                                 .FirstOrDefault(v => v.IdVrstaOpreme.Equals(id))?
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
                _dbContext.VrstaOpreme
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
            if (_dbContext.VrstaOpreme.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.VrstaOpreme
                          .AsNoTracking()
                          .FirstOrDefault(cv => cv.IdVrstaOpreme.Equals(id));
            if (model is not null)
            {
                _dbContext.VrstaOpreme.Remove(model);

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
            if (_dbContext.VrstaOpreme.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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