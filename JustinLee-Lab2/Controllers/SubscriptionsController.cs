using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;

namespace JustinLee_Lab2.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly DemoData data = new DemoData();
        public IActionResult Index()
        {
            return View(data.Subscriptions);
        }
        public IActionResult Details(int id)
        {
            var model = data.Subscriptions.Find(item => item.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Customers = data.Customers;
            ViewBag.Plans = data.Plans;
            ViewBag.Subscriptions = data.Subscriptions;
            return View(new Subscription());
        }
        [HttpPost]
        public IActionResult Create(Subscription model)
        {
            if (model.CustomerId != null && data.Customers.Find(c => c.Id == model.CustomerId) == null) ModelState.AddModelError("CustomerId", "Choose an existing customer.");
            if (model.PlanId != null && data.Plans.Find(p => p.Id == model.PlanId) == null) ModelState.AddModelError("PlanId", "Choose an existing plan.");
            if (model.Status != "Active" && model.Status != "Trial" && model.Status != "Paused") ModelState.AddModelError("Status", "Choose Active, Trial or Paused.");
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = data.Customers;
            ViewBag.Plans = data.Plans;
            ViewBag.Subscriptions = data.Subscriptions;
                return View(model);
            }
            model.Customer = data.Customers.Find(c => c.Id == model.CustomerId);
            model.Plan = data.Plans.Find(p => p.Id == model.PlanId);
            // Lab 2 demonstrates binding and validation without saving a record.
            ViewBag.IsPreview = true;
            return View("Details", model);
        }
    }
}
