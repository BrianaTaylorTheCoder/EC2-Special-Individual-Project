using EC2_2100212.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EC2_2100212.ViewModels
{
    public class MaterialViewModel : CreateMaterialViewModel
    {
        public int Id { get; set; }
    }

    public class CreateMaterialViewModel
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Material Name is too Long")]
        [Display(Name = "Material Name")]
        public string? Name { get; set; }
        public string? Image { get; set; }

        [Display(Name = "Material Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
