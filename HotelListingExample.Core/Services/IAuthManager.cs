using System.Threading.Tasks;
using HotelListingExample.Core.DTOs;

namespace HotelListingExample.Core.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDto userDto);
        Task<string> CreateToken();
    }
}