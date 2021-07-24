using Common;
using DAL.Models;
using Model;
using Model.Common;
using Project.Model;
using Project.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Common
{
    public interface IVehicleMakesService
    {
        Task<IPagedList<VehicleMake>> GetVehicleMakes(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleMakeFilterParams vehicleMakeFilterParams);
        Task<VehicleMake> GetVehicleMake(int id);
        Task<VehicleMake> InsertVehicleMake(VehicleMake vehicleMake);
        Task<int> DeleteVehicleMake(VehicleMake vehicleMakeViewModel);
        Task<int> UpdateVehicleMake(VehicleMake vehicleMakeViewModel);
    }
}
