using System.ComponentModel.DataAnnotations;


namespace BNS2025.ViewModels
{
    public class RoleViewModel : CreateRoleViewModel
    {
        [Display(Name = "Role Id")]
        public string? Id { get; set; }

    }

    public class CreateRoleViewModel
    {

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role Needed")]
        public string? Name { get; set; }

        public List<UserViewModel>? Users { get; set; }


    }
}
