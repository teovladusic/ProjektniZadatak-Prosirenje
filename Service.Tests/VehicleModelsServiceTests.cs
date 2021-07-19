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
            var filterParams = new VehicleModelFilterParams(pagingParams, sortParams,
                parameters.SearchQuery, parameters.MakeName);

            var pagedModels = new PagedList<VehicleModel>(list, 1,
                filterParams.PagingParams.CurrentPage, filterParams.PagingParams.PageSize);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.GetAll(filterParams))
                .ReturnsAsync(pagedModels);

            var mappedObjects = new List<VehicleModelViewModel> { new VehicleModelViewModel() };

            mapperStub.Setup(mapper => mapper.Map<List<VehicleModelViewModel>>(It.IsAny<PagedList<VehicleModel>>()))
                .Returns(mappedObjects);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
              mapperStub.Object, loggerStub.Object);

            var result = await service.GetVehicleModels(filterParams);

            result.Should().BeEquivalentTo(
                result,
                options => options.ComparingByMembers<VehicleModelViewModel>());
        }

        [Fact]
        public async Task GetVehicleModel_WithValidId_ReturnsViewModel()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            int id = 1;

            var model = new VehicleModel();
            var viewModel = new VehicleModelViewModel();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.GetById(id))
                .ReturnsAsync(model);

            mapperStub.Setup(mapper => mapper.Map<VehicleModelViewModel>(model))
                .Returns(viewModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
              mapperStub.Object, loggerStub.Object);

            var result = await service.GetVehicleModel(id);

            result.Should().BeEquivalentTo(
                viewModel,
                options => options.ComparingByMembers<VehicleModelViewModel>());
        }

        [Fact]
        public async Task GetVehicleModel_WithInvalidId_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            int id = 1;

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.GetById(id))
                .ReturnsAsync((VehicleModel)null);

            mapperStub.Setup(mapper => mapper.Map<VehicleModelViewModel>(null))
                .Returns((VehicleModelViewModel)null);

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

            var viewModelToInsert = new CreateVehicleModelViewModel();
            var modelToInsert = new VehicleModel();
            var insertedViewModel = new VehicleModelViewModel();

            mapperStub.Setup(mapper => mapper.Map<VehicleModel>(viewModelToInsert))
                .Returns(modelToInsert);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.Insert(modelToInsert))
                .Returns(modelToInsert);

            mapperStub.Setup(mapper => mapper.Map<VehicleModelViewModel>(modelToInsert))
                .Returns(insertedViewModel);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleModel(viewModelToInsert);

            result.Should().BeEquivalentTo(
                insertedViewModel,
                options => options.ComparingByMembers<VehicleModelViewModel>());
        }

        [Fact]
        public async Task Insert_InsertedItemIsNull_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            var viewModelToInsert = new CreateVehicleModelViewModel();
            var modelToInsert = new VehicleModel();
            var insertedViewModel = new VehicleModelViewModel();

            mapperStub.Setup(mapper => mapper.Map<VehicleModel>(viewModelToInsert))
                .Returns(modelToInsert);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.Insert(modelToInsert))
                .Returns((VehicleModel)null);

            mapperStub.Setup(mapper => mapper.Map<VehicleModelViewModel>(null))
                .Returns((VehicleModelViewModel)null);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleModel(viewModelToInsert);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            var viewModelToDelete = new VehicleModelViewModel();
            var modelToDelete = new VehicleModel();

            mapperStub.Setup(mapper => mapper.Map<VehicleModel>(viewModelToDelete))
                .Returns(modelToDelete);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.Delete(It.IsAny<VehicleModel>()));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.DeleteVehicleModel(viewModelToDelete);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Update_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleModelsService>>();

            var viewModelToUpdate = new EditVehicleModelViewModel();
            var modelToUpdate = new VehicleModel();

            mapperStub.Setup(mapper => mapper.Map<VehicleModel>(viewModelToUpdate))
                .Returns(modelToUpdate);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleModels.Update(modelToUpdate));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleModelsService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.UpdateVehicleModel(viewModelToUpdate);

            result.Should().Be(1);
        }
    }
}
