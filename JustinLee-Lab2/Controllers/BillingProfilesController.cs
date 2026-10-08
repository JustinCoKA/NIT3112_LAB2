using Microsoft.AspNetCore.Mvc;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class BillingProfilesController : Controller
    {
        private readonly DemoData data = new DemoData();
        public IActionResult Edit(int id)
        {
            var profile = data.BillingProfiles.Find(p => p.CustomerId == id);
            if (profile == null) return NotFound();
            return View(profile);
        }
        [HttpPost]
        public IActionResult Edit(int id, BillingProfile model)
        {
            var customer = data.Customers.Find(c => c.Id == id);
            if (customer == null) return NotFound();
            model.CustomerId = id;
            if (!ModelState.IsValid) return View(model);
            ViewBag.IsPreview = true;
            return View(model);
        }
    }
}
