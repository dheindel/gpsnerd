using System;
using Cravens.Infrastructure.Repository;

namespace Tracker.Data.Entities
{
	public class Location : IKeyed<int>
	{
		public virtual int Id { get; private set; }
		public virtual DateTime Timestamp { get; set; }
		public virtual double Latitude { get; set; }
		public virtual double Longitude { get; set; }
		public virtual Truck Truck { get; set; }
	}
}
