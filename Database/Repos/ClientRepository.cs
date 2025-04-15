using Database.Data;
using Database.Entities;

namespace Database.Repos;

public class ClientRepository : BaseRepository<ClientEntity>
{
    public ClientRepository(AppDbContext context) : base(context)
    {
    }
}
