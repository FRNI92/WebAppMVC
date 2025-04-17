using Database.Data;
using Database.Entities;
using Database.ReposResult;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Database.Repos;

public class ProjectRepository(AppDbContext context) : BaseRepository<ProjectEntity>(context)
{

    private readonly AppDbContext _context = context;

    public async Task<List<ProjectEntity>> GetAllWithFullRelationsAsync()
    {
        return await _context.Projects
            .Include(p => p.Client)
            .Include(p => p.Status)
            .Include(p => p.ProjectMembers)
                .ThenInclude(pm => pm.Member)
            .ToListAsync();
    }


    public override async Task<ReposResult<ProjectEntity>> UpdateAsync(Expression<Func<ProjectEntity, bool>> expression, ProjectEntity updatedEntity)
    {
        var result = new ReposResult<ProjectEntity>();

        try
        {
            var existing = await _context.Projects
                .Include(p => p.ProjectMembers)
                .FirstOrDefaultAsync(expression);

            if (existing == null)
            {
                result.Succeeded = false;
                result.StatusCode = 404;
                result.Error = "Project not found.";
                return result;
            }

            // Uppdatera main properties
            _context.Entry(existing).CurrentValues.SetValues(updatedEntity);

            // Uppdatera ProjectMembers
            existing.ProjectMembers.Clear();
            foreach (var pm in updatedEntity.ProjectMembers)
            {
                existing.ProjectMembers.Add(new ProjectMemberEntity
                {
                    ProjectId = existing.Id,
                    MemberId = pm.MemberId
                });
            }

            result.Succeeded = true;
            result.StatusCode = 200;
            result.Result = existing;
        }
        catch (Exception ex)
        {
            result.Succeeded = false;
            result.StatusCode = 500;
            result.Error = ex.Message;
        }

        return result;
    }
}