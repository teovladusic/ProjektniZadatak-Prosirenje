using Common;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Common
{
    public interface IVehicleMakesService
    {
        Task<PagedList<VehicleMake>> GetVehicleMakes(IVehicleMakeFilterParams parameters);
        Task<VehicleMake> GetVehicleMake(int id);
        Task InsertVehicleMake(VehicleMake vehicleMake);
        Task UpdateVehicleMake(VehicleMake vehicleMake);
        Task DeleteVehicleMake(VehicleMake vehicleMake);
    }
}
