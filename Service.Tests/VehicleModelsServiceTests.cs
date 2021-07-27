using AutoMapper;
using Common;
using DAL.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Model;
using Model.VehicleModels;
using Moq;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Service.Tests
{
    public class VehicleModelsServiceTests
    {

        [Fact]
        public async Task GetVehicleModels_ValidParameters_ReturnsModels()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();
            var vehicleModelsRepositoryStub = new Mock<IVehicleModelsRepository>();

            var parameters = new VehicleModelParams();
            var pagingParams = CommonFactory.CreatePagingParams(parameters.PageNumber,
                parameters.PageSize);
            var sortParams = CommonFactory.CreateSortParams(parameters.OrderBy);
            var filterParams = CommonFactory.CreateVehicleModelFilterParams(parameters.SearchQuery,
                parameters.MakeName);

            var modelsList = new List<VehicleModel> { new VehicleModel() };
            var pagedModelsList = CommonFactory.CreatePagedList(modelsList, modelsList.Count,
                pagingParams.CurrentPage, pagingParams.PageSize);

            vehicleModelsRepositoryStub.Setup(repo => repo.GetAll(It.IsAny<ISortParams>(),
                It.IsAny<IPagingParams>(), It.IsAny<IVehicleModelFilterParams>()))
                .ReturnsAsync(pagedModelsList);

            var domainModelsList = new List<VehicleModelDomainModel> { new VehicleModelDomainModel() };
            var pagedDomainModelsList = CommonFactory.CreatePagedList(domainModelsList, domainModelsList.Count,
                pagingParams.CurrentPage, pagingParams.PageSize);

            mapperStub.Setup(mapper => mapper.Map<List<VehicleModelDomainModel>>(pagedModelsList))
                .Returns(domainModelsList);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
              loggerStub.Object, mapperStub.Object, vehicleModelsRepositoryStub.Object);

            var result = await service.GetVehicleModels(sortParams, pagingParams, filterParams);

            result.Should().BeEquivalentTo(
                pagedDomainModelsList,
                options => options.ComparingByMembers<IPagedList<VehicleModel>>());
        }

        [Fact]
        public async Task GetVehicleModel_WithValidId_ReturnsViewModel()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();
            var vehicleModelsRepositoryStub = new Mock<IVehicleModelsRepository>();

            int id = 1;

            var vehicleMake = new VehicleMake { Id = 1, Name = "makeName", Abrv = "makeAbrv" };
            var existingModel = new VehicleModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = vehicleMake.Id,
                VehicleMake = vehicleMake
            };

            vehicleModelsRepositoryStub.Setup(repo => repo.GetById(id))
                .ReturnsAsync(existingModel);

            var vehicleModelDomainModel = new VehicleModelDomainModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = vehicleMake.Id,
                VehicleMake = vehicleMake
            };

            mapperStub.Setup(mapper => mapper.Map<VehicleModelDomainModel>(existingModel))
                .Returns(vehicleModelDomainModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
              loggerStub.Object, mapperStub.Object, vehicleModelsRepositoryStub.Object);

            var result = await service.GetVehicleModel(id);

            result.Should().BeEquivalentTo(
                vehicleModelDomainModel,
                options => options.ComparingByMembers<VehicleModel>());
        }

        [Fact]
        public async Task GetVehicleModel_WithInvalidId_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();
            var vehicleModelsRepositoryStub = new Mock<IVehicleModelsRepository>();

            int id = 1;

            var existingModel = (VehicleModel)null;

            vehicleModelsRepositoryStub.Setup(repo => repo.GetById(id))
                .ReturnsAsync(existingModel);

            var vehicleModelDomainModel = (VehicleModelDomainModel)null;

            mapperStub.Setup(mapper => mapper.Map<VehicleModelDomainModel>(existingModel))
                .Returns(vehicleModelDomainModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
              loggerStub.Object, mapperStub.Object, vehicleModelsRepositoryStub.Object);

            var result = await service.GetVehicleModel(id);

            result.Should().BeEquivalentTo(
                vehicleModelDomainModel,
                options => options.ComparingByMembers<VehicleModel>());
        }

        [Fact]
        public async Task Insert_ValidEntry_ReturnInsertedViewModel()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();
            var vehicleModelsRepositoryStub = new Mock<IVehicleModelsRepository>();

            var createVehicleModelDomainModel = new CreateVehicleModelDomainModel
            {
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            var modelToCreate = new VehicleModel
            {
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            mapperStub.Setup(mapper => mapper.Map<VehicleModel>(createVehicleModelDomainModel))
                .Returns(modelToCreate);

            var createdVehicleModel = new VehicleModel
            {
                Id = 842,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            vehicleModelsRepositoryStub.Setup(repo => repo.Insert(modelToCreate))
                .Returns(createdVehicleModel);

            var createdVehicleModelDomainModel = new VehicleModelDomainModel
            {
                Id = 842,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1

            };
            mapperStub.Setup(mapper => mapper.Map<VehicleModelDomainModel>(createdVehicleModel))
                .Returns(createdVehicleModelDomainModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
             loggerStub.Object, mapperStub.Object, vehicleModelsRepositoryStub.Object);

            var result = await service.InsertVehicleModel(createVehicleModelDomainModel);

            result.Should().BeEquivalentTo(
                createdVehicleModelDomainModel,
                options => options.ComparingByMembers<VehicleModel>());
        }

        [Fact]
        public async Task Insert_InsertedItemIsNull_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();
            var vehicleModelsRepositoryStub = new Mock<IVehicleModelsRepository>();

            var createVehicleModelDomainModel = new CreateVehicleModelDomainModel
            {
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            var modelToCreate = new VehicleModel
            {
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = 1
            };

            mapperStub.Setup(mapper => mapper.Map<VehicleModel>(createVehicleModelDomainModel))
                .Returns(modelToCreate);

            var createdVehicleModel = (VehicleModel)null;

            vehicleModelsRepositoryStub.Setup(repo => repo.Insert(modelToCreate))
                .Returns(createdVehicleModel);

            var createdVehicleModelDomainModel = (VehicleModelDomainModel)null;

            mapperStub.Setup(mapper => mapper.Map<VehicleModelDomainModel>(createdVehicleModel))
                .Returns(createdVehicleModelDomainModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
             loggerStub.Object, mapperStub.Object, vehicleModelsRepositoryStub.Object);

            var result = await service.InsertVehicleModel(createVehicleModelDomainModel);

            result.Should().BeEquivalentTo(
                createdVehicleModelDomainModel,
                options => options.ComparingByMembers<VehicleModel>());
        }

        [Fact]
        public async Task Delete_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();
            var vehicleModelsRepositoryStub = new Mock<IVehicleModelsRepository>();

            var idToDelete = 1;

            vehicleModelsRepositoryStub.Setup(repo => repo.Delete(idToDelete));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
             loggerStub.Object, mapperStub.Object, vehicleModelsRepositoryStub.Object);

            var result = await service.DeleteVehicleModel(idToDelete);

            vehicleModelsRepositoryStub.Verify(repo => repo.Delete(It.IsAny<int>()),
                Times.Once);
            result.Should().Be(1);
        }

        [Fact]
        public async Task Update_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();
            var vehicleModelsRepositoryStub = new Mock<IVehicleModelsRepository>();

            var vehicleMake = new VehicleMake { Id = 1, Name = "makeName", Abrv = "makeAbrv" };
            var vehicleModelDomainModel = new VehicleModelDomainModel
            {
                Id = 1,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = vehicleMake.Id,
                VehicleMake = vehicleMake
            };

            var vehicleModel = new VehicleModel
            {
                Id = 1,
                Name = "name",
                Abrv = "abrv",
                VehicleMakeId = vehicleMake.Id,
                VehicleMake = vehicleMake
            };

            mapperStub.Setup(mapper => mapper.Map<VehicleModel>(vehicleModelDomainModel))
                .Returns(vehicleModel);

            vehicleModelsRepositoryStub.Setup(repo => repo.Update(vehicleModel));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
            loggerStub.Object, mapperStub.Object, vehicleModelsRepositoryStub.Object);

            var result = await service.UpdateVehicleModel(vehicleModelDomainModel);

            vehicleModelsRepositoryStub.Verify(repo => repo.Update(It.IsAny<VehicleModel>()),
                Times.Once);
            result.Should().Be(1);
        }
    }
}
