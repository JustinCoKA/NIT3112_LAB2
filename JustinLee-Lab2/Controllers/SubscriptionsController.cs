using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class SubscriptionsController : QuTechAdminController
    {
        public SubscriptionsController(QuTechContext db) : base(db) { }
        public IActionResult Index() => View(Db.Subscriptions.Include(x => x.Customer).Include(x => x.Plan).Include(x => x.Discount).OrderBy(x => x.Id).ToList());
        public IActionResult Details(int id)
        {
            var model = Db.Subscriptions.Include(x => x.Customer).Include(x => x.Plan).Include(x => x.Discount).FirstOrDefault(x => x.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create() { Choices(); return View("Form", new Subscription()); }
        [HttpPost]
        public IActionResult Create([Bind("CustomerId,PlanId,DiscountId,StartDate,Status")] Subscription model)
        {
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }

            Db.Subscriptions.Add(model);
            if (!SaveAudited("Created Subscription", "Customer #" + model.CustomerId + " / Plan #" + model.PlanId)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Subscription created and saved.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        public IActionResult Edit(int id)
        {
            var model = Db.Subscriptions.Find(id);
            if (model == null) return NotFound();
            Choices(); return View("Form", model);
        }
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,CustomerId,PlanId,DiscountId,StartDate,Status")] Subscription model)
        {
            if (id != model.Id) return BadRequest();
            var existing = Db.Subscriptions.Find(id);
            if (existing == null) return NotFound();
            if (existing.CustomerId != model.CustomerId && (Db.Invoices.Any(x => x.SubscriptionId == id) || Db.UsageRecords.Any(x => x.SubscriptionId == id))) ModelState.AddModelError("CustomerId", "The customer cannot change after billing or usage history exists.");
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }
            existing.CustomerId = model.CustomerId;
            existing.PlanId = model.PlanId;
            existing.DiscountId = model.DiscountId;
            existing.StartDate = model.StartDate;
            existing.Status = model.Status;
            if (!SaveAudited("Updated Subscription", "Subscription #" + id)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Changes saved.";
            return RedirectToAction(nameof(Details), new { id });
        }
        public IActionResult Delete(int id)
        {
            var model = Db.Subscriptions.Find(id);
            if (model == null) return NotFound();
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var model = Db.Subscriptions.Find(id);
            if (model == null) return NotFound();
            // Keep related billing and activity history intact instead of deleting parent records.
            if (Db.Invoices.Any(x => x.SubscriptionId == id) || Db.UsageRecords.Any(x => x.SubscriptionId == id)) { ModelState.AddModelError("", "This record is in use. Remove or reassign its dependent records first."); return View("Delete", model); }

            Db.Subscriptions.Remove(model);
            if (!SaveAudited("Deleted Subscription", "Subscription #" + id)) return View("Delete", model);
            TempData["Success"] = "Subscription deleted."; return RedirectToAction(nameof(Index));
        }
        private void Validate(Subscription model)
        {
            if (!Db.Customers.Any(x => x.Id == model.CustomerId)) ModelState.AddModelError("CustomerId", "Choose an existing customer.");
            if (!Db.Plans.Any(x => x.Id == model.PlanId)) ModelState.AddModelError("PlanId", "Choose an existing plan.");
            if (model.DiscountId != null && !Db.Discounts.Any(x => x.Id == model.DiscountId && x.Enabled)) ModelState.AddModelError("DiscountId", "Choose an enabled discount or no discount.");
            if (model.Status != "Active" && model.Status != "Trial" && model.Status != "Paused") ModelState.AddModelError("Status", "Choose Active, Trial or Paused.");
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
