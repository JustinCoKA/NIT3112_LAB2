using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JustinLee_Lab2.Models
{
    // Session 9: Identity owns credentials; the domain administrator owns contact details.
    public class ApplicationUser : IdentityUser
    {
        [Range(1, int.MaxValue, ErrorMessage = "Choose an administrator profile.")]
        public int AdministratorId { get; set; }
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Administrator Administrator { get; set; }
    }
}
