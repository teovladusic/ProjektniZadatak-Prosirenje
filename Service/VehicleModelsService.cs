using AutoMapper;
using Common;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Model;
using Model.Common;
using Repository;
using Repository.Common;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class VehicleModelsService : IVehicleModelsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VehicleModelsService> _logger;
        private readonly IVehicleModelsRepository _vehicleModelsRepository;

        public VehicleModelsService(IUnitOfWork unitOfWork, ISortHelper<VehicleModel> sortHelper, ILogger<VehicleModelsService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _vehicleModelsRepository = new VehicleModelsRepository(_unitOfWork, sortHelper);
        }

        public async Task<IPagedList<VehicleModel>> GetVehicleModels(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleModelFilterParams vehicleModelFilterParams)
        {
            var pagedModels = await _vehicleModelsRepository.GetAll(sortParams, pagingParams, vehicleModelFilterParams);

            return pagedModels;
        }

        public async Task<VehicleModel> GetVehicleModel(int id)
        {
            var model = await _vehicleModelsRepository.GetById(id);
            return model;
        }

        public async Task<VehicleModel> InsertVehicleModel(VehicleModel vehicleModel)
        {
            var createdModel = _vehicleModelsRepository.Insert(vehicleModel);
            await _unitOfWork.Complete();
            return createdModel;
        }

        public async Task<int> DeleteVehicleModel(VehicleModel vehicleModel)
        {
            _vehicleModelsRepository.Delete(vehicleModel);
            return await _unitOfWork.Complete();
        }

        public async Task<int> UpdateVehicleModel(VehicleModel vehicleModel)
        {
            _vehicleModelsRepository.Update(vehicleModel);
            return await _unitOfWork.Complete();
        }
    }
}
