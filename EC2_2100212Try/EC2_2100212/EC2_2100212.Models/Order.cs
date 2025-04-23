using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EC2_2100212.Models
{
    public class Order
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int BagId { get; set; }

        public DateTime TransactionDate { get; set; }

        public string? ShippingAddress { get; set; }

        public string? CardNumber { get; set; }

        [NotMapped]
        public string? CardExpirationDate { get; set; }

        [NotMapped]
        public string? CardSecurityCode { get; set; }

        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public float Total { get; set; }
    }
}
