using AutoMapper;
using Common;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        /*Unit of Work is the concept related to the effective implementation of the repository
        pattern.non-generic repository pattern, generic repository pattern.Unit of Work is referred
        to as a single transaction that involves multiple operations of insert/update/delete and so on*/

        private readonly ApplicationDbContext _context;
        private readonly ISortHelper<VehicleMake> _vehicleMakeSortHelper;
        private readonly ISortHelper<VehicleModel> _vehicleModelSortHelper;
        private readonly ILogger<VehicleModelsRepository> _logger;
        
        public UnitOfWork(ApplicationDbContext context, ISortHelper<VehicleMake> vehicleMakeSortHelper, IMapper mapper,
            ISortHelper<VehicleModel> vehicleModelSortHelper, ILogger<VehicleModelsRepository> logger)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _vehicleMakeSortHelper = vehicleMakeSortHelper;
            _vehicleModelSortHelper = vehicleModelSortHelper;
            _logger = logger;
        }

        private IVehicleMakesRepository vehicleMakes;
        public IVehicleMakesRepository VehicleMakes
        {
            get
            {
                if (vehicleMakes == null)
                {
                    this.vehicleMakes = new VehicleMakesRepository(_context, _vehicleMakeSortHelper);
                }
                return this.vehicleMakes;
            }
        }


        private IVehicleModelsRepository vehicleModels;
        public IVehicleModelsRepository VehicleModels
        {
            get
            {
                if (vehicleModels == null)
                {
                    this.vehicleModels = new VehicleModelsRepository(_context, _vehicleModelSortHelper, _logger);
                }
                return vehicleModels;
            }
        }

        public virtual Task<T> AddAsync<T>(T entity) where T : BaseEntity
        {
            try
            {
                var dbEntityEntry = _context.Entry(entity);
                if (dbEntityEntry.State != EntityState.Detached)
                {
                    dbEntityEntry.State = EntityState.Added;
                }
                else
                {
                    _context.Set<T>().Add(entity);
                }
                return Task.FromResult(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual Task<int> UpdateAsync<T>(T entity) where T : class
        {
            try
            {
                var dbEntityEntry = _context.Entry(entity);
                if (dbEntityEntry.State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                }
                dbEntityEntry.State = EntityState.Modified;
                return Task.FromResult(1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual Task<int> DeleteAsync<T>(T entity) where T : class
        {
            try
            {
                var dbEntityEntry = _context.Entry(entity);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    _context.Set<T>().Attach(entity);
                    _context.Set<T>().Remove(entity);
                }
                return Task.FromResult(1);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public virtual Task<int> DeleteAsync<T>(Guid id) where T : class
        {
            var entity = _context.Set<T>().Find(id);
            if (entity == null)
            {
                return Task.FromResult(0);
            }
            return DeleteAsync<T>(entity);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
