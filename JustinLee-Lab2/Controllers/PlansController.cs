using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class PlansController : QuTechAdminController
    {
        public PlansController(QuTechContext db) : base(db) { }
        public IActionResult Index() => View(Db.Plans.OrderBy(x => x.Id).ToList());
        public IActionResult Details(int id)
        {
            var model = Db.Plans.FirstOrDefault(x => x.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create() { Choices(); return View("Form", new Plan()); }
        [HttpPost]
        public IActionResult Create([Bind("Name,Description,MonthlyPrice,IncludedApiCalls")] Plan model)
        {
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }

            Db.Plans.Add(model);
            if (!SaveAudited("Created Plan", "Plan: " + model.Name)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Plan created and saved.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        public IActionResult Edit(int id)
        {
            var model = Db.Plans.Find(id);
            if (model == null) return NotFound();
            Choices(); return View("Form", model);
        }
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,MonthlyPrice,IncludedApiCalls")] Plan model)
        {
            if (id != model.Id) return BadRequest();
            var existing = Db.Plans.Find(id);
            if (existing == null) return NotFound();
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }
            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.MonthlyPrice = model.MonthlyPrice;
            existing.IncludedApiCalls = model.IncludedApiCalls;
            if (!SaveAudited("Updated Plan", "Plan #" + id)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Changes saved.";
            return RedirectToAction(nameof(Details), new { id });
        }
        public IActionResult Delete(int id)
        {
            var model = Db.Plans.Find(id);
            if (model == null) return NotFound();
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var model = Db.Plans.Find(id);
            if (model == null) return NotFound();
            // Keep related billing and activity history intact instead of deleting parent records.
            if (Db.Subscriptions.Any(x => x.PlanId == id) || Db.PlanModules.Any(x => x.PlanId == id)) { ModelState.AddModelError("", "This record is in use. Remove or reassign its dependent records first."); return View("Delete", model); }

            Db.Plans.Remove(model);
            if (!SaveAudited("Deleted Plan", "Plan #" + id)) return View("Delete", model);
            TempData["Success"] = "Plan deleted."; return RedirectToAction(nameof(Index));
        }
        private void Validate(Plan model)
        {
            if (model.MonthlyPrice < 0) ModelState.AddModelError("MonthlyPrice", "The price cannot be negative.");
            if (model.IncludedApiCalls < 1) ModelState.AddModelError("IncludedApiCalls", "Include at least one API call.");
        }
        private void Choices()
        {
            ViewBag.Customers = Db.Customers.OrderBy(x => x.CompanyName).ToList();
            ViewBag.Plans = Db.Plans.OrderBy(x => x.Name).ToList();
            ViewBag.Discounts = Db.Discounts.Where(x => x.Enabled).OrderBy(x => x.Name).ToList();
            ViewBag.Subscriptions = Db.Subscriptions.Include(x => x.Customer).ToList();
        }
    }
}
