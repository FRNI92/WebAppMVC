
using System.ComponentModel.DataAnnotations;

namespace Domain.FormModels;

public class ClientFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Client name is required")]
    public string ClientName { get; set; } = null!;
}
