using AutoMapper;
using WheelSell.BLL.DTO;
using WheelSell.DAL.Entities;

namespace WheelSell.BLL.MappingProfiles
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<Photo, PhotoDTO>();
            CreateMap<PhotoDTO, Photo>();
        }
    }
}