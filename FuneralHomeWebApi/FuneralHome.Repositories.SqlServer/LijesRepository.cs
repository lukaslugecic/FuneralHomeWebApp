using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class LijesRepository : ILijesRepository<int, Lijes>
{
    private readonly FuneralHomeContext _dbContext;

    public LijesRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(Lijes model)
    {
        return _dbContext.Lijes
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.Lijes
                              .AsNoTracking()
                              .FirstOrDefault(l => l.Id.Equals(id));
        return model is not null;
    }

    public Option<Lijes> Get(int id)
    {
        var model = _dbContext.Lijes
                              .AsNoTracking()
                              .FirstOrDefault(l => l.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<Lijes>();
    }


    public IEnumerable<Lijes> GetAll()
    {
        var models = _dbContext.Lijes
                               .ToList();

        return models;
    }


    public bool Insert(Lijes model)
    {
        if (_dbContext.Lijes.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.Lijes
                              .AsNoTracking()
                              .FirstOrDefault(l => l.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.Lijes.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(Lijes model)
    {
        // detach
        if (_dbContext.Lijes.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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