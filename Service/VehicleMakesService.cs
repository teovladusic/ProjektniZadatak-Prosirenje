using AutoMapper;
using Common;
using DAL;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Model;
using Model.Common;
using Project.Model;
using Project.Model.Common;
using Repository;
using Repository.Common;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service
{
    public class VehicleMakesService : IVehicleMakesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VehicleMakesService> _logger;
        private readonly IVehicleMakesRepository _vehicleMakesRepository;

        public VehicleMakesService(IUnitOfWork unitOfWork, ILogger<VehicleMakesService> logger, ISortHelper<VehicleMake> sortHelper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _vehicleMakesRepository = new VehicleMakesRepository(_unitOfWork, sortHelper);
        }

        public async Task<IPagedList<VehicleMake>> GetVehicleMakes(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleMakeFilterParams vehicleMakeFilterParams)
        {
            var pagedMakes = await _vehicleMakesRepository.GetAll(sortParams, pagingParams, vehicleMakeFilterParams);

            return pagedMakes;
        }

        public async Task<VehicleMake> GetVehicleMake(int id)
        {
            var make = await _vehicleMakesRepository.GetById(id);
            return make;
        }

        public async Task<VehicleMake> InsertVehicleMake(VehicleMake vehicleMake)
        {
            var createdMake = _vehicleMakesRepository.Insert(vehicleMake);
            await _unitOfWork.Complete();
            return createdMake;
        }

        public async Task<int> DeleteVehicleMake(VehicleMake vehicleMake)
        {
            _vehicleMakesRepository.Delete(vehicleMake);
            return await _unitOfWork.Complete();
        }

        public async Task<int> UpdateVehicleMake(VehicleMake vehicleMake)
        {
            _vehicleMakesRepository.Update(vehicleMake);
            return await _unitOfWork.Complete();
        }
    }
}
