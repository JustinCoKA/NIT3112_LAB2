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
    public decimal? MonthlyPrice { get; set; }
    [Required(ErrorMessage = "Enter the monthly API call allowance.")]
    public int? IncludedApiCalls { get; set; }
    public List<PlanModule> PlanModules { get; set; } = new List<PlanModule>();
    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
}
