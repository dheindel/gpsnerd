using System.Collections.Generic;

namespace Cravens.Infrastructure.Validation
{
    public interface IValidator<T>
    {
        bool IsValid(T entity);
        IEnumerable<string> BrokenRules(T entity);
    }
}
