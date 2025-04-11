
using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class StatusEntity
{
    [Key]
    public int Id { get; set; }
    public string StatusName { get; set; } = null!;
}