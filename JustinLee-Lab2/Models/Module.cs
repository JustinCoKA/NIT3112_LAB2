using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
public class Module
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Enter a module name.")]
    [StringLength(60, ErrorMessage = "The module name must not exceed 60 characters.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Describe the module's purpose.")]
    [StringLength(250, ErrorMessage = "The module description must not exceed 250 characters.")]
    public string Description { get; set; }
    public List<PlanModule> PlanModules { get; set; } = new List<PlanModule>();
}
}
