

using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class TargetGroupEntity
{
    [Key]
    public int Id { get; set; }
    public string TargetGroup { get; set; } = null!;
    public ICollection<NotificationEntity> NotificationEntities { get; set; } = []; 
}