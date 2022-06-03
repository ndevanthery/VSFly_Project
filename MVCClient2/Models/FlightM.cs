using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient2.Models
{
    public class FlightM
    {
        public int FlightNo { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }


        public double Price { get; set; }

        public int takenSeats { get; set; }

        public int totalSeats { get; set; }
        public DateTime Date { get; set; }
    }
}
