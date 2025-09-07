using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace WheelSell.DAL.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public ICollection<Car> Cars { get; set; }
    }
}