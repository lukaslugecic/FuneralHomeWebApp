using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class KorisnikRepository : IKorisnikRepository<int, Korisnik>
{
    private readonly FuneralHomeContext _dbContext;

    public KorisnikRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Korisnik model)
    {
        return _dbContext.Korisnik
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.Korisnik
                              .AsNoTracking()
                              .FirstOrDefault(k => k.Id.Equals(id));
        return model is not null;
    }

    public Option<Korisnik> Get(int id)
    {
        var model = _dbContext.Korisnik
                              .AsNoTracking()
                              .FirstOrDefault(k => k.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<Korisnik>();
    }

    public Option<Korisnik> GetAggregate(int id)
    {
        var model = _dbContext.Korisnik
                               .Include(k => k.Osiguranje)
                              .Include(k => k.SmrtniSlucaj)
                              .AsNoTracking()
                              .FirstOrDefault(k => k.Id.Equals(id)); // give me the first or null; substitute for .Where()
                                                                               // single or default throws an exception if more than one element meets the criteria

        return model is not null
            ? Options.Some(model)
            : Options.None<Korisnik>();
    }

    public IEnumerable<Korisnik> GetAll()
    {
        var models = _dbContext.Korisnik
                               .ToList();

        return models;
    }

    public IEnumerable<Korisnik> GetAllAggregates()
    {
        var models = _dbContext.Korisnik
                                .Include(k => k.Osiguranje)
                               .Include(k => k.SmrtniSlucaj)
                               .ToList();

        return models;
    }

    public bool Insert(Korisnik model)
    {
        if (_dbContext.Korisnik.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
        {
            var isSuccess = _dbContext.SaveChanges() > 0;

            // every Add attaches the entity object and EF begins tracking
            // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
            _dbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            return isSuccess;
        }

        return false;
    }

    public bool Remove(int id)
    {
        var model = _dbContext.Korisnik
                              .AsNoTracking()
                              .FirstOrDefault(k => k.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.Korisnik.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(Korisnik model)
    {
        // detach
        if (_dbContext.Korisnik.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
        {
            var isSuccess = _dbContext.SaveChanges() > 0;

            // every Update attaches the entity object and EF begins tracking
            // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
            _dbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            return isSuccess;
        }

        return false;
    }

    public bool UpdateAggregate(Korisnik model)
    {
        if (_dbContext.Korisnik.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
        {
            var isSuccess = _dbContext.SaveChanges() > 0;

            // every Update attaches the entity object and EF begins tracking
            // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
            _dbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            return isSuccess;
        }

        return false;
    }
}