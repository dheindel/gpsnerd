using System.Linq;
using Cravens.Infrastructure.Repository;
using Tracker.Data.Entities;

namespace Tracker.Data.Services
{
    public class DataService
    {
        private readonly IKeyedRepository<int, User> _userRepo;
        private readonly IKeyedRepository<int, Truck> _truckRepo;
        private readonly IKeyedRepository<int, Location> _locationRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UserService Users { get; private set; }
        public TruckService Trucks { get; private set; }
        public LocationService Locations { get; private set; }

        public DataService(IKeyedRepository<int, User> userRepo, 
            IKeyedRepository<int, Truck> truckRepo,
            IKeyedRepository<int, Location> locationRepo,
            IUnitOfWork unitOfWork)
        {
            _userRepo = userRepo;
            _truckRepo = truckRepo;
            _locationRepo = locationRepo;
            _unitOfWork = unitOfWork;

            Users = new UserService(userRepo);
            Trucks = new TruckService(truckRepo);
            Locations = new LocationService(locationRepo);
        }

        public Location GetCurrentLocation(int truckId)
        {
            return _locationRepo
                .FilterBy(x => x.Truck.Id == truckId)
                .OrderByDescending(c => c.Timestamp)
                .FirstOrDefault();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}