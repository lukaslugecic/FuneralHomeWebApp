using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class PaketOsiguranjaRepository : IPaketOsiguranjaRepository
{
    private readonly FuneralHomeContext _dbContext;

    public PaketOsiguranjaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(PaketOsiguranja model)
    {
        try
        {
            return _dbContext.PaketOsiguranja
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
            return _dbContext.PaketOsiguranja
                             .AsNoTracking()
                             .FirstOrDefault(v => v.IdPaketOsiguranja.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<PaketOsiguranja> Get(int id)
    {
        try
        {
            var paket = _dbContext.PaketOsiguranja
                                 .AsNoTracking()
                                 .FirstOrDefault(v => v.IdPaketOsiguranja.Equals(id))?
                                 .ToDomain();

            return paket is not null
            ? Results.OnSuccess(paket)
                : Results.OnFailure<PaketOsiguranja>($"No package with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<PaketOsiguranja>(e);
        }

    }

    public Result<IEnumerable<PaketOsiguranja>> GetAll()
    {
        try
        {
            var vrsta =
                _dbContext.PaketOsiguranja
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(vrsta);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<PaketOsiguranja>>(e);
        }
    }


    public Result Insert(PaketOsiguranja model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.PaketOsiguranja.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.PaketOsiguranja
                          .AsNoTracking()
                          .FirstOrDefault(cv => cv.IdPaketOsiguranja.Equals(id));
            if (model is not null)
            {
                _dbContext.PaketOsiguranja.Remove(model);

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

    public Result Update(PaketOsiguranja model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.PaketOsiguranja.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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