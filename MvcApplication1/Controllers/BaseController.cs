using System;
using System.Web.Mvc;
using Cravens.Infrastructure.Logging;
using Tracker.Data.Entities;
using Tracker.Data.Services;

namespace TruckTrackerWeb.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger _logger;
        protected readonly DataService _dataService;

        protected BaseController(ILogger logger, 
                                DataService dataService)
		{
            _logger = logger;
            _dataService = dataService;
		}

        protected override void OnException(ExceptionContext filterContext)
        {
            _logger.Error("*************Caught exception in BaseController************");
            if (filterContext == null)
            {
                return;
            }

            Exception ex = filterContext.Exception ?? new Exception("No further information exists.");
            _logger.Error("EXCEPTION INFO", ex);

            filterContext.ExceptionHandled = true;
            base.OnException(filterContext);
        }

        protected void Growl(string title, string message)
        {
            TempData["Growl"] = title + "\t" + message;
        }

        protected bool Authorize(int truckId)
        {
            User user;
            Truck truck;
            return Authorize(truckId, out user, out truck);
        }

        protected bool Authorize(int truckId, out User user, out Truck truck)
        {
            truck = null;
            if (!FetchUser(out user))
            {
                return false;
            }
            if (!FetchTruck(truckId, out truck))
            {
                _logger.Debug("No truck with id=" + truckId + " (" + user.UserName + ")");
                return false;
            }
            return Authorize(truck, user);
        }

        protected bool Authorize(Truck truck, User user)
        {
            if (truck.User.Id != user.Id)
            {
                return false;
            }

            return true;
        }

        protected bool FetchUser(out User user)
        {
            user = FetchUser();
            if (user == null)
            {
                return false;
            }
            return true;
        }

        protected User FetchUser()
        {
            string userName = (string)Session["UserName"];
            if(string.IsNullOrEmpty(userName))
            {
                _logger.Debug("No user name in the session.");
                return null;
            }
            _logger.Debug("Session['UserName'] = " + userName + "");
            return _dataService.Users.FindBy(x => x.UserName == userName);
        }

        protected bool FetchTruck(int truckId, out Truck truck)
        {
            truck = FetchTruck(truckId);
            if (truck == null)
            {
                return false;
            }
            return true;
        }

        private Truck FetchTruck(int truckId)
        {
            return _dataService.Trucks.FindBy(truckId);
        }
    }
}
