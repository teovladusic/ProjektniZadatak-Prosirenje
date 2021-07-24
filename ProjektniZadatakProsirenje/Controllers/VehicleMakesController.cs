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
        private readonly IMapper _mapper;

        public VehicleMakesController(IVehicleMakesService vehicleMakesService, ILogger<VehicleMakesController> logger,
            IMapper mapper)
        {
            _vehicleMakesService = vehicleMakesService;
            _logger = logger;
            _mapper = mapper;
        }


        //GET /VehicleMakes
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] VehicleMakeParams vehicleMakeParams)
        {
            var pagingParams = CommonFactory.CreatePagingParams(vehicleMakeParams.PageNumber, vehicleMakeParams.PageSize);
            var sortParams = CommonFactory.CreateSortParams(vehicleMakeParams.OrderBy);
            var vehicleMakeFilterParams = CommonFactory.CreateVehicleMakeFilterParams(vehicleMakeParams.SearchQuery);

            var pagedMakes = await _vehicleMakesService.GetVehicleMakes(sortParams, pagingParams, vehicleMakeFilterParams);

            var makeViewModels = _mapper.Map<List<VehicleMakeViewModel>>(pagedMakes);

            var pagedMakeViewModels = 
                CommonFactory.CreatePagedList(makeViewModels, pagedMakes.TotalCount,
                pagedMakes.CurrentPage, pagedMakes.PageSize);

            return Ok(pagedMakeViewModels);
        }

        //GET /VehicleMakes/{id}
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleMake = await _vehicleMakesService.GetVehicleMake((int)id);

            if (vehicleMake is null)
            {
                return NotFound();
            }

            var vehicleMakeViewModel = _mapper.Map<VehicleMakeViewModel>(vehicleMake);

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

            var vehicleMake = _mapper.Map<VehicleMake>(createVehicleMakeViewModel);

            var response = await _vehicleMakesService.InsertVehicleMake(vehicleMake);

            if (response is null)
            {
                return BadRequest();
            }

            var vehicleMakeViewModel = _mapper.Map<VehicleMakeViewModel>(response);

            return Created(vehicleMakeViewModel.Id.ToString(), vehicleMakeViewModel);
        }

        //POST /VehicleMakes/Delete/{id}
        [HttpPost("Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var vehicleMake = await _vehicleMakesService.GetVehicleMake((int)id);

            if (vehicleMake == null)
            {
                return NotFound();
            }

            await _vehicleMakesService.DeleteVehicleMake(vehicleMake);

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

            await _vehicleMakesService.UpdateVehicleMake(vehicleMake);
            return Ok();
        }
    }
}