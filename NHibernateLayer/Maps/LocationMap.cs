using FluentNHibernate.Mapping;
using Tracker.Data.Entities;

namespace Tracker.Data.NHibernateLayer.Maps
{
	public class LocationMap : ClassMap<Location>
	{
		public LocationMap()
		{
			Table("locations");
			Id(x => x.Id);
			Map(x => x.Timestamp).Column("DateTime");
			Map(x => x.Latitude);
			Map(x => x.Longitude);
			References(x => x.Truck).Column("TruckId");
		}
	}
}