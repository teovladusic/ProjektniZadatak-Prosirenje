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
        public ApplicationDbContext Context;
        private DbSet<T> _entities;

        public Repository(ApplicationDbContext dbContext)
        {
            Context = dbContext;
            Context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public Repository(IUnitOfWork unitOfWork) : this(unitOfWork.Context)
        {
        }

        protected virtual DbSet<T> Entities
        {
            get { return _entities ?? (_entities = Context.Set<T>()); }
        }

        public async Task<List<T>> GetAll()
        {
            return await Entities.ToListAsync();
        }

        public async Task<T> GetById(object id)
        {
            return await Entities.FindAsync(id);
        }


        public T Insert(T entity)
        {
            return Entities.Add(entity).Entity;
        }

        public virtual void Delete(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Entities.Attach(entity);
            }
            Entities.Remove(entity);
        }

        public virtual void Delete(object id)
        {
            T entity = Entities.Find(id);
            Entities.Remove(entity);
        }


        public void Update(T entity)
        {
            Entities.Update(entity);
        }
    }
}
