using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Service;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleModelsController : Controller
    {
        private readonly IVehicleModelsService _vehicleModelsService;
        private readonly ILogger<VehicleMakesController> _logger;

        public VehicleModelsController(IVehicleModelsService vehicleModelsService, ILogger<VehicleMakesController> logger)
        {
            _vehicleModelsService = vehicleModelsService;
            _logger = logger;
        }


        //GET /VehicleMakes
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] VehicleModelParams vehicleMakeParams)
        {
            var pagingParams = new PagingParams(vehicleMakeParams.PageNumber, vehicleMakeParams.PageSize);
            var sortParams = new SortParams(vehicleMakeParams.OrderBy);

            var parameters = new VehicleModelFilterParams(pagingParams, sortParams, vehicleMakeParams.SearchQuery,
                vehicleMakeParams.MakeName);

            var makes = await _vehicleModelsService.GetVehicleModels(parameters);

            return Ok(makes);
        }

        //GET /VehicleMakes/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleModel = await _vehicleModelsService.GetVehicleModel((int)id);

            if (vehicleModel is null)
            {
                return NotFound();
            }

            return Ok(vehicleModel);
        }

        //POST /VehicleMakes/Delete/{id}
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleModelViewModel = await _vehicleModelsService.GetVehicleModel((int)id);

            if (vehicleModelViewModel == null)
            {
                return NotFound();
            }

            await _vehicleModelsService.DeleteVehicleModel(vehicleModelViewModel);

            return Ok();
        }

        //POST /VehicleMakes/Create
        [HttpPost("Create/")]
        public async Task<IActionResult> Create([Bind("Name,Abrv,VehicleMakeId")] CreateVehicleModelViewModel createVehicleMakeViewModel)
        {
            await _vehicleModelsService.InsertVehicleModel(createVehicleMakeViewModel);

            return Ok();
        }

        [HttpPost("Edit/")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Abrv,VehicleMakeId")] EditVehicleModelViewModel editVehicleModelViewModel)
        {
            try
            {
                await _vehicleModelsService.UpdateVehicleModel(editVehicleModelViewModel);

            }
            catch (Exception e)
            {
                return NotFound("An error occurred. Check Id or make Id");
            }
            
            return Ok();
        }
    }
}
