using Common;
using DAL.Models;
using Model;
using Model.Common;
using Project.Model.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IVehicleModelsService
    {
        
        Task<PagedList<IVehicleModelViewModel>> GetVehicleModels(VehicleModelFilterParams parameters);
        Task<IVehicleModelViewModel> GetVehicleModel(int id);
        Task<VehicleModelViewModel> InsertVehicleModel(ICreateVehicleModelViewModel vehicleModel);
        Task DeleteVehicleModel(IVehicleModelViewModel vehicleModelViewModel);
        Task UpdateVehicleModel(IEditVehicleModelViewModel editVehicleModelViewModel);
    }
}