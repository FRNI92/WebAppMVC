using Domain.Dtos;
using IdentityDatabase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Business.Services;

//add private field of type userManager and your entity
public class UserService(UserManager<AppUserEntity> userManager)
{
    private readonly UserManager<AppUserEntity> _userManager = userManager;

    public async Task<bool> ExistsAsync(string email)
    {
        return await _userManager.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<AppUserEntity?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<bool> CreateAsync(SignUpDto dto)
    {
        if (dto == null)
            return false;

        var appUser = new AppUserEntity
        {
            UserName = dto.Email,
            Email = dto.Email
        };

        var result = await _userManager.CreateAsync(appUser, dto.PassMord);

        return result.Succeeded;
    }
}
