

namespace BNS2025.Models
{
    public class Transaction
    {

        public int Id { get; set; }

        public String? UserId { get; set; }

        public String? Source { get; set; }

        public string? AccountNumber { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionType TransactionType { get; set; }

        public double Amount { get; set; }

    }
}
