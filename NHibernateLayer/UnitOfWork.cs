using System;
using System.Data;
using Cravens.Infrastructure.Repository;
using NHibernate;

namespace Tracker.Data.NHibernateLayer
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ISessionFactory _sessionFactory;
		private readonly ITransaction _transaction;
		public ISession Session { get; private set; }

		public UnitOfWork(ISessionFactory sessionFactory)
		{
			_sessionFactory = sessionFactory;
			Session = _sessionFactory.OpenSession();
			Session.FlushMode = FlushMode.Auto;
			_transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted);
		}
        ~UnitOfWork()
        {
            Dispose();
        }

		public void Dispose()
		{
            lock (Session)
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                }
            }
            GC.SuppressFinalize(this);
		}

		public void Commit()
		{
			if(!_transaction.IsActive)
			{
				throw new InvalidOperationException("No active transation");
			}
			_transaction.Commit();
		}

		public void Rollback()
		{
			if(_transaction.IsActive)
			{
				_transaction.Rollback();
			}
		}
	}
}