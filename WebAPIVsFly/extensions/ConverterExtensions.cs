using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPIVsFly.extensions
{
    public static class ConverterExtensions
    {
        public static Models.FlightM convertToFlightM(this EFCoreApp2021.Flight f)
        {
            Models.FlightM fM = new Models.FlightM();
            fM.Date = f.Date;
            fM.Departure = f.Departure;
            fM.Destination = f.Destination;
            fM.FlightNo = f.FlightNo;
            fM.Price = f.Price;
            int takenPlaces = 0;
            if(f.BookingSet!=null)
            {
                takenPlaces = f.BookingSet.Count;
            }
            fM.takenSeats = takenPlaces;
            fM.totalSeats = f.Seats;
            return fM;
        }

        public static EFCoreApp2021.Flight ConvertToFlightEF (this Models.FlightM fM)
        {
            EFCoreApp2021.Flight f = new EFCoreApp2021.Flight();
            f.Date = fM.Date;
            f.Departure = fM.Departure;
            f.Destination = fM.Destination;
            f.FlightNo = fM.FlightNo;
            return f;
        }

    }
}
