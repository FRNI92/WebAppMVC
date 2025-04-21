using Domain.Dtos;
using Domain.FormModels;

namespace WebApplication1.ViewModels;

public class MemberViewModel
{
    public MemberFormModel FormModel { get; set; } = new()
    {
        Address = new AddressFormModel()
    };

    public List<MemberCardViewModel> MemberCards { get; set; } = new();
}