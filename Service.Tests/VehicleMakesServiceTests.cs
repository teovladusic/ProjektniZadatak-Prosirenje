using AutoMapper;
using Common;
using DAL.Models;
using Microsoft.Extensions.Logging;
using Moq;
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
using Model.VehicleMakes;

namespace Service.Tests
{
    public class VehicleMakesServiceTests
    {
        [Fact]
        public async Task GetVehicleMakes_ValidParameters_ReturnsMakes()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var sortHelperStub = new Mock<ISortHelper<VehicleMake>>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var parameters = new VehicleMakeParams();
            var pagingParams = CommonFactory.CreatePagingParams(parameters.PageNumber, parameters.PageSize);
            var sortParams = CommonFactory.CreateSortParams(parameters.OrderBy);
            var filterParams = CommonFactory.CreateVehicleMakeFilterParams(parameters.SearchQuery);

            var makes = new List<VehicleMake> { new VehicleMake() };
            var pagedMakes = new PagedList<VehicleMake>(makes, makes.Count,
                pagingParams.CurrentPage, pagingParams.PageSize);

            repositoryStub.Setup(repo => repo.GetAll(It.IsAny<ISortParams>(), It.IsAny<IPagingParams>(),
                It.IsAny<IVehicleMakeFilterParams>()))
                .ReturnsAsync(pagedMakes);

            var domainModelsList = new List<VehicleMakeDomainModel> { new VehicleMakeDomainModel() };
            mapperStub.Setup(mapper => mapper.Map<List<VehicleMakeDomainModel>>(pagedMakes))
                .Returns(domainModelsList);

            var pagedDomainModels = CommonFactory.CreatePagedList(domainModelsList, pagedMakes.TotalCount, pagedMakes.CurrentPage,
                pagedMakes.PageSize);

            VehicleMakesService vehicleMakesService = new(unitOfWorkStub.Object,
               loggerStub.Object, sortHelperStub.Object, mapperStub.Object, repositoryStub.Object);

            var result = await vehicleMakesService.GetVehicleMakes(sortParams, pagingParams, filterParams);

            result.Should().BeEquivalentTo(
                pagedDomainModels,
                options => options.ComparingByMembers<IPagedList<VehicleMakeDomainModel>>());
        }

        [Fact]
        public async Task GetVehicleMake_WithExistingItem_ReturnsMake()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var sortHelperStub = new Mock<ISortHelper<VehicleMake>>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            int id = 1;

