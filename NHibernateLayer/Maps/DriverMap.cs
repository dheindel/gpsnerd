using FluentNHibernate.Mapping;
using Tracker.Data.Entities;

namespace Tracker.Data.NHibernateLayer.Maps
{
	public class DriverMap : ClassMap<Driver>
	{
		public DriverMap()
		{
			Table("drivers");
			Id(x => x.Id);
			Map(x => x.FirstName);
			Map(x => x.LastName);
			References(x => x.Truck).Column("TruckId");
		}
	}
}
