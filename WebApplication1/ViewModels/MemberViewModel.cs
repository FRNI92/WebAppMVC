using Domain.Dtos;
using Domain.FormModels;

namespace WebApplication1.ViewModels;

public class MemberViewModel
{

    public MemberFormModel FormModel { get; set; } = new MemberFormModel
    {
        Address = new AddressFormModel()
    };

    public List<MemberDto> MemberList { get; set; } = new();
}
