using System.Collections.Generic;

namespace WheelSellTA.DAL.Entities
{
    public class FuelType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}