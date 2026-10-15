using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
    public class UsageRecord
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Choose a subscription for the usage record.")]
        public int? SubscriptionId { get; set; }
        [Required(ErrorMessage = "Choose the usage period start date.")]
        [DataType(DataType.Date)]
        public DateTime? PeriodStart { get; set; }
        [Required(ErrorMessage = "Enter the number of API calls.")]
        [Range(0, 2000000000, ErrorMessage = "Enter API calls from 0 to 2,000,000,000.")]
        public int? ApiCalls { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Subscription Subscription { get; set; }
    }
}
