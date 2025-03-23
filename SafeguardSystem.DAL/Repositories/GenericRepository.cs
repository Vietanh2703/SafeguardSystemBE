using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SafeguardSystem.DAL.IRepositories;

namespace SafeguardSystem.DAL.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly SafeguardDbContext _context; // Change to protected
    private readonly DbSet<T> _dbSet;

    public GenericRepository(SafeguardDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public T Add(T entity)
    {
        _dbSet.Add(entity);
        return entity;
    }

    public bool Delete(T entity)
    {
        _dbSet.Remove(entity);
        return true;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByGuIdAsync(id);
        if (entity != null)
        {
            Delete(entity);
            await _context.SaveChangesAsync();
        }
    }

    public IQueryable<T> FindAll(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression).AsQueryable();
    }

    public IEnumerable<T> FindAllAsync(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression).AsEnumerable();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public IQueryable<T> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<List<T>> GetAllByListAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public async Task<List<T>> GetAllByListAsync(Expression<Func<T, bool>> expression, object include)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public T GetById(string id)
    {
        throw new NotImplementedException();
    }

    public T GetByGuid(Guid id)
    {
        return _dbSet.Find(id);
    }

    public async Task<T> GetByGuIdAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty", nameof(id));

        return await _dbSet.FindAsync(id);
    }


    public async Task<T> GetByIdsAsync(int id)
    {
        if (id == 0) throw new ArgumentException("Id cannot be zero", nameof(id));

        return await _dbSet.FindAsync(id);
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);

        return entity;
    }


    public void UpdateRange(List<T> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void RemoveRange(List<T> entities)
    {
        _dbSet.RemoveRange(entities);
    }


    public void AddRange(List<T> entity)
    {
        _dbSet.AddRange(entity);
    }

    public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    public async Task<T> GetByConditionAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.FirstOrDefaultAsync(expression);
    }

    public Task<List<T>> ToListAsync()
    {
        return _dbSet.ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentException("Id cannot be null or empty", nameof(id));

        return await _dbSet.FindAsync(id);
    }
}