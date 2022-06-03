using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient2.Models
{
    public class DestStatVM
    {
        public StatisticsFlightM myStat{get;set;}

        public IEnumerable<BookingM> myBookings { get; set; }
    }
}
