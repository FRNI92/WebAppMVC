
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDatabase.Data;

//add identitydbcontext to inherit what it can do. specifiy your entity.
//generate ctor and remove protected
//add you context inside<> after dbcontextoptions
// use primary ctor
// then register dbcontext in program
public class IdentityAppContext(DbContextOptions<IdentityAppContext> options) : IdentityDbContext<AppUserEntity>(options)
{
}
