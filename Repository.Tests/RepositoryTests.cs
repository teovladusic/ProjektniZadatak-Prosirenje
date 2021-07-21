using DAL;
using DAL.Models;
using Moq;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using MockQueryable.Moq;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Project.Model;
using Common;
using Model;

namespace Repository.Tests
{
    public class RepositoryTests
    {

        //currently method GetAll is not used and cannot
        //be tested because test relays on other methods calling this one.
        [Fact]
        public void GetAll_ReturnsAllItems()
        {
        }

        [Fact]
        public async Task GetById_WithExistingItem_ReturnsItem()
        {
            int id = 1;
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeToReturn = new VehicleMake();
            var vehicleMakeViewModel = new VehicleMakeViewModel();

            repositoryStub.Setup(repo => repo.GetById(id))
                .ReturnsAsync(vehicleMakeToReturn);

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleMakeViewModel>(vehicleMakeToReturn))
                .Returns(vehicleMakeViewModel);

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var actual = await service.GetVehicleMake(id);

            repositoryStub.Verify();
            actual.Should().BeEquivalentTo(
                vehicleMakeViewModel,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }

        [Fact]
        public async Task GetById_WithUnexistingItem_ReturnsNull()
        {
            int id = 1;
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeToReturn = (VehicleMake)null;
            var vehicleMakeViewModel = (VehicleMakeViewModel)null;

            repositoryStub.Setup(repo => repo.GetById(id))
                .ReturnsAsync(vehicleMakeToReturn);

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleMakeViewModel>(vehicleMakeToReturn))
                .Returns(vehicleMakeViewModel);

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var actual = await service.GetVehicleMake(id);

            repositoryStub.Verify();
            actual.Should().BeEquivalentTo(
                vehicleMakeViewModel,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }

        [Fact]
        public async Task Insert_WithValidEntity_ReturnsInsertedItem()
        {
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var createVehicleMakeViewModel = new CreateVehicleMakeViewModel();
            var vehicleMakeToInsert = new VehicleMake();
            var vehicleMakeViewModel = new VehicleMakeViewModel();

            repositoryStub.Setup(repo => repo.Insert(vehicleMakeToInsert))
                .Returns(vehicleMakeToInsert);

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(createVehicleMakeViewModel))
                .Returns(vehicleMakeToInsert);

            mapperStub.Setup(mapper => mapper.Map<VehicleMakeViewModel>(vehicleMakeToInsert))
                .Returns(vehicleMakeViewModel);

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var result = await service.InsertVehicleMake(createVehicleMakeViewModel);

            result.Should().BeEquivalentTo(
                vehicleMakeViewModel,
                options => options.ComparingByMembers<VehicleMakeViewModel>());
        }

        [Fact]
        public async Task Delete_WithValidEntity_DeletesItemAndReturns1()
        {
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeToDelete = new VehicleMake();
            var vehicleMakeViewModel = new VehicleMakeViewModel();

            repositoryStub.Setup(repo => repo.Delete(vehicleMakeToDelete));

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(vehicleMakeViewModel))
                .Returns(vehicleMakeToDelete);

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var result = await service.DeleteVehicleMake(vehicleMakeViewModel);

            result.Should().Be(1);
        }

        [Fact]
        public async Task Update_WithValidEntity_UpdatesItemAndReturns1()
        {
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeToUpdate = new VehicleMake();
            var vehicleMakeViewModel = new VehicleMakeViewModel();

            repositoryStub.Setup(repo => repo.Update(vehicleMakeToUpdate));

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            unitOfWorkStub.Setup(unitOfWork => unitOfWork.Complete())
                .ReturnsAsync(1);

            var mapperStub = new Mock<IMapper>();
            mapperStub.Setup(mapper => mapper.Map<VehicleMake>(vehicleMakeViewModel))
                .Returns(vehicleMakeToUpdate);

            var loggerStub = new Mock<ILogger<VehicleMakesService>>();

            var service = new VehicleMakesService(unitOfWorkStub.Object,
                mapperStub.Object, loggerStub.Object);

            var result = await service.UpdateVehicleMake(vehicleMakeViewModel);

            result.Should().Be(1);
        }
    }
}
