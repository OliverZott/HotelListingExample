using HotelListingExample.Data;
using System;
using System.Threading.Tasks;

namespace HotelListingExample.IRepository
{
    // Service class that registers all versions of generic repository implementation 
    // for generic class T.
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotels { get; }

        // In the generic repository all changes are just STAGED!
        Task Save();
    }
}
