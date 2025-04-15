

using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Domain.FormModels;

public class ProjectFormModel
{
    [DataType(DataType.ImageUrl)]
    [Display(Name = "Image", Prompt = "Select a image")]
    public string? Image { get; set; }


    //[Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    public string ProjectName { get; set; } = null!;

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Description", Prompt = "Enter a description")]
    public string Description { get; set; } = null!;

    [DataType(DataType.Date)]
    public DateTime? StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    public int ClientId { get; set; }
    public string? Member { get; set; }

    public decimal? Budget { get; set; }

    public IEnumerable<ClientDto> Clients { get; set; } = new List<ClientDto>();
}
