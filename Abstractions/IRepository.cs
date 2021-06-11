using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Abstractions
{
    public interface IRepository
    {
        Task<IQueryable<T>> All<T>() where T : class, new();
        Task<IQueryable<T>> Where<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task<T> Single<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task Delete<T>(Expression<Func<T, bool>> expression) where T : class, new();
        Task Add<T>(T item) where T : class, new();
        Task AddMany<T>(IEnumerable<T> items) where T : class, new();
        Task Update<T>(Expression<Func<T, bool>> expression, T item) where T : class, new();
        bool CollectionEmpty<T>() where T : class, new();
    }

}
