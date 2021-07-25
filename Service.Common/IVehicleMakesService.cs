using Common;
using DAL.Models;
using Model;
using Model.VehicleMakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Common
{
    public interface IVehicleMakesService
    {
        Task<IPagedList<VehicleMakeDomainModel>> GetVehicleMakes(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleMakeFilterParams vehicleMakeFilterParams);
        Task<VehicleMakeDomainModel> GetVehicleMake(int id);
        Task<VehicleMakeDomainModel> InsertVehicleMake(CreateVehicleMakeDomainModel vehicleMakeDomainModel);
        Task<int> DeleteVehicleMake(VehicleMakeDomainModel vehicleMakeDomainModel);
        Task<int> UpdateVehicleMake(VehicleMakeDomainModel vehicleMakeDomainModel);
    }
}
