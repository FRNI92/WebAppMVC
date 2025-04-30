using Domain.Dtos;
using Domain.FormModels;
using IdentityDatabase.Entities;

namespace WebApplication1.ViewModels;

public class MemberViewModel
{
    public MemberFormModel FormModel { get; set; } = new()
    {
        Address = new AddressFormModel()
    };

    public List<MemberCardViewModel> MemberCards { get; set; } = new();


    public IEnumerable<AppUserEntity> AllUsers { get; set; } = new List<AppUserEntity>();
    
    public string? ConnectedAppUserId { get; set; } // Admin can connect member to appuser


    public MemberDto? CurrentMember { get; set; }    // to be able to show correct logged in user

    public MemberDto? LoggedInUserMember { get; set; }
}
