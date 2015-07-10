using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Data.Entities;
using Tracker.Data.Services;

namespace TruckTrackerWeb.Models
{
    public class JsonMapper
    {
        private readonly DataService _dataService;

        public JsonMapper(DataService dataService)
        {
            _dataService = dataService;
        }

        public JsonTruckInfo MapToTruckInfo(Truck truck, DateTime fromDate, DateTime toDate)
        {
            if (truck != null)
            {
                Location currentLocation = _dataService.GetCurrentLocation(truck.Id);
                JsonLocation jsonLocation = new JsonLocation
                                                {
                                                    lat = currentLocation.Latitude, 
                                                    lng = currentLocation.Longitude
                                                };
                List<JsonLocation> locations = _dataService.Locations
                    .FilterBy(x => x.Truck.Id == truck.Id && x.Timestamp >= fromDate && x.Timestamp <= toDate)
                    .OrderByDescending(c => c.Timestamp)
                    .Select(
                        location => new JsonLocation
                                        {
                                            lat = location.Latitude,
                                            lng = location.Longitude
                                        }
                    )
                    .ToList();

                JsonTruckInfo jsonTruckInfo = new JsonTruckInfo
                                                  {
                                                      name = truck.Name,
                                                      plate = truck.PlateNumber,
                                                      type = truck.Type,
                                                      user = truck.User.FirstName + " " + truck.User.LastName,
                                                      location = jsonLocation,
                                                      history = locations
                                                  };
                return jsonTruckInfo;
            }
            return null;
        }
    }
}