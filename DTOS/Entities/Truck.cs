using Cravens.Infrastructure.Repository;

namespace Tracker.Data.Entities
{
	public class Truck : IKeyed<int>
	{
		public virtual int Id { get; private set; }
        public virtual User User { get; set; }
        public virtual bool IsPrivate { get; set; }
		public virtual string Name { get; set; }
		public virtual string Type { get; set; }
		public virtual string PlateNumber { get; set; }
	}
}