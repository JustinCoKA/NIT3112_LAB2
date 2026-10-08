using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;

namespace JustinLee_Lab2.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly DemoData data = new DemoData();
        public IActionResult Index()
        {
            return View(data.Invoices);
        }
        public IActionResult Details(int id)
        {
            var model = data.Invoices.Find(item => item.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Customers = data.Customers;
            ViewBag.Plans = data.Plans;
            ViewBag.Subscriptions = data.Subscriptions;
            return View(new Invoice());
        }
        [HttpPost]
        public IActionResult Create(Invoice model)
        {
            if (model.SubscriptionId != null && data.Subscriptions.Find(s => s.Id == model.SubscriptionId) == null) ModelState.AddModelError("SubscriptionId", "Choose an existing subscription.");
            if (model.Amount < 0) ModelState.AddModelError("Amount", "The invoice amount cannot be negative.");
            if (model.DueOn < model.IssuedOn) ModelState.AddModelError("DueOn", "The due date must be on or after the issue date.");
            if (model.Status != "Draft" && model.Status != "Unpaid" && model.Status != "Paid") ModelState.AddModelError("Status", "Choose Draft, Unpaid or Paid.");
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = data.Customers;
            ViewBag.Plans = data.Plans;
            ViewBag.Subscriptions = data.Subscriptions;
                return View(model);
            }
            model.Subscription = data.Subscriptions.Find(s => s.Id == model.SubscriptionId);
            // Lab 2 demonstrates binding and validation without saving a record.
            ViewBag.IsPreview = true;
            return View("Details", model);
        }
    }
}
