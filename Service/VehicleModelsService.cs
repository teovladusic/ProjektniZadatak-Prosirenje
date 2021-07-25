using AutoMapper;
using Common;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.VehicleModels;
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
        private readonly IMapper _mapper;

        public VehicleModelsService(IUnitOfWork unitOfWork, ISortHelper<VehicleModel> sortHelper, ILogger<VehicleModelsService> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _vehicleModelsRepository = new VehicleModelsRepository(_unitOfWork, sortHelper);
            _mapper = mapper;
        }

        public async Task<IPagedList<VehicleModelDomainModel>> GetVehicleModels(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleModelFilterParams vehicleModelFilterParams)
        {
            var pagedModels = await _vehicleModelsRepository.GetAll(sortParams, pagingParams, vehicleModelFilterParams);

            var domainModelsList = _mapper.Map<List<VehicleModelDomainModel>>(pagedModels);

            var pagedDomainModels = CommonFactory.CreatePagedList(domainModelsList, pagedModels.TotalCount, pagedModels.CurrentPage,
                pagedModels.PageSize);

            return pagedDomainModels;
        }

        public async Task<VehicleModelDomainModel> GetVehicleModel(int id)
        {
            var model = await _vehicleModelsRepository.GetById(id);
            return _mapper.Map<VehicleModelDomainModel>(model);
        }

        public async Task<VehicleModelDomainModel> InsertVehicleModel(CreateVehicleModelDomainModel createVehicleModelDomainModel)
        {
            var modelToCreate = _mapper.Map<VehicleModel>(createVehicleModelDomainModel);
            var createdModel = _vehicleModelsRepository.Insert(modelToCreate);
            var createdDomainModel = _mapper.Map<VehicleModelDomainModel>(createdModel);
            await _unitOfWork.Complete();
            return createdDomainModel;
        }

        public async Task<int> DeleteVehicleModel(int id)
        {
            _vehicleModelsRepository.Delete(id);
            var result = await _unitOfWork.Complete();
            return result;
        }

        public async Task<int> UpdateVehicleModel(VehicleModelDomainModel vehicleModelDomainModel)
        {
            var vehicleModelToUpdate = _mapper.Map<VehicleModel>(vehicleModelDomainModel);
            _vehicleModelsRepository.Update(vehicleModelToUpdate);
            return await _unitOfWork.Complete();
        }
    }
}
