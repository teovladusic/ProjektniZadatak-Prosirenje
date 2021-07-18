using Common;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Common
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(IApplicationDbContext dbContext)
        {
            _context = dbContext;
            _entities = _context.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            var entites = _entities.Where(x => true);
            return await entites.ToListAsync();
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
        }


        public async Task<T> GetById(object id)
        {
            return await _entities.FindAsync(id);
        }


        public T Insert(T entity)
        {
            return _entities.Add(entity).Entity;
        }


        public void Update(T entity)
        {
            _entities.Update(entity);
        }
    }
}
