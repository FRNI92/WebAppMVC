using Database.Data;
using Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repos
{
   public class ProjectMemberRepository : BaseRepository<ProjectMemberEntity>
    {
        public ProjectMemberRepository(AppDbContext context) : base(context)
        {
        }
    }

}
