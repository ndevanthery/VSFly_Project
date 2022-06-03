using EFCoreApp2021;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient2.Models
{
    public class FlightReservation
    {
        public FlightM flight { get; set; }

        public Passenger passenger { get; set; }

    }
}
