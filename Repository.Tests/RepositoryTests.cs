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

namespace Repository.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task GetAll_ReturnsAllItems()
        {
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var listToReturn = new List<VehicleMake> { new VehicleMake() };

            /*var vehicleMakeFilterParams = new VehicleMakeFilterParams();

            repositoryStub.Setup(repo => repo.GetAll())
                .ReturnsAsync(listToReturn);

            var unitOfWorkStub = new Mock<IUnitOfWork>();
            unitOfWorkStub.Setup(unitOfWork => unitOfWork.VehicleMakes)
                .Returns(repositoryStub.Object).Verifiable();

            var mapperStub = new Mock<IMapper>();*/
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
    }
}
