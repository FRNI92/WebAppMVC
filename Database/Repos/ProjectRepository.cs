using Database.Data;
using Database.Entities;

namespace Database.Repos;

public class ProjectRepository(AppDbContext context) : BaseRepository<ProjectEntity>(context)
{

    private readonly AppDbContext _context = context;
}
