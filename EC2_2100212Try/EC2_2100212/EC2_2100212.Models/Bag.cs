using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EC2_2100212.Models
{
    public class Bag
    {
        /*Student Name: Briana Taylor
        ID Number: 2100212 
        Module: Enterprise Computing 2 Semester 2 AY 2024/25 
        Activity: Milestone 1 */

        public int Id { get; set; }
 
        public string? Brand { get; set; }
        [Required]
        public required string Model { get; set; }
        [Required]
        public required string Colour { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Manufacture Date")]
        public DateTime ManufactureDate { get; set; }
        [Required]
        [Range(0, 10000)]
        public int Quantity { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public float Price { get; set; }
        [Required]
        public required string Image { get; set; }

        [NotMapped]
        public virtual IEnumerable<Material>? Materials { get; set; }

        [NotMapped]
        public virtual IEnumerable<Category>? Categories { get; set; }

    }
}
