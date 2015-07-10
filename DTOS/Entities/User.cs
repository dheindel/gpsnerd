using Cravens.Infrastructure.Repository;

namespace Tracker.Data.Entities
{
    public class User : IKeyed<int>
    {
        public virtual int Id { get; private set; }
        public virtual string UserName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
    }
}