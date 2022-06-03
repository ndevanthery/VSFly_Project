using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreApp2021
{
    public class Passenger:Person
    {
        public int Weight { get; set; }

        // not finished
        // Flight <---- Booking ----> Passenger
        public virtual ICollection<Booking> BookingSet { get; set; }
    }
}
