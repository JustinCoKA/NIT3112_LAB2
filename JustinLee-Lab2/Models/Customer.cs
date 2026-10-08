using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
public class Customer : Person
{
    [Required(ErrorMessage = "Enter the customer's organisation name.")]
    [StringLength(100, ErrorMessage = "The organisation name must not exceed 100 characters.")]
    public string CompanyName { get; set; }
    // One customer has one billing profile and can hold many subscriptions.
    public BillingProfile BillingProfile { get; set; }
    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
}
