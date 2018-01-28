using System.Collections.Generic;

namespace Rental.Models
{
    public class Car
    {
        public int CarId { get; set; }
        public string LicenseNumber { get; set; }
        public string Color { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public bool AirCon { get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }
        public int PricePerDay { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
        public CarType CarType { get; set; }
    }
}
