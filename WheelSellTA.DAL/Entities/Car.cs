using System.Collections.Generic;

namespace WheelSellTA.DAL.Entities
{
    public class Car
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
        public Brand Brand { get; set; }

        public int ModelId { get; set; }
        public Model Model { get; set; }

        public int FuelTypeId { get; set; }
        public FuelType FuelType { get; set; }

        public int TransmissionId { get; set; }
        public Transmission Transmission { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<Photo> Photos { get; set; }
        public ICollection<Video> Videos { get; set; }
    }
}