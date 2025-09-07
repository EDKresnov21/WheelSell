using AutoMapper;
using WheelSell.BLL.DTO;
using WheelSell.DAL.Entities;

namespace WheelSell.BLL.MappingProfiles
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