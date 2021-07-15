﻿using Common;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Common
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<List<T>> GetAll();
        Task<T> GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}