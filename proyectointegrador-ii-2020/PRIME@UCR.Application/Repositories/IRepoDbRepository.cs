namespace PRIME_UCR.Application.Repositories
{
    public interface IRepoDbRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
    }
}