using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EC2_2100212.ViewModels
{
    public class RoleViewModel : CreateRoleViewModel
    {
        [Display(Name = "Role Id")]
        public string Id { get; set; }

    }

    public class CreateRoleViewModel
    {

        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "Role Needed")]
        public string? Name { get; set; }

        [NotMapped]
        public List<UserViewModel>? Users { get; set; }

    }
}
