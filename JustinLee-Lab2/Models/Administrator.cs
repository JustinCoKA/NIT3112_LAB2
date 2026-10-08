using System.ComponentModel.DataAnnotations;

namespace JustinLee_Lab2.Models
{
public class Administrator : Person
{
    [Required(ErrorMessage = "Enter the administrator's department.")]
    [StringLength(60, ErrorMessage = "The department must not exceed 60 characters.")]
    public string Department { get; set; }
    public List<AuditEntry> AuditEntries { get; set; } = new List<AuditEntry>();
}
}
