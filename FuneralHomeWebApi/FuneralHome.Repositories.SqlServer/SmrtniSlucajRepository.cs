using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class SmrtniSlucajRepository : ISmrtniSlucajRepository<int, SmrtniSlucaj>
{
    private readonly FuneralHomeContext _dbContext;

    public SmrtniSlucajRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(SmrtniSlucaj model)
    {
        return _dbContext.SmrtniSlucaj
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.SmrtniSlucaj
                              .AsNoTracking()
                              .FirstOrDefault(ss => ss.Id.Equals(id));
        return model is not null;
    }

    public Option<SmrtniSlucaj> Get(int id)
    {
        var model = _dbContext.SmrtniSlucaj
                              .AsNoTracking()
                              .FirstOrDefault(ss => ss.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<SmrtniSlucaj>();
    }

    public Option<SmrtniSlucaj> GetAggregate(int id)
    {
        var model = _dbContext.SmrtniSlucaj
                              .Include(ss => ss.Oglas)
                              .AsNoTracking()
                              .FirstOrDefault(k => k.Id.Equals(id)); // give me the first or null; substitute for .Where()
                                                                     // single or default throws an exception if more than one element meets the criteria

        return model is not null
            ? Options.Some(model)
            : Options.None<SmrtniSlucaj>();
    }

    public IEnumerable<SmrtniSlucaj> GetAll()
    {
        var models = _dbContext.SmrtniSlucaj
                               .ToList();

        return models;
    }

    public IEnumerable<SmrtniSlucaj> GetAllAggregates()
    {
        var models = _dbContext.SmrtniSlucaj
                               .Include(ss => ss.Oglas)
                               .ToList();

        return models;
    }

    public bool Insert(SmrtniSlucaj model)
    {
        if (_dbContext.SmrtniSlucaj.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.SmrtniSlucaj
                              .AsNoTracking()
                              .FirstOrDefault(ss => ss.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.SmrtniSlucaj.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(SmrtniSlucaj model)
    {
        // detach
        if (_dbContext.SmrtniSlucaj.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
        {
            var isSuccess = _dbContext.SaveChanges() > 0;

            // every Update attaches the entity object and EF begins tracking
            // we detach the entity object from tracking, because this can cause problems when a repo is not set as a transient service
            _dbContext.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            return isSuccess;
        }

        return false;
    }

    public bool UpdateAggregate(SmrtniSlucaj model)
    {
        if (_dbContext.SmrtniSlucaj.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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