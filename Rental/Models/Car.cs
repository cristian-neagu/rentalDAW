using System.Collections.Generic;

namespace Rental.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public int Seats { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public CarType CarType { get; set; }
    }
}
