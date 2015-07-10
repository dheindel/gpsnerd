namespace Cravens.Infrastructure.Membership
{
	public interface IFormsAuthentication
	{
		void SignIn(string userName, bool createPersistentCookie);
		void SignOut();
	}
}