using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleMakesController : Controller
    {
        private readonly IVehicleMakesService _vehicleMakesService;
        private readonly ILogger<VehicleMakesController> _logger;

        public VehicleMakesController(IVehicleMakesService vehicleMakesService, ILogger<VehicleMakesController> logger)
        {
            _vehicleMakesService = vehicleMakesService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] VehicleMakeParams vehicleMakeParams)
        {
            var pagingParams = new PagingParams(vehicleMakeParams.PageNumber, vehicleMakeParams.PageSize);
            var sortParams = new SortParams(vehicleMakeParams.OrderBy);

            VehicleMakeFilterParams parameters = new VehicleMakeFilterParams
            {
                PagingParams = pagingParams,
                SortParams = sortParams,
                SearchQuery = vehicleMakeParams.SearchQuery
            };

            var makes = await _vehicleMakesService.GetVehicleMakes(parameters);

            return Ok(makes);
        }
    }
}