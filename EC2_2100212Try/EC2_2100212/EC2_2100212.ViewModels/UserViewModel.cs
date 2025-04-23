using EC2_2100212.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EC2_2100212.ViewModels
{
    public class UserViewModel : CreateUserViewModel
    {
        public string? Id { get; set; }

    }

    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Username")]
        public string? UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }


    public class CreateUserViewModel : LoginViewModel
    {
        [Required]
        public string? Name { get; set; }

        public string? Address { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }


        [Required]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public Gender Gender { get; set; }

        public string? Image { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }


        public string? Roles { get; set; }
        public IEnumerable<string>? RolesCollection { get; set; }
    }
}
