using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class PogrebRepository : IPogrebRepository<int, Pogreb>
{
    private readonly FuneralHomeContext _dbContext;

    public PogrebRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Pogreb model)
    {
        return _dbContext.Pogreb
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.Pogreb
                              .AsNoTracking()
                              .FirstOrDefault(p => p.Id.Equals(id));
        return model is not null;
    }

    public Option<Pogreb> Get(int id)
    {
        var model = _dbContext.Pogreb
                              .AsNoTracking()
                              .FirstOrDefault(p => p.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<Pogreb>();
    }

    public Option<Pogreb> GetAggregate(int id)
    {
        var model = _dbContext.Pogreb
                              .Include(p => p.SmrtniSlucaj)
                              .Include(p => p.Urna)
                              .Include(p => p.Lijes)
                              .Include(p => p.Cvijece)
                              .Include(p => p.NadgrobniZnak)
                              .Include(p => p.Glazba)
                              .AsNoTracking()
                              .FirstOrDefault(k => k.Id.Equals(id)); // give me the first or null; substitute for .Where()
                                                                     // single or default throws an exception if more than one element meets the criteria

        return model is not null
            ? Options.Some(model)
            : Options.None<Pogreb>();
    }

    public IEnumerable<Pogreb> GetAll()
    {
        var models = _dbContext.Pogreb
                               .ToList();

        return models;
    }

    public IEnumerable<Pogreb> GetAllAggregates()
    {
        var models = _dbContext.Pogreb
                                .Include(p => p.SmrtniSlucaj)
                              .Include(p => p.Urna)
                              .Include(p => p.Lijes)
                              .Include(p => p.Cvijece)
                              .Include(p => p.NadgrobniZnak)
                              .Include(p => p.Glazba)
                               .ToList();

        return models;
    }

    public bool Insert(Pogreb model)
    {
        if (_dbContext.Pogreb.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.Pogreb
                              .AsNoTracking()
                              .FirstOrDefault(p => p.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.Pogreb.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(Pogreb model)
    {
        // detach
        if (_dbContext.Pogreb.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
        {
            var isSuccess = _dbContext.SaveChanges() > 0;

            // every Update attaches the entity object and EF begins tracking
            // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
            _dbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            return isSuccess;
        }

        return false;
    }

    public bool UpdateAggregate(Pogreb model)
    {
        if (_dbContext.Pogreb.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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