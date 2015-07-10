using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Cravens.Infrastructure.Logging;
using Cravens.Infrastructure.Membership;
using Cravens.Infrastructure.Repository;
using DotNetOpenAuth.ApplicationBlock;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using DotNetOpenAuth.OpenId.RelyingParty;
using DotNetOpenAuth.OAuth;
using Tracker.Data.Entities;
using Tracker.Data.Services;

namespace TruckTrackerWeb.Controllers
{
	[HandleError]
	public class AccountController : BaseController
	{
        static private readonly OpenIdRelyingParty _openid = new OpenIdRelyingParty();
        static private readonly InMemoryTokenManager _tokenManager = new InMemoryTokenManager("consumer_key", "consumer_secret");

        private readonly IFormsAuthentication _formsAuth;
	    private readonly IUnitOfWork _unitOfWork;

        public AccountController(
            IFormsAuthentication formsAuthentication,
            IKeyedRepository<int, User> userRepository,
            IUnitOfWork unitOfWork,
            ILogger logger,
            DataService dataService)
            : base(logger, dataService)
        {
            _formsAuth = formsAuthentication;
            _unitOfWork = unitOfWork;
        }

	    public ActionResult LogIn()
		{
            // Stage 1: Display login form to user.
            return View();
		}

        public ActionResult OpenId(string openIdUrl)
        {
            var response = _openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(openIdUrl, out id))
                {
                    try
                    {
                        var request = _openid.CreateRequest(openIdUrl);
                        var fetch = new FetchRequest();
                        fetch.Attributes.AddRequired(WellKnownAttributes.Contact.Email);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.First);
                        fetch.Attributes.AddRequired(WellKnownAttributes.Name.Last);
                        request.AddExtension(fetch);
                        return request.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        _logger.Error("OpenID Exception...", ex);
                        return RedirectToAction("Login");
                    }
                }
                _logger.Info("OpenID Error...invalid url. url='" + openIdUrl + "'");
                return RedirectToAction("Login");
            }

            // Stage 3: OpenID Provider sending assertion response
            switch (response.Status)
            {
                case AuthenticationStatus.Authenticated:
                    var fetch = response.GetExtension<FetchResponse>();
                    string firstName = "";
                    string lastName = "";
                    string email = "";
                    if(fetch!=null)
                    {
                        firstName = fetch.GetAttributeValue(WellKnownAttributes.Name.First);
                        lastName = fetch.GetAttributeValue(WellKnownAttributes.Name.Last);
                        email = fetch.GetAttributeValue(WellKnownAttributes.Contact.Email);
                    }
                    return CreateUser(response.ClaimedIdentifier, firstName, lastName, email);
                case AuthenticationStatus.Canceled:
                    _logger.Info("OpenID: Cancelled at provider.");
                    return RedirectToAction("Login");
                case AuthenticationStatus.Failed:
                    _logger.Error("OpenID Exception...", response.Exception);
                    return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }

        public ActionResult OAuth(string returnUrl)
        {
            var twitter = new WebConsumer(TwitterConsumer.ServiceDescription, _tokenManager);
            string url = Request.Url.ToString().Replace("OAuth", "OAuthCallback");
            var callBackUrl = new Uri(url);
            twitter.Channel.Send(twitter.PrepareRequestUserAuthorization(callBackUrl, null, null));

            return RedirectToAction("Login");
        }

        public ActionResult OAuthCallback()
        {
            var twitter = new WebConsumer(TwitterConsumer.ServiceDescription, _tokenManager);
            var accessTokenResponse = twitter.ProcessUserAuthorization();
            if(accessTokenResponse!=null)
            {
                string userName = accessTokenResponse.ExtraData["screen_name"];
                return CreateUser(userName, null, null, null);
            }
            _logger.Error("OAuth: No access token response!");
            return RedirectToAction("Login");
        }

		[Authorize]
		public ActionResult LogOff()
		{
			_formsAuth.SignOut();
		    Session["UserName"] = null;
		    Session["FullName"] = null;

			return RedirectToAction("Index", "Home");
		}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(User user)
        {
            // Validate the information.
            if(_dataService.Users.FindBy(x=>x.UserName == user.UserName)==null)
            {
                IEnumerable<string> brokenRules;
                _dataService.Users.Add(user, out brokenRules);
               _unitOfWork.Commit();
            }
            Session["UserName"] = user.UserName;
            string fullName = user.FirstName + " " + user.LastName;
            Session["FullName"] = fullName;
            _formsAuth.SignIn(fullName, false);

            Growl("Welcome", "Thanks for registering and welcome to Truck Tracker.");

            return RedirectToAction("Index", "Truck");
        }

        private ActionResult CreateUser(string userName, string firstName, string lastName, string email)
        {
            User user = _dataService.Users.FindBy(x => x.UserName == userName);
            if(user==null)
            {
                user = new User
                           {
                               UserName = userName,
                               FirstName = firstName,
                               LastName = lastName,
                               Email = email
                           };
                ViewData["HideLogin"] = true;
                return View("CreateUser", user);
            }
            Session["UserName"] = userName;
            string fullName = user.FirstName + " " + user.LastName;
            Session["FullName"] = fullName;
            _formsAuth.SignIn(fullName, false);
            return RedirectToAction("Index", "Truck");
        }
	}
}


