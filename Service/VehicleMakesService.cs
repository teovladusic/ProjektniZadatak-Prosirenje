using AutoMapper;
using Common;
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
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleMakesService> _logger;

        public VehicleMakesService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VehicleMakesService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task DeleteVehicleMake(IVehicleMakeViewModel vehicleMakeViewModel)
        {
            var vehicleMake = _mapper.Map<VehicleMake>(vehicleMakeViewModel);
            _unitOfWork.VehicleMakes.Delete(vehicleMake);
            await _unitOfWork.Complete();
        }

        public async Task<IVehicleMakeViewModel> GetVehicleMake(int id)
        {
            var make = await _unitOfWork.VehicleMakes.GetById(id);
            return _mapper.Map<VehicleMakeViewModel>(make);
        }

        public async Task<PagedList<IVehicleMakeViewModel>> GetVehicleMakes(VehicleMakeFilterParams parameters)
        {
            var pagedMakes = await _unitOfWork.VehicleMakes.GetAll(parameters);

            var vehicleMakeViewModels = _mapper.Map<List<VehicleMakeViewModel>>(pagedMakes).Cast<IVehicleMakeViewModel>().ToList();

            return new PagedList<IVehicleMakeViewModel>(vehicleMakeViewModels, pagedMakes.TotalCount,
                parameters.PagingParams.CurrentPage, parameters.PagingParams.PageSize);
        }

        public async Task InsertVehicleMake(ICreateVehicleMakeViewModel createVehicleMakeViewModel)
        {
            var vehicleMake = _mapper.Map<VehicleMake>(createVehicleMakeViewModel);
            _unitOfWork.VehicleMakes.Insert(vehicleMake);
            await _unitOfWork.Complete();
        }

        public async Task UpdateVehicleMake(IVehicleMakeViewModel vehicleMakeViewModel)
        {
            var vehicleMake = _mapper.Map<VehicleMake>(vehicleMakeViewModel);
            _unitOfWork.VehicleMakes.Update(vehicleMake);
            await _unitOfWork.Complete();
        }
    }
}
