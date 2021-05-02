using HotelListingExample.Data;
using HotelListingExample.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HotelListingExample.Repository
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
            this._db = _context.Set<T>();   // not the same from the DatabaseContext ???
        }

        public async Task Delete(int id)
        {
            var entity = await _db.FindAsync(id);       // why user async here ???
            _db.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);                  // why no async here ???
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _db;      // Why not use _db directly ???

            // Optional "includes" parameter will get Foreign Key object as well without making an additional query manually
            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.AsNoTracking().FirstOrDefaultAsync(expression);      // Using lambda expression to define what we want to find
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = _db;      // Why not use _db directly ???

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var includeProperty in includes)
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task Insert(T entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _db.AddRangeAsync(entities);
        }

        // Attach is used because AsNotTracking is user ?!
        public void Update(T entitiy)
        {
            _db.Attach(entitiy);            // means: "pay attention to this entity" Adding change-detection
            _context.Entry(entitiy).State = EntityState.Modified;
        }
    }
}
