using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class NadgrobniZnakRepository : INadgrobniZnakRepository
{
    private readonly FuneralHomeContext _dbContext;

    public NadgrobniZnakRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(NadgrobniZnak model)
    {
        try
        {
            return _dbContext.NadgrobniZnak
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
            return _dbContext.NadgrobniZnak
                             .AsNoTracking()
                             .FirstOrDefault(nz => nz.Id.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<NadgrobniZnak> Get(int id)
    {
        try
        {
            var znakovi = _dbContext.NadgrobniZnak
                                 .AsNoTracking()
                                 .FirstOrDefault(cv => cv.Id.Equals(id))?
                                 .ToDomain();

            return znakovi is not null
            ? Results.OnSuccess(znakovi)
                : Results.OnFailure<NadgrobniZnak>($"No signs with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<NadgrobniZnak>(e);
        }

    }

    public Result<IEnumerable<NadgrobniZnak>> GetAll()
    {
        try
        {
            var znakovi =
                _dbContext.NadgrobniZnak
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(znakovi);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<NadgrobniZnak>>(e);
        }
    }


    public Result Insert(NadgrobniZnak model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.NadgrobniZnak.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.NadgrobniZnak
                          .AsNoTracking()
                          .FirstOrDefault(nz => nz.Id.Equals(id));
            if (model is not null)
            {
                _dbContext.NadgrobniZnak.Remove(model);

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

    public Result Update(NadgrobniZnak model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.NadgrobniZnak.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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