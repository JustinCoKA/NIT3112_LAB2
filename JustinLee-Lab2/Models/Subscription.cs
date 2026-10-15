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
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "Choose a subscription status.")]
        [RegularExpression("^(Active|Trial|Paused)$", ErrorMessage = "Choose a valid subscription status.")]
        public string Status { get; set; }
        public int? DiscountId { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Discount Discount { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Customer Customer { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Plan Plan { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public List<UsageRecord> UsageRecords { get; set; } = new List<UsageRecord>();
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
