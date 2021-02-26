using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => 
                src.Photo.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => 
                src.DateOfBirth.CalcAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDto,AppUser>();
        }
    }
}