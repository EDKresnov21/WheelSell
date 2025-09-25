using AutoMapper;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL.Entities;

namespace WheelSellTA.BLL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<RegisterDTO, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));
            CreateMap<User, RegisterDTO>();
        }
    }
}