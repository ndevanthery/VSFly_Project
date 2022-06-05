using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EFCoreApp2021;
using WebAPIVsFly.Models;
using WebAPIVsFly.extensions;
using Microsoft.VisualBasic.CompilerServices;

namespace WebAPIVsFly.Controllers
{
    [Route("api/[controller]/[action]")]
    //[RoutePrefix("v1/MyName/{action}")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly WWWingsContext _context;

        public FlightsController(WWWingsContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]

        public async Task<ActionResult<IEnumerable<FlightM>>> GetFlights()
        {
            var flightList = await _context.FlightSet.ToListAsync();

            List<FlightM> flightMs = new List<FlightM>();
            foreach (Flight f in flightList)
            {
                var bookingList = await _context.BookingSet.ToListAsync();
                f.BookingSet = bookingList.Where(b => b.FlightNo == f.FlightNo).ToList();
                int takenSeats = 0;
                if (f.BookingSet != null)
                {
                    takenSeats = f.BookingSet.Count;
                }
                if (takenSeats < f.Seats && f.Date.CompareTo(DateTime.Now) > 0)
                {
                    f.Price = calculatePrice(takenSeats, f.Seats, f.Date, f.Price);
                    var fM = f.convertToFlightM();
                    flightMs.Add(fM);
                }
            }
            return flightMs;
        }


        private double calculatePrice(int seatsTaken, short seats, DateTime date, double basePrice)
        {
            if (1.0 * seatsTaken / seats >= 0.8) // flight is filled more than 80%
            {
                return 1.5 * basePrice;
            }
            else
            {
                if (DateTime.Now.AddMonths(1).CompareTo(date) > 0 && 1.0 * seatsTaken / seats < 0.5)//the flight is in less than 1 month & is filled less than 50%
                {
                    return 0.7 * basePrice;

                }
                
                else
                {
                    if (DateTime.Now.AddMonths(2).CompareTo(date) > 0 && 1.0 * seatsTaken / seats < 0.2) // the flight is in less than 2 month & is filled less than 20%
                    {
                        return 0.8 * basePrice;
                    }
                    else
                    {
                        return basePrice;
                    }


                }




            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await _context.FlightSet.FindAsync(id);

            
            if (flight == null)
            {
                return NotFound();
            }
            else
            {
                var bookingList = await _context.BookingSet.ToListAsync();
                flight.BookingSet = bookingList.Where(b => b.FlightNo == flight.FlightNo).ToList();
                int takenSeats = 0;
                if (flight.BookingSet != null)
                {
                    takenSeats = flight.BookingSet.Count;
                }
                flight.Price = calculatePrice(takenSeats, flight.Seats, flight.Date, flight.Price);
            }

            return flight;
        }


        ////POST: api/Flights
        ////To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Flight>> PostFlight(FlightM flightm)
        //{
        //    _context.FlightSet.Add(flightm.ConvertToFlightEF());
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetFlight", new { id = flightm.FlightNo }, flightm);
        //}



        [HttpPost]
        public async Task<IActionResult> Reserve(Booking booking)
        {
            if (booking.Passenger != null)
            {



                try
                {
                    //int i = _context.PassengerSet.OrderBy(p=>p.PersonID).Last().PersonID +1;
                    //booking.Passenger.PersonID = i;
                    var newPassenger = _context.PassengerSet.Add(booking.Passenger).Entity;
                    booking.PassengerID = newPassenger.PersonID;


                }
                catch (Exception e)
                {
                    var mess = e.Message;

                }
                try
                {

                    _context.BookingSet.Add(booking);
                    _context.SaveChanges();


                }
                catch (Exception e)
                {
                    var mess = e.Message;
                }


            }
            return NoContent();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<StatisticsFlightM>> GetStatsFlight(int id)
        {
            Flight flight = await _context.FlightSet.FindAsync(id); ;
            var result = new StatisticsFlightM();

     
            
            if(flight == null)
            {
                result.TotalPrice = 0;
                result.TicketsBought = 0;
                result.AveragePrice = 0;
            }
            else
            {
                var bookingList = await _context.BookingSet.ToListAsync();
                flight.BookingSet = bookingList.Where(b => b.FlightNo == flight.FlightNo).ToList();
                if (flight.BookingSet.Count ==0)
                {
                    result.TotalPrice = 0;
                    result.TicketsBought = 0;
                    result.AveragePrice = 0;
                }
                else
                {
                    var total = 0.0;
                    result.TicketsBought = flight.BookingSet.Count;
                    for (int i=0;i<result.TicketsBought;i++)
                    {
                        total += flight.BookingSet.ElementAt(i).SalePrice;
                    }
                    result.TotalPrice = total;
                    result.AveragePrice = total / result.TicketsBought;
                }
            }
            return result;


        }
        [HttpGet("{destination}")]
        public async Task<ActionResult<IEnumerable<BookingM>>> GetBookingsDestination(string destination)
        {
            var flights = await _context.FlightSet.ToListAsync();
            var concernedFlights = flights.Where(f => f.Destination == destination).ToList();

            var bookings = await _context.BookingSet.ToListAsync();
            var concernedBookings = new List<Booking>();
            foreach (var flight in concernedFlights)
            {
                concernedBookings.AddRange(bookings.Where(f => f.FlightNo == flight.FlightNo).ToList());
            }

            //return VM not booking
            var listBookingM = new List<BookingM>();
            foreach( var booking in concernedBookings)
            {
                var pers = await _context.PassengerSet.FindAsync(booking.PassengerID);
                listBookingM.Add(new BookingM { LastName = pers.GivenName , FirstName = pers.Surname , FlightNo = booking.FlightNo , SellPrice = booking.SalePrice }); 
            }

            return listBookingM;



        }


        [HttpGet("{destination}")]
        public async Task<ActionResult<StatisticsFlightM>> GetStatsDestination(string destination)
        {
            StatisticsFlightM result = new StatisticsFlightM() ;
            var flights = await _context.FlightSet.ToListAsync();
            var concernedFlights = flights.Where(f => f.Destination == destination).ToList();

            var bookings = await _context.BookingSet.ToListAsync();
            var concernedBookings = new List<Booking>();
            foreach (var flight in concernedFlights)
            {
                concernedBookings.AddRange(bookings.Where(f => f.FlightNo == flight.FlightNo).ToList());
            }



            if (concernedBookings.Count == 0)
            {
                result.TotalPrice = 0;
                result.TicketsBought = 0;
                result.AveragePrice = 0;
            }
            else
            {
                var total = 0.0;
                result.TicketsBought = concernedBookings.Count;
                for (int i = 0; i < result.TicketsBought; i++)
                {
                    total += concernedBookings.ElementAt(i).SalePrice;
                }
                result.TotalPrice = total;
                result.AveragePrice = total / result.TicketsBought;
            }
            

            return result;


        }





    }
}
