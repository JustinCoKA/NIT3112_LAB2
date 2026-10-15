using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter an invoice reference.")]
        [StringLength(30, ErrorMessage = "The invoice reference must not exceed 30 characters.")]
        public string Reference { get; set; }
        [Required(ErrorMessage = "Choose the billed subscription.")]
        public int? SubscriptionId { get; set; }
        [Required(ErrorMessage = "Choose the invoice issue date.")]
        [DataType(DataType.Date)]
        public DateTime? IssuedOn { get; set; }
        [Required(ErrorMessage = "Choose the invoice due date.")]
        [DataType(DataType.Date)]
        public DateTime? DueOn { get; set; }
        [Required(ErrorMessage = "Enter the invoice amount in AUD.")]
        [Range(typeof(decimal), "0", "999999999", ErrorMessage = "Enter an amount from 0 to 999,999,999 AUD.")]
        public decimal? Amount { get; set; }
        [Required(ErrorMessage = "Choose the invoice status.")]
        [RegularExpression("^(Draft|Unpaid|Paid)$", ErrorMessage = "Choose a valid invoice status.")]
        public string Status { get; set; }
        // Store billing snapshots: later plan/discount changes must not rewrite invoices.
        [Required(ErrorMessage = "Choose the billing period start date.")]
        [DataType(DataType.Date)]
        public DateTime? PeriodStart { get; set; }
        [Range(typeof(decimal), "0", "999999999", ErrorMessage = "The invoice subtotal must be non-negative.")]
        public decimal Subtotal { get; set; }
        [Range(typeof(decimal), "0", "999999999", ErrorMessage = "The invoice discount must be non-negative.")]
        public decimal DiscountAmount { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Subscription Subscription { get; set; }
    }
}
