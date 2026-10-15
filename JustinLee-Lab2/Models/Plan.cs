using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
    public class Plan
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter a plan name.")]
        [StringLength(60, ErrorMessage = "The plan name must not exceed 60 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Describe who this plan is for.")]
        [StringLength(250, ErrorMessage = "The plan description must not exceed 250 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Enter the monthly price in AUD.")]
        [Range(typeof(decimal), "0", "999999999", ErrorMessage = "Enter an amount from 0 to 999,999,999 AUD.")]
        public decimal? MonthlyPrice { get; set; }
        [Required(ErrorMessage = "Enter the monthly API call allowance.")]
        [Range(1, 2000000000, ErrorMessage = "Enter an API allowance from 1 to 2,000,000,000.")]
        public int? IncludedApiCalls { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public List<PlanModule> PlanModules { get; set; } = new List<PlanModule>();
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
