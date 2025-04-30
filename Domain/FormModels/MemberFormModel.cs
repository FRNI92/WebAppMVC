using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.FormModels
{
  public class MemberFormModel
    {
        public int Id { get; set; }
        public string? Image { get; set; }

        //not need in dto
        public IFormFile? ImageFile { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string? FirstName { get; set; } = null!;
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; } = null!;
        public string? JobTitle { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
       
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }


        public int? AddressId { get; set; }

        //not need in dto
        public AddressFormModel? Address { get; set; }

        //to connect member to appuser
        public string? ConnectedAppUserId { get; set; }

    }
}
