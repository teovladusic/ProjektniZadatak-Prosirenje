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
        IVehicleModelsRepository VehicleModels { get; }
        Task<int> Complete();
        Task<T> AddAsync<T>(T entity) where T : BaseEntity;
    }
}
