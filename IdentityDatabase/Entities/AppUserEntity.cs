using Microsoft.AspNetCore.Identity;

namespace IdentityDatabase.Entities;

public class AppUserEntity : IdentityUser
{
    public int? MemberId { get; set; }
}
