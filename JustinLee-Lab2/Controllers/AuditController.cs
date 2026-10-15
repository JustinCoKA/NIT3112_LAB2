using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
namespace JustinLee_Lab2.Controllers
{
    public class AuditController : QuTechAdminController
    {
        public AuditController(QuTechContext db) : base(db) { }
        // Audit entries have no update/delete endpoints so operation history stays accountable.
        public IActionResult Index() => View(Db.AuditEntries.Include(x => x.Administrator).OrderByDescending(x => x.OccurredAt).ToList());
    }
}
