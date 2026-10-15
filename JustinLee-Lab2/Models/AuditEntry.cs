using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
    public class AuditEntry
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Identify the administrator responsible for the action.")]
        public int? AdministratorId { get; set; }
        [Required(ErrorMessage = "Record when the action occurred.")]
        [DataType(DataType.Date)]
        public DateTime? OccurredAt { get; set; }
        [Required(ErrorMessage = "Describe the recorded action.")]
        [StringLength(200, ErrorMessage = "The audit action must not exceed 200 characters.")]
        public string Action { get; set; }
        [Required(ErrorMessage = "Identify the affected record.")]
        [StringLength(80, ErrorMessage = "The target reference must not exceed 80 characters.")]
        public string Target { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Administrator Administrator { get; set; }
    }
}
