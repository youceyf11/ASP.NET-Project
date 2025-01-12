using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;

namespace ASP.NET.Models;

public class Repository<T> : IRepository<T> where T : class
{
    
    protected YoussefDbContext _context { get; set; }
    
    private DbSet<T> _dbSet { get; set; }

    public Repository(YoussefDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id, QueryOptions<T> options)
    {
        IQueryable<T> query = _dbSet;
        if (options.HasWhere)
        {
            query = query.Where(options.Where);
        }
        if(options.HasOrderBy)
        {
            query = query.OrderBy(options.OrderBy);
        }

        foreach (string include in options.GetIncludes())
        {
            query = query.Include(include);
        }

        var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
        string primaryKeyName = key?.Name;
        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();

    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }
}