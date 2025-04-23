using BNS2025.Models;
using System.ComponentModel.DataAnnotations;

namespace BNS2025.ViewModels
{
    public class AccountViewModel: CreateAccountViewModel
    {
        public int Id { get; set; }
    }

    public class CreateAccountViewModel
    {
        [Display(Name = "User")]
        public Guid UserId { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [Required]
        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }

        [Display(Name = "Card Number")]
        public string? CardNumber { get; set; }

        [Display(Name = "Card Expiration Date")]
        public string? CardExpirationDate { get; set; }
        
        [Display(Name = "Card Security Code")]
        [Required]
        public string? CardSecurityCode { get; set; }

        [Required]
        [Display(Name ="Account Type")]
        public AccountType AccountType { get; set; }

        [DataType(DataType.Currency)]
        public double Balance { get; set; }
        public DateTime CreateDate { get; set; }

        public CurrencyViewModel? Currency { get; set; }
        public UserViewModel? User { get; set; }

    }
}