using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class KorisnikRepository : IKorisnikRepository
{
    private readonly FuneralHomeContext _dbContext;

    public KorisnikRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Korisnik model)
    {
        try
        {
            return _dbContext.Korisnik
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
            var model = _dbContext.Korisnik
                          .AsNoTracking()
                          .FirstOrDefault(k => k.Id.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool Exists(string mail)
    {
        try
        {
            var model = _dbContext.Korisnik
                          .AsNoTracking()
                          .FirstOrDefault(k => k.Mail.Equals(mail));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Korisnik> Get(int id)
    {
        try
        {
            var model = _dbContext.Korisnik
                          .AsNoTracking()
                          .FirstOrDefault(k => k.Id.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Korisnik>($"No user with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Korisnik>(e);
        }
    }

    public Result<Korisnik> GetByMail(string mail)
    {
        try
        {
            var model = _dbContext.Korisnik
                          .AsNoTracking()
                          .FirstOrDefault(k => k.Mail.Equals(mail))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Korisnik>($"No user with mail {mail} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Korisnik>(e);
        }
    }

    public Result<Korisnik> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.Korisnik
                          .Include(k => k.Osiguranje)
                          .Include(k => k.SmrtniSlucaj)
                          .AsNoTracking()
                          .FirstOrDefault(k => k.Id.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Korisnik>();
        }
        catch (Exception e)
        {
            return Results.OnException<Korisnik>(e);
        }
    }

    public Result<IEnumerable<Korisnik>> GetAll()
    {
        try
        {
            var models = _dbContext.Korisnik
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Korisnik>>(e);
        }
    }

    public Result<IEnumerable<Korisnik>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.Korisnik
                           .Include(K => K.Osiguranje)
                           .Include(K => K.SmrtniSlucaj)
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Korisnik>>(e);
        }
    }

    public Result Insert(Korisnik model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Korisnik.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Korisnik
                          .AsNoTracking()
                          .FirstOrDefault(k => k.Id.Equals(id));

            if (model is not null)
            {
                _dbContext.Korisnik.Remove(model);

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

    public Result Update(Korisnik model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Korisnik.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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


    /*
    public Result UpdateAggregate(Korisnik model)
    {}
    */
}