using HotelListingExample.Models;
using System.Threading.Tasks;

namespace HotelListingExample.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(UserDto userDto);
        Task<string> CreateToken();
    }
}
