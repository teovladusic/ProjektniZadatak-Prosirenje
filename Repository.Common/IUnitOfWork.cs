using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Common
{
    public interface IUnitOfWork
    {
        IVehicleMakesRepository VehicleMakes { get; }
        IRepository<VehicleModel> VehicleModels { get; }
        //IRepository<VehicleMake> VehicleMakes { get; }
        Task<int> Complete();
    }
}
