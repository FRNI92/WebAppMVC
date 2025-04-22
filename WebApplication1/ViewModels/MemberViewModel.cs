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



    //to connect member to appuser if we want
    public string? ConnectedAppUserId { get; set; } // för bindning från dropdown
    public IEnumerable<AppUserEntity> AllUsers { get; set; } = new List<AppUserEntity>();
}