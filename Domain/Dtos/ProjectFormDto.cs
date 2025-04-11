
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class ProjectFormDto
{

    public string? Image { get; set; }



    public string ProjectName { get; set; } = null!;


    public string Description { get; set; } = null!;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Member { get; set; }

    public decimal? Budget { get; set; }
}
