using Database.Data;
using Database.Entities;
using Database.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repos
{
    public class AddressRepository : BaseRepository<AddressEntity>
    {
        public AddressRepository(AppDbContext context) : base(context)
        {
        }
    }
}
