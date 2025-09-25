using System.Collections.Generic;

namespace WheelSellTA.DAL.Entities
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}