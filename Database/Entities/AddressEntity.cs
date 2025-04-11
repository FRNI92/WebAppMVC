namespace Database.Entities;

public class AddressEntity
{

    public int Id { get; set; }
    public string? StreetName { get; set; }
    public string? StreetNumber { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }
}