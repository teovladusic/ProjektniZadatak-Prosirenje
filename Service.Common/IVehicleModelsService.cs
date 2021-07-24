using Common;
using DAL.Models;
using Model;
using Model.Common;
using Project.Model.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Common
{
    public interface IVehicleModelsService
    {
        Task<IPagedList<VehicleModel>> GetVehicleModels(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleModelFilterParams vehicleModelFilterParams);
        Task<VehicleModel> GetVehicleModel(int id);
        Task<VehicleModel> InsertVehicleModel(VehicleModel vehicleModel);
        Task<int> DeleteVehicleModel(VehicleModel vehicleModel);
        Task<int> UpdateVehicleModel(VehicleModel vehicleModel);
    }
}