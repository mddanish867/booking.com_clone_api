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
            //CreateMap<HotelDto, Hotel>().ReverseMap();
            CreateMap<HotelDto, Hotel>()
           .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(url => new Image { Url = url }).ToList()))
           .ForMember(dest => dest.HotelFacilities, opt => opt.MapFrom(src => src.HotelFacilities.Select(name => new Facility { Name = name }).ToList()));

            CreateMap<Hotel, HotelDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(image => image.Url).ToList()))
                .ForMember(dest => dest.HotelFacilities, opt => opt.MapFrom(src => src.HotelFacilities.Select(facility => facility.Name).ToList()));
        }
    }

   
}
