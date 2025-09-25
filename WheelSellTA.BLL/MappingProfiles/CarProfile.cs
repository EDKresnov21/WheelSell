using AutoMapper;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL.Entities;

//Просто Профиль Маппинга, чтобы программа понимала, какие два объекта связывать. Все же, поля в них разные, просто взять и захардкастить один в другой не выйдет - программа будет бить ошибку
namespace WheelSellTA.BLL.MappingProfiles
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