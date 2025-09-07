using AutoMapper;
using WheelSell.BLL.DTO;
using WheelSell.DAL.Entities;

namespace WheelSell.BLL.MappingProfiles
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandDTO>();
            CreateMap<BrandDTO, Brand>();
        }
    }
}