using System;
using System.Text;

namespace TruckTrackerWeb.Code.Gps
{
    public class GprmcParser
    {
        // Parse the GPRMC line
        //
        public static GpsPoint Parse(string line)
        {
            // $GPRMC,040302.663,A,3939.7,N,10506.6,W,0.27,358.86,200804,,*1A

            if(!IsCheckSumGood(line))
            {
                return null;
            }

            try
            {
                string[] parts = line.Split(',');
                if (parts.Length != 12)
                {
                    return null;
                }

                if (parts[2] != "A")
                {
                    return null;
                }

                string date = parts[9]; // UTC Date DDMMYY
                if (date.Length != 6)
                {
                    return null;
                }
                int year = 2000 + int.Parse(date.Substring(4, 2));
                int month = int.Parse(date.Substring(2, 2));
                int day = int.Parse(date.Substring(0, 2));
                string time = parts[1]; // HHMMSS.XXX
                if (time.Length != 10)
                {
                    return null;
                }
                int hour = int.Parse(time.Substring(0, 2));
                int minute = int.Parse(time.Substring(2, 2));
                int second = int.Parse(time.Substring(4, 2));
                int milliseconds = int.Parse(time.Substring(7, 3));
                DateTime utcTime = new DateTime(year, month, day, hour, minute, second, milliseconds);

                string lat = parts[3];  // HHMM.MMMM....i have seen varying lengths, but always two chars for the HH
                double latHours = double.Parse(lat.Substring(0, 2));
                double latMins = double.Parse(lat.Substring(2));
                double latitude = latHours + latMins / 60.0;
                if (parts[4] == "S")       // N or S
                {
                    latitude = -latitude;
                }

                string lng = parts[5];  // HHHMM.M....I have seen varing lengths, but always three chars the for HHH.
                double lngHours = double.Parse(lng.Substring(0, 3));
                double lngMins = double.Parse(lng.Substring(3));
                double longitude = lngHours + lngMins / 60.0;
                if (parts[6] == "W")
                {
                    longitude = -longitude;
                }

                double speed = double.Parse(parts[7]);
                double bearing = double.Parse(parts[8]);

                // Should probably validate check sum

                GpsPoint gpsPoint = new GpsPoint
                                        {
                                            BearingInDegrees = bearing,
                                            Latitude = latitude,
                                            Longitude = longitude,
                                            SpeedInKnots = speed,
                                            Timestamp = utcTime
                                        };
                return gpsPoint;
                
            }
            catch (Exception)
            {
                // One of our parses failed...ignore.
            }
            return null;
        }

        private static bool IsCheckSumGood(string sentence)
        {
            int index1 = sentence.IndexOf("$");
            int index2 = sentence.LastIndexOf("*");

            string checkSumString = sentence.Substring(index2 + 1, 2);
            int checkSum1 = Convert.ToInt32(checkSumString, 16);

            int checkSum2 = CalculateCheckSum(sentence);

            return checkSum1 == checkSum2;
        }

        public static int CalculateCheckSum(string sentence)
        {
            int index1 = sentence.IndexOf("$");
            int index2 = sentence.LastIndexOf("*");

            if (index1 != 0 || index2 != sentence.Length - 3)
            {
                return -1;
            }


            string valToCheck = sentence.Substring(index1 + 1, index2 - 1);
            char c = valToCheck[0];
            for (int i = 1; i < valToCheck.Length; i++)
            {
                c ^= valToCheck[i];
            }

            return c;
        }

        public static string CreateLine(DateTime timeStamp, double lat, double lng)
        {
            // $GPRMC,040302.663,A,3939.7,N,10506.6,W,0.27,358.86,200804,,*1A
            StringBuilder result = new StringBuilder("$GPRMC,");

            // HHMMSS.XXX timestamp
            string timeString = timeStamp.ToString("HHmmss.000,");
            result.Append(timeString);

            // fix status 'A'
            result.Append("A,");

            // lattidue HHMM.MMMM
            string latHemisphere = lat < 0 ? "S" : "N";
            double latCopy = Math.Abs(lat);
            double latHours = Math.Floor(latCopy);
            double latMins = (latCopy - latHours)*60.0;
            string latString = string.Format("{0:00}{1:00.0000}", latHours, latMins);
            result.Append(latString + "," + latHemisphere + ",");

            // longitude HHHMM.M
            string lngHemisphere = lng < 0 ? "W" : "E";
            double lngCopy = Math.Abs(lng);
            double lngHours = Math.Floor(lngCopy);
            double lngMins = (lngCopy - lngHours) * 60.0;
            string lngString = string.Format("{0:000}{1:00.0}", lngHours, lngMins);
            result.Append(lngString + "," + lngHemisphere + ",");

            // speed (unknown)
            result.Append("0.00,");

            // bearing (unknown)
            result.Append("0.00,");

            // date DDMMYY
            string date = timeStamp.ToString("ddMMyy");
            result.Append(date + ",,*");

            // checksum
            string temp = result + "00";
            int checkSum = CalculateCheckSum(temp);
            string check = string.Format("{0:x2}", checkSum);
            result.Append(check);

            return result.ToString();
        }
    }
}