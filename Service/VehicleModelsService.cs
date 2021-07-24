using AutoMapper;
using Common;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Model;
using Model.Common;
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
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleModelsService> _logger;

        public VehicleModelsService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VehicleModelsService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IPagedList<VehicleModel>> GetVehicleModels(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleModelFilterParams vehicleModelFilterParams)
        {
            var pagedModels = await _unitOfWork.VehicleModels.GetAll(sortParams, pagingParams, vehicleModelFilterParams);

            return pagedModels;
        }

        public async Task<VehicleModel> GetVehicleModel(int id)
        {
            var model = await _unitOfWork.VehicleModels.GetById(id);
            return model;
        }

        public async Task<VehicleModel> InsertVehicleModel(VehicleModel vehicleModel)
        {
            var createdModel = _unitOfWork.VehicleModels.Insert(vehicleModel);
            await _unitOfWork.Complete();
            return createdModel;
        }

        public async Task<int> DeleteVehicleModel(VehicleModel vehicleModel)
        {
            _unitOfWork.VehicleModels.Delete(vehicleModel);
            return await _unitOfWork.Complete();
        }

        public async Task<int> UpdateVehicleModel(VehicleModel vehicleModel)
        {
            _unitOfWork.VehicleModels.Update(vehicleModel);
            return await _unitOfWork.Complete();
        }
    }
}
