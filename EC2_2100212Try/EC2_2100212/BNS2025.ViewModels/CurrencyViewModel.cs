using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BNS2025.ViewModels
{
    public class CurrencyViewModel: CreateCurrencyViewModel
    {
        public int Id { get; set; }
    }

    public class CreateCurrencyViewModel
    {
        public string? Name { get; set; }

        [StringLength(3)]
        [Display(Name = "Short Name")]
        public string? ShortName { get; set; }
    }
}
