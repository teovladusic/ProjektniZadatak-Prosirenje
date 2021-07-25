using AutoMapper;
using Common;
using DAL.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Moq;
using Service;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Controllers;
using Xunit;

namespace WebAPI.Tests
{
    public class VehicleModelsControllerTests
    {

        [Fact]
        public async Task Details_WithUnexistingModel_ReturnsNotFound()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            int itemId = 1;

            vehicleModelsServiceStub.Setup(service => service.GetVehicleModel(itemId))
                .ReturnsAsync((VehicleModel)null);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Details(itemId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Details_WithExistingModel_ReturnsOk()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            int itemId = 1;

            var expectedVehicleModel = new VehicleModel
            {
                Id = itemId,
                Name = "name",
                Abrv = "abrv"
            };

            vehicleModelsServiceStub.Setup(service => service.GetVehicleModel(itemId))
                .ReturnsAsync(expectedVehicleModel);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);
            var result = await controller.Details(itemId) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(
                expectedVehicleModel,
                options => options.ComparingByMembers<VehicleModelViewModel>());
        }

        [Fact]
        public async Task Details_WithoutId_ReturnsNotFound()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            int? itemId = null;

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Details(itemId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Index_WithValidParams_ReturnsAllModels()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var parameters = new VehicleModelParams();
            var list = new List<VehicleModel> { new VehicleModel() };
            var pagingParams = new PagingParams(parameters.PageNumber, parameters.PageSize);
            var sortParams = new SortParams(parameters.OrderBy);
            var filterParams = new VehicleModelFilterParams(parameters.SearchQuery, parameters.MakeName);

            var expectedResult = new PagedList<VehicleModel>(list, 1,
                pagingParams.CurrentPage, pagingParams.PageSize);

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModels(sortParams, pagingParams, filterParams))
                .ReturnsAsync(expectedResult);

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Index(parameters) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(
                expectedResult,
                options => options.ComparingByMembers<PagedList<IVehicleModelViewModel>>());
        }

        [Fact]
        public async Task Create_WithValidEntry_ReturnsOkAndCreatesItem()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var vehicleModelToInsert = new VehicleModel();
            var insertedModel = new VehicleModel { Id = 1 };

            var createVehicleModelViewModel = new CreateVehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            vehicleModelsServiceStub.Setup(x => x.InsertVehicleModel(vehicleModelToInsert))
                .ReturnsAsync(insertedModel);

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Create(createVehicleModelViewModel) as CreatedResult;

            result.Value.Should().BeEquivalentTo(
                insertedModel,
                options => options.ComparingByMembers<IVehicleModelViewModel>());
        }

        [Theory]
        [InlineData("", "abrv", 0)]
        [InlineData("name", " ", 0)]
        [InlineData(" ", "abrv", 0)]
        [InlineData("name", "abrv", 0)]
        [InlineData("name", "abrv", -1)]
        public async Task Create_WithInvalidEntries_ReturnsBadRequest(
            string name, string abrv, int vehicleMakeId)
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var createVehicleModelViewModel = new CreateVehicleModelViewModel
            {
                Name = name,
                Abrv = abrv,
                VehicleMakeId = vehicleMakeId
            };

            var vehicleModel = new VehicleModel
            {
                Name = name,
                Abrv = abrv,
                VehicleMakeId = vehicleMakeId
            };

            vehicleModelsServiceStub.Setup(x => x.InsertVehicleModel(vehicleModel))
                .ReturnsAsync((VehicleModel)null);

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Create(createVehicleModelViewModel);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsOkAndDeltesItem()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var id = new Random().Next();

            var vehicleModelViewModel = new VehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id,
                MakeName = "makeName"
            };

            var vehicleModel = new VehicleModel();

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(id))
                .ReturnsAsync(vehicleModel);

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            int? id = null;

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_WithIUnexistingItem_ReturnsNotFound()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            int id = new Random().Next();

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(id))
                .ReturnsAsync((VehicleModel)null);

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Edit_WithIUnexistingItem_ReturnsNotFound()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            int id = new Random().Next();

            var editedModel = new EditVehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id,
                VehicleMakeId = 1
            };

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(It.IsAny<int>()))
                .ReturnsAsync((VehicleModel)null);

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(editedModel);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData(" ", "abrv", 0)]
        [InlineData("name", " ", 0)]
        [InlineData("", "abrv", 0)]
        [InlineData("name", "", 0)]
        [InlineData("name", "abrv", -1)]
        public async Task Edit_WithInvalidParameters_ReturnsBadRequest(
            string Name, string Abrv, int VehicleMakeId)
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            int id = new Random().Next();

            var modelToEdit = new EditVehicleModelViewModel
            {
                Name = Name,
                Abrv = Abrv,
                Id = id,
                VehicleMakeId = VehicleMakeId
            };

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(modelToEdit);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Edit_WithValidParameters_ReturnsOk()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            int id = new Random().Next();

            var editedModel = new EditVehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id,
                VehicleMakeId = 1
            };

            var vehicleModel = new VehicleModel();

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(id))
                .ReturnsAsync(vehicleModel);

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(editedModel);

            result.Should().BeOfType<OkResult>();
        }
    }
}
