using DAL;
using DAL.Models;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            VehicleMakes = new VehicleMakesRepository(context);
        }
        public IVehicleMakesRepository VehicleMakes { get; }

        public IRepository<VehicleModel> VehicleModels { get; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
