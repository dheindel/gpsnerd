using System.Collections.Generic;

namespace TruckTrackerWeb.Models
{
    public class JsonTruckInfo
    {
        public string name { get; set; }
        public string type { get; set; }
        public string plate { get; set; }
        public string user { get; set; }
        public JsonLocation location { get; set; }
        public List<JsonLocation> history { get; set; }
    }
}