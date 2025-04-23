using System.ComponentModel.DataAnnotations;

namespace EC2_2100212.ViewModels
{
    public class OrderViewModel : CreateOrderViewModel
    {
        public int Id { get; set; }

    }

    public class CreateOrderViewModel
    {
        public Guid UserId { get; set; }

        public int BagId { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Transaction Date")]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Shipping Address")]
        public string? ShippingAddress { get; set; }

        [Display(Name = "Card Number")]
        public string? CardNumber { get; set; }

        [Display(Name = "Card Expiration Date")]
        public string? CardExpirationDate { get; set; }

        [Display(Name = "Card Security Code")]
        public string? CardSecurityCode { get; set; }

        [Range(1, 5)]
        public int Quantity { get; set; }

        [DataType(DataType.Currency)]
        public float Total { get; set; }
        public BagViewModel? Bag { get; set; }

    }
}

