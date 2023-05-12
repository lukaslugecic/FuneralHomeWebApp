using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class SmrtniSlucajRepository : ISmrtniSlucajRepository
{
    private readonly FuneralHomeContext _dbContext;

    public SmrtniSlucajRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(SmrtniSlucaj model)
    {
        try
        {
            return _dbContext.SmrtniSlucaj
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
            var model = _dbContext.SmrtniSlucaj
                          .AsNoTracking()
                          .FirstOrDefault(ss => ss.IdSmrtniSlucaj.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<SmrtniSlucaj> Get(int id)
    {
        try
        {
            var model = _dbContext.SmrtniSlucaj
                          .AsNoTracking()
                          .FirstOrDefault(ss => ss.IdSmrtniSlucaj.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<SmrtniSlucaj>($"No death case with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<SmrtniSlucaj>(e);
        }
    }

    public Result<SmrtniSlucaj> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.SmrtniSlucaj
                          .Include(ss => ss.Korisnik)
                          .Include(ss => ss.Osmrtnica)
                          .AsNoTracking()
                          .FirstOrDefault(ss => ss.IdSmrtniSlucaj.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<SmrtniSlucaj>();
        }
        catch (Exception e)
        {
            return Results.OnException<SmrtniSlucaj>(e);
        }
    }

    public Result<IEnumerable<SmrtniSlucaj>> GetAll()
    {
        try
        {
            var models = _dbContext.SmrtniSlucaj
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<SmrtniSlucaj>>(e);
        }
    }

    
    public Result<IEnumerable<SmrtniSlucaj>> GetAllWithoutFuneral()
    {
        try
        {
            // ako u pogrebima nema pogreba sa smrtnim slucajem, onda vrati smrtne slucajeve
            var models = _dbContext.SmrtniSlucaj
                           .AsNoTracking()
                           .Where(ss => !_dbContext.Pogreb
                                   .AsNoTracking()
                                   .Any(p => p.SmrtniSlucaj.IdSmrtniSlucaj.Equals(ss.IdSmrtniSlucaj)))
                           .Select(Mapping.ToDomain);
                            
            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<SmrtniSlucaj>>(e);
        }
    }

    public Result<IEnumerable<SmrtniSlucaj>> GetAllWithoutFuneralByKorisnikId(int id)
    {
        try
        {
            // ako u pogrebima nema pogreba sa smrtnim slucajem, onda vrati smrtne slucajeve
            var models = _dbContext.SmrtniSlucaj
                           .AsNoTracking()
                           .Where(ss => !_dbContext.Pogreb
                                   .AsNoTracking()
                                   .Any(p => p.SmrtniSlucaj.IdSmrtniSlucaj.Equals(ss.IdSmrtniSlucaj)))
                            .Where(ss => ss.Korisnik.IdKorisnik.Equals(id))
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<SmrtniSlucaj>>(e);
        }
    }

    public Result<IEnumerable<SmrtniSlucaj>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.SmrtniSlucaj
                            .Include(ss => ss.Korisnik)
                            .Include(ss => ss.Osmrtnica)
                            .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<SmrtniSlucaj>>(e);
        }
    }

    public Result Insert(SmrtniSlucaj model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.SmrtniSlucaj.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.SmrtniSlucaj
                          .AsNoTracking()
                          .FirstOrDefault(ss => ss.IdSmrtniSlucaj.Equals(id));

            if (model is not null)
            {
                _dbContext.SmrtniSlucaj.Remove(model);

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

    public Result Update(SmrtniSlucaj model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.SmrtniSlucaj.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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


    
    public Result UpdateAggregate(SmrtniSlucaj model)
    {
        return Results.OnFailure();
    }
    
}