using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cravens.Infrastructure.Repository;
using Tracker.Data.Entities;
using Tracker.Data.Validators;

namespace Tracker.Data.Services
{
    public class LocationService
    {
        private readonly IKeyedRepository<int, Location> _repo;
        private readonly LocationValidator _validation;

        public LocationService(IKeyedRepository<int, Location> locationRepo)
        {
            _repo = locationRepo;
            _validation = new LocationValidator();
        }

        public IList<Location> All()
        {
            return _repo.All().ToList();
        }

        public Location FindBy(Expression<Func<Location, bool>> expression)
        {
            return _repo.FindBy(expression);
        }

        public IList<Location> FilterBy(Expression<Func<Location, bool>> expression)
        {
            return _repo.FilterBy(expression).ToList();
        }

        public bool Add(Location entity, out IEnumerable<string> brokenRules)
        {
            if(!_validation.IsValid(entity))
            {
                brokenRules = _validation.BrokenRules(entity);
                return false;
            }
            brokenRules = null;
            return _repo.Add(entity);
        }

        public bool Add(IEnumerable<Location> items, out IEnumerable<string> brokenRules)
        {
            foreach (Location location in items)
            {
                if (!_validation.IsValid(location))
                {
                    brokenRules = _validation.BrokenRules(location);
                    return false;
                }
            }
            brokenRules = null;
            return _repo.Add(items);
        }

        public bool Update(Location entity, out IEnumerable<string> brokenRules)
        {
            if (!_validation.IsValid(entity))
            {
                brokenRules = _validation.BrokenRules(entity);
                return false;
            }
            brokenRules = null;
            return _repo.Update(entity);
        }

        public bool Delete(Location entity)
        {
            return _repo.Delete(entity);
        }

        public bool Delete(IEnumerable<Location> entities)
        {
            return _repo.Delete(entities);
        }

        public Location FindBy(int id)
        {
            return _repo.FindBy(id);
        }
    }
}