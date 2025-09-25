using AutoMapper;
using WheelSellTA.BLL.DTO;
using WheelSellTA.DAL.Entities;

namespace WheelSellTA.BLL.MappingProfiles
{
    public class TransmissionProfile : Profile
    {
        public TransmissionProfile()
        {
            CreateMap<Transmission, TransmissionDTO>();
            CreateMap<TransmissionDTO, Transmission>();
        }
    }
}