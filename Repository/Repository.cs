
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

    public async Task<IEnumerable<T>> GetAsyncAll()
    {
        return await _dbset.ToListAsync();
    }
}