﻿
using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class MemberEntity
{
    [Key]
    public int Id { get; set; }
    public string? Image { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int AddressId { get; set; }
    public AddressEntity Address { get; set; } = null!;

    public ICollection<ProjectMemberEntity> ProjectMembers { get; set; } = new List<ProjectMemberEntity>();
}
