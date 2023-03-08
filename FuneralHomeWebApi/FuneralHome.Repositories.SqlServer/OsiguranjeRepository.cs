using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class OsiguranjeRepository : IOsiguranjeRepository<int, Osiguranje>
{
    private readonly FuneralHomeContext _dbContext;

    public OsiguranjeRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Osiguranje model)
    {
        return _dbContext.Osiguranje
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.Osiguranje
                              .AsNoTracking()
                              .FirstOrDefault(o => o.Id.Equals(id));
        return model is not null;
    }

    public Option<Osiguranje> Get(int id)
    {
        var model = _dbContext.Osiguranje
                              .AsNoTracking()
                              .FirstOrDefault(o => o.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<Osiguranje>();
    }


    public IEnumerable<Osiguranje> GetAll()
    {
        var models = _dbContext.Osiguranje
                               .ToList();

        return models;
    }


    public bool Insert(Osiguranje model)
    {
        if (_dbContext.Osiguranje.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.Osiguranje
                              .AsNoTracking()
                              .FirstOrDefault(o => o.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.Osiguranje.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(Osiguranje model)
    {
        // detach
        if (_dbContext.Osiguranje.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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