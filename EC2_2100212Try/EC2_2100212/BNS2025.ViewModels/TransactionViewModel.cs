using BNS2025.Models;
using System.ComponentModel.DataAnnotations;

namespace BNS2025.ViewModels
{
    public class TransactionViewModel: CreateTransactionViewModel
    {
        public int Id { get; set; }
    }

    public class CreateTransactionViewModel
    {

        [Required]
        public String? UserId { get; set; }

        public String? Source { get; set; }

        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [Display(Name = "Transaction Date")]
        public DateTime? TransactionDate { get; set; }

        [Required]
        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }

        [DataType(DataType.Currency)]
        public double Amount { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        public string? CardNumber { get; set; }

        [Required]
        [Display(Name ="Card Security Code")]
        public string? CardSecurityCode { get; set; }

        [Required]
        [Display(Name = "Card Expiration Date")]
        public string? CardExpirationDate { get; set; }


        [Display(Name = "Customer")]
        public UserViewModel? User { get; set; }
    }
}
