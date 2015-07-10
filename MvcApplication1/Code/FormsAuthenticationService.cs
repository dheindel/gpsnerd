using System.Web.Security;
using Cravens.Infrastructure.Membership;

namespace TruckTrackerWeb.Code
{
	public class FormsAuthenticationService : IFormsAuthentication
	{
		public void SignIn(string userName, bool createPersistentCookie)
		{
			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}
		public void SignOut()
		{
			FormsAuthentication.SignOut();
		}
	}
}