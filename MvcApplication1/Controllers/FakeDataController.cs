using System;
using System.IO;
using System.Web.Mvc;
using TruckTrackerWeb.Code.Gps;

namespace TruckTrackerWeb.Controllers
{
    public class FakeDataController : Controller
    {
        //
        // GET: /FakeData/

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Download(double startLat, double startLng, double endLat, double endLng)
        {
            // Generate a fake data file that is parsable by the upload routine.
            //
            // Each line will have the following format...
            //
            // $GPRMC,040302.663,A,3939.7,N,10506.6,W,0.27,358.86,200804,,*1A
            //
            const int totalPoints = 50;
            DateTime dtEnd = DateTime.Now;
            DateTime dtStart = dtEnd.AddHours(-24.0);
            double deltaLat = (endLat - startLat)/totalPoints;
            double deltaLng = (endLng - startLng)/totalPoints;
            double deltaTimeInSeconds = 24.0*60.0*60.0/totalPoints;

            MemoryStream stream = new MemoryStream();
            TextWriter writer = new StreamWriter(stream);
            DateTime dtPt = dtStart;
            double lat = startLat;
            double lng = startLng;
            for (int i = 0; i <= totalPoints;i++ )
            {
                string line = GprmcParser.CreateLine(dtPt, lat, lng);
                writer.WriteLine(line);

                dtPt = dtPt.AddSeconds(deltaTimeInSeconds);
                lat += deltaLat;
                lng += deltaLng;
            }
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "text/plain", "map_data.txt");
        }
    }
}
