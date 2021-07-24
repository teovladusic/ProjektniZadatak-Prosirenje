using AutoMapper;
using Common;
using DAL.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Model;
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

            var list = new List<VehicleModel> { new VehicleModel() };
            var parameters = new VehicleModelParams();
            var pagingParams = new PagingParams(parameters.PageNumber, parameters.PageSize);
            var sortParams = new SortParams(parameters.OrderBy);
            var filterParams = new VehicleModelFilterParams(parameters.SearchQuery, parameters.MakeName);

            var pagedModels = new PagedList<VehicleModel>(list, 1,
                pagingParams.CurrentPage, pagingParams.PageSize);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.GetAll(sortParams, pagingParams, filterParams))
                .ReturnsAsync(pagedModels);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
              mapperStub.Object, loggerStub.Object);

            var result = await service.GetVehicleModels(sortParams, pagingParams, filterParams);

            result.Should().BeEquivalentTo(
                result,
                options => options.ComparingByMembers<IPagedList<VehicleModel>>());
        }

        [Fact]
        public async Task GetVehicleModel_WithValidId_ReturnsViewModel()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            int id = 1;

            var model = new VehicleModel();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.GetById(id))
                .ReturnsAsync(model);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
              mapperStub.Object, loggerStub.Object);

            var result = await service.GetVehicleModel(id);

            result.Should().BeEquivalentTo(
                model,
                options => options.ComparingByMembers<VehicleModel>());
        }

        [Fact]
        public async Task GetVehicleModel_WithInvalidId_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            int id = 1;

            var returnedVehicleModel = (VehicleModel)null;

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.GetById(id))
                .ReturnsAsync(returnedVehicleModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
              mapperStub.Object, loggerStub.Object);

            var result = await service.GetVehicleModel(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Insert_ValidEntry_ReturnInsertedViewModel()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            var modelToInsert = new VehicleModel();
            var insertedModel = new VehicleModel { Id = 1 };

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.Insert(modelToInsert))
                .Returns(insertedModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleModel(modelToInsert);

            result.Should().BeEquivalentTo(
                insertedModel,
                options => options.ComparingByMembers<VehicleModel>());
        }

        [Fact]
        public async Task Insert_InsertedItemIsNull_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            var modelToInsert = new VehicleModel();
            var insertedModel = (VehicleModel)null;

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.Insert(modelToInsert))
                .Returns(insertedModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleModel(modelToInsert);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            var modelToDelete = new VehicleModel();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.Delete(modelToDelete));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.DeleteVehicleModel(modelToDelete);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Update_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            var modelToUpdate = new VehicleModel();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.Update(modelToUpdate));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.UpdateVehicleModel(modelToUpdate);

            result.Should().Be(1);
        }
    }
}
