using Cravens.Infrastructure.Logging;
using Cravens.Infrastructure.Membership;
using Cravens.Infrastructure.Repository;
using Logging;
using NHibernate;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Tracker.Data.Entities;
using Tracker.Data.NHibernateLayer;
using Tracker.Data.Services;
using TruckTrackerWeb.Code;

namespace TruckTrackerWeb.NinjectFramework
{
	public class AppModule : NinjectModule
	{
		public override void Load()
		{
			// Wire up the logger
			//
			Bind<ILogger>().To<Log4NetLogger>().InSingletonScope();
			ILogger logger = Kernel.Get<ILogger>();
			logger.Info("Application Started");

			// Wire up the NHibernate repository
			//
			logger.Info("Binding NHibernate dependencies.");
			NHibernateHelper helper = new NHibernateHelper();
			Bind<ISessionFactory>().ToConstant(helper.SessionFactory)
				.InSingletonScope();
		    Bind<TripSessions>().ToSelf()
		        .InSingletonScope();

			Bind<IUnitOfWork>().To<UnitOfWork>()
				.InRequestScope();
			Bind<ISession>().ToProvider(new SessionProvider())
                .InRequestScope();
			Bind<IKeyedRepository<int, Truck>>().To<Repository<Truck>>()
                .InRequestScope();
		    Bind<IKeyedRepository<int, User>>().To<Repository<User>>()
		        .InRequestScope();
		    Bind<IKeyedRepository<int, Location>>().To<Repository<Location>>()
		        .InRequestScope();

		    Bind<DataService>().ToSelf()
		        .InRequestScope();

		    Bind<IFormsAuthentication>().To<FormsAuthenticationService>().InRequestScope();
		}
	}

	public class SessionProvider : Provider<ISession>
	{
		protected override ISession CreateInstance(IContext context)
		{
			UnitOfWork unitOfWork = (UnitOfWork)context.Kernel.Get<IUnitOfWork>();
			return unitOfWork.Session;
		}
	}

}