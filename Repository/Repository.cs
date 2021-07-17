using Common;
using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _entities = _context.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            var entites = from v in _entities select v;
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


        public void Insert(T entity)
        {
            _entities.Add(entity);
        }


        public void Update(T entity)
        {
            _entities.Update(entity);
        }
    }
}
