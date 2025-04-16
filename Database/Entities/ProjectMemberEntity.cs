
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

public class ProjectMemberEntity
{

    public int ProjectId { get; set; }
    public ProjectEntity Project { get; set; } = null!;

    public int MemberId { get; set; }
    public MemberEntity Member { get; set; } = null!;

    //public DbSet<ProjectMemberEntity> ProjectMembers { get; set; } // MOVE THIS TO AppDbCOntext later when I neeed many to many

}
