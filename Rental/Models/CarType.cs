using System.Collections.Generic;

namespace Rental.Models
{
    public class CarType
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string FullType { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public ICollection<Car> Cars { get; set; }
    }
}
