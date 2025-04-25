

using Domain.Dtos;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.FormModels;

public class ProjectFormModel
{

    public ProjectFormModel()
    {
        StartDate = DateTime.Today;
        EndDate = DateTime.Today.AddDays(7);
    }
    public int Id { get; set; }

    [DataType(DataType.ImageUrl)]
    [Display(Name = "Image", Prompt = "Select a image")]
    public string? Image { get; set; }

    //[Required(ErrorMessage = "Please upload an image.")]
    public IFormFile? ImageFile { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    public string ProjectName { get; set; } = null!;

    [Required]
    [DataType(DataType.Text)]
    [Display(Name = "Description", Prompt = "Enter a description")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required(ErrorMessage = "Please choose a client.")]
    public int ClientId { get; set; }

    //[Required(ErrorMessage = "Please choose at least one member.")]
    public List<int> MemberIds { get; set; } = new();
    public int StatusId { get; set; }

    [Required(ErrorMessage = "Please enter a budget.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Budget must be greater than 0.")]
    public decimal Budget { get; set; }

    public IEnumerable<ClientDto> Clients { get; set; } = new List<ClientDto>();
}
