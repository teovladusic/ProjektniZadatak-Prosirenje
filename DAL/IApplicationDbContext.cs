using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace DAL
{
    public interface IApplicationDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseModel;
        int SaveChanges();
    }
}