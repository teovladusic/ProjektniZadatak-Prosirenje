using AutoMapper;
using Common;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Project.Model;
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
        private readonly IApplicationDbContext _context;
        private readonly ISortHelper<VehicleMake> _sortHelper;

        public VehicleMakesRepository(IApplicationDbContext dbContext, ISortHelper<VehicleMake> sortHelper) 
            : base(dbContext)
        {
            _context = dbContext;
            _sortHelper = sortHelper;
        }

        public async Task<PagedList<VehicleMake>> GetAll(VehicleMakeFilterParams parameters)
        {
            var vehicleMakes = (from v in _context.VehicleMakes select v).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))
            {
                vehicleMakes = _context.VehicleMakes.Where(make => make.Name.Contains(parameters.SearchQuery)
            || make.Abrv.Contains(parameters.SearchQuery));
            }

            var sortedMakes = _sortHelper.ApplySort(vehicleMakes, parameters.SortParams.OrderBy);

            return await PagedList<VehicleMake>.ToPagedList(
                sortedMakes, 
                parameters.PagingParams.CurrentPage,
                parameters.PagingParams.PageSize,
                sortedMakes.Count());
        }
    }
}
