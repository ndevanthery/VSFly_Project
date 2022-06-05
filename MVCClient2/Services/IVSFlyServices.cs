using Microsoft.AspNetCore.Mvc;
using MVCClient2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient2.Services
{
    public interface IVSFlyServices
    {
        public Task<IEnumerable<FlightM>> GetFlights();
        public Task<FlightM> GetFlight(int id);
        public Task<IActionResult> Reserve(FlightReservation flightReservation);
        public Task<StatisticsFlightM> GetStatsFlight(int id);
        public Task<IEnumerable<BookingM>> GetBookingsDestination(string destination);
        public Task<StatisticsFlightM> GetStatsDestination(string destination);


    }
}
