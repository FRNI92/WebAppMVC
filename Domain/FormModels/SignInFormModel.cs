using System.ComponentModel.DataAnnotations;

namespace Domain.FormModels;

public class SignInFormModel
{

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Email", Prompt = "Enter your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "too short.")]
    [Display(Name = "Password", Prompt = "Enter a password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

}
