using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class UrnaRepository : IUrnaRepository<int, Urna>
{
    private readonly FuneralHomeContext _dbContext;

    public UrnaRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Urna model)
    {
        return _dbContext.Urna
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.Urna
                              .AsNoTracking()
                              .FirstOrDefault(u => u.Id.Equals(id));
        return model is not null;
    }

    public Option<Urna> Get(int id)
    {
        var model = _dbContext.Urna
                              .AsNoTracking()
                              .FirstOrDefault(u => u.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<Urna>();
    }


    public IEnumerable<Urna> GetAll()
    {
        var models = _dbContext.Urna
                               .ToList();

        return models;
    }


    public bool Insert(Urna model)
    {
        if (_dbContext.Urna.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.Urna
                              .AsNoTracking()
                              .FirstOrDefault(u => u.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.Urna.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(Urna model)
    {
        // detach
        if (_dbContext.Urna.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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