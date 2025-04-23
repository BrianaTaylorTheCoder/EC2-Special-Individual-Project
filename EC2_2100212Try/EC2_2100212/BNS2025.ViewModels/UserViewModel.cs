using BNS2025.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BNS2025.ViewModels
{

    public class LoginViewModel
    {

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(15, ErrorMessage = "Your Password should be 8 characters")]
        public string? Password { get; set; }

    }
    public class UserViewModel: LoginViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public Gender Gender { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [StringLength(15, ErrorMessage = "Your Password should be 8 characters")]
        public string? ConfirmPassword { get; set; }
        public ICollection<string>? Roles { get; set; }
        public string? SelectedRoles { get; set; }


    }
}
