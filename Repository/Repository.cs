
using Microsoft.EntityFrameworkCore;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly MovieContext _context;
    private readonly DbSet<T> _dbset;
    public Repository(MovieContext dbContext)
    {
        _context = dbContext;
        _dbset = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAsyncAll(CancellationToken cancellationToken)
    {
        return await _dbset.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbset.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbset.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbset.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbset.Remove(entity);
    }
}