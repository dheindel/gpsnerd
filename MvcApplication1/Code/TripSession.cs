using System;

namespace TruckTrackerWeb.Code
{
    public class TripSession
    {
        public Guid Id { get; set; }
        public DateTime Expiration { get; set; }
        public string UserName { get; set; }
        public int TruckId { get; set; }

        public override string ToString()
        {
            return Id + "\t" + Expiration + "\t" + UserName + "\t" + TruckId;
        }

        public static TripSession ParseLine(string line)
        {
            string[] parts = line.Split('\t');
            if(parts.Length!=4)
            {
                return null;
            }
            try
            {
                TripSession tripSession = new TripSession
                                              {
                                                  Id = new Guid(parts[0]),
                                                  Expiration = DateTime.Parse(parts[1]),
                                                  UserName = parts[2],
                                                  TruckId = int.Parse(parts[3])
                                              };
                return tripSession;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}