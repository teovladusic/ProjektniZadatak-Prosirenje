using AutoMapper;
using Common;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IApplicationDbContext _context;
        ISortHelper<VehicleMake> _vehicleMakeSortHelper;
        ISortHelper<VehicleModel> _vehicleModelSortHelper;
        ILogger<VehicleModelsRepository> _logger;

        public UnitOfWork(IApplicationDbContext context, ISortHelper<VehicleMake> vehicleMakeSortHelper, IMapper mapper,
            ISortHelper<VehicleModel> vehicleModelSortHelper, ILogger<VehicleModelsRepository> logger)
        {
            _context = context;
            _context.SetNoQueryTrackingBehaviour();
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
