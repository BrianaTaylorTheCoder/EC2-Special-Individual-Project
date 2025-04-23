using System.ComponentModel.DataAnnotations;

namespace EC2_2100212.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
