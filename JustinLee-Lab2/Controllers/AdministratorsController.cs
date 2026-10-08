using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;

namespace JustinLee_Lab2.Controllers
{
    public class AdministratorsController : Controller
    {
        private readonly DemoData data = new DemoData();
        public IActionResult Index()
        {
            return View(data.Administrators);
        }
        public IActionResult Details(int id)
        {
            var model = data.Administrators.Find(item => item.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Customers = data.Customers;
            ViewBag.Plans = data.Plans;
            ViewBag.Subscriptions = data.Subscriptions;
            return View(new Administrator());
        }
        [HttpPost]
        public IActionResult Create(Administrator model)
        {

            
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
