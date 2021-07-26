using AutoMapper;
using Common;
using DAL.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model;
using Model.VehicleModels;
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
        public async Task Index_WithValidParams_ReturnsAllModels()
        {
            var parameters = new VehicleModelParams();
            var sortParams = CommonFactory.CreateSortParams(parameters.OrderBy);
            var pagingParams = CommonFactory.CreatePagingParams(parameters.PageNumber, parameters.PageSize);
            var filterParams = CommonFactory.CreateVehicleModelFilterParams(parameters.SearchQuery,
                parameters.MakeName);

            var domainModelsList = new List<VehicleModelDomainModel> { new VehicleModelDomainModel() };
            var pagedDomainModels = new PagedList<VehicleModelDomainModel>(domainModelsList, 1,
                pagingParams.CurrentPage, pagingParams.PageSize);

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            vehicleModelsServiceStub.Setup(x => x.GetVehicleModels(It.IsAny<ISortParams>(), It.IsAny<IPagingParams>(),
                It.IsAny<IVehicleModelFilterParams>()))
                .ReturnsAsync(pagedDomainModels);

            var viewModelsList = new List<VehicleModelViewModel> { new VehicleModelViewModel() };
            var pagedViewModels = new PagedList<VehicleModelViewModel>(viewModelsList, 1,
                pagingParams.CurrentPage, pagingParams.PageSize);

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<List<VehicleModelViewModel>>(pagedDomainModels))
                .Returns(viewModelsList);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Index(parameters) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(
                pagedViewModels,
                options => options.ComparingByMembers<PagedList<VehicleModelViewModel>>());
        }
        
        [Fact]
        public async Task Details_WithUnexistingModel_ReturnsNotFound()
        {
            int itemId = new Random().Next();

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            vehicleModelsServiceStub.Setup(service => service.GetVehicleModel(itemId))
                .ReturnsAsync((VehicleModelDomainModel)null);

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
            int itemId = 1;

            var vehicleMake = new VehicleMake { Name = "makeName" };
            var vehicleModelDomainModel = new VehicleModelDomainModel
            {
                Id = itemId,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1,
                VehicleMake = vehicleMake
            };

            var vehicleModelViewModel = new VehicleModelViewModel
            {
                Id = itemId,
                Name = "name",
                Abrv = "abrv",
                MakeName = vehicleMake.Name
            };

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            vehicleModelsServiceStub.Setup(service => service.GetVehicleModel(itemId))
                .ReturnsAsync(vehicleModelDomainModel);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleModelViewModel>(vehicleModelDomainModel))
                .Returns(vehicleModelViewModel);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);
            var result = await controller.Details(itemId) as OkObjectResult;

            result.Value.Should().BeEquivalentTo(
                vehicleModelViewModel,
                options => options.ComparingByMembers<VehicleModelViewModel>());
        }
        
        [Fact]
        public async Task Details_WithoutId_ReturnsNotFound()
        {
            int? itemId = null;

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Details(itemId);

            vehicleModelsServiceStub.Verify(service => service.GetVehicleModel(It.IsAny<int>()),
                Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task Create_WithValidEntry_ReturnsOkAndCreatesItem()
        {
            var createVehicleModelViewModel = new CreateVehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            var createVehicleModelDomainModel = new CreateVehicleModelDomainModel
            {
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            var vehicleMake = new VehicleMake
            {
                Id = 1,
                Name = "makeName",
                Abrv = "makeAbrv"
            };
            var createdVehicleModelDomainModel = new VehicleModelDomainModel
            {
                Id = 1,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1,
                VehicleMake = vehicleMake
            };
            var createdVehicleModelViewModel = new VehicleModelViewModel
            {
                Id = 1,
                Name = "name",
                Abrv = "abrv",
                MakeName = vehicleMake.Name
            };

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            vehicleModelsServiceStub.Setup(x => x.InsertVehicleModel(createVehicleModelDomainModel))
                .ReturnsAsync(createdVehicleModelDomainModel);

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<CreateVehicleModelDomainModel>(createVehicleModelViewModel))
                .Returns(createVehicleModelDomainModel);

            mapperStub.Setup(mapper => mapper.Map<VehicleModelViewModel>(createdVehicleModelDomainModel))
                .Returns(createdVehicleModelViewModel);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Create(createVehicleModelViewModel) as CreatedResult;

            result.Value.Should().BeEquivalentTo(
                createdVehicleModelViewModel,
                options => options.ComparingByMembers<VehicleModelViewModel>());
        }
        
        [Theory]
        [InlineData("", "abrv", 1)]
        [InlineData("name", " ", 1)]
        [InlineData(" ", "abrv", 1)]
        [InlineData("name", "abrv", -1)]
        [InlineData("", "", -1)]
        public async Task Create_WithInvalidEntries_ReturnsBadRequest(
            string name, string abrv, int vehicleMakeId)
        {
            var createVehicleModelViewModel = new CreateVehicleModelViewModel
            {
                Name = name,
                Abrv = abrv,
                VehicleMakeId = vehicleMakeId
            };

            var createVehicleModelDomainModel = new CreateVehicleModelDomainModel
            {
                Name = name,
                Abrv = abrv,
                VehicleMakeId = vehicleMakeId
            };

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<CreateVehicleModelDomainModel>(createVehicleModelViewModel))
                .Returns(createVehicleModelDomainModel);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Create(createVehicleModelViewModel);

            vehicleModelsServiceStub.Verify(service => service.InsertVehicleModel(It.IsAny<CreateVehicleModelDomainModel>()),
                Times.Never);
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsOkAndDeltesItem()
        {
            var id = new Random().Next();

            var vehicleMake = new VehicleMake { Id = 1, Name = "makeName", Abrv = "makeAbrv" };
            var vehicleModelDomainModel = new VehicleModelDomainModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id,
                VehicleMake = vehicleMake,
                VehicleMakeId = vehicleMake.Id
            };

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            vehicleModelsServiceStub.Setup(service => service.GetVehicleModel(id))
                .ReturnsAsync(vehicleModelDomainModel);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var mapperStub = new Mock<IMapper>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Delete(id);

            result.Should().BeOfType<OkResult>();
        }
        
        [Fact]
        public async Task Delete_WithInvalidId_ReturnsNotFound()
        {
            int? id = null;

            var mapperStub = new Mock<IMapper>();

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Delete(id);

            vehicleModelsServiceStub.Verify(service => service.GetVehicleModel(It.IsAny<int>()),
                Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task Delete_WithIUnexistingItem_ReturnsNotFound()
        {
            int id = new Random().Next();

            var domainModelToDelete = (VehicleModelDomainModel)null;

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(id))
                .ReturnsAsync(domainModelToDelete);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var mapperStub = new Mock<IMapper>();
            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Delete(id);

            vehicleModelsServiceStub.Verify(service => service.DeleteVehicleModel(It.IsAny<int>()),
                Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public async Task Edit_WithIUnexistingItem_ReturnsNotFound()
        {
            int id = new Random().Next();

            var editedViewModel = new EditVehicleModelViewModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            var vehicleMake = new VehicleMake { Id = id, Name = "makeName", Abrv = "makeAbrv" };
            var editedDomainModel = new VehicleModelDomainModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1,
                VehicleMake = vehicleMake
            };

            var domainModelToEdit = (VehicleModelDomainModel)null;

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(id))
                .ReturnsAsync(domainModelToEdit);

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();
            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleModelDomainModel>(editedViewModel))
                .Returns(editedDomainModel);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(editedViewModel);

            vehicleModelsServiceStub.Verify(service => service.UpdateVehicleModel(It.IsAny<VehicleModelDomainModel>()),
                Times.Never);
            result.Should().BeOfType<NotFoundResult>();
        }
        
        [Theory]
        [InlineData(" ", "abrv", 1)]
        [InlineData("name", " ", 1)]
        [InlineData("", "abrv", 1)]
        [InlineData("name", "", 1)]
        [InlineData("name", "abrv", -1)]
        [InlineData("", "", -1)]
        [InlineData("", "", 1)]
        public async Task Edit_WithInvalidParameters_ReturnsBadRequest(
            string name, string abrv, int vehicleMakeId)
        {
            int id = new Random().Next();

            var editedViewModel = new EditVehicleModelViewModel
            {
                Name = name,
                Abrv = abrv,
                Id = id,
                VehicleMakeId = vehicleMakeId
            };

            var vehicleMake = new VehicleMake { Id = vehicleMakeId, Name = "makeName", Abrv = "makeAbrv" };
            var editedDomainModel = new VehicleModelDomainModel
            {
                Name = name,
                Abrv = abrv,
                Id = id,
                VehicleMakeId = vehicleMakeId,
                VehicleMake = vehicleMake
            };

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleModelDomainModel>(editedViewModel))
                .Returns(editedDomainModel);

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(editedViewModel);

            vehicleModelsServiceStub.Verify(service => service.GetVehicleModel(It.IsAny<int>()),
                Times.Never);
            result.Should().BeOfType<BadRequestResult>();
        }
    
        [Fact]
        public async Task Edit_WithValidParameters_ReturnsOk()
        {
            int id = new Random().Next();
            var vehicleMakeId = 1;
            var editedViewModel = new EditVehicleModelViewModel
            {
                Name = "name",
                Abrv = "abrv",
                Id = id,
                VehicleMakeId = vehicleMakeId
            };

            var vehicleMake = new VehicleMake { Id = vehicleMakeId, Name = "makeName", Abrv = "makeAbrv" };
            var editedDomainModel = new VehicleModelDomainModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = vehicleMakeId,
                VehicleMake = vehicleMake
            };

            var existingVehicleMakeId = 83;
            var existingVehicleMake = new VehicleMake { 
                Id = existingVehicleMakeId,
                Name = "existingMakeName", 
                Abrv = "existingMakeAbrv" 
            };
            var existingVehicleModelDomainModel = new VehicleModelDomainModel
            {
                Id = 2,
                Name = "existingName",
                Abrv = "existingAbrv",
                VehicleMakeId = existingVehicleMakeId
            };

            var loggerStub = new Mock<ILogger<VehicleModelsController>>();

            var vehicleModelsServiceStub = new Mock<IVehicleModelsService>();
            vehicleModelsServiceStub.Setup(x => x.GetVehicleModel(id))
                .ReturnsAsync(existingVehicleModelDomainModel);

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleModelDomainModel>(editedViewModel))
                .Returns(editedDomainModel);

            var controller = new VehicleModelsController(vehicleModelsServiceStub.Object, loggerStub.Object,
                mapperStub.Object);

            var result = await controller.Edit(editedViewModel);

            vehicleModelsServiceStub.Verify(service => service.UpdateVehicleModel(editedDomainModel),
                Times.Once);
            result.Should().BeOfType<OkResult>();
        }
    }
}
