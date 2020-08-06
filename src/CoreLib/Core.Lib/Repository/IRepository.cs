using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Lib.Repository
{
    public interface IRepository<TContext> where TContext: DbContext
    {
        Task<IList<T>> GetAll<T>() where T : class;
        T GetById<T>(object id) where T : class;
        Task Insert<T>(T obj) where T : class;
        void Update<T>(T obj) where T : class;
        void Delete<T>(object id) where T : class;
        Task Save();
        Task<T> Get<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<bool> Any<T>(Expression<Func<T, bool>> expression) where T : class;
        IQueryable<T> GetQuery<T>(Expression<Func<T, bool>> expression) where T : class;
    }
}
