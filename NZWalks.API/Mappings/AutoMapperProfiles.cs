using AutoMapper;
using Booking.Com_Clone_API.Models.Domain;
using Booking.Com_Clone_API.Models.DTO;

namespace NZWalks.API.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        // Insatll AutoMapper Nuget named AutoMapper and inject into Program.cs file
        public AutoMapperProfiles()
        {                      
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<VerificationDto, User>().ReverseMap();
            CreateMap<LoginDto, User>().ReverseMap();

        }
    }

   
}
