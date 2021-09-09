using HotelListingExample.Models;
using System.Threading.Tasks;

namespace HotelListingExample.Services
{
    interface IAuthManager
    {
        Task<bool> ValidateUser(UserDto userDto);
        Task<string> CreateToken();
    }
}
