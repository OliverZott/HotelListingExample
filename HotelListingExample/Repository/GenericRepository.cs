using HotelListingExample.Data;
using HotelListingExample.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelListingExample.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {

        // DI service which already registered in startup
        private readonly DatabaseContext _context;
        private readonly DbSet<T> _db;


        // Here the parameter accesses the Service which is registered in Startup.cs
        public GenericRepository(DatabaseContext context)
        {
            _context = context;
            this._db = _context.Set<T>();   // not the same from the DatabaseContext ???
        }


        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<T> Get(System.Linq.Expressions.Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> include = null)
        {
            throw new NotImplementedException();
        }

        public Task Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task Update(T entitiy)
        {
            throw new NotImplementedException();
        }
    }
}
