using Common;
using DAL.Models;
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

        public VehicleMakesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteVehicleMake(VehicleMake vehicleMake)
        {
            _unitOfWork.VehicleMakes.Delete(vehicleMake);
            await _unitOfWork.Complete();
        }

        public async Task<VehicleMake> GetVehicleMake(int id)
        {
            return await _unitOfWork.VehicleMakes.GetById(id);
        }

        public async Task<List<VehicleMake>> GetVehicleMakes(IVehicleMakeFilterParams parameters)
        {
            return await _unitOfWork.VehicleMakes.GetAll(parameters);
        }

        public async Task InsertVehicleMake(VehicleMake vehicleMake)
        {
            _unitOfWork.VehicleMakes.Insert(vehicleMake);
            await _unitOfWork.Complete();
        }

        public async Task UpdateVehicleMake(VehicleMake vehicleMake)
        {
            _unitOfWork.VehicleMakes.Update(vehicleMake);
            await _unitOfWork.Complete();
        }
    }
}
