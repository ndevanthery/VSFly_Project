using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MVCClient2.Models;
using System.Text;

namespace MVCClient2.Services
{
    public class VSFlyServices : IVSFlyServices
    {
        private readonly HttpClient _client;
        private readonly string _baseuri;

        public VSFlyServices(HttpClient client)
        {
            _client = client;
            _baseuri = "https://localhost:44336/api/Flights/";

        }

        public async Task<IEnumerable<BookingM>> GetBookingsDestination(string destination)
        {
            var book = await _client.GetAsync(_baseuri + "GetBookingsDestination/" + destination, HttpCompletionOption.ResponseHeadersRead);
            book.EnsureSuccessStatusCode();
            var data2 = await book.Content.ReadAsStringAsync();
            var myBookings = JsonConvert.DeserializeObject<List<BookingM>>(data2);
            return myBookings;
        }

        public async Task<FlightM> GetFlight(int id)
        {
            var response = await _client.GetAsync(_baseuri + "GetFlight/" + id.ToString(), HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            var myFlight = JsonConvert.DeserializeObject<FlightM>(data);

            return myFlight;
        }

        public async Task<IEnumerable<FlightM>> GetFlights()
        {
            var response = await _client.GetAsync(_baseuri + "GetFlights", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();
            var Listdata = JsonConvert.DeserializeObject<List<FlightM>>(data);

            return Listdata;
        }

        public async Task<StatisticsFlightM> GetStatsDestination(string destination)
        {
            var Stats = await _client.GetAsync(_baseuri+ "GetStatsDestination/" + destination, HttpCompletionOption.ResponseHeadersRead);
            Stats.EnsureSuccessStatusCode();
            var data = await Stats.Content.ReadAsStringAsync();
            var myStats = JsonConvert.DeserializeObject<StatisticsFlightM>(data);

            return myStats;
        }

        public async Task<StatisticsFlightM> GetStatsFlight(int id)
        {
            var stats = await _client.GetAsync(_baseuri + "GetStatsFlight/" + id.ToString(), HttpCompletionOption.ResponseHeadersRead);
            
            stats.EnsureSuccessStatusCode();
            var data2 = await stats.Content.ReadAsStringAsync();
            var myStats = JsonConvert.DeserializeObject<StatisticsFlightM>(data2);
            return myStats;
            
        }

        public async void Reserve(FlightReservation flightReservation)
        {
            var payload = "{\"flightNo\": " + flightReservation.flight.FlightNo + ",\"passengerID\": 0,\"salePrice\": " + flightReservation.flight.Price + ",\"flight\": null,\"passenger\": {\"personID\": 0,\"surname\": \"" + flightReservation.passenger.Surname + "\",\"givenName\": \"" + flightReservation.passenger.GivenName + "\",\"weight\": " + flightReservation.passenger.Weight + ",\"bookingSet\": []}}";
            HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_baseuri + "Reserve/", c);
            response.EnsureSuccessStatusCode();
        }
    }
}
