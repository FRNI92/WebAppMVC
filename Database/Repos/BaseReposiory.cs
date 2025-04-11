
using Database.Data;
using Database.Entities;
using Database.ReposResult;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq.Expressions;
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

    public virtual async Task<ReposResult<int>> SaveAsync()
    {
        try
        {
            //savechanges returns a value of how many rows were saved. so it can succed but make no changes if no rows were affected
            Console.WriteLine("Saving Something");
            var result = await _context.SaveChangesAsync();

            return new ReposResult<int>
            {
                Succeeded = true,
                StatusCode = 200,
                Result = result
            };
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error In SaveAsync: {ex.Message}");
            return new ReposResult<int>
            {
                Succeeded = false,
                StatusCode = 500,
                Error = $"Error in SaveAsync: {ex.Message}",
                Result = 0
            };
        }
    }

    //public virtual async Task<ReposResult<IEnumerable<T>>> GetAllAsync<T>()
    //{
    //    try
    //    {
    //        var allEntities = await _dbSet.ToListAsync();
    //        return new ReposResult<IEnumerable<T>>
    //        {
    //            Succeeded = true,
    //            StatusCode = 200,
    //            Result = result
    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine($"Error In GetAllAsync:{ex.Message} {ex.StackTrace}");
    //        return null!;
    //    }
    //}

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

    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity)
    {
        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(expression);
            if (existingEntity != null && updatedEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                return existingEntity;
            }
            return null!;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateAsync:{ex.Message}");
            return null!;
        }
    }

    public virtual async Task<bool> RemoveAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expression);
            if (entity != null)
            {
                _context.Remove(entity);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error With DeleteAsync{ex.Message}");
            return false;
        }
    }

    public virtual async Task<bool> DoesEntityExistAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            return await _dbSet.AnyAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error with DoesEntityExistAsync{ex.Message}");
            return false;
        }
    }
}
