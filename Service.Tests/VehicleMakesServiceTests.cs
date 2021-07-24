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
            var filterParams = new VehicleMakeFilterParams(parameters.SearchQuery);

            var pagedMakes = new PagedList<VehicleMake>(list, 1,
                pagingParams.CurrentPage, pagingParams.PageSize);

            unitOfWorkStub.Setup(x => x.VehicleMakes.GetAll(sortParams, pagingParams, filterParams))
                .ReturnsAsync(pagedMakes);

            VehicleMakesService vehicleMakesService = new(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await vehicleMakesService.GetVehicleMakes(sortParams, pagingParams, filterParams);

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

            unitOfWorkStub.Setup(x => x.VehicleMakes.GetById(id))
                .ReturnsAsync(expectedMake);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.GetVehicleMake(id);

            result.Should().BeEquivalentTo(
                expectedMake,
                options => options.ComparingByMembers<VehicleMake>());
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

            var makeToInsert = new VehicleMake();
            var insertedMake = new VehicleMake { Id = 1 };

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes.Insert(makeToInsert))
                .Returns(insertedMake);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleMake(makeToInsert);

            result.Should().BeEquivalentTo(
                insertedMake,
                options => options.ComparingByMembers<VehicleMake>());
        }

        [Fact]
        public async Task Insert_InsertedItemIsNull_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var makeToInsert = new VehicleMake();
            var insertedMake = (VehicleMake)null;

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes.Insert(makeToInsert))
                .Returns(insertedMake);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleMake(makeToInsert);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var makeToDelete = new VehicleMake();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes.Delete(makeToDelete));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.DeleteVehicleMake(makeToDelete);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Update_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var makeToUpdate = new VehicleMake();

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(makeToUpdate))
                .Returns(makeToUpdate);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes.Update(makeToUpdate));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var service = new VehicleMakesService(unitOfWorkStub.Object,
               mapperStub.Object, loggerStub.Object);

            var result = await service.UpdateVehicleMake(makeToUpdate);

            result.Should().Be(1);
        }
    }
}
