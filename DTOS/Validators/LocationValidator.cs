using System.Collections.Generic;
using System.Linq;
using Cravens.Infrastructure.Validation;
using Tracker.Data.Entities;

namespace Tracker.Data.Validators
{
    public class LocationValidator : IValidator<Location>
    {
        public bool IsValid(Location entity)
        {
            return BrokenRules(entity).Count() == 0;
        }

        public IEnumerable<string> BrokenRules(Location entity)
        {
            if(entity.Latitude<-90 || entity.Latitude>90.0)
            {
                yield return "Latitude range is -90.0 to 90.0";
            }
            if(entity.Longitude<-180.0 || entity.Latitude>180.0)
            {
                yield return "Longitude range is -180.0 to 180.0";
            }

            yield break;
        }
    }
}