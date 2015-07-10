using System;

namespace Cravens.Infrastructure.Repository
{
	public interface IUnitOfWork : IDisposable
	{
		void Commit();
		void Rollback();
	}
}