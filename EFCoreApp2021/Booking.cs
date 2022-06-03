using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EFCoreApp2021
{
    public class Booking
    {
        [Key]
        public int FlightNo { get; set; }  // declare those keys in WWWxxxxContext
        public int PassengerID { get; set; }

        public double SalePrice { get; set; }
        //public virtual Flight Flight { get; set; }
        public virtual Passenger Passenger { get; set; }
    }
}
