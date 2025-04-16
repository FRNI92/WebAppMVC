
using Database.Data;
using Database.Entities;
using Database.Repos;

namespace Database.Repos;

public class StatusRepository : BaseRepository<StatusEntity>
{
    public StatusRepository(AppDbContext context) : base(context)
    {
    }
}

