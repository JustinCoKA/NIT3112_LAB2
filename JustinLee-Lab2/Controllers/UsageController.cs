using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;

namespace JustinLee_Lab2.Controllers
{
    public class UsageController : Controller
    {
        private readonly DemoData data = new DemoData();
        public IActionResult Index()
        {
            return View(data.UsageRecords);
        }
        public IActionResult Details(int id)
        {
            var model = data.UsageRecords.Find(item => item.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Customers = data.Customers;
            ViewBag.Plans = data.Plans;
            ViewBag.Subscriptions = data.Subscriptions;
            return View(new UsageRecord());
        }
        [HttpPost]
        public IActionResult Create(UsageRecord model)
        {
            if (model.SubscriptionId != null && data.Subscriptions.Find(s => s.Id == model.SubscriptionId) == null) ModelState.AddModelError("SubscriptionId", "Choose an existing subscription.");
            if (model.ApiCalls < 0) ModelState.AddModelError("ApiCalls", "API call usage cannot be negative.");
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
