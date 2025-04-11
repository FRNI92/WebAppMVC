//using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace Domain.FormModels.SignUpFormModel;

public class SignUpFormModel
{
    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "First Name", Prompt = "Enter your first name")]
    public string FirstName { get; set; } = null!;


    [Required(ErrorMessage = "This field is required.")]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name", Prompt = "Enter your last name")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Your must enter your email address.")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Your must enter an valid email address.")]
    [Display(Name = "Email", Prompt = "Enter your email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "You must enter a password")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d).{8,}$", ErrorMessage = "You must enter a strong password")]
    [Display(Name = "Password", Prompt = "Enter password")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "This filed is required.")]
    [Compare(nameof(Password), ErrorMessage = "your password does not match")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password", Prompt = "Confirm you password")]
    public string ConfirmPassword { get; set; } = null!;


    [Required(ErrorMessage = "Click it!")]
    [Display(Name = "Terms & Conditions", Prompt = "I accept")]
    public bool TermsAndCondition { get; set; }
}