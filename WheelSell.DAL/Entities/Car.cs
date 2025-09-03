namespace WheelSell.DAL.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public decimal Price { get; set; }
        public int Year { get; set; }
        public string FuelType { get; set; } = null!;
        public double EngineCapacity { get; set; }
        public double FuelConsumption { get; set; }
        public string Transmission { get; set; } = null!;
        public string Location { get; set; } = null!;
        public int Mileage { get; set; }
        public int OwnerId { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsSold { get; set; }

        // Навигационное свойство
        public User Owner { get; set; } = null!;
    }
}