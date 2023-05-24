using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;
using Microsoft.IdentityModel.Tokens;

namespace FuneralHome.Repositories.SqlServer;
public class KupnjaRepository : IKupnjaRepository
{
    private readonly FuneralHomeContext _dbContext;

    public KupnjaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Kupnja model)
    {
        try
        {
            return _dbContext.Kupnja
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
            var model = _dbContext.Kupnja
                          .AsNoTracking()
                          .FirstOrDefault(k => k.IdKupnja.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Kupnja> Get(int id)
    {
        try
        {
            var model = _dbContext.Kupnja
                          .Include(k => k.Korisnik)
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdKupnja.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Kupnja>($"No purchase with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Kupnja>(e);
        }
    }

    public Result<Kupnja> GetLatestByKorisnikId(int korisnikId)
    {
        try
        {
            var model = _dbContext.Kupnja
                          .Include(k => k.Korisnik)
                          .AsNoTracking()
                          .Where(k => k.KorisnikId.Equals(korisnikId))
                          .OrderByDescending(k => k.IdKupnja)
                          .FirstOrDefault()
                          ?.ToDomain();
            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Kupnja>($"No purchase with id {korisnikId} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Kupnja>(e);
        }
    }


    public Result<Kupnja> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.Kupnja
                          .Include(k => k.Korisnik)
                          .Include(k => k.KupnjaOpremaUsluge)
                          .ThenInclude(kou => kou.OpremaUsluga)
                          .ThenInclude(o => o.VrstaOpremeUsluge)
                          .ThenInclude(v => v.JedinicaMjere)
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdKupnja.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Kupnja>();
        }
        catch (Exception e)
        {
            return Results.OnException<Kupnja>(e);
        }
    }

    public Result<IEnumerable<Kupnja>> GetAll()
    {
        try
        {
            var models = _dbContext.Kupnja
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Kupnja>>(e);
        }
    }

    public Result<IEnumerable<Kupnja>> GetAllByKorisnikId(int id)
    {
        try
        {
            var models = _dbContext.Kupnja
                           .AsNoTracking()
                           .Where(k => k.KorisnikId.Equals(id))
                           .Select(Mapping.ToDomain);
            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Kupnja>>(e);
        }
    }

    public Result<IEnumerable<Kupnja>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.Kupnja
                            .Include(k => k.Korisnik)
                            .Include(k => k.KupnjaOpremaUsluge)
                            .ThenInclude(kou => kou.OpremaUsluga)
                            .ThenInclude(o => o.VrstaOpremeUsluge)
                            .ThenInclude(v => v.JedinicaMjere)
                            .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Kupnja>>(e);
        }
    }

    public Result<IEnumerable<Kupnja>> GetAllAggregatesByKorisnikId(int korisnikId)
    {
        try
        {
            var models = _dbContext.Kupnja
                            .Include(k => k.Korisnik)
                            .Include(k => k.KupnjaOpremaUsluge)
                            .ThenInclude(kou => kou.OpremaUsluga)
                            .ThenInclude(o => o.VrstaOpremeUsluge)
                            .ThenInclude(v => v.JedinicaMjere)
                            .Where(k => k.KorisnikId.Equals(korisnikId))
                            .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Kupnja>>(e);
        }
    }

    public Result Insert(Kupnja model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Kupnja.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Kupnja
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdKupnja.Equals(id));

            if (model is not null)
            {
                _dbContext.Kupnja.Remove(model);

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

    public Result Update(Kupnja model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Kupnja.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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

   
    public Result UpdateAggregate(Kupnja model)
    {
        try
        {
            _dbContext.ChangeTracker.Clear();

            var dbModel = _dbContext.Kupnja
                              .Include(_ => _.KupnjaOpremaUsluge)
                              .ThenInclude(_ => _.OpremaUsluga)
                              .ThenInclude(o => o.VrstaOpremeUsluge)
                              //.AsNoTracking()
                              .FirstOrDefault(_ => _.IdKupnja == model.Id);
            if (dbModel == null)
                return Results.OnFailure($"Purchase with id {model.Id} not found.");



            dbModel.KorisnikId = model.KorisnikId;
            dbModel.DatumKupovine = model.DatumKupovine;
            dbModel.UkupnaCijena = model.UkupnaCijena;

            foreach (var kupnjaOpremaUsluga in model.KupnjaOpremaUsluge)
            {
                // it exists in the DB, so just update it
                var kupnjaOpremaToUpdate =
                    dbModel.KupnjaOpremaUsluge
                           .FirstOrDefault(po => po.KupnjaId.Equals(model.Id) && po.OpremaUslugaId.Equals(kupnjaOpremaUsluga.OpremaUsluga.Id));
                if (kupnjaOpremaToUpdate != null)
                {
                    kupnjaOpremaToUpdate.Kolicina = kupnjaOpremaUsluga.Kolicina;
                }
                else // it does not exist in the DB, so add it
                {
                    dbModel.KupnjaOpremaUsluge.Add(kupnjaOpremaUsluga.ToDbModel(model.Id));
                }
            }

            dbModel.KupnjaOpremaUsluge
                  .Where(po => !model.KupnjaOpremaUsluge.Any(_ => _.OpremaUsluga.Id == po.OpremaUslugaId))
                  .ToList()
                  .ForEach(pogrebOprema =>
                  {
                      dbModel.KupnjaOpremaUsluge.Remove(pogrebOprema);
                  });


            _dbContext.Kupnja
                      .Update(dbModel);

            var isSuccess = _dbContext.SaveChanges() > 0;
            _dbContext.ChangeTracker.Clear();
            return isSuccess
                ? Results.OnSuccess()
                : Results.OnFailure();
        }
        catch (Exception e)
        {
            return Results.OnException(e);
        }
    }
}