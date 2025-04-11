
using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class ClientEntity
{
    [Key]
    public int Id { get; set; }
    public string ClientName { get; set; } = null!;
}
