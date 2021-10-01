using System;
using System.Threading.Tasks;
using HotelListingExample.Data;

namespace HotelListingExample.Core.Repository
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