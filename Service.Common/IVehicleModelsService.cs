using Common;
using DAL.Models;
using Model.VehicleModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Common
{
    public interface IVehicleModelsService
    {
        Task<IPagedList<VehicleModelDomainModel>> GetVehicleModels(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleModelFilterParams vehicleModelFilterParams);
        Task<VehicleModelDomainModel> GetVehicleModel(int id);
        Task<VehicleModelDomainModel> InsertVehicleModel(CreateVehicleModelDomainModel createVehicleModelDomainModel);
        Task<int> DeleteVehicleModel(int id);
        Task<int> UpdateVehicleModel(VehicleModelDomainModel vehicleModelDomainModel);
    }
}