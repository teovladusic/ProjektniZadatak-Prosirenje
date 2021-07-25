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
        private readonly ISortHelper<VehicleModel> _sortHelper;

        public VehicleModelsRepository(ApplicationDbContext context, ISortHelper<VehicleModel> sortHelper) : base(context)
        {
            _sortHelper = sortHelper;
        }

        public VehicleModelsRepository(IUnitOfWork unitOfWork, ISortHelper<VehicleModel> sortHelper) : base(unitOfWork)
        {
            _sortHelper = sortHelper;
        }

        public async Task<IPagedList<VehicleModel>> GetAll(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleModelFilterParams vehicleModelFilterParams)
        {
            var vehicleModels = Context.VehicleModels.Include(m => m.VehicleMake).AsQueryable();

            if (!string.IsNullOrWhiteSpace(vehicleModelFilterParams.SearchQuery))
            {
                vehicleModels = Context.VehicleModels.Where(model => model.Name.Contains(vehicleModelFilterParams.SearchQuery)
            || model.Abrv.Contains(vehicleModelFilterParams.SearchQuery)).Include(m => m.VehicleMake);
            }

            if (!string.IsNullOrEmpty(vehicleModelFilterParams.MakeName))
            {
                var make = Context.VehicleMakes.Where(make => make.Name == vehicleModelFilterParams.MakeName).FirstOrDefault();
                vehicleModels = vehicleModels.Where(model => model.VehicleMakeId == make.Id).Include(m => m.VehicleMake);
            }

            var sortedModels = _sortHelper.ApplySort(vehicleModels, sortParams.OrderBy);

            return await IPagedList<VehicleModel>.ToPagedList(
                sortedModels,
                pagingParams.CurrentPage,
                pagingParams.PageSize,
                sortedModels.Count());
        }

        public virtual async Task<VehicleModel> GetById(int id)
        {
            var model = await Context.VehicleModels.Where(m => m.Id == id).Include(m => m.VehicleMake)
                .AsNoTracking().FirstOrDefaultAsync();
            return model;
        }
    }
}
