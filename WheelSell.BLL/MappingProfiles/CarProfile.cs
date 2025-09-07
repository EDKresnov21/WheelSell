using AutoMapper;
using WheelSell.BLL.DTO;
using WheelSell.DAL.Entities;

namespace WheelSell.BLL.MappingProfiles
{
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            CreateMap<Car, CarDTO>();
            CreateMap<CarDTO, Car>();
        }
    }
}