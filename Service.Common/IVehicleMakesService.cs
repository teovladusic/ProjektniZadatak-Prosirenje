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
        Task<PagedList<IVehicleMakeViewModel>> GetVehicleMakes(VehicleMakeFilterParams parameters);
        Task<IVehicleMakeViewModel> GetVehicleMake(int id);
        Task InsertVehicleMake(ICreateVehicleMakeViewModel vehicleMake);
        Task UpdateVehicleMake(IVehicleMakeViewModel vehicleMakeViewModel);
        Task DeleteVehicleMake(IVehicleMakeViewModel vehicleMakeViewModel);
    }
}
