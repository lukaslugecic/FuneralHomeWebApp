using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class LijesRepository : ILijesRepository
{
    private readonly FuneralHomeContext _dbContext;

    public LijesRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Lijes model)
    {
        try
        {
            return _dbContext.Lijes
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
            return _dbContext.Lijes
                             .AsNoTracking()
                             .FirstOrDefault(l => l.Id.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<Lijes> Get(int id)
    {
        try
        {
            var lijesovi = _dbContext.Lijes
                                 .AsNoTracking()
                                 .FirstOrDefault(l => l.Id.Equals(id))?
                                 .ToDomain();

            return lijesovi is not null
            ? Results.OnSuccess(lijesovi)
                : Results.OnFailure<Lijes>($"No coffins with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<Lijes>(e);
        }

    }

    public Result<IEnumerable<Lijes>> GetAll()
    {
        try
        {
            var lijesovi =
                _dbContext.Lijes
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(lijesovi);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Lijes>>(e);
        }
    }


    public Result Insert(Lijes model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Lijes.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Lijes
                          .AsNoTracking()
                          .FirstOrDefault(l => l.Id.Equals(id));
            if (model is not null)
            {
                _dbContext.Lijes.Remove(model);

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

    public Result Update(Lijes model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Lijes.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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