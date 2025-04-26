

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class NotificationDismissedEntity
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(Member))]
    public int MemberId { get; set; }
    public MemberEntity Member { get; set; } = null!;


    [ForeignKey(nameof(Notification))]
    public string NotificationId { get; set; } = null!;
    public NotificationEntity Notification { get; set; } = null!;
}