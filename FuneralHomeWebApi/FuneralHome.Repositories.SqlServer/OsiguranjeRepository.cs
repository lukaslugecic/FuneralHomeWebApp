using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class OsiguranjeRepository : IOsiguranjeRepository
{
    private readonly FuneralHomeContext _dbContext;

    public OsiguranjeRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Osiguranje model)
    {
        try
        {
            return _dbContext.Osiguranje
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
            return _dbContext.Osiguranje
                             .AsNoTracking()
                             .FirstOrDefault(o => o.IdOsiguranje.Equals(id)) != null;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public Result<Osiguranje> Get(int id)
    {
        try
        {
            var model = _dbContext.Osiguranje
                                 .Include(o => o.Korisnik)
                                 .AsNoTracking()
                                 .FirstOrDefault(o => o.IdOsiguranje.Equals(id))?
                                 .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Osiguranje>($"No insurances with such id {id}");
        }
        catch (Exception e)
        {
            return Results.OnException<Osiguranje>(e);
        }

    }

    public Result<IEnumerable<Osiguranje>> GetByKorisnikId(int id)
    {
        try
        {
            var models = _dbContext.Osiguranje
                                 .Include(o => o.Korisnik)
                                 .AsNoTracking()
                                 .Where(o => o.KorisnikId.Equals(id))
                                 .Select(Mapping.ToDomain);
            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Osiguranje>>(e);
        }
    }   

    public Result<IEnumerable<Osiguranje>> GetAll()
    {
        try
        {
            var models =
                _dbContext.Osiguranje
                          .Include(o => o.Korisnik)
                          .AsNoTracking()
                          .Select(Mapping.ToDomain);
            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Osiguranje>>(e);
        }
    }


    public Result Insert(Osiguranje model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Osiguranje.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Osiguranje
                          .AsNoTracking()
                          .FirstOrDefault(o => o.IdOsiguranje.Equals(id));
            if (model is not null)
            {
                _dbContext.Osiguranje.Remove(model);

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

    public Result Update(Osiguranje model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Osiguranje.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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