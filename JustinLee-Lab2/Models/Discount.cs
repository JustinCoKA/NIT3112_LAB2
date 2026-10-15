using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
    // Discounts are a separate business concept required by the actual Lab 3 brief.
    // A subscription may select a time-limited percentage discount.
    public class Discount
    {
        public bool Enabled { get; set; } = true;
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter a discount name.")]
        [StringLength(60, ErrorMessage = "The discount name must not exceed 60 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Describe when this discount applies.")]
        [StringLength(250, ErrorMessage = "The discount description must not exceed 250 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Enter the discount percentage.")]
        [Range(typeof(decimal), "0", "100", ErrorMessage = "Enter a discount percentage from 0 to 100.")]
        public decimal? Percentage { get; set; }

        [Required(ErrorMessage = "Choose the discount start date.")]
        [DataType(DataType.Date)]
        public DateTime? StartsOn { get; set; }

        [Required(ErrorMessage = "Choose the discount end date.")]
        [DataType(DataType.Date)]
        public DateTime? EndsOn { get; set; }
    }
}
