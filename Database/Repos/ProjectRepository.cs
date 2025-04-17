using Database.Data;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

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

}