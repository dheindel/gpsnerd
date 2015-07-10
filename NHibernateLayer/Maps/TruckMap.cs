using FluentNHibernate.Mapping;
using Tracker.Data.Entities;

namespace Tracker.Data.NHibernateLayer.Maps
{
	public class TruckMap : ClassMap<Truck>
	{
		public TruckMap()
		{
			Table("trucks");
			Id(x => x.Id);
		    Map(x => x.IsPrivate);
			Map(x => x.Name);
			Map(x => x.Type);
			Map(x => x.PlateNumber);
		    References(x => x.User).Column("UserId");
			HasOne(x => x.Driver).LazyLoad().Cascade.All();
			HasMany(x => x.Locations).LazyLoad().Inverse().Cascade.All();
		}
	}
}