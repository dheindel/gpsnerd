using System;
using System.Collections.Generic;
using Tracker.Data.Entities;
using Tracker.Data.Services;

namespace TruckTrackerWeb.Models
{
    public class NewTruckInfo
    {
        public string TruckName { get; set; }
        public string TruckType { get; set; }
        public string TruckPlate { get; set; }
        public bool IsPrivate { get; set; }
        public double LatValue { get; set; }
        public double LngValue { get; set; }

        public int AddTruck(DataService dataService, User user)
        {
            // Create the truck, driver, and route
            //
            Truck truck = new Truck
            {
                Name = TruckName,
                PlateNumber = TruckPlate,
                Type = TruckType,
                IsPrivate = IsPrivate,
                User = user
            };
            IEnumerable<string> brokenRules;
            dataService.Trucks.Add(truck, out brokenRules);

            // Add the intial position of the truck
            //
            Location startLocation = new Location
                                         {
                                             Latitude = LatValue,
                                             Longitude = LngValue,
                                             Timestamp = DateTime.Now,
                                             Truck = truck
                                         };
            dataService.Locations.Add(startLocation, out brokenRules);

            dataService.Commit();

            return truck.Id;
        }
    }
}