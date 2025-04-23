using System.ComponentModel.DataAnnotations.Schema;

namespace BNS2025.Models
{
    public class Account
    {
        public int Id { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public Guid UserId { get; set; }
        public string? AccountNumber { get; set; }

        [ForeignKey(nameof(Currency))]
        public int CurrencyId { get; set; }
        public string? CardNumber { get; set; }
        public string? CardExpirationDate { get; set; }
        public string? CardSecurityCode { get; set; }
        public AccountType AccountType { get; set; }
        public double Balance { get; set; }
        public DateTime CreateDate { get; set; }

        [NotMapped]
        public virtual ApplicationUser? User { get; set; }
        [NotMapped]
        public virtual Currency? Currency { get; set; }

    }
}