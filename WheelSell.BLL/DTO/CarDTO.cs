using System.Collections.Generic;

namespace WheelSell.BLL.DTO
{
    public class CarDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public int Year { get; set; }
        public int Mileage { get; set; }
        public string Location { get; set; }
        public int HorsePower { get; set; }
        public bool IsSold { get; set; }

        public int BrandId { get; set; }
        public int ModelId { get; set; }
        public int FuelTypeId { get; set; }
        public int TransmissionId { get; set; }

        public UserDTO Owner { get; set; }
        public BrandDTO Brand { get; set; }
        public ModelDTO Model { get; set; }
        public FuelTypeDTO FuelType { get; set; }
        public TransmissionDTO Transmission { get; set; }

        public ICollection<PhotoDTO> Photos { get; set; }
        public ICollection<VideoDTO> Videos { get; set; }
    }
}