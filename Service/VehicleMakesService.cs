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
        private readonly IMapper _mapper;
        private readonly ILogger<VehicleMakesService> _logger;
        private readonly IVehicleMakesRepository _vehicleMakesRepository;

        public VehicleMakesService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VehicleMakesService> logger, ISortHelper<VehicleMake> sortHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _vehicleMakesRepository = new VehicleMakesRepository(_unitOfWork, sortHelper);
        }

        public async Task<IPagedList<VehicleMake>> GetVehicleMakes(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleMakeFilterParams vehicleMakeFilterParams)
        {
            var pagedMakes = await _unitOfWork.VehicleMakes.GetAll(sortParams, pagingParams, vehicleMakeFilterParams);

            return pagedMakes;
        }

        public async Task<VehicleMake> GetVehicleMake(int id)
        {
            var make = await _unitOfWork.VehicleMakes.GetById(id);
            return make;
        }

        public async Task<VehicleMake> InsertVehicleMake(VehicleMake vehicleMake)
        {
            var createdMake = await _unitOfWork.AddAsync(vehicleMake);
            //var createdMake = _unitOfWork.VehicleMakes.Insert(vehicleMake);
            await _unitOfWork.Complete();
            return createdMake;
        }

        public async Task<int> DeleteVehicleMake(VehicleMake vehicleMake)
        {
            _unitOfWork.VehicleMakes.Delete(vehicleMake);
            return await _unitOfWork.Complete();
        }

        public async Task<int> UpdateVehicleMake(VehicleMake vehicleMake)
        {
            _unitOfWork.VehicleMakes.Update(vehicleMake);
            return await _unitOfWork.Complete();
        }
    }
}
