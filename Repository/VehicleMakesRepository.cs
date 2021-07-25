using AutoMapper;
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
        private readonly ISortHelper<VehicleMake> _sortHelper;

        public VehicleMakesRepository(IUnitOfWork unitOfWork, ISortHelper<VehicleMake> sortHelper)
            : base(unitOfWork)
        {
            _sortHelper = sortHelper;
        }

        public VehicleMakesRepository(ApplicationDbContext context, ISortHelper<VehicleMake> sortHelper)
            : base(context)
        {
            _sortHelper = sortHelper;
        }

        public async Task<IPagedList<VehicleMake>> GetAll(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleMakeFilterParams vehicleMakeFilterParams)
        {
            IQueryable<VehicleMake> vehicleMakes;

            if (!string.IsNullOrWhiteSpace(vehicleMakeFilterParams.SearchQuery))
            {
                vehicleMakes = Context.VehicleMakes.Where(make => make.Name.Contains(vehicleMakeFilterParams.SearchQuery)
            || make.Abrv.Contains(vehicleMakeFilterParams.SearchQuery));
            }
            else
            {
                vehicleMakes = Context.VehicleMakes.AsQueryable();
            }

            var sortedMakes = _sortHelper.ApplySort(vehicleMakes, sortParams.OrderBy);

            return await IPagedList<VehicleMake>.ToPagedList(
                sortedMakes,
                pagingParams.CurrentPage,
                pagingParams.PageSize,
                sortedMakes.Count());
        }
    }
}
