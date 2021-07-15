using DAL.Models;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class VehicleModelsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleModelsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteVehicleMake(VehicleModel vehicleModel)
        {
            _unitOfWork.VehicleModels.Delete(vehicleModel);
            await _unitOfWork.Complete();
        }

        public async Task<VehicleModel> GetVehicleModel(int id)
        {
            return await _unitOfWork.VehicleModels.GetById(id);
        }

        public async Task<List<VehicleModel>> GetVehicleModels()
        {
            return await _unitOfWork.VehicleModels.GetAll();
        }

        public async Task InsertVehicleMake(VehicleModel vehicleModel)
        {
            _unitOfWork.VehicleModels.Insert(vehicleModel);
            await _unitOfWork.Complete();
        }

        public async Task UpdateVehicleMake(VehicleModel vehicleModel)
        {
            _unitOfWork.VehicleModels.Update(vehicleModel);
            await _unitOfWork.Complete();
        }
    }
}
