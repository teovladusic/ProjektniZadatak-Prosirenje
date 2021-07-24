using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Common
{
    public interface IUnitOfWork
    {
        ApplicationDbContext Context { get; }
        Task<int> Complete();
        public void Dispose();
    }
}
