using FluentNHibernate.Mapping;
using Tracker.Data.Entities;

namespace Tracker.Data.NHibernateLayer.Maps
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("users");
            Id(x => x.Id);
            Map(x => x.UserName);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Email);
            HasMany(x => x.Trucks).LazyLoad().Inverse().Cascade.All();
        }
    }
}