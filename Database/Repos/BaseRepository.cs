
using Database.Data;
using Database.Entities;
using Database.ReposResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ObjectiveC;
namespace Database.Repos;


public abstract class BaseRepository<TEntity>(AppDbContext context) where TEntity : class
{
    private readonly AppDbContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private IDbContextTransaction _transaction = null!;

    public virtual async Task BeginTransactionAsync()
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
    }

    public virtual async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }

    }

    public virtual async Task RollBackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null!;
        }
    }



    public virtual async Task <ReposResult<TEntity>> AddAsync(TEntity entity)
    {
        try
        {
            if (entity == null)
            {
                return new ReposResult<TEntity>
                {
                    Succeeded = false,
                    StatusCode = 400,
                    Error = "Entity is null",
                    Result = entity
                };
            }

            await _dbSet.AddAsync(entity);
            return new ReposResult<TEntity>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = null!
            };
        }
        catch (Exception ex)
        {
            return new ReposResult<TEntity>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = $"Error In AddAsync: {ex.Message}",
                Result = null!
            };
        }

    }

  
    //getallasync knows what type from <IEnumerable<TEntity>>
    public virtual async Task<ReposResult<IEnumerable<TEntity>>> GetAllAsync
        (
            Expression<Func<TEntity, bool>>? where = null,
            Expression<Func<TEntity, object>>? sortBy = null,
            bool orderByDescending = false,
            params Expression<Func<TEntity, object>>[] includes
        )
    {
        try
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
                query = query.Where(where);

            if (includes != null && includes.Length > 0)
            {
                foreach (var include in includes)
                    query = query.Include(include);
            }

            if (sortBy != null)
            {
                query = orderByDescending
                    ? query.OrderByDescending(sortBy)
                    : query.OrderBy(sortBy);
            }

            var entities = await query.ToListAsync();

            return new ReposResult<IEnumerable<TEntity>>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ReposResult<IEnumerable<TEntity>>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = $"Error in GetAllAsync: {ex.Message}"
            };
        }
    }

    //public: acces everywhere
    // virtual: can t be be overwriten with an override. ex from projectrepos
    // async: the method is async and returns a task. need to use await in the codeblock
    // returns resposresult of any type. can also just have success true or false and a statuscode
    // GetAsync: method name and what type it can handle.
    // expression says what I want from the database. works like filter. select * FROM 
    public virtual async Task<ReposResult<TEntity>> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expression);

            if (entity == null)
            {
                return new ReposResult<TEntity>
                {
                    Succeeded = false,
                    StatusCode = 404,
                    Error = "Entity not found"
                };
            }

            return new ReposResult<TEntity>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ReposResult<TEntity>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = $"Error in GetAsync: {ex.Message}"
            };
        }
    }

    public virtual async Task<ReposResult<int>> SaveAsync()
    {
        var result = new ReposResult<int>();
        try
        {
            Console.WriteLine("Saving Something");
            result.Result = await _context.SaveChangesAsync();
            result.Succeeded = true;
            result.StatusCode = 200;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in SaveAsync: {ex.Message} | Inner: {ex.InnerException?.Message}");
            result.Succeeded = false;
            result.StatusCode = 500;
            result.Error = $"Error in SaveAsync: {ex.Message}";
            result.Result = 0;
        }

        return result;
    }

    
    public virtual async Task<ReposResult<TEntity>> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity)
    {
        var result = new ReposResult<TEntity>();
        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(expression);
            if (existingEntity != null && updatedEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                
                result.Succeeded = true;
                result.StatusCode = 200;
                result.Result = existingEntity;
                return result;
            }

            result.Succeeded = false;
            result.StatusCode = 404;
            result.Error = "Entity not found";
            return result;
        }
        catch (Exception ex)
        {
            result.Succeeded = false;
            result.StatusCode = 500;
            result.Error = $"Error in UpdateAsync: {ex.Message}";
            return result;
        }
    }

    public virtual async Task<ReposResult<bool>> RemoveAsync(Expression<Func<TEntity, bool>> expression)
    {
        var result = new ReposResult<bool>();
        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expression);
            if (entity != null)
            {
                _context.Remove(entity);
                result.Result = true;
                result.Succeeded = true;
                result.StatusCode = 200;
                return result;
            }

            result.Result = false;
            result.Succeeded = false;
            result.StatusCode = 404;
            result.Error = "Entity not found.";
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error With DeleteAsync {ex.Message}");
            result.Result = false;
            result.Succeeded = false;
            result.StatusCode = 500;
            result.Error = "Failed to remove entity.";
            return result;
        }
    }

    public virtual async Task<ReposResult<bool>> DoesEntityExistAsync(Expression<Func<TEntity, bool>> expression)
    {
        var result = new ReposResult<bool>();
        try
        {
            result.Result = await _dbSet.AnyAsync(expression);
            result.Succeeded = true;
            result.StatusCode = 200;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error with DoesEntityExistAsync {ex.Message}");
            result.Succeeded = false;
            result.StatusCode = 500;
            result.Error = "Failed to check if entity exists.";
        }

        return result;
    }
}
