using FuneralHome.DataAccess.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using FuneralHome.Domain.Models;
using BaseLibrary;

namespace FuneralHome.Repositories.SqlServer;
public class PogrebRepository : IPogrebRepository
{
    private readonly FuneralHomeContext _dbContext;

    public PogrebRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Pogreb model)
    {
        try
        {
            return _dbContext.Pogreb
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
            var model = _dbContext.Pogreb
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdPogreb.Equals(id));
            return model is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Result<Pogreb> Get(int id)
    {
        try
        {
            var model = _dbContext.Pogreb
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdPogreb.Equals(id))?
                          .ToDomain();

            return model is not null
            ? Results.OnSuccess(model)
                : Results.OnFailure<Pogreb>($"No funeral with id {id} found");
        }
        catch (Exception e)
        {
            return Results.OnException<Pogreb>(e);
        }
    }

    public Result<Pogreb> GetAggregate(int id)
    {
        try
        {
            var model = _dbContext.Pogreb
                          //.Include(p => p.SmrtniSlucaj)
                          .Include(p => p.PogrebOprema)
                          .ThenInclude(po => po.Oprema)
                          .Include(p => p.Usluga)
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdPogreb.Equals(id)) // give me the first or null; substitute for .Where() // single or default throws an exception if more than one element meets the criteria
                          ?.ToDomain();


            return model is not null
                ? Results.OnSuccess(model)
                : Results.OnFailure<Pogreb>();
        }
        catch (Exception e)
        {
            return Results.OnException<Pogreb>(e);
        }
    }

    public Result<IEnumerable<Pogreb>> GetAll()
    {
        try
        {
            var models = _dbContext.Pogreb
                           .AsNoTracking()
                           .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Pogreb>>(e);
        }
    }

    public Result<IEnumerable<Pogreb>> GetAllAggregates()
    {
        try
        {
            var models = _dbContext.Pogreb
                            .Include(p => p.SmrtniSlucaj)
                            .Include(p => p.PogrebOprema)
                            .Include(p => p.Usluga) // ????
                            .Select(Mapping.ToDomain);

            return Results.OnSuccess(models);
        }
        catch (Exception e)
        {
            return Results.OnException<IEnumerable<Pogreb>>(e);
        }
    }

    public Result Insert(Pogreb model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            if (_dbContext.Pogreb.Add(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
            var model = _dbContext.Pogreb
                          .AsNoTracking()
                          .FirstOrDefault(p => p.IdPogreb.Equals(id));

            if (model is not null)
            {
                _dbContext.Pogreb.Remove(model);

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

    public Result Update(Pogreb model)
    {
        try
        {
            var dbModel = model.ToDbModel();
            // detach
            if (_dbContext.Pogreb.Update(dbModel).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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


    
    public Result UpdateAggregate(Pogreb model)
    {
        try
        {
            _dbContext.ChangeTracker.Clear();

            var dbModel = _dbContext.Pogreb
                              .Include(_ => _.PogrebOprema)
                              .ThenInclude(_ => _.Oprema)
                              .Include(_ => _.Usluga)
                              //.AsNoTracking()
                              .FirstOrDefault(_ => _.IdPogreb == model.Id);
            if (dbModel == null)
                return Results.OnFailure($"Funeral with id {model.Id} not found.");

            

            dbModel.SmrtniSlucajId = model.SmrtniSlucajId;
            dbModel.DatumPogreb = model.DatumPogreba;
            dbModel.Kremacija = model.Kremacija;

            
            foreach (var pogrebOprema in model.PogrebOprema)
            {
                // it exists in the DB, so just update it
                var pogrebOpremaToUpdate =
                    dbModel.PogrebOprema
                           .FirstOrDefault(po => po.PogrebId.Equals(model.Id) && po.OpremaId.Equals(pogrebOprema.Oprema.Id));
                if (pogrebOpremaToUpdate != null)
                {
                    pogrebOprema.Kolicina = pogrebOprema.Kolicina;
                }
                else // it does not exist in the DB, so add it
                {
                    dbModel.PogrebOprema.Add(pogrebOprema.ToDbModel(model.Id));
                }
            }

            dbModel.PogrebOprema
                  .Where(po => !model.PogrebOprema.Any(_ => _.Oprema.Id == po.OpremaId))
                  .ToList()
                  .ForEach(pogrebOprema =>
                  {
                      dbModel.PogrebOprema.Remove(pogrebOprema);
                  });

            foreach (var pogrebUsluga in model.PogrebUsluga)
            {
                var pogrebUslugaToUpdate = dbModel.Usluga.FirstOrDefault(pu => pu.Pogreb.Any(p => p.IdPogreb.Equals(model.Id)) && pu.IdUsluga.Equals(pogrebUsluga.Id));
                if (pogrebUslugaToUpdate != null)
                {
                    pogrebUslugaToUpdate.VrstaUslugeId = pogrebUsluga.VrstaUslugeId;
                    pogrebUslugaToUpdate.Naziv = pogrebUsluga.Naziv;
                    pogrebUslugaToUpdate.Opis = pogrebUsluga.Opis;
                    pogrebUslugaToUpdate.Cijena = pogrebUsluga.Cijena;
                }
                else
                {
                    dbModel.Usluga.Add(pogrebUsluga.ToDbModel());
                }
            }

            dbModel.Usluga
                .Where(po => !model.PogrebUsluga.Any(_ => _.Id == po.IdUsluga))
                  .ToList()
                  .ForEach(pogrebUsluga =>
                  {
                      dbModel.Usluga.Remove(pogrebUsluga);
                  });


            _dbContext.Pogreb
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