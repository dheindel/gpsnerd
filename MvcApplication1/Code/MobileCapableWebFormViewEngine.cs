using System.Web.Mvc;

namespace TruckTrackerWeb.Code
{
    public class MobileCapableWebFormViewEngine : WebFormViewEngine
    {
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result = null;
            var request = controllerContext.HttpContext.Request;

            // Avoid unnecessary checks if this device isn't suspected to be a mobile device
            if (request.Browser.IsMobileDevice)
            {
                result = base.FindView(controllerContext, "Mobile/" + viewName, masterName, useCache);
            }

            //Fall back to desktop view if no other view has been selected
            if (result == null || result.View == null)
            {
                result = base.FindView(controllerContext, viewName, masterName, useCache);
            }

            return result;
        }
    }
}