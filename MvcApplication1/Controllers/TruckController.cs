using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Cravens.Infrastructure.Logging;
using Tracker.Data.Entities;
using Tracker.Data.Services;
using TruckTrackerWeb.Code.Gps;
using TruckTrackerWeb.Models;

namespace TruckTrackerWeb.Controllers
{
    public class TruckController : BaseController
    {
        public TruckController(ILogger logger,
                                DataService dataService)
            : base(logger, dataService)
        {
        }

        public ActionResult Index(int truckId)
        {
            _logger.Debug("HomeController/Truck (" + User.Identity.Name + ")");
            User user = FetchUser();

            // No id and no user....show the public trucks.
            //
            if (truckId == 0)
            {
                if (user == null)
                {
                    return View("Index", _dataService.Trucks.All().Where(x => x.IsPrivate == false).OrderBy(x => x.Name));
                }
                // Show this user's trucks
                return View("Index", _dataService.Trucks.All().Where(x => x.User.Id == user.Id).OrderBy(x => x.Name));
            }

            // Fetch the truck.
            //
            Truck truck;
            if(!FetchTruck(truckId, out truck))
            {
                _logger.Debug("No truck with id=" + truckId);
                Growl("Error", "Could not find the selected truck.");
                if(user == null)
                {
                    // Show the public list
                    return View("Index", _dataService.Trucks.All().Where(x => x.IsPrivate == false).OrderBy(x => x.Name));
                }
                // Show this user's trucks
                return View("Index", _dataService.Trucks.All().Where(x => x.User.Id == user.Id).OrderBy(x => x.Name));
            }

            // Is this user allowed to see this truck.
            if (truck.IsPrivate)
            {
                if (truck.User.Id != user.Id)
                {
                    Growl("Access Error", "You do not own this truck.");
                    // Show the public list
                    return View("Index", _dataService.Trucks.All().Where(x => x.IsPrivate == false).OrderBy(x => x.Name));
                }
            }

            // Add the selected truck to the view data
            //
            ViewData["selectId"] = truck.Id;
            if (user == null)
            {
                // Show the public list
                return View("Index", _dataService.Trucks.All().Where(x => x.IsPrivate == false).OrderBy(x => x.Name));
            }
            // Show this user's trucks
            return View("Index", _dataService.Trucks.All().Where(x => x.User.Id == user.Id).OrderBy(x => x.Name));
        }

		public ActionResult Info(int truckId)
		{
            return InfoFrom(truckId, DateTime.Now.AddDays(-1));
		}
        public ActionResult InfoFrom(int truckId, DateTime fromDate)
        {
            return InfoBetween(truckId, fromDate, DateTime.Now.AddDays(2));
        }
        public ActionResult InfoBetween(int truckId, DateTime fromDate, DateTime toDate)
        {
            if (Request.IsAjaxRequest())
            {
                Truck truck;
                if(!FetchTruck(truckId, out truck))
                {
                    _logger.Debug("No truck with id=" + truckId);
                    Growl("Error", "Could not find the selected truck.");
                    return new EmptyResult();
                }
                if(truck.IsPrivate)
                {
                    User user;
                    if (!FetchUser(out user))
                    {
                        Growl("Access Error", "Login first.");
                        return new EmptyResult();
                    }
                    if (truck.User.Id != user.Id)
                    {
                        Growl("Access Error", "You do not own this truck.");
                        return new EmptyResult();
                    }
                }
                JsonMapper mapper = new JsonMapper(_dataService);
                JsonTruckInfo locations = mapper.MapToTruckInfo(truck, fromDate, toDate);
                if (locations != null)
                {
                    return Json(locations, JsonRequestBehavior.AllowGet);
                }
            }
            return new EmptyResult();
        }

        [Authorize]
        public ActionResult AddTruck()
        {
            _logger.Debug("HomeController/AddTruck (" + User.Identity.Name + " )");
            return View();
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AddTruck(NewTruckInfo newTruckInfo)
        {
            _logger.Debug("HomeController/AddTruck (Post) (" + User.Identity.Name + ")");
            string userName = (string)Session["UserName"];
            User user = _dataService.Users.FindBy(x => x.UserName == userName);
            if(user==null)
            {
                return View();
            }

            int truckId = newTruckInfo.AddTruck(_dataService, user);
            return RedirectToAction("Index", new{@truckId=truckId});
        }

        [Authorize]
        public ActionResult Manage(int truckId)
        {
            Truck truck;
            User user;
            if (!Authorize(truckId, out user, out truck))
            {
                Growl("Access Error", "You do not own this truck.");
                return RedirectToAction("Index");
            }
            return View("ManageTruck", truck);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateTruckInfo(int truckId, Truck truck)
        {
            User user;
            Truck truckOriginal;
            if(!Authorize(truckId, out user, out truckOriginal))
            {
                return RedirectToAction("Index");
            }
            truckOriginal.Name = truck.Name;
            truckOriginal.PlateNumber = truck.PlateNumber;
            truckOriginal.Type = truck.Type;
            truckOriginal.IsPrivate = truck.IsPrivate;
            IEnumerable<string> brokenRules;
            _dataService.Trucks.Update(truckOriginal, out brokenRules);
            _dataService.Commit();

            Growl("Update - " + truckOriginal.Name, "The truck was successfully updated.");

            return RedirectToAction("Index");
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ProcessGpsFile(int truckId)
        {
            Truck truck;
            User user;
            if(!Authorize(truckId, out user, out truck))
            {
                Growl("Access Error", "You do not own this truck.");
                return RedirectToAction("Index");
            }
            if(Request.Files.Count==0)
            {
                Growl("File Not Found", "Upload a file for processing.");
                return Manage(truckId);
            }

            // Parse the file
            StreamReader reader = new StreamReader(Request.Files[0].InputStream);
            List<string> lines = new List<string>();
            string line;
            while( (line= reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            reader.Close();
            List<GpsPoint> gpsData = GpsFileHelper.PaseLines(lines);

            // Create the new locations
            foreach (GpsPoint data in gpsData)
            {
                Location location = new Location
                                        {
                                            Latitude = data.Latitude,
                                            Longitude = data.Longitude,
                                            Timestamp = data.Timestamp,
                                            Truck = truck
                                        };
                IEnumerable<string> brokenRules;
                _dataService.Locations.Add(location, out brokenRules);
            }
            _dataService.Commit();

            return RedirectToAction("Index");
        }
    }
}
