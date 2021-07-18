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

namespace Repository.Tests
{
    public class RepositoryTest
    {

        //UnitOfWork_StateUnderTest_ExpectedBehaviour

        [Fact]
        public async Task GetById_WithUnexsistingItem_ReturnsNotFound()
        {
            
        }
    }
}
