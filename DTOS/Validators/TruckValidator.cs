using System.Collections.Generic;
using System.Linq;
using Cravens.Infrastructure.Validation;
using Tracker.Data.Entities;

namespace Tracker.Data.Validators
{
    public class TruckValidator : IValidator<Truck>
    {
        public bool IsValid(Truck entity)
        {
            return BrokenRules(entity).Count() == 0;
        }

        public IEnumerable<string> BrokenRules(Truck entity)
        {
            if(string.IsNullOrEmpty(entity.Name))
            {
                yield return "Truck name is required.";
            }
            if(string.IsNullOrEmpty(entity.Type))
            {
                yield return "Truck type is required.";
            }
            if(string.IsNullOrEmpty(entity.PlateNumber))
            {
                yield return "Plate number is required.";
            }
        }
    }
}