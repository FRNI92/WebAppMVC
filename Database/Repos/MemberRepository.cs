
using Database.Data;
using Database.Entities;

namespace Database.Repos;

public class MemberRepository : BaseRepository<MemberEntity>
{
    public MemberRepository(AppDbContext context) : base(context)
    {
    }
}

