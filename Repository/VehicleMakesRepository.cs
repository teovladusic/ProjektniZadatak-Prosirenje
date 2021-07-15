using Common;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class VehicleMakesRepository : Repository<VehicleMake>, IVehicleMakesRepository
    {
        private readonly ApplicationDbContext _context;

        public VehicleMakesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<VehicleMake>> GetAll(IVehicleMakeFilterParams parameters)
        {
            var entites = from v in _context.VehicleMakes select v;

            return await entites.ToListAsync();
        }
    }
}
