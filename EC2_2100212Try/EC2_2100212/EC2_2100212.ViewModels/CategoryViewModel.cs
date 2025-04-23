using EC2_2100212.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EC2_2100212.ViewModels
{
    public class CategoryViewModel : CreateCategoryViewModel
    {
        public int Id { get; set; }
    }

    public class CreateCategoryViewModel
    {

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Category Name is too Long")]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

    }
}
