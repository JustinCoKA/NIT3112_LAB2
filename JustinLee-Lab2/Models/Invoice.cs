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
    public DateTime? IssuedOn { get; set; }
    [Required(ErrorMessage = "Choose the invoice due date.")]
    public DateTime? DueOn { get; set; }
    [Required(ErrorMessage = "Enter the invoice amount in AUD.")]
    public decimal? Amount { get; set; }
    [Required(ErrorMessage = "Choose the invoice status.")]
    public string Status { get; set; }
    public Subscription Subscription { get; set; }
}
}
