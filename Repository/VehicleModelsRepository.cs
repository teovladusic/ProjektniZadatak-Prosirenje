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

        public async Task<IPagedList<VehicleModel>> GetAll(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleModelFilterParams vehicleModelFilterParams)
        {
            var vehicleModels = _context.VehicleModels.Include(m => m.VehicleMake).AsQueryable();

            if (!string.IsNullOrWhiteSpace(vehicleModelFilterParams.SearchQuery))
            {
                vehicleModels = _context.VehicleModels.Where(model => model.Name.Contains(vehicleModelFilterParams.SearchQuery)
            || model.Abrv.Contains(vehicleModelFilterParams.SearchQuery)).Include(m => m.VehicleMake);
            }

            if (!string.IsNullOrEmpty(vehicleModelFilterParams.MakeName))
            {
                var make = _context.VehicleMakes.Where(make => make.Name == vehicleModelFilterParams.MakeName).FirstOrDefault();
                vehicleModels = vehicleModels.Where(model => model.VehicleMakeId == make.Id).Include(m => m.VehicleMake);
            }

            var sortedModels = _sortHelper.ApplySort(vehicleModels, sortParams.OrderBy);

            return await IPagedList<VehicleModel>.ToPagedList(
                sortedModels,
                pagingParams.CurrentPage,
                pagingParams.PageSize,
                sortedModels.Count());
        }

        public async Task<VehicleModel> GetById(int id)
        {
            var model = await _context.VehicleModels.Where(m => m.Id == id).Include(m => m.VehicleMake).FirstOrDefaultAsync();
            return model;
        }
    }
}
