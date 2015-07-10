using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracker.Data.NHibernateLayer;

namespace NHibernateLayerTests
{
    /// <summary>
    /// These are not meant to be 'unit' level tests. They represent
    /// integration level operations and are meant to demonstrate
    /// how to excercise the fluent nhibernate repository.
    /// </summary>
    [TestClass]
    public class ExampleUseCases
    {
 /*       [TestMethod]
        public void Add_100_Trucks_With_1000_Location_Points_Each()
        {
            NHibernateHelper helper = new NHibernateHelper();

            for (int i = 0; i < 100; i++)
            {
                // Notice the unit of work we are using is to commit
                //	one truck's data at a time.
                using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
                {
                    Repository<Truck> repository = new Repository<Truck>(unitOfWork.Session);

                    Truck truck = CreateTruck(string.Format("Truck {0}", i + 1), 1000);
                    repository.Add(truck);

                    unitOfWork.Commit();
                }
            }
        }

        [TestMethod]
        public void Count_The_Number_Of_Locations_In_The_Db()
        {
            NHibernateHelper helper = new NHibernateHelper();

            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<Location> repository = new Repository<Location>(unitOfWork.Session);

                // This call uses LINQ to NHibernate to create an optimized SQL query.
                //	So instead of pulling all the entities from the DB and then using
                //	LINQ to count them, it instead pushes the counting to the DB by
                //	generating the appropriate SQL. Much much much faster!
                int count = repository.All().Count();
            }
        }

        [TestMethod]
        public void Get_The_Last_10_Locations_Of_A_Given_Truck()
        {
            NHibernateHelper helper = new NHibernateHelper();

            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<Truck> truckRepo = new Repository<Truck>(unitOfWork.Session);
                Truck truck = truckRepo.All().First();
                if (truck != null)
                {
                    Repository<Location> locationRepo = new Repository<Location>(unitOfWork.Session);

                    // Again the power of LINQ to NHibernates optimized queries and delayed execution.
                    IEnumerable<Location> pagedLocations = locationRepo.FilterBy(x => x.Truck.Id == truck.Id)
                        .OrderByDescending(x => x.Timestamp)
                        .Take(10);
                }
            }
        }

        [TestMethod]
        public void Delete_All_DataValues()
        {
            NHibernateHelper helper = new NHibernateHelper();

            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                unitOfWork.Session.Delete("from Location o");
                unitOfWork.Commit();
            }
        }

        [TestMethod]
        public void Add_Realistic_Random_Routes_To_Trucks()
        {
            NHibernateHelper helper = new NHibernateHelper();

            using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
            {
                Repository<Truck> repository = new Repository<Truck>(unitOfWork.Session);
                IQueryable<Truck> trucks = repository.All();

                DateTime start = DateTime.Now.AddYears(-1);
                DateTime stop = DateTime.Now;

                foreach (Truck truck in trucks)
                {
                    AddRandomRoute(10, start, stop, truck);
                }

                unitOfWork.Commit();
            }
        }

        [TestMethod]
        public void Add_Releastic_Truck_Routes()
        {
            NHibernateHelper helper = new NHibernateHelper();

            const float latStart = 43.0730517f;
            const float lngStart = -89.4012302f;
            DateTime dtStart = DateTime.Now;
            float[] lats = {41.850033f, 44.9799654f, 41.6005448f};
            float[] lngs = {-87.6500523f, -93.2638361f, -93.6091064f};
            float[] numHours = {3.5f, 6.0f, 7.0f};
            string[] truckNames = {"Chicago", "Minneapolis", "Des Moines"};
            string[] plateNumbers = {"ABC-001", "ABC-002", "ABC-003"};
            string[] types = {"Flat Bed", "Dump Truck", "Moving Van"};

            for(int i=0;i<lats.Length;i++)
            {
                // Create the truck, driver, and route in memory
                //
                Truck truck = new Truck
                                  {
                                      Name = truckNames[i],
                                      PlateNumber = plateNumbers[i],
                                      Type = types[i]
                                  };

                const int numPoints = 100;
                float deltaLat = (lats[i] - latStart)/(numPoints - 1);
                float deltaLng = (lngs[i] - lngStart)/(numPoints - 1);
                float deltaHours = numHours[i]/(numPoints - 1);

                List<Location> locations = new List<Location>();
                for(int j=0;j<numPoints;j++)
                {
                    Location location = new Location
                                            {
                                                Latitude = latStart + deltaLat*j,
                                                Longitude = lngStart + deltaLng*j,
                                                Timestamp = dtStart.AddHours(deltaHours * j),
                                                Truck = truck
                                            };
                }

                // Add it to the database.
                //
                using (UnitOfWork unitOfWork = new UnitOfWork(helper.SessionFactory))
                {
                    Repository<Truck> repository = new Repository<Truck>(unitOfWork.Session);
                    repository.Add(truck);

                    Repository<Location> locationRepo = new Repository<Location>(unitOfWork.Session);
                    locationRepo.Add(locations);

                    unitOfWork.Commit();
                }
            }

        }

        private static void AddRandomRoute(int numPoints, DateTime start, DateTime stop, Truck truck)
        {
            const float latStart = 43.1f;
            const float longStart = 89.5f;
            const float maxChangePerHour = 1.0f;
            Random random = new Random(DateTime.Now.Millisecond);

            TimeSpan timeSpan = stop - start;
            double totalHours = timeSpan.TotalHours;
            double hoursPerPoint = totalHours/numPoints;
            float maxChangerPerPoint = maxChangePerHour*(float) hoursPerPoint;

            float latitude = latStart;
            float longitude = longStart;
            DateTime timeStamp = start;
            for(int i=0;i<numPoints;i++)
            {
                Location location = new Location
                                        {
                                            Latitude = latitude,
                                            Longitude = longitude,
                                            Timestamp = timeStamp
                                        };
                truck.AddLocation(location);

                // Move the truck
                float angle = (float) random.NextDouble()*2.0f*(float)Math.PI;
                latitude += maxChangerPerPoint*(float)Math.Cos(angle);
			
                longitude += maxChangerPerPoint*(float) Math.Sin(angle);
                timeStamp = timeStamp.AddHours(hoursPerPoint);
            }
        }

        private static Truck CreateTruck(string name, int numberOfLocations)
        {
            Truck truck = new Truck
                              {
                                  Name = name,
                                  PlateNumber = string.Format("ABC-{0}", name),
                                  Type = string.Format("Type {0}", name)
                              };

            for (int j = 0; j < numberOfLocations; j++)
            {
                Location location = new Location
                                        {
                                            Timestamp = DateTime.Now.AddMinutes(5*j),
                                            Latitude = 10.0f + j,
                                            Longitude = -10.0f - j
                                        };
                truck.AddLocation(location);
            }
            return truck;
        }*/
    }
}