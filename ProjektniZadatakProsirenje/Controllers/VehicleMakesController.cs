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
using Model;
using AutoMapper;
using Project.Model;
using Project.Model.Common;

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


        //GET /VehicleMakes
        [HttpGet]
        public async Task<ActionResult<PagedList<IVehicleMakeViewModel>>> Index([FromQuery] VehicleMakeParams vehicleMakeParams)
        {
            var pagingParams = new PagingParams(vehicleMakeParams.PageNumber, vehicleMakeParams.PageSize);
            var sortParams = new SortParams(vehicleMakeParams.OrderBy);

            var parameters = new VehicleMakeFilterParams(sortParams, pagingParams, vehicleMakeParams.SearchQuery);

            var makes = await _vehicleMakesService.GetVehicleMakes(parameters);

            return makes;
        }

        //GET /VehicleMakes/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleMakeViewModel = await _vehicleMakesService.GetVehicleMake((int)id);

            if (vehicleMakeViewModel is null)
            {
                return NotFound();
            }

            return Ok(vehicleMakeViewModel);
        }

        //POST /VehicleMakes/Create
        [HttpPost("Create/")]
        public async Task<IActionResult> Create([Bind("Name,Abrv")] CreateVehicleMakeViewModel createVehicleMakeViewModel)
        {
            if (string.IsNullOrEmpty(createVehicleMakeViewModel.Name.Trim()) ||
                string.IsNullOrEmpty(createVehicleMakeViewModel.Abrv.Trim()))
            {
                return BadRequest();
            }

            var response = await _vehicleMakesService.InsertVehicleMake(createVehicleMakeViewModel);

            if (response is null)
            {
                return BadRequest();
            }

            return Created(response.Id.ToString(), response);
        }

        //POST /VehicleMakes/Delete/{id}
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleMakeViewModel = await _vehicleMakesService.GetVehicleMake((int)id);

            if (vehicleMakeViewModel == null)
            {
                return NotFound();
            }

            await _vehicleMakesService.DeleteVehicleMake(vehicleMakeViewModel);

            return Ok();
        }

        [HttpPost("Edit/")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Abrv")] VehicleMakeViewModel vehicleMakeViewModel)
        {
            if (string.IsNullOrEmpty(vehicleMakeViewModel.Name.Trim()) ||
                string.IsNullOrEmpty(vehicleMakeViewModel.Abrv.Trim()) ||
                vehicleMakeViewModel.Id < 0)
            {
                return BadRequest();
            }

            var vehicleMake = await _vehicleMakesService.GetVehicleMake(vehicleMakeViewModel.Id);

            if (vehicleMake is null)
            {
                return NotFound();
            }

            await _vehicleMakesService.UpdateVehicleMake(vehicleMakeViewModel);
            return Ok();
        }
    }
}