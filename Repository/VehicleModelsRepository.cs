using Common;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class VehicleModelsRepository : Repository<VehicleModel>, IVehicleModelsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ISortHelper<VehicleModel> _sortHelper;
        private readonly ILogger<VehicleModelsRepository> _logger;

        public VehicleModelsRepository(ApplicationDbContext context, ISortHelper<VehicleModel> sortHelper,
            ILogger<VehicleModelsRepository> logger)
            : base(context)
        {
            _context = context;
            _sortHelper = sortHelper;
            _logger = logger;
        }

        public async Task<PagedList<VehicleModel>> GetAll(VehicleModelFilterParams parameters)
        {
            var vehicleModels = _context.VehicleModels.Include(m => m.VehicleMake).AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                vehicleModels = _context.VehicleModels.Where(model => model.Name.Contains(parameters.SearchQuery)
            || model.Abrv.Contains(parameters.SearchQuery));
            }

            if (!string.IsNullOrEmpty(parameters.MakeName))
            {
                var make = _context.VehicleMakes.Where(make => make.Name == parameters.MakeName).FirstOrDefault();
                vehicleModels = vehicleModels.Where(model => model.VehicleMakeId == make.Id);
            }

            var sortedModels = _sortHelper.ApplySort(vehicleModels, parameters.SortParams.OrderBy);

            return await PagedList<VehicleModel>.ToPagedList(
                sortedModels,
                parameters.PagingParams.CurrentPage,
                parameters.PagingParams.PageSize,
                sortedModels.Count());
        }

        public async Task<VehicleModel> GetById(int id)
        {
            var model = await _context.VehicleModels.Where(m => m.Id == id).Include(m => m.VehicleMake).FirstOrDefaultAsync();
            return model;
        }
    }
}
