

using System.ComponentModel.DataAnnotations;
using System.Security;

namespace Database.Entities;

public class NotificationTypeEntity
{
    [Key]
    public int Id { get; set; }
    public string NotificationType { get; set; } = null!;
    public ICollection<NotificationEntity> NotificationEntities { get; set; } = [];
}
