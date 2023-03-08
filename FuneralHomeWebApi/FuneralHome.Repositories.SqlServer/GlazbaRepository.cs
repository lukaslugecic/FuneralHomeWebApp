using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class GlazbaRepository : IGlazbaRepository<int, Glazba>
{
    private readonly FuneralHomeContext _dbContext;

    public GlazbaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Glazba model)
    {
        return _dbContext.Glazba
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.Glazba
                              .AsNoTracking()
                              .FirstOrDefault(g => g.Id.Equals(id));
        return model is not null;
    }

    public Option<Glazba> Get(int id)
    {
        var model = _dbContext.Glazba
                              .AsNoTracking()
                              .FirstOrDefault(g => g.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<Glazba>();
    }


    public IEnumerable<Glazba> GetAll()
    {
        var models = _dbContext.Glazba
                               .ToList();

        return models;
    }


    public bool Insert(Glazba model)
    {
        if (_dbContext.Glazba.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.Glazba
                              .AsNoTracking()
                              .FirstOrDefault(g => g.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.Glazba.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(Glazba model)
    {
        // detach
        if (_dbContext.Glazba.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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