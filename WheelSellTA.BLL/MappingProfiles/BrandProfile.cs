using AutoMapper;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL.Entities;

namespace WheelSellTA.BLL.MappingProfiles
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