using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class CvijeceRepository : ICvijeceRepository<int, Cvijece>
{
    private readonly FuneralHomeContext _dbContext;

    public CvijeceRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Cvijece model)
    {
        return _dbContext.Cvijece
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.Cvijece
                              .AsNoTracking()
                              .FirstOrDefault(c => c.Id.Equals(id));
        return model is not null;
    }

    public Option<Cvijece> Get(int id)
    {
        var model = _dbContext.Cvijece
                              .AsNoTracking()
                              .FirstOrDefault(c => c.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<Cvijece>();
    }

  
    public IEnumerable<Cvijece> GetAll()
    {
        var models = _dbContext.Cvijece
                               .ToList();

        return models;
    }


    public bool Insert(Cvijece model)
    {
        if (_dbContext.Cvijece.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.Cvijece
                              .AsNoTracking()
                              .FirstOrDefault(c => c.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.Cvijece.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(Cvijece model)
    {
        // detach
        if (_dbContext.Cvijece.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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