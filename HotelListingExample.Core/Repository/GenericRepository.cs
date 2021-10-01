using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HotelListingExample.Core.Models;
using HotelListingExample.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using X.PagedList;

namespace HotelListingExample.Core.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        // DI service which already registered in startup
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _db;


        // Here the parameter accesses the Service (copy of it?) which is registered in Startup.cs
        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            _db = _context.Set<T>(); // creating DbSet entity for querying database
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id); // why user async here ???
            _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities); // why no async here ???
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _db; // Why not use _db directly ???

            // Optional "includes" parameter will get Foreign Key object as well without making an additional query manually
            if (include != null) query = include(query);

            return await query.AsNoTracking()
                .FirstOrDefaultAsync(expression); // Using lambda expression to define what we want to find
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _db; // Why not use _db directly ???

            if (expression != null) query = query.Where(expression);

            if (include != null) query = include(query);

            if (orderBy != null) query = orderBy(query);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IPagedList<T>> GetAll(RequestParams requestParams,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _db; // Why not use _db directly ???

            if (include != null) query = include(query);

            return await query.AsNoTracking().ToPagedListAsync(requestParams.PageNumber, requestParams.PageSize);
        }

        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        // Attach is used because AsNotTracking is used ?!
        public void Update(T entity)
        {
            _db.Attach(entity); // means: "pay attention to this entity" Adding change-detection
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}