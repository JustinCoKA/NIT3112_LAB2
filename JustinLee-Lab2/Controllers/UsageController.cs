using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class UsageController : QuTechAdminController
    {
        public UsageController(QuTechContext db) : base(db) { }
        public IActionResult Index() => View(Db.UsageRecords.Include(x => x.Subscription).ThenInclude(x => x.Customer).OrderBy(x => x.Id).ToList());
        public IActionResult Details(int id)
        {
            var model = Db.UsageRecords.Include(x => x.Subscription).ThenInclude(x => x.Customer).FirstOrDefault(x => x.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create() { Choices(); return View("Form", new UsageRecord()); }
        [HttpPost]
        public IActionResult Create([Bind("SubscriptionId,PeriodStart,ApiCalls")] UsageRecord model)
        {
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }

            Db.UsageRecords.Add(model);
            if (!SaveAudited("Created UsageRecord", "Subscription #" + model.SubscriptionId + " / " + model.PeriodStart.Value.ToString("yyyy-MM"))) { Choices(); return View("Form", model); }
            TempData["Success"] = "UsageRecord created and saved.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        public IActionResult Edit(int id)
        {
            var model = Db.UsageRecords.Find(id);
            if (model == null) return NotFound();
            Choices(); return View("Form", model);
        }
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,SubscriptionId,PeriodStart,ApiCalls")] UsageRecord model)
        {
            if (id != model.Id) return BadRequest();
            var existing = Db.UsageRecords.Find(id);
            if (existing == null) return NotFound();
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }
            existing.SubscriptionId = model.SubscriptionId;
            existing.PeriodStart = model.PeriodStart;
            existing.ApiCalls = model.ApiCalls;
            if (!SaveAudited("Updated UsageRecord", "UsageRecord #" + id)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Changes saved.";
            return RedirectToAction(nameof(Details), new { id });
        }
        public IActionResult Delete(int id)
        {
            var model = Db.UsageRecords.Find(id);
            if (model == null) return NotFound();
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var model = Db.UsageRecords.Find(id);
            if (model == null) return NotFound();
            // Keep related billing and activity history intact instead of deleting parent records.

            Db.UsageRecords.Remove(model);
            if (!SaveAudited("Deleted UsageRecord", "UsageRecord #" + id)) return View("Delete", model);
            TempData["Success"] = "UsageRecord deleted."; return RedirectToAction(nameof(Index));
        }
        private void Validate(UsageRecord model)
        {
            if (!Db.Subscriptions.Any(x => x.Id == model.SubscriptionId)) ModelState.AddModelError("SubscriptionId", "Choose an existing subscription.");
            if (model.ApiCalls < 0) ModelState.AddModelError("ApiCalls", "API usage cannot be negative.");
            if (model.PeriodStart != null && model.PeriodStart.Value.Day != 1) ModelState.AddModelError("PeriodStart", "Choose the first day of the reporting month.");
            if (Db.UsageRecords.Any(x => x.SubscriptionId == model.SubscriptionId && x.PeriodStart == model.PeriodStart && x.Id != model.Id)) ModelState.AddModelError("PeriodStart", "Usage already exists for this subscription and month.");
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
