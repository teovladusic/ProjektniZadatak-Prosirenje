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
        private readonly ApplicationDbContext _context;
        private readonly ISortHelper<VehicleMake> _sortHelper;
        private DbSet<VehicleMake> _entities;

        public VehicleMakesRepository(ApplicationDbContext dbContext, ISortHelper<VehicleMake> sortHelper)
            : base(dbContext)
        {
            _context = dbContext;
            _sortHelper = sortHelper;
        }

        public async Task<IPagedList<VehicleMake>> GetAll(ISortParams sortParams, IPagingParams pagingParams,
            IVehicleMakeFilterParams vehicleMakeFilterParams)
        {
            IQueryable<VehicleMake> vehicleMakes;

            if (!string.IsNullOrWhiteSpace(vehicleMakeFilterParams.SearchQuery))
            {
                vehicleMakes = _context.VehicleMakes.Where(make => make.Name.Contains(vehicleMakeFilterParams.SearchQuery)
            || make.Abrv.Contains(vehicleMakeFilterParams.SearchQuery));
            }
            else
            {
                vehicleMakes = _context.VehicleMakes.AsQueryable();
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
