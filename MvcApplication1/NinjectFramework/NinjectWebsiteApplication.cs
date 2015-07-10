using System;
using System.Web;
using System.Web.Mvc;
using Cravens.Infrastructure.Logging;
using Cravens.Infrastructure.Repository;
using Ninject.Web.Mvc;
using Ninject.Activation.Strategies;
using Ninject;
using TruckTrackerWeb.Code;

namespace TruckTrackerWeb.NinjectFramework
{
	public class NinjectWebsiteApplication : NinjectHttpApplication
	{
        public NinjectWebsiteApplication()
        {
            Error += NinjectWebsiteApplication_Error;
            BeginRequest += NinjectWebsiteApplication_BeginRequest;
            EndRequest += NinjectWebsiteApplication_EndRequest;
        }

        void NinjectWebsiteApplication_Error(object sender, System.EventArgs e)
        {
            ILogger logger = Kernel.Get<ILogger>();
            logger.Error("ooooooooooooooooooo APPLICATION EXCEPTION ooooooooooooooooooooo");
            if(HttpContext.Current!=null && HttpContext.Current.Server!=null)
            {
                Exception lastException = HttpContext.Current.Server.GetLastError();
                logger.Error("EXCEPTION INFO: ", lastException);
            }
            else
            {
                logger.Error("NO MORE INFO!");
            }
            
        }

		protected override void OnApplicationStarted()
		{
            // Register mobile view engine
            //
		    ViewEngines.Engines.Clear();
		    ViewEngines.Engines.Add(new MobileCapableWebFormViewEngine());

			var bootstrapper = Kernel.Get<Bootstrapper>();
			AreaRegistration.RegisterAllAreas();
			bootstrapper.RegisterRoutes();

            ILogger logger = Kernel.Get<ILogger>();
            logger.Debug("ooooooooooooooooooo APPLICATION STARTED ooooooooooooooooooooo");
        }

        void NinjectWebsiteApplication_EndRequest(object sender, System.EventArgs e)
        {
            ILogger logger = Kernel.Get<ILogger>();

            // Normally Ninject does a great job of handling the lifetime of our objects. 
            //  However, it does this using WeakReferences to the GC. However, the GC collection
            //  cycle may not be sufficient to prevent excess DB connections. The following
            //  lines take explicit control over this lifetime.
            //
            IUnitOfWork uow = Kernel.Get<IUnitOfWork>();
            uow.Dispose();

            logger.Debug("**********REQUEST END******************");
        }

        void NinjectWebsiteApplication_BeginRequest(object sender, System.EventArgs e)
        {
            ILogger logger = Kernel.Get<ILogger>();
            logger.Debug("**********REQUEST BEGIN****************");
            logger.Debug("**** URL = " + Request.Url.AbsolutePath);
        }

		protected override IKernel CreateKernel()
		{
            var kernel = new StandardKernel();
			kernel.Components.Add<IActivationStrategy, MyMonitorActivationStrategy>();

			kernel.Load<AppModule>();

			return kernel;
		}
	}
}