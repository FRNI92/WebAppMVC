
using System.ComponentModel.DataAnnotations;

namespace Domain.FormModels;

public class ClientFormModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Client name is required")]

    //TODO: add this here and in js if I have time
    //[StringLength(50, ErrorMessage = "50 characters or less")]
    //[MinLength(2, ErrorMessage = "2 character minimum")]
    public string ClientName { get; set; } = null!;
}
