using System.Collections.Generic;

namespace TruckTrackerWeb.Code.Gps
{
    public class GpsFileHelper
    {
        public static List<GpsPoint> PaseLines(List<string> lines)
        {
            List<GpsPoint> result = new List<GpsPoint>();
            foreach (string line in lines)
            {
                GpsPoint gpsPoint = GprmcParser.Parse(line);
                if(gpsPoint!=null)
                {
                    result.Add(gpsPoint);
                }
            }

            return result;
        }
    }
}