using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
    public class PlanModule
    {
        // PlanId + ModuleId is the composite database key for the many-to-many link.
        [Required(ErrorMessage = "Choose a subscription plan.")]
        public int? PlanId { get; set; }
        [Required(ErrorMessage = "Choose a software module.")]
        public int? ModuleId { get; set; }
        public bool Enabled { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Plan Plan { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Module Module { get; set; }
    }
}
