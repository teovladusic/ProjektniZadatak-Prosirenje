using AutoMapper;
using Common;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public ApplicationDbContext Context
        {
            get { return _context; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
