using Common;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Model.Common;
using Moq;
using Service;
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

            vehicleModelsServiceStub.Setup(service => service.GetVehicleModel(It.IsAny<int>()))
                .ReturnsAsync((IVehicleModelViewModel)null);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

            var result = await controller.Details(itemId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Details_WithExistingModel_ReturnsOk()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            int itemId = 1;

            var expectedViewModel = new VehicleModelViewModel
            {
                Id = itemId,
                Name = "name",
                Abrv = "abrv",
                MakeName = "makeName"
            };

            vehicleModelsServiceStub.Setup(service => service.GetVehicleModel(itemId))
                .ReturnsAsync(expectedViewModel);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

            var result = await controller.Details(itemId) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(
                expectedViewModel,
                options => options.ComparingByMembers<VehicleModelViewModel>());
        }

        [Fact]
        public async Task Details_WithoutId_ReturnsNotFound()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            int? itemId = null;

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

            var result = await controller.Details(itemId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Index_WithValidParams_ReturnsAllModels()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var parameters = new VehicleModelParams();
            var list = new List<IVehicleModelViewModel> { new VehicleModelViewModel() };
            var pagingParams = new PagingParams(parameters.PageNumber, parameters.PageSize);
            var sortParams = new SortParams(parameters.OrderBy);
            var filterParams = new VehicleModelFilterParams(pagingParams, sortParams, parameters.SearchQuery,
                parameters.MakeName);

            var expectedResult = new PagedList<IVehicleModelViewModel>(list, 1,
                filterParams.PagingParams.CurrentPage, filterParams.PagingParams.PageSize);

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModels(It.IsAny<VehicleModelFilterParams>()))
                .ReturnsAsync(expectedResult);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

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

            var createVehicleModelViewModel = new CreateVehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = new Random().Next()
            };

            var id = new Random().Next();

            var vehicleModelViewModel = new VehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id,
                MakeName = "makeName"
            };

            vehicleModelsServiceStub.Setup(x => x.InsertVehicleModel(createVehicleModelViewModel))
                .ReturnsAsync(vehicleModelViewModel);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

            var result = await controller.Create(createVehicleModelViewModel) as CreatedResult;

            result.Value.Should().BeEquivalentTo(
                vehicleModelViewModel,
                options => options.ComparingByMembers<IVehicleModelViewModel>());
        }

        [Theory]
        [InlineData("", "abrv", 0)]
        [InlineData("name", " ", 0)]
        [InlineData(" ", "abrv", 0)]
        [InlineData("name", "abrv", 0)]
        [InlineData("name", "abrv", -1)]
        public async Task Create_WithInvalidEntries_ReturnsBadRequest(
            string Name, string Abrv, int VehicleMakeId)
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var createVehicleModelViewModel = new CreateVehicleModelViewModel
            {
                Name = Name,
                Abrv = Abrv,
                VehicleMakeId = VehicleMakeId
            };

            vehicleModelsServiceStub.Setup(x => x.InsertVehicleModel(createVehicleModelViewModel))
                .ReturnsAsync((VehicleModelViewModel)null);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

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

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(id))
                .ReturnsAsync(vehicleModelViewModel);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            int? id = null;

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_WithIUnexistingItem_ReturnsNotFound()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            int id = new Random().Next();

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(It.IsAny<int>()))
                .ReturnsAsync((VehicleModelViewModel)null);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

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
                .ReturnsAsync((VehicleModelViewModel)null);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

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

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

            var result = await controller.Edit(modelToEdit);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Edit_WithValidParameters_ReturnsOk()
        {
            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            int id = new Random().Next();

            var existingModel = new VehicleModelViewModel
            {
                Name = "existingName",
                Abrv = "existingAbrv",
                Id = id,
                MakeName = "existingMakeName"
            };

            var editedModel = new EditVehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id,
                VehicleMakeId = 1
            };

            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(It.IsAny<int>()))
                .ReturnsAsync(existingModel);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object);

            var result = await controller.Edit(editedModel);

            result.Should().BeOfType<OkResult>();
        }
    }
}
