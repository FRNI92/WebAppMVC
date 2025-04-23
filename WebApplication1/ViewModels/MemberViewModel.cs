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
    // Bindas från dropdown när admin väljer en AppUser
    public string? ConnectedAppUserId { get; set; }

    // För att i t.ex. layouten/vyn kunna visa namn/image
    public MemberDto? CurrentMember { get; set; }

    public MemberDto? LoggedInUserMember { get; set; }
}
