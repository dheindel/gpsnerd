using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cravens.Infrastructure.Repository;
using Tracker.Data.Entities;
using Tracker.Data.Validators;

namespace Tracker.Data.Services
{
    public class TruckService
    {
        private readonly IKeyedRepository<int, Truck> _truckRepo;
        private readonly TruckValidator _validation;

        public TruckService(IKeyedRepository<int, Truck> truckRepo)
        {
            _truckRepo = truckRepo;
            _validation = new TruckValidator();
        }

        public IList<Truck> All()
        {
            return _truckRepo.All().ToList();
        }

        public Truck FindBy(Expression<Func<Truck, bool>> expression)
        {
            return _truckRepo.FindBy(expression);
        }

        public IList<Truck> FilterBy(Expression<Func<Truck, bool>> expression)
        {
            return _truckRepo.FilterBy(expression).ToList();
        }

        public bool Add(Truck entity, out IEnumerable<string> brokenRules)
        {
            if(!_validation.IsValid(entity))
            {
                brokenRules = _validation.BrokenRules(entity);
                return false;
            }
            brokenRules = null;
            return _truckRepo.Add(entity);
        }

        public bool Add(IEnumerable<Truck> items, out IEnumerable<string> brokenRules)
        {
            foreach (Truck truck in items)
            {
                if (!_validation.IsValid(truck))
                {
                    brokenRules = _validation.BrokenRules(truck);
                    return false;
                }
            }
            brokenRules = null;
            return _truckRepo.Add(items);
        }

        public bool Update(Truck entity, out IEnumerable<string> brokenRules)
        {
            if (!_validation.IsValid(entity))
            {
                brokenRules = _validation.BrokenRules(entity);
                return false;
            }
            brokenRules = null;
            return _truckRepo.Update(entity);
        }

        public bool Delete(Truck entity)
        {
            return _truckRepo.Delete(entity);
        }

        public bool Delete(IEnumerable<Truck> entities)
        {
            return _truckRepo.Delete(entities);
        }

        public Truck FindBy(int id)
        {
            return _truckRepo.FindBy(id);
        }
    }
}