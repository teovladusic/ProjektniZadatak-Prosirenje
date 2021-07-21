using Common;
using DAL.Models;
using Moq;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Repository.Tests
{
    public class VehicleMakesRepositoryTests
    {
        [Fact]
        public async Task  GetAll()
        {
            var repositoryStub = new Mock<IVehicleMakesRepository>();

            var vehicleMakeParams = new VehicleMakeParams();
            var sortParams = new SortParams(vehicleMakeParams.OrderBy);
            var pagingParams = new PagingParams(vehicleMakeParams.PageNumber, vehicleMakeParams.PageSize);
            var vehicleMakeFilterParams = new VehicleMakeFilterParams(sortParams, pagingParams, vehicleMakeParams.SearchQuery);

            var listToReturn = new List<VehicleMake> { new VehicleMake() };
            var pagedListToReturn = new PagedList<VehicleMake>(listToReturn, listToReturn.Count, pagingParams.CurrentPage,
                pagingParams.PageSize);

            repositoryStub.Setup(repo => repo.GetAll(vehicleMakeFilterParams))
                .ReturnsAsync(pagedListToReturn);
        }
    }
}