            var returnedMake = new VehicleMake
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            var domainMake = new VehicleMakeDomainModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            repositoryStub.Setup(repo => repo.GetById(id))
                .ReturnsAsync(returnedMake);

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeDomainModel>(returnedMake))
                .Returns(domainMake);

            VehicleMakesService service = new(unitOfWorkStub.Object,
                loggerStub.Object, sortHelperStub.Object, mapperStub.Object, repositoryStub.Object);

            var result = await service.GetVehicleMake(id);

            result.Should().BeEquivalentTo(
                domainMake,
                options => options.ComparingByMembers<VehicleMakeDomainModel>());
        }

        [Fact]
        public async Task GetVehicleMake_WithUnexistingItem_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var sortHelperStub = new Mock<ISortHelper<VehicleMake>>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            int id = 1;
            var returnedMake = (VehicleMake)null;

            repositoryStub.Setup(repo => repo.GetById(id))
                .ReturnsAsync(returnedMake);

            var domainMake = (VehicleMakeDomainModel)null;

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeDomainModel>(returnedMake))
                .Returns(domainMake);

            VehicleMakesService service = new(unitOfWorkStub.Object,
               loggerStub.Object, sortHelperStub.Object, mapperStub.Object, repositoryStub.Object);

            var result = await service.GetVehicleMake(id);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Insert_ValidEntry_ReturnsInsertedViewModel()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var sortHelperStub = new Mock<ISortHelper<VehicleMake>>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var createVehicleMakeDomainModel = new CreateVehicleMakeDomainModel
            {
                Name = "name",
                Abrv = "abrv",
            };

            var makeToCreate = new VehicleMake
            {
                Name = "name",
                Abrv = "abrv"
            };

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(createVehicleMakeDomainModel))
                .Returns(makeToCreate);

            var id = new Random().Next();

            var createdMake = new VehicleMake
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            var createdDomainMake = new VehicleMakeDomainModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            repositoryStub.Setup(repo => repo.Insert(makeToCreate))
                .Returns(createdMake);


            mapperStub.Setup(mapper => mapper.Map<VehicleMakeDomainModel>(createdMake))
                .Returns(createdDomainMake);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            VehicleMakesService service = new(unitOfWorkStub.Object,
               loggerStub.Object, sortHelperStub.Object, mapperStub.Object, repositoryStub.Object);

            var result = await service.InsertVehicleMake(createVehicleMakeDomainModel);

            result.Should().BeEquivalentTo(
                createdDomainMake,
                options => options.ComparingByMembers<VehicleMake>());
        }

        [Fact]
        public async Task Insert_InsertedItemIsNull_ReturnsNull()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var sortHelperStub = new Mock<ISortHelper<VehicleMake>>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var createVehicleMakeDomainModel = new CreateVehicleMakeDomainModel
            {
                Name = "name",
                Abrv = "abrv",
            };

            var makeToCreate = new VehicleMake
            {
                Name = "name",
                Abrv = "abrv"
            };

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(createVehicleMakeDomainModel))
                .Returns(makeToCreate);

            var id = new Random().Next();

            var createdMake = (VehicleMake)null;

            var createdDomainMake = (VehicleMakeDomainModel)null;

            repositoryStub.Setup(repo => repo.Insert(makeToCreate))
                .Returns(createdMake);

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeDomainModel>(createdMake))
                .Returns(createdDomainMake);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            VehicleMakesService service = new(unitOfWorkStub.Object,
               loggerStub.Object, sortHelperStub.Object, mapperStub.Object, repositoryStub.Object);

            var result = await service.InsertVehicleMake(createVehicleMakeDomainModel);

            result.Should().BeEquivalentTo(
                createdDomainMake,
                options => options.ComparingByMembers<VehicleMake>());
        }

        [Fact]
        public async Task Delete_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var sortHelperStub = new Mock<ISortHelper<VehicleMake>>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var id = new Random().Next();

            var domainMakeToDelete = new VehicleMakeDomainModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            var makeToDelete = new VehicleMake
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(domainMakeToDelete))
                .Returns(makeToDelete);

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            VehicleMakesService service = new(unitOfWorkStub.Object, loggerStub.Object, 
                sortHelperStub.Object, mapperStub.Object, repositoryStub.Object);

            var result = await service.DeleteVehicleMake(domainMakeToDelete);

            repositoryStub.Verify(repo => repo.Delete(makeToDelete), Times.Once);
            result.Should().Be(1);
        }
        
        [Fact]
        public async Task Update_ValidEntry_Returns1()
        {
            var unitOfWorkStub = new Mock<IUnitOfWork>();
            var sortHelperStub = new Mock<ISortHelper<VehicleMake>>();
            var mapperStub = new Mock<IMapper>();
            var loggerStub = new Mock<ILogger<VehicleMakesService>>();
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var id = new Random().Next();
            var updatedDomainModel = new VehicleMakeDomainModel
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            var updatedMake = new VehicleMake
            {
                Id = id,
                Name = "name",
                Abrv = "abrv"
            };

            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(updatedDomainModel))
                .Returns(updatedMake);

            repositoryStub.Setup(repo => repo.Update(updatedMake));

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            VehicleMakesService service = new(unitOfWorkStub.Object, loggerStub.Object,
                sortHelperStub.Object, mapperStub.Object, repositoryStub.Object);

            var result = await service.UpdateVehicleMake(updatedDomainModel);

            result.Should().Be(1);
        }
    }
}
