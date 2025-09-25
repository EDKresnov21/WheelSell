using AutoMapper;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL.Entities;

namespace WheelSellTA.BLL.MappingProfiles
{
    public class FuelTypeProfile : Profile
    {
        public FuelTypeProfile()
        {
            CreateMap<FuelType, FuelTypeDTO>();
            CreateMap<FuelTypeDTO, FuelType>();
        }
    }
}