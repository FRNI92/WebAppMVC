using Domain.Dtos;
using Domain.FormModels;

namespace WebApplication1.ViewModels;

public class ClientViewModel
{
    public IEnumerable<ClientDto> Clients { get; set; } = new List<ClientDto>();
    public ClientFormModel FormModel { get; set; } = new();
}
