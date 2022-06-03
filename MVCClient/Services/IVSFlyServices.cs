﻿using MVCClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCClient.Services
{
    public interface IVSFlyServices
    {
        public Task<IEnumerable<FlightM>> GetFlights();
    }
}
