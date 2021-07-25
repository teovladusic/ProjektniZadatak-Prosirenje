using AutoMapper;
using Common;
using DAL;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Model;
using Model.VehicleMakes;
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
        private readonly IMapper _mapper;

        public VehicleMakesService(IUnitOfWork unitOfWork, ILogger<VehicleMakesService> logger, ISortHelper<VehicleMake> sortHelper,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _vehicleMakesRepository = new VehicleMakesRepository(_unitOfWork, sortHelper);
            _mapper = mapper;
        }

        public async Task<IPagedList<VehicleMakeDomainModel>> GetVehicleMakes(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleMakeFilterParams vehicleMakeFilterParams)
        {
            var pagedMakes = await _vehicleMakesRepository.GetAll(sortParams, pagingParams, vehicleMakeFilterParams);

            var domainModels = _mapper.Map<List<VehicleMakeDomainModel>>(pagedMakes);
            var pagedDomainModels = CommonFactory.CreatePagedList(domainModels, pagedMakes.TotalCount, pagedMakes.CurrentPage,
                pagedMakes.PageSize);

            return pagedDomainModels;
        }

        public async Task<VehicleMakeDomainModel> GetVehicleMake(int id)
        {
            var make = await _vehicleMakesRepository.GetById(id);
            return _mapper.Map<VehicleMakeDomainModel>(make);
        }

        public async Task<VehicleMakeDomainModel> InsertVehicleMake(CreateVehicleMakeDomainModel vehicleMakeDomainModel)
        {
            var makeToCreate = _mapper.Map<VehicleMake>(vehicleMakeDomainModel);

            var createdMake = _vehicleMakesRepository.Insert(makeToCreate);
            await _unitOfWork.Complete();
            return _mapper.Map<VehicleMakeDomainModel>(createdMake);
        }

        public async Task<int> DeleteVehicleMake(VehicleMakeDomainModel vehicleMakeDomainModel)
        {
            var vehicleMake = _mapper.Map<VehicleMake>(vehicleMakeDomainModel);
            _vehicleMakesRepository.Delete(vehicleMake);
            return await _unitOfWork.Complete();
        }

        public async Task<int> UpdateVehicleMake(VehicleMakeDomainModel vehicleMakeDomainModel)
        {
            var vehicleMake = _mapper.Map<VehicleMake>(vehicleMakeDomainModel);
            _vehicleMakesRepository.Update(vehicleMake);
            return await _unitOfWork.Complete();
        }
    }
}
