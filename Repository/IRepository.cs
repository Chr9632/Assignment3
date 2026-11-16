public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAsyncAll(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}