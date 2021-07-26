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
using Microsoft.Extensions.Logging;
using WebAPI.Controllers;
using Project.Model;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using FluentAssertions;
using Common;
using Model;
using Xunit.Abstractions;
using AutoMapper;
using Model.VehicleModels;
using Model.VehicleMakes;

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
        public async Task Index_WithValidParams_ReturnsAllMakes()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var parameters = new VehicleMakeParams();

            var pagingParams = CommonFactory.CreatePagingParams(parameters.PageNumber, parameters.PageSize);
            var sortParams = CommonFactory.CreateSortParams(parameters.OrderBy);
            var filterParams = CommonFactory.CreateVehicleMakeFilterParams(parameters.SearchQuery);

            var domainModelsList = new List<VehicleMakeDomainModel> { new VehicleMakeDomainModel() };
            var pagedDomainModelsList = CommonFactory.CreatePagedList(domainModelsList, domainModelsList.Count,
                pagingParams.CurrentPage, pagingParams.PageSize);

            vehicleMakesServiceStub.Setup(x => x.GetVehicleMakes(It.IsAny<ISortParams>(), It.IsAny<IPagingParams>(),
                It.IsAny<IVehicleMakeFilterParams>()))
                .ReturnsAsync(pagedDomainModelsList);

            var mapperStub = new Mock<IMapper>();

            var listOfViewModels = new List<VehicleMakeViewModel> { new VehicleMakeViewModel() };
            mapperStub.Setup(mapper => mapper.Map<List<VehicleMakeViewModel>>(pagedDomainModelsList))
                .Returns(listOfViewModels);

            var pagedMakeViewModels =
                CommonFactory.CreatePagedList(listOfViewModels, listOfViewModels.Count,
                pagingParams.CurrentPage, pagingParams.PageSize);


            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Index(parameters) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(
                pagedMakeViewModels,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }

        [Fact]
        public async Task Details_WithUnexistingMake_ReturnsNotFound()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            int itemId = 1;
            vehicleMakesServiceStub.Setup(service => service.GetVehicleMake(itemId))
                .ReturnsAsync((VehicleMakeDomainModel)null);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();
            var mapperStub = new Mock<IMapper>();

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeViewModel>(It.IsAny<VehicleMakeDomainModel>()))
                .Returns(new VehicleMakeViewModel());

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Details(itemId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Details_WithExistingMake_ReturnsOk()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            int itemId = 1;

            var expectedDomainModel = new VehicleMakeDomainModel
            {
                Id = itemId,
                Name = "name",
                Abrv = "abrv"
            };

            var expectedViewModel = new VehicleMakeViewModel
            {
                Id = itemId,
                Name = "name",
                Abrv = "abrv"
            };

            var vehicleMake = new VehicleMake();

            vehicleMakesServiceStub.Setup(service => service.GetVehicleMake(itemId))
                .ReturnsAsync(expectedDomainModel);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();
            var mapperStub = new Mock<IMapper>();

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeViewModel>(expectedDomainModel))
                .Returns(expectedViewModel);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

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

            var mapperStub = new Mock<IMapper>();

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Details(itemId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_WithValidEntry_ReturnsOkAndCreatesItem()
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var createVehicleMakeViewModel = new CreateVehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv"
            };

            var createVehicleMakeDomainModel = new CreateVehicleMakeDomainModel
            {
                Name = "name",
                Abrv = "abrv",
            };

            var id = new Random().Next();

            var insertedDomainModel = new VehicleMakeDomainModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            var insertedViewModel = new VehicleMakeViewModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            vehicleMakesServiceStub.Setup(x => x.InsertVehicleMake(createVehicleMakeDomainModel))
                .ReturnsAsync(insertedDomainModel);

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<CreateVehicleMakeDomainModel>(createVehicleMakeViewModel))
                .Returns(createVehicleMakeDomainModel);

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeViewModel>(insertedDomainModel))
                .Returns(insertedViewModel);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Create(createVehicleMakeViewModel) as CreatedResult;

            result.Value.Should().BeEquivalentTo(
                insertedViewModel,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }


        [Theory]
        [InlineData("", "abrv")]
        [InlineData("", "")]
        [InlineData("name", " ")]
        [InlineData(" ", "abrv")]
        public async Task Create_WithInvalidEntries_ReturnsBadRequest(
            string name, string abrv)
        {
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var createVehicleMakeViewModel = new CreateVehicleMakeViewModel
            {
                Name = name,
                Abrv = abrv
            };

            var createVehicleMakeDomainModel = new CreateVehicleMakeDomainModel
            {
                Name = name,
                Abrv = abrv
            };

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<CreateVehicleMakeDomainModel>(createVehicleMakeViewModel))
                .Returns(createVehicleMakeDomainModel);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();
            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Create(createVehicleMakeViewModel);

            vehicleMakesServiceStub.Verify(service => service.InsertVehicleMake(It.IsAny<CreateVehicleMakeDomainModel>()),
                Times.Never);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Create_WithCreatedMakeNull_ReturnsBadRequest()
        {
            var createVehicleMakeViewModel = new CreateVehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv"
            };

            var createVehicleMakeDomainModel = new CreateVehicleMakeDomainModel
            {
                Name = "name",
                Abrv = "abrv"
            };
            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            vehicleMakesServiceStub.Setup(service => service.InsertVehicleMake(createVehicleMakeDomainModel))
                .ReturnsAsync((VehicleMakeDomainModel)null);

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<CreateVehicleMakeDomainModel>(createVehicleMakeViewModel))
                .Returns(createVehicleMakeDomainModel);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();
            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Create(createVehicleMakeViewModel);

            result.Should().BeOfType<BadRequestResult>();
        }

        
        [Fact]
        public async Task Delete_WithValidId_ReturnsOkAndDeltesItem()
        {
            var id = new Random().Next();

            var vehicleMakeDomainModelToDelete = new VehicleMakeDomainModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            vehicleMakesServiceStub.Setup(service => service.GetVehicleMake(id))
                .ReturnsAsync(vehicleMakeDomainModelToDelete);

            vehicleMakesServiceStub.Setup(service => service.DeleteVehicleMake(vehicleMakeDomainModelToDelete));

            var mapperStub = new Mock<IMapper>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<OkResult>();
        }
        
        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            int? id = null;

            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var mapperStub = new Mock<IMapper>();

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object, 
                mapperStub.Object);

            var result = await controller.Delete(id);

            vehicleMakesServiceStub.Verify(service => service.GetVehicleMake(It.IsAny<int>()),
                Times.Never);
            vehicleMakesServiceStub.Verify(service => service.DeleteVehicleMake(It.IsAny<VehicleMakeDomainModel>()),
                Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task Delete_WithIUnexistingItem_ReturnsNotFound()
        {
            int id = new Random().Next();

            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            vehicleMakesServiceStub.Setup(x => x.GetVehicleMake(id))
                .ReturnsAsync((VehicleMakeDomainModel)null);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();
            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Delete(id);

            vehicleMakesServiceStub.Verify(service => service.DeleteVehicleMake(It.IsAny<VehicleMakeDomainModel>()),
                Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task Edit_WithIUnexistingItem_ReturnsNotFound()
        {
            int id = new Random().Next();

            var editedMakeViewModel = new VehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            var editedDomainMake = new VehicleMakeDomainModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            VehicleMakeDomainModel existingVehicleMakeDomainModel = null;

            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            vehicleMakesServiceStub.Setup(x => x.GetVehicleMake(id))
                .ReturnsAsync(existingVehicleMakeDomainModel);

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleMakeDomainModel>(editedMakeViewModel))
                .Returns(editedDomainMake);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(editedMakeViewModel);

            vehicleMakesServiceStub.Verify(service => service.UpdateVehicleMake(It.IsAny<VehicleMakeDomainModel>()),
                Times.Never);
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
            int id = new Random().Next();

            var editedViewModel = new VehicleMakeViewModel
            {
                Name = Name,
                Abrv = Abrv,
                Id = id
            };

            var editedMakeDomainModel = new VehicleMakeDomainModel
            {
                Name = Name,
                Abrv = Abrv,
                Id = id
            };

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleMakeDomainModel>(editedViewModel))
                .Returns(editedMakeDomainModel);

            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();
            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(editedViewModel);

            vehicleMakesServiceStub.Verify(service => service.GetVehicleMake(It.IsAny<int>()),
                Times.Never);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Edit_WithValidParameters_ReturnsOk()
        {
            int id = new Random().Next();

            var editedMakeViewModel = new VehicleMakeViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            var editedDomainMake = new VehicleMakeDomainModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id
            };

            var existingVehicleMakeDomainModel = new VehicleMakeDomainModel
            {
                Name = "old name",
                Abrv = "old abrv",
                Id = id
            };

            var vehicleMakesServiceStub = new Mock<IVehicleMakesService>();
            vehicleMakesServiceStub.Setup(service => service.GetVehicleMake(id))
                .ReturnsAsync(existingVehicleMakeDomainModel);

            vehicleMakesServiceStub.Setup(service => service.UpdateVehicleMake(editedDomainMake));

            var loggerStub = new Mock<ILogger<VehicleMakesController>>();

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleMakeDomainModel>(editedMakeViewModel))
                .Returns(editedDomainMake);

            var controller = new VehicleMakesController(vehicleMakesServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(editedMakeViewModel) ;

            vehicleMakesServiceStub.Verify(service => service.UpdateVehicleMake(It.IsAny<VehicleMakeDomainModel>()),
                Times.Once);
            result.Should().BeOfType<OkResult>();
        }
    }
}
