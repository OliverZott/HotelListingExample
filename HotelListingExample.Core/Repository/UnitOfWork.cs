using System;
using System.Threading.Tasks;
using HotelListingExample.Data;

namespace HotelListingExample.Core.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Country> _countries;
        private IGenericRepository<Hotel> _hotels;


        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }


        // Compare property versions as "block body" vs "expression body"
        public IGenericRepository<Country> Countries => _countries ??= new GenericRepository<Country>(_context);

        public IGenericRepository<Hotel> Hotels
        {
            get { return _hotels ??= new GenericRepository<Hotel>(_context); }
        }


        public void Dispose()
        {
            _context.Dispose(); // if Dispose is called, dispose of context, kill any memory of connection/resources, ... and use Garbage Collector
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}