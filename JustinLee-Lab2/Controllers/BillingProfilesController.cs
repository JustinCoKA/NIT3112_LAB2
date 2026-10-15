using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class BillingProfilesController : QuTechAdminController
    {
        public BillingProfilesController(QuTechContext db) : base(db) { }
        public IActionResult Edit(int id) { var model = Db.BillingProfiles.Find(id); if (model == null) return NotFound(); return View(model); }
        [HttpPost]
        public IActionResult Edit(int id, [Bind("BillingName,BillingEmail,Address")] BillingProfile model)
        {
            var existing = Db.BillingProfiles.Find(id); if (existing == null) return NotFound();
            model.CustomerId = id;
            if (!ModelState.IsValid) return View(model);
            existing.BillingName = model.BillingName; existing.BillingEmail = model.BillingEmail; existing.Address = model.Address;
            if (!SaveAudited("Updated billing profile", "Customer #" + id)) return View(model);
            TempData["Success"] = "Billing profile saved."; return RedirectToAction("Details", "Customers", new { id });
        }
    }
}
