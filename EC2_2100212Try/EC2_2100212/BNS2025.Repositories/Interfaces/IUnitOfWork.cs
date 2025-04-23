namespace BNS2025.Repositories.Interfaces
{
    public interface IUnitOfWork: IDisposable
    {
        IGenericRepository<T> GenericRepository<T>() where T : class;
        Task Save();
    }
}
