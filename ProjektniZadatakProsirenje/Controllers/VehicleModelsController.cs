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
        private readonly ILogger<VehicleModelsController> _logger;

        public VehicleModelsController(IVehicleModelsService vehicleModelsService, ILogger<VehicleModelsController> logger)
        {
            _vehicleModelsService = vehicleModelsService;
            _logger = logger;
        }


        //GET /VehicleModels
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] VehicleModelParams vehicleModelParams)
        {
            var pagingParams = new PagingParams(vehicleModelParams.PageNumber, vehicleModelParams.PageSize);
            var sortParams = new SortParams(vehicleModelParams.OrderBy);

            var parameters = new VehicleModelFilterParams(pagingParams, sortParams, vehicleModelParams.SearchQuery,
                vehicleModelParams.MakeName);

            var models = await _vehicleModelsService.GetVehicleModels(parameters);

            return Ok(models);
        }

        //GET /VehicleModels/{id}
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

        //POST /VehicleModels/Create
        [HttpPost("Create/")]
        public async Task<IActionResult> Create([Bind("Name,Abrv,VehicleMakeId")] CreateVehicleModelViewModel createVehicleModelViewModel)
        {
            if (string.IsNullOrEmpty(createVehicleModelViewModel.Name.Trim()) ||
                string.IsNullOrEmpty(createVehicleModelViewModel.Abrv.Trim()) ||
                createVehicleModelViewModel.VehicleMakeId < 0)
            {
                return BadRequest();
            }

            try
            {
                var response = await _vehicleModelsService.InsertVehicleModel(createVehicleModelViewModel);
                if (response is null)
                {
                    return BadRequest();
                }

                return Created(response.Id.ToString(), response);
            }
            catch (Exception _)
            {
                return BadRequest("Invalid parameters. Check make ID!");
            }
        }

        //POST /VehicleModels/Delete/{id}
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

        [HttpPost("Edit/")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Abrv,VehicleMakeId")] EditVehicleModelViewModel editVehicleModelViewModel)
        {
            if (string.IsNullOrEmpty(editVehicleModelViewModel.Name.Trim()) ||
               string.IsNullOrEmpty(editVehicleModelViewModel.Abrv.Trim()) ||
               editVehicleModelViewModel.VehicleMakeId < 0)
            {
                return BadRequest();
            }

            var modelToEdit = await _vehicleModelsService.GetVehicleModel(editVehicleModelViewModel.Id);

            if (modelToEdit is null)
            {
                return NotFound();
            }

            try
            {
                await _vehicleModelsService.UpdateVehicleModel(editVehicleModelViewModel);

            }
            catch (Exception _)
            {
                return NotFound("An error occurred. Check Id or make Id");
            }

            return Ok();
        }
    }
}
