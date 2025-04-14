using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }

    public string? Image { get; set; }
    public string? ProjectName { get; set; }
    public string? Description { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime EndDate { get; set; }

    public DateTime Created { get; set; }
    public decimal Budget { get; set; }

    // Relations
    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;

    [ForeignKey(nameof(Client))]
    public int ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;


    [ForeignKey(nameof(Members))]
    public ICollection<MemberEntity> Members { get; set; } = new List<MemberEntity>();
}