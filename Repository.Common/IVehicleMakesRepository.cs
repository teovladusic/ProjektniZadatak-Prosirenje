using Common;
using DAL.Models;
using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Common
{
    public interface IVehicleMakesRepository : IRepository<VehicleMake>
    {
        Task<PagedList<VehicleMake>> GetAll(VehicleMakeFilterParams parameters);
    }
}
