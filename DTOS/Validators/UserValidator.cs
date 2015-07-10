using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Cravens.Infrastructure.Repository;
using Cravens.Infrastructure.Validation;
using Tracker.Data.Entities;

namespace Tracker.Data.Validators
{
    public class UserValidator : IValidator<User>
    {
        private readonly IKeyedRepository<int, User> _userRepo;
        public UserValidator(IKeyedRepository<int, User> userRepo)
        {
            _userRepo = userRepo;
        }

        public bool IsValid(User entity)
        {
            return BrokenRules(entity).Count() == 0;
        }

        public IEnumerable<string> BrokenRules(User entity)
        {
            if(string.IsNullOrEmpty(entity.UserName))
            {
                yield return "User name is required.";
            }
            else
            {
                if(_userRepo.FindBy(x=>x.UserName==entity.UserName)!=null)
                {
                    yield return "User name is already taken.";
                }
            }
            if(string.IsNullOrEmpty(entity.FirstName))
            {
                yield return "First name is required.";
            }
            if(string.IsNullOrEmpty(entity.LastName))
            {
                yield return "Last name is required.";
            }
            if(string.IsNullOrEmpty(entity.Email))
            {
                yield return "Email address is required.";
            }
            else
            {
                const string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                                             + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                             + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                             + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                             + @"[a-zA-Z]{2,}))$";
                Regex regexStrict = new Regex(patternStrict);
                if(!regexStrict.IsMatch(entity.Email))
                {
                    yield return "Email must be valid format.";
                }
            }

            yield break;
        }
    }
}
