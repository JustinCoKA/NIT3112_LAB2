using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
public class BillingProfile
{
    // Shared key: the profile's ID is the customer's ID (one-to-one design).
    public int CustomerId { get; set; }
    [Required(ErrorMessage = "Enter the billing organisation name.")]
    [StringLength(100, ErrorMessage = "The billing name must not exceed 100 characters.")]
    public string BillingName { get; set; }
    [Required(ErrorMessage = "Enter the billing email address.")]
    [EmailAddress(ErrorMessage = "Enter a valid billing email address.")]
    public string BillingEmail { get; set; }
    [Required(ErrorMessage = "Enter the billing address.")]
    [StringLength(200, ErrorMessage = "The billing address must not exceed 200 characters.")]
    public string Address { get; set; }
    public Customer Customer { get; set; }
}
}
