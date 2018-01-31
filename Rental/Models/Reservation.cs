using System;

namespace Rental.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public Car Car { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Observations { get; set; }
    }
}
