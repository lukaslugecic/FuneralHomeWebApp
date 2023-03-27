using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class CvijeceRepository : ICvijeceRepository
{
    private readonly FuneralHomeContext _dbContext;

    public CvijeceRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Cvijece model)
    {
        try
        {
            return _dbContext.Cvijece
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
            return _dbContext.Cvijece
                             .AsNoTracking()
                             .FirstOrDefault(cv => cv.Id.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<Cvijece> Get(int id)
    {
        try
        {
            var cvijece = _dbContext.Cvijece
                                 .AsNoTracking()
                                 .FirstOrDefault(cv => cv.Id.Equals(id))?
                                 .ToDomain();

            return cvijece is not null
            ? Results.OnSuccess(cvijece)
                : Results.OnFailure<Cvijece>($"No flowers with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<Cvijece>(e);
        }

    }

    public Result<IEnumerable<Cvijece>> GetAll()
    {
        try
        {
            var cvijece =
                _dbContext.Cvijece
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(cvijece);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Cvijece>>(e);
        }
    }


    public Result Insert(Cvijece model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Cvijece.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Cvijece
                          .AsNoTracking()
                          .FirstOrDefault(cv => cv.Id.Equals(id));
            if (model is not null)
            {
                _dbContext.Cvijece.Remove(model);

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

    public Result Update(Cvijece model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Cvijece.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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