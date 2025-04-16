
using Microsoft.EntityFrameworkCore;

namespace Database.Entities;

public class ProjectMemberEntity
{

    public int ProjectId { get; set; }
    public ProjectEntity Project { get; set; } = null!;

    public int MemberId { get; set; }
    public MemberEntity Member { get; set; } = null!;


}
