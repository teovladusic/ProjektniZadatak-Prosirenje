using AutoMapper;
using Common;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Project.Model;
using Project.Model.Common;
using Repository.Common;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Service;
using System.Threading.Tasks;
using FluentAssertions;
using Model;

namespace Service.Tests
{
    public class VehicleMakesServiceTests
    {
        [Fact]
        public async Task GetVehicleMakes_ValidParameters_ReturnsMakes()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var list = new List<VehicleMake> { new VehicleMake() };
            var parameters = new VehicleMakeParams();
            var pagingParams = new PagingParams(parameters.PageNumber, parameters.PageSize);
            var sortParams = new SortParams(parameters.OrderBy);
            var filterParams = new VehicleMakeFilterParams(sortParams, pagingParams, parameters.SearchQuery);

            var pagedMakes = new PagedList<VehicleMake>(list, 1,
                filterParams.PagingParams.CurrentPage, filterParams.PagingParams.PageSize);

            unitOfWorkStub.Setup(x => x.VehicleMakes.GetAll(It.IsAny<VehicleMakeFilterParams>()))
                .ReturnsAsync(pagedMakes);

            var mappedObjects = new List<VehicleMakeViewModel> { new VehicleMakeViewModel() };

            mapperStub.Setup(x => x.Map<List<VehicleMakeViewModel>>(It.IsAny<PagedList<VehicleMake>>()))
                .Returns(mappedObjects);

            VehicleMakesService vehicleMakesService = new(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await vehicleMakesService.GetVehicleMakes(filterParams);

            result.Should().BeEquivalentTo(
                result,
                options => options.ComparingByMembers<IVehicleMakeViewModel>());
        }

        [Fact]
        public async Task GetVehicleMake_WithExistingItem_ReturnsMake()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            int id = 1;

            var expectedMake = new VehicleMake();
            var expectedViewModel = new VehicleMakeViewModel();

            unitOfWorkStub.Setup(x => x.VehicleMakes.GetById(It.IsAny<int>()))
                .ReturnsAsync(expectedMake);

            mapperStub.Setup(x => x.Map<VehicleMakeViewModel>(expectedMake))
                .Returns(expectedViewModel);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.GetVehicleMake(id);

            result.Should().BeEquivalentTo(
                expectedViewModel,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }

        [Fact]
        public async Task GetVehicleMake_WithUnExistingItem_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            int id = 1;

            unitOfWorkStub.Setup(x => x.VehicleMakes.GetById(It.IsAny<int>()))
                .ReturnsAsync((VehicleMake)null);

            mapperStub.Setup(x => x.Map<VehicleMakeViewModel>(It.IsAny<VehicleMake>()))
                .Returns((VehicleMakeViewModel)null);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.GetVehicleMake(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Insert_ValidEntry_ReturnInsertedViewModel()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var viewModelToInsert = new CreateVehicleMakeViewModel();
            var makeToInsert = new VehicleMake();
            var insertedViewModel = new VehicleMakeViewModel();

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(viewModelToInsert))
                .Returns(makeToInsert);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes.Insert(makeToInsert))
                .Returns(makeToInsert);

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeViewModel>(makeToInsert))
                .Returns(insertedViewModel);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleMake(viewModelToInsert);

            result.Should().BeEquivalentTo(
                insertedViewModel,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }

        [Fact]
        public async Task Insert_InsertedItemIsNull_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var viewModelToInsert = new CreateVehicleMakeViewModel();
            var makeToInsert = new VehicleMake();
            var insertedViewModel = new VehicleMakeViewModel();

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(viewModelToInsert))
                .Returns(makeToInsert);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes.Insert(makeToInsert))
                .Returns((VehicleMake)null);

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeViewModel>(null))
                .Returns((VehicleMakeViewModel)null);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleMake(viewModelToInsert);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var viewModelToDelete = new VehicleMakeViewModel();
            var makeToDelete = new VehicleMake();

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(viewModelToDelete))
                .Returns(makeToDelete);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes.Delete(It.IsAny<VehicleMake>()));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.DeleteVehicleMake(viewModelToDelete);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Update_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var viewModelToUpdate = new VehicleMakeViewModel();
            var makeToUpdate = new VehicleMake();

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(viewModelToUpdate))
                .Returns(makeToUpdate);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes.Update(makeToUpdate));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.UpdateVehicleMake(viewModelToUpdate);

            result.Should().Be(1);
        }
    }
}
