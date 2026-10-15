using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;

namespace JustinLee_Lab2.Controllers
{
    [Authorize(Roles = "Admin")]
    public abstract class QuTechAdminController : Controller
    {
        protected readonly QuTechContext Db;
        protected QuTechAdminController(QuTechContext db) { Db = db; }

        // The business change and its audit entry are saved in one database transaction.
        protected bool SaveAudited(string action, string target)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var actor = Db.Users.SingleOrDefault(u => u.Id == userId);
            if (actor == null) { ModelState.AddModelError("", "Your administrator session could not be verified. Sign in again."); return false; }
            Db.AuditEntries.Add(new AuditEntry { AdministratorId = actor.AdministratorId, OccurredAt = DateTime.UtcNow, Action = action.Length > 200 ? action.Substring(0, 200) : action, Target = target.Length > 80 ? target.Substring(0, 80) : target });
            try { Db.SaveChanges(); return true; }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "This change conflicts with an existing or linked record. Review the values and try again.");
                return false;
            }
        }
    }
}
