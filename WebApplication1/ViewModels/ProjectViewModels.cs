using Domain.Dtos;
using Domain.FormModels;

namespace WebApplication1.ViewModels
{
    public class ProjectViewModels
    {
        public ProjectFormModel FormModel { get; set; } = new();
        public IEnumerable<ProjectFormDto> ProjectList { get; set; } = new List<ProjectFormDto>();
        public IEnumerable<ClientDto> Clients { get; set; } = new List<ClientDto>();
    }
}
