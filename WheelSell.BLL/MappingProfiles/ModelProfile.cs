using AutoMapper;
using WheelSell.BLL.DTO;
using WheelSell.DAL.Entities;

namespace WheelSell.BLL.MappingProfiles
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<Model, ModelDTO>();
            CreateMap<ModelDTO, Model>();
        }
    }
}