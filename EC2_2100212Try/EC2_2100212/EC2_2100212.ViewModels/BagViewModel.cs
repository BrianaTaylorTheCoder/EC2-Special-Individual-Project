
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EC2_2100212.ViewModels
{
    public class BagViewModel : CreateBagViewModel
    {
        public int Id { get; set; }

    }

    public class CreateBagViewModel
    {

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Bag brand is too long")]
        [Display(Name = "Brand")]
        public string? Brand { get; set; }

        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Bag model is too long")]
        [Display(Name = "Model")]
        public string? Model { get; set; }

        [Required]
        [Display(Name = "Colour")]
        public string? Colour { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Manufacture Date")]
        public DateTime ManufactureDate { get; set; }

        [Range(0, 10000)]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public float Price { get; set; }


        public string Image { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        
        public string? Materials { get; set; }
        public string? Categories { get; set; }


        
        [Display(Name = "Materials")]
        public List<int>? MaterialSelectedList { get; set; }

        [Display(Name = "Categories")]
        public List<int>? CategorySelectedList { get; set; }



        [Display(Name = "Materials")]
        public MultiSelectList? ListOfMaterials { get; set; }

        [Display(Name = "Categories")]
        public MultiSelectList? ListOfCategories { get; set; }

    }
}
