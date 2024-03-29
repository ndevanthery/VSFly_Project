﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EFCoreApp2021
{
    public class WWWingsContext : DbContext {
        public DbSet<Flight> FlightSet { get; set; }
        public DbSet<Pilot> PilotSet { get; set; }
        public DbSet<Passenger> PassengerSet { get; set; }
        public DbSet<Booking> BookingSet { get; set; }


        // SQL Express
        //public static string ConnectionString { get; set; } = @"Server=(localDB)\MSSQLLocalDB;Database=WWWings_2022Step2;" +
        //A                                          "Trusted_Connection=True;App=EFCoreApp2021;MultipleActiveResultSets=true";
        public static string ConnectionString { get; set; } = "Data Source=153.109.124.35;Initial Catalog=NicoAbdu_VsFly;Persist Security Info=True;User ID=6231db;Password=Pwd46231.";
        public WWWingsContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder) {
            builder.UseSqlServer(ConnectionString);

            //builder.UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            // composed key
            builder.Entity<Booking>().HasKey(x => new { x.FlightNo, x.PassengerID ,x.SalePrice});

           

        }


    }
}
