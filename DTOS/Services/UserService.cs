using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cravens.Infrastructure.Repository;
using Tracker.Data.Entities;
using Tracker.Data.Validators;

namespace Tracker.Data.Services
{
    public class UserService
    {
        private readonly IKeyedRepository<int, User> _userRepo;
        private readonly UserValidator _validation;

        public UserService(IKeyedRepository<int, User> userRepo)
        {
            _userRepo = userRepo;
            _validation = new UserValidator(userRepo);
        }
        public IList<User> All()
        {
            return _userRepo.All().ToList();
        }

        public User FindBy(Expression<Func<User, bool>> expression)
        {
            return _userRepo.FindBy(expression);
        }

        public IList<User> FilterBy(Expression<Func<User, bool>> expression)
        {
            return _userRepo.FilterBy(expression).ToList();
        }

        public bool Add(User entity, out IEnumerable<string> brokenRules)
        {
            if(!_validation.IsValid(entity))
            {
                brokenRules = _validation.BrokenRules(entity);
                return false;
            }
            brokenRules = null;
            return _userRepo.Add(entity);
        }

        public bool Add(IEnumerable<User> items, out IEnumerable<string> brokenRules)
        {
            foreach (User item in items)
            {
                if(!_validation.IsValid(item))
                {
                    brokenRules = _validation.BrokenRules(item);
                    return false;
                }
            }
            brokenRules = null;
            return _userRepo.Add(items);
        }

        public bool Update(User entity, out IEnumerable<string> brokenRules)
        {
            if(!_validation.IsValid(entity))
            {
                brokenRules = _validation.BrokenRules(entity);
                return false;
            }
            brokenRules = null;
            return _userRepo.Update(entity);
        }

        public bool Delete(User entity)
        {
            return _userRepo.Delete(entity);
        }

        public bool Delete(IEnumerable<User> entities)
        {
            return _userRepo.Delete(entities);
        }

        public User FindBy(int id)
        {
            return _userRepo.FindBy(id);
        }
    }
}
