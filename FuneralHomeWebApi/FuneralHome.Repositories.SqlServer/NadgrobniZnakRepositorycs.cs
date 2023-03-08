using FuneralHome.Commons;
using FuneralHome.DataAccess.SqlServer.Data;
using FuneralHome.DataAccess.SqlServer.Data.DbModels;
using Microsoft.EntityFrameworkCore;
using System;

namespace FuneralHome.Repositories.SqlServer;
public class NadgrobniZnakRepository : INadgrobniZnakRepository<int, NadgrobniZnak>
{
    private readonly FuneralHomeContext _dbContext;

    public NadgrobniZnakRepository(FuneralHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Exists(NadgrobniZnak model)
    {
        return _dbContext.NadgrobniZnak
                         .AsNoTracking()
                         .Contains(model);
    }

    public bool Exists(int id)
    {
        var model = _dbContext.NadgrobniZnak
                              .AsNoTracking()
                              .FirstOrDefault(nz => nz.Id.Equals(id));
        return model is not null;
    }

    public Option<NadgrobniZnak> Get(int id)
    {
        var model = _dbContext.NadgrobniZnak
                              .AsNoTracking()
                              .FirstOrDefault(nz => nz.Id.Equals(id));

        return model is not null
            ? Options.Some(model)
            : Options.None<NadgrobniZnak>();
    }


    public IEnumerable<NadgrobniZnak> GetAll()
    {
        var models = _dbContext.NadgrobniZnak
                               .ToList();

        return models;
    }


    public bool Insert(NadgrobniZnak model)
    {
        if (_dbContext.NadgrobniZnak.Add(model).State == Microsoft.EntityFrameworkCore.EntityState.Added)
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
        var model = _dbContext.NadgrobniZnak
                              .AsNoTracking()
                              .FirstOrDefault(g => g.Id.Equals(id));

        if (model is not null)
        {
            _dbContext.NadgrobniZnak.Remove(model);

            return _dbContext.SaveChanges() > 0;
        }
        return false;
    }

    public bool Update(NadgrobniZnak model)
    {
        // detach
        if (_dbContext.NadgrobniZnak.Update(model).State == Microsoft.EntityFrameworkCore.EntityState.Modified)
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