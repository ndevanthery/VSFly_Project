using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCClient2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using MVCClient2.Services;

namespace MVCClient2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVSFlyServices _vsFly;
        public HomeController(ILogger<HomeController> logger, IVSFlyServices vsFly)
        {
            _logger = logger;
            _vsFly = vsFly;

        }

        public async Task<IActionResult> Index()

        {

            var Listdata = await _vsFly.GetFlights();

            return View(Listdata);
        }
        

       public async Task<IActionResult> BuyTicket(int id)
        {
            var myFlight = await _vsFly.GetFlight(id);
            FlightReservation flightReservation = new FlightReservation {  flight = myFlight, passenger = new EFCoreApp2021.Passenger() };

            return View(flightReservation);


        }
        [HttpPost]
        public async Task<IActionResult> BuyTicket(FlightReservation flightReservation)
        {
            if (ModelState.IsValid)
            {
                
                if (flightReservation != null)
                {


                    try
                    {
                        await _vsFly.Reserve(flightReservation);
                        return RedirectToAction("Index", "Home");


                    }
                    catch (Exception e)
                    {
                        
                    }

                }
                ModelState.AddModelError(string.Empty, "please enter valid infos");

            }
            ModelState.AddModelError(string.Empty, "please enter valid infos");

            return View(flightReservation);
        }

        
        public async Task<IActionResult> Statistics(int id)

        {

            var myFlight =await _vsFly.GetFlight(id);



            FlightStatVM myVM;
            try
            {
                var myStats = await _vsFly.GetStatsFlight(id);
                myVM = new FlightStatVM { myFlight = myFlight, myStat = myStats };

            }
            catch(Exception e)
            {
                myVM = new FlightStatVM { myFlight = new FlightM(), myStat = new StatisticsFlightM() };
            }








            return View(myVM);


        }

        public async Task<IActionResult> StatisticsDestination(string destination)

        {
            var myStats = await _vsFly.GetStatsDestination(destination);


            var myBookings = await _vsFly.GetBookingsDestination(destination);


            DestStatVM myVM = new DestStatVM {  myStat = myStats , myBookings = myBookings };



            return View(myVM);


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
