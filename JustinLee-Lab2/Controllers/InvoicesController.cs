using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class InvoicesController : QuTechAdminController
    {
        public InvoicesController(QuTechContext db) : base(db) { }
        public IActionResult Index() => View(Db.Invoices.Include(x => x.Subscription).ThenInclude(x => x.Customer).OrderByDescending(x => x.IssuedOn).ToList());
        public IActionResult Details(int id)
        {
            var invoice = Db.Invoices.Include(x => x.Subscription).ThenInclude(x => x.Customer).ThenInclude(x => x.BillingProfile).Include(x => x.Subscription.Plan).FirstOrDefault(x => x.Id == id);
            if (invoice == null) return NotFound(); return View(invoice);
        }
        public IActionResult Create() { Choices(); return View(new Invoice { PeriodStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), IssuedOn = DateTime.Today, DueOn = DateTime.Today.AddDays(14) }); }
        [HttpPost]
        public IActionResult Create([Bind("SubscriptionId,PeriodStart,IssuedOn,DueOn")] Invoice model)
        {
            // Prices are calculated on the server; submitted amounts cannot override billing.
            ModelState.Remove("Reference"); ModelState.Remove("Amount"); ModelState.Remove("Status");
            var subscription = Db.Subscriptions.Include(x => x.Plan).Include(x => x.Discount).FirstOrDefault(x => x.Id == model.SubscriptionId);
            if (subscription == null) ModelState.AddModelError("SubscriptionId", "Choose an existing subscription.");
            else if (subscription.Status != "Active") ModelState.AddModelError("SubscriptionId", "Only active subscriptions can be invoiced.");
            if (model.PeriodStart != null && model.PeriodStart.Value.Day != 1) ModelState.AddModelError("PeriodStart", "Choose the first day of the billing month.");
            if (subscription != null && model.PeriodStart != null && subscription.StartDate != null && model.PeriodStart.Value < new DateTime(subscription.StartDate.Value.Year, subscription.StartDate.Value.Month, 1)) ModelState.AddModelError("PeriodStart", "The billing month precedes the subscription.");
            if (model.DueOn < model.IssuedOn) ModelState.AddModelError("DueOn", "The due date must be on or after the issue date.");
            if (Db.Invoices.Any(x => x.SubscriptionId == model.SubscriptionId && x.PeriodStart == model.PeriodStart)) ModelState.AddModelError("PeriodStart", "An invoice already exists for this subscription and billing month.");
            if (!ModelState.IsValid) { Choices(); return View(model); }
            model.Reference = "QT-" + model.PeriodStart.Value.ToString("yyyyMM") + "-S" + subscription.Id;
            model.Subtotal = subscription.Plan.MonthlyPrice.Value;
            var discount = subscription.Discount;
            model.DiscountAmount = discount != null && discount.Enabled && discount.StartsOn <= model.PeriodStart && discount.EndsOn >= model.PeriodStart ? decimal.Round(model.Subtotal * discount.Percentage.Value / 100m, 2, MidpointRounding.AwayFromZero) : 0m;
            model.Amount = model.Subtotal - model.DiscountAmount; model.Status = "Unpaid";
            Db.Invoices.Add(model);
            if (!SaveAudited("Generated invoice", model.Reference)) { Choices(); return View(model); }
            TempData["Success"] = "Invoice generated and saved. Prices and discounts are recorded as a billing snapshot.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        [HttpPost]
        public IActionResult MarkPaid(int id)
        {
            var invoice = Db.Invoices.Find(id); if (invoice == null) return NotFound();
            if (invoice.Status == "Paid") { TempData["Error"] = "This invoice is already paid."; return RedirectToAction(nameof(Details), new { id }); }
            invoice.Status = "Paid";
            if (SaveAudited("Marked invoice paid", invoice.Reference)) TempData["Success"] = "Payment status recorded.";
            else TempData["Error"] = "Payment status could not be saved.";
            return RedirectToAction(nameof(Details), new { id });
        }
        private void Choices() { ViewBag.Subscriptions = Db.Subscriptions.Include(x => x.Customer).Include(x => x.Plan).Where(x => x.Status == "Active").ToList(); }
    }
}
