using System.Collections.Generic;

namespace WheelSell.DAL.Entities
{
    public class FuelType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}