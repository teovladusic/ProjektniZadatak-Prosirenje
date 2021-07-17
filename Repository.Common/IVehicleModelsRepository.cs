using Common;
using DAL.Models;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Common
{
    public interface IVehicleModelsRepository : IRepository<VehicleModel>
    {
        Task<PagedList<VehicleModel>> GetAll(VehicleModelFilterParams parameters);

        Task<VehicleModel> GetById(int id);
    }
}
