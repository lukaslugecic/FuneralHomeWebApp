using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class OglasRepository : IOglasRepository<int, Oglas>
{
    private readonly FuneralHomeContext _dbContext;

    public OglasRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Oglas model)
    {
        return _dbContext.Oglas
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.Oglas
                              .AsNoTracking()
                              .FirstOrDefault(o => o.Id.Equals(id));
        return model is not null;
    }

    public Option<Oglas> Get(int id)
    {
        var model = _dbContext.Oglas
                              .AsNoTracking()
                              .FirstOrDefault(o => o.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<Oglas>();
    }


    public IEnumerable<Oglas> GetAll()
    {
        var models = _dbContext.Oglas
                               .ToList();

        return models;
    }


    public bool Insert(Oglas model)
    {
        if (_dbContext.Oglas.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.Oglas
                              .AsNoTracking()
                              .FirstOrDefault(o => o.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.Oglas.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(Oglas model)
    {
        // detach
        if (_dbContext.Oglas.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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