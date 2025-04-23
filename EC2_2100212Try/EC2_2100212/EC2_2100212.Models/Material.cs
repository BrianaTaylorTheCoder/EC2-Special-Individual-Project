using System.ComponentModel.DataAnnotations;

namespace EC2_2100212.Models
{
    public class Material
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public string? Image { get; set; }
    }
}
