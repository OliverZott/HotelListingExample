using AutoMapper;
using HotelListingExample.Core.DTOs;
using HotelListingExample.Data;

namespace HotelListingExample.Core.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CreateCountryDto>().ReverseMap();
            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, CreateHotelDto>().ReverseMap();
            CreateMap<ApiUser, UserDto>().ReverseMap();
            CreateMap<ApiUser, LoginUserDto>().ReverseMap();
        }
    }
}