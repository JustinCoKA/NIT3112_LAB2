using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;

namespace JustinLee_Lab2.Controllers
{
    public class PlansController : Controller
    {
        private readonly DemoData data = new DemoData();
        public IActionResult Index()
        {
            return View(data.Plans);
        }
        public IActionResult Details(int id)
        {
            var model = data.Plans.Find(item => item.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Customers = data.Customers;
            ViewBag.Plans = data.Plans;
            ViewBag.Subscriptions = data.Subscriptions;
            return View(new Plan());
        }
        [HttpPost]
        public IActionResult Create(Plan model)
        {
            if (model.MonthlyPrice < 0) ModelState.AddModelError("MonthlyPrice", "The monthly price cannot be negative.");
            if (model.IncludedApiCalls < 1) ModelState.AddModelError("IncludedApiCalls", "Include at least one API call per month.");
            if (!ModelState.IsValid)
            {
                ViewBag.Customers = data.Customers;
            ViewBag.Plans = data.Plans;
            ViewBag.Subscriptions = data.Subscriptions;
                return View(model);
            }
            
            // Lab 2 demonstrates binding and validation without saving a record.
            ViewBag.IsPreview = true;
            return View("Details", model);
        }
    }
}
