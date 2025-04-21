
using Database.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Dtos;

public class ProjectFormDto
{
    //added id so I cant generate unique id on edit drop down
    public int Id { get; set; }
    public string Image { get; set; }
    public string? ProjectName { get; set; }
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
    //to handle the time left
    public string TimeLeftText { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public decimal Budget { get; set; }


    public int StatusId { get; set; }
    public string? StatusName { get; set; } // mappa från entity.Status.StatusName

    public int ClientId { get; set; }
    public string? ClientName { get; set; } // mappa från entity.Client.ClientName


    public List<int> MemberIds { get; set; } = new();
    public List<MemberDto> ProjectMembers { get; set; } = new();
}
