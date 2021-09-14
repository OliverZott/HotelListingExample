using HotelListingExample.Models;
using System.Threading.Tasks;

namespace HotelListingExample.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDto userDto);
        Task<string> CreateToken();
    }
}
