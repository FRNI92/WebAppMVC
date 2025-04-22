using Domain.Dtos;
using Domain.FormModels;

namespace WebApplication1.ViewModels
{
    public class ProjectViewModels
    {
        public ProjectFormModel FormModel { get; set; } = new();
        public IEnumerable<ProjectFormDto> ProjectList { get; set; } = new List<ProjectFormDto>();
        public IEnumerable<ClientDto> Clients { get; set; } = new List<ClientDto>();
        public IEnumerable<MemberDto> Members { get; set; } = new List<MemberDto>();
        public IEnumerable<StatusDto> Status { get; set; } = new List<StatusDto>();





        // LoggedInUserMember is user to show info about the person logged in
        // AppUserEntity has a connection (MemberId) to the right MemberEntity in the system.
        // To preven sending the full IdentityUser to the view,
        // I get the correct Member and maps it to MemberDto.
        // thats why I name it LoggedInUserMember it represents the logged in member
        public MemberDto? LoggedInUserMember { get; set; }
    }
}
