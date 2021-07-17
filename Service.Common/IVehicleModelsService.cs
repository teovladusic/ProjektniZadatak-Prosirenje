using Common;
using DAL.Models;
using Model;
using Model.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IVehicleModelsService
    {
        Task DeleteVehicleModel(IVehicleModelViewModel vehicleModelViewModel);
        Task<IVehicleModelViewModel> GetVehicleModel(int id);
        Task<PagedList<IVehicleModelViewModel>> GetVehicleModels(VehicleModelFilterParams parameters);
        Task InsertVehicleModel(ICreateVehicleModelViewModel vehicleModel);
        Task UpdateVehicleModel(IEditVehicleModelViewModel editVehicleModelViewModel);
    }
}