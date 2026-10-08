using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
public class UsageRecord
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Choose a subscription for the usage record.")]
    public int? SubscriptionId { get; set; }
    [Required(ErrorMessage = "Choose the usage period start date.")]
    public DateTime? PeriodStart { get; set; }
    [Required(ErrorMessage = "Enter the number of API calls.")]
    public int? ApiCalls { get; set; }
    public Subscription Subscription { get; set; }
}
}
