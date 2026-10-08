using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
public class Subscription
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Choose the customer for this subscription.")]
    public int? CustomerId { get; set; }
    [Required(ErrorMessage = "Choose a subscription plan.")]
    public int? PlanId { get; set; }
    [Required(ErrorMessage = "Choose a subscription start date.")]
    public DateTime? StartDate { get; set; }
    [Required(ErrorMessage = "Choose a subscription status.")]
    public string Status { get; set; }
    public Customer Customer { get; set; }
    public Plan Plan { get; set; }
    public List<UsageRecord> UsageRecords { get; set; } = new List<UsageRecord>();
    public List<Invoice> Invoices { get; set; } = new List<Invoice>();
}
}
