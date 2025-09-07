using AutoMapper;
using WheelSell.BLL.DTO;
using WheelSell.DAL.Entities;

namespace WheelSell.BLL.MappingProfiles
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