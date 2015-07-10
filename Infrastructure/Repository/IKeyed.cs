namespace Cravens.Infrastructure.Repository
{
    public interface IKeyed<TKey>
    {
        TKey Id { get; }
    }
}