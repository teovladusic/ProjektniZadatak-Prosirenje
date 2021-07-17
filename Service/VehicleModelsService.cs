using AutoMapper;
using Common;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Model;
using Model.Common;
using Repository.Common;
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

        public async Task DeleteVehicleModel(IVehicleModelViewModel vehicleModelViewModel)
        {
            var vehicleModel = _mapper.Map<VehicleModel>(vehicleModelViewModel);
            _unitOfWork.VehicleModels.Delete(vehicleModel);
            await _unitOfWork.Complete();
        }

        public async Task<IVehicleModelViewModel> GetVehicleModel(int id)
        {
            var model = await _unitOfWork.VehicleModels.GetById(id);
            return _mapper.Map<VehicleModelViewModel>(model);
        }

        public async Task<PagedList<IVehicleModelViewModel>> GetVehicleModels(VehicleModelFilterParams parameters)
        {
            var pagedModels = await _unitOfWork.VehicleModels.GetAll(parameters);

            var vehicleModelViewModels = _mapper.Map<List<VehicleModelViewModel>>(pagedModels).Cast<IVehicleModelViewModel>().ToList();

            return new PagedList<IVehicleModelViewModel>(vehicleModelViewModels, pagedModels.TotalCount,
                parameters.PagingParams.CurrentPage, parameters.PagingParams.PageSize);
        }

        public async Task InsertVehicleModel(ICreateVehicleModelViewModel createVehicleModelViewModel)
        {
            var vehicleModel = _mapper.Map<VehicleModel>(createVehicleModelViewModel);
            _unitOfWork.VehicleModels.Insert(vehicleModel);
            await _unitOfWork.Complete();
        }

        public async Task UpdateVehicleModel(IEditVehicleModelViewModel editVehicleModelViewModel)
        {
            var vehicleModel = _mapper.Map<VehicleModel>(editVehicleModelViewModel);
            _unitOfWork.VehicleModels.Update(vehicleModel);
            await _unitOfWork.Complete();
        }
    }
}
