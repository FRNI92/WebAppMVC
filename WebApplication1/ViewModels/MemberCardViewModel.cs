using Domain.Dtos;
using Domain.FormModels;

namespace WebApplication1.ViewModels
{
    public class MemberCardViewModel
    {
        public MemberDto Member { get; set; } = new();
        public MemberFormModel FormModel { get; set; } = new()
        {
            Address = new AddressFormModel()
        };
        public AddressDto Address { get; set; } = new();
    }
}