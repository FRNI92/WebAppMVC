
namespace Domain.Dtos;

public class MemberDto
{

    public int Id { get; set; }
    public string? Image { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public int AddressId { get; set; }


}
