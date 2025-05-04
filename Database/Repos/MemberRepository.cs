
using Database.Data;
using Database.Entities;
using Database.ReposResult;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Database.Repos;

public class MemberRepository(AppDbContext context) : BaseRepository<MemberEntity>(context)
{
    private readonly AppDbContext _context = context;

    public override async Task<ReposResult<MemberEntity>> UpdateAsync(Expression<Func<MemberEntity, bool>> expression, MemberEntity updatedEntity)
    {
        var result = new ReposResult<MemberEntity>();

        try
        {
            var existing = await _context.Members
                .Include(m => m.Address) //inclued Address to access related address data 
                .FirstOrDefaultAsync(expression);

            if (existing == null)
            {
                result.Succeeded = false;
                result.StatusCode = 404;
                result.Error = "Member not found.";
                return result;
            }

            _context.Entry(existing).CurrentValues.SetValues(updatedEntity);

            result.Succeeded = true;
            result.StatusCode = 200;
            result.Result = existing;
        }
        catch (Exception ex)
        {
            result.Succeeded = false;
            result.StatusCode = 500;
            result.Error = $"Error updating member: {ex.Message}";
        }

        return result;
    }
}

