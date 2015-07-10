using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cravens.Infrastructure.Logging;
using Tracker.Data.Entities;
using Tracker.Data.Services;
using TruckTrackerWeb.Code;

namespace TruckTrackerWeb.Controllers
{
    public class MobileController : BaseController
    {
        private readonly TripSessions _tripSessions;

        public MobileController(ILogger logger,
                                DataService dataService,
                                TripSessions tripSessions)
            : base(logger, dataService)
        {
            _tripSessions = tripSessions;
        }

        public ActionResult Index()
        {
            User user = FetchUser();
            if(user==null)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Truck");
        }

        public ActionResult Trip(int truckId)
        {
            User user;
            Truck truck;
            if(!Authorize(truckId, out user, out truck))
            {
                if(user==null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Truck");
            }
            return View(truck);
        }

        public string StartTrip(int truckId)
        {
            if(!Request.IsAjaxRequest())
            {
                return "";
            }
            User user;
            Truck truck;
            if (!Authorize(truckId, out user, out truck))
            {
                if (user == null)
                {
                    return "";
                }
                return "";
            }
            TripSession tripSession = _tripSessions.CreateSession(user.UserName, truckId);
            if(tripSession!=null)
            {
                return tripSession.Id.ToString();
            }
            return "";
        }

        public string AddLocations(string id, JsonLocationPoint[] pts)
        {
            if(!Request.IsAjaxRequest())
            {
                return "";
            }
            Guid tripId;
            try
            {
                tripId = new Guid(id);
            }
            catch (Exception)
            {
                _logger.Error("Invalid trip id. id=" + id);
                return "invalid trip id";
            }

            // Get the trip session
            //
            TripSession tripSession = _tripSessions.GetSession(tripId);
            if(tripSession==null)
            {
                _logger.Error("Failed to find trip session.");
                return "failed to find trip session";
            }

            // Get the truck and the trip
            //
            User user = _dataService.Users.FindBy(x => x.UserName == tripSession.UserName);
            if(user==null)
            {
                _logger.Error("User not found. username=" + tripSession.UserName);
                return "user not found";
            }
            Truck truck = _dataService.Trucks.FindBy(tripSession.TruckId);
            if(truck==null)
            {
                _logger.Error("Truck not found. truckid=" + tripSession.TruckId);
                return "truck not found";
            }

            string result = "";
            foreach (JsonLocationPoint pt in pts)
            {
                Location location = new Location
                {
                    Latitude = pt.lat,
                    Longitude = pt.lng,
                    Timestamp = pt.ts,
                    Truck = truck
                };
                IEnumerable<string> brokenRules;
                if (!_dataService.Locations.Add(location, out brokenRules))
                {
                    result += brokenRules.First() + ",";
                }
            }
            _dataService.Commit();
            if(result=="")
            {
                return "Ok";
            }
            return result;
        }

    }
}
