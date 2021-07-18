using DAL;
using DAL.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Model.Common;
using Project.Model.Common;
using Microsoft.Extensions.Logging;
using WebAPI.Controllers;
using Project.Model;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using FluentAssertions;
using Common;
using Model;
using Xunit.Abstractions;

namespace WebAPI.Tests
{
    public class VehicleMakesControllerTests
    {
        private readonly ITestOutputHelper output;

        public VehicleMakesControllerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task Details_WithUnexistingMake_ReturnsNotFound()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            int itemId = 1;
            vehicleMakesServiceStub.Setup(service => service.GetVehicleMake(itemId))
                .ReturnsAsync((IVehicleMakeViewModel)null);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Details(itemId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Details_WithExistingMake_ReturnsOk()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            int itemId = 1;

            var expectedViewModel = new VehicleMakeViewModel
            {
                Id = itemId,
                Name = "name",
                Abrv = "abrv"
            };

            vehicleMakesServiceStub.Setup(service => service.GetVehicleMake(itemId))
                .ReturnsAsync(expectedViewModel);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Details(itemId) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(
                expectedViewModel,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }

        [Fact]
        public async Task Details_WithoutId_ReturnsNotFound()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            int? itemId = null;

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Details(itemId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Index_WithValidParams_ReturnsAllMakes()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var parameters = new VehicleMakeParams();
            var list = new List<IVehicleMakeViewModel> { new VehicleMakeViewModel { Name = "name" } };
            var pagingParams = new PagingParams(parameters.PageNumber, parameters.PageSize);
            var sortParams = new SortParams(parameters.OrderBy);
            var filterParams = new VehicleMakeFilterParams(sortParams, pagingParams, parameters.SearchQuery);

            var expectedResult = new PagedList<IVehicleMakeViewModel>(list, 1,
                filterParams.PagingParams.CurrentPage, filterParams.PagingParams.PageSize);

            vehicleMakesServiceStub.Setup(x => x.GetVehicleMakes(It.IsAny<VehicleMakeFilterParams>()))
                .ReturnsAsync(expectedResult);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Index(parameters);

            result.Value.Should().BeEquivalentTo(
                expectedResult,
                options => options.ComparingByMembers<PagedList<IVehicleMakeViewModel>>());
        }

        [Fact]
        public async Task Create_WithValidEntry_ReturnsOkAndCreatesItem()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var createVehicleMakeViewModel = new CreateVehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv"
            };

            var id = new Random().Next();

            var vehicleMakeViewModel = new VehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            vehicleMakesServiceStub.Setup(x => x.InsertVehicleMake(createVehicleMakeViewModel))
                .ReturnsAsync(vehicleMakeViewModel);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Create(createVehicleMakeViewModel) as CreatedResult;

            result.Value.Should().BeEquivalentTo(
                vehicleMakeViewModel,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }


        [Theory]
        [InlineData("", "abrv")]
        [InlineData("name", " ")]
        [InlineData(" ", "abrv")]
        [InlineData("name", "abrv")]
        public async Task Create_WithInvalidEntries_ReturnsBadRequest(
            string Name, string Abrv)
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var createVehicleMakeViewModel = new CreateVehicleMakeViewModel
            {
                Name = Name,
                Abrv = Abrv
            };

            vehicleMakesServiceStub.Setup(x => x.InsertVehicleMake(createVehicleMakeViewModel))
                .ReturnsAsync((VehicleMakeViewModel)null);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Create(createVehicleMakeViewModel);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsOkAndDeltesItem()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var id = new Random().Next();

            var vehicleMakeViewModel = new VehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            vehicleMakesServiceStub.Setup(x => x.GetVehicleMake(id))
                .ReturnsAsync(vehicleMakeViewModel);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            int? id = null;

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_WithIUnexistingItem_ReturnsNotFound()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            int id = new Random().Next();

            vehicleMakesServiceStub.Setup(x => x.GetVehicleMake(It.IsAny<int>()))
                .ReturnsAsync((VehicleMakeViewModel)null);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Edit_WithIUnexistingItem_ReturnsNotFound()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            int id = new Random().Next();

            var editedMake = new VehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            vehicleMakesServiceStub.Setup(x => x.GetVehicleMake(It.IsAny<int>()))
                .ReturnsAsync((VehicleMakeViewModel)null);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Edit(editedMake);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Theory]
        [InlineData(" ", "abrv")]
        [InlineData("name", " ")]
        [InlineData("", "abrv")]
        [InlineData("name", "")]
        public async Task Edit_WithInvalidParameters_ReturnsBadRequest(
            string Name, string Abrv)
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            int id = new Random().Next();

            var makeToEdit = new VehicleMakeViewModel
            {
                Name = Name,
                Abrv = Abrv,
                Id = id
            };

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Edit(makeToEdit);

            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Edit_WithValidParameters_ReturnsOk()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            int id = new Random().Next();

            var existingMake = new VehicleMakeViewModel
            {
                Name = "existingName",
                Abrv = "existingAbrv",
                Id = id
            };

            var editedMake = new VehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            vehicleMakesServiceStub.Setup(x => x.GetVehicleMake(It.IsAny<int>()))
                .ReturnsAsync(existingMake);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object);

            var result = await controller.Edit(editedMake);

            result.Should().BeOfType<OkResult>();
        }
    }
}
