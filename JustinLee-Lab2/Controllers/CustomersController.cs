using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class CustomersController : QuTechAdminController
    {
        public CustomersController(QuTechContext db) : base(db) { }
        public IActionResult Index() => View(Db.Customers.OrderBy(x => x.Id).ToList());
        public IActionResult Details(int id)
        {
            var model = Db.Customers.FirstOrDefault(x => x.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create() { Choices(); return View("Form", new Customer()); }
        [HttpPost]
        public IActionResult Create([Bind("FullName,CompanyName,Email,Phone")] Customer model, string billingAddress)
        {
            Validate(model); if (string.IsNullOrWhiteSpace(billingAddress) || billingAddress.Length > 200) ModelState.AddModelError("billingAddress", "Enter a billing address of at most 200 characters.");
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }
            model.BillingProfile = new BillingProfile { BillingName = model.CompanyName, BillingEmail = model.Email, Address = billingAddress };
            Db.Customers.Add(model);
            if (!SaveAudited("Created Customer", "Customer: " + model.CompanyName)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Customer created and saved.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        public IActionResult Edit(int id)
        {
            var model = Db.Customers.Find(id);
            if (model == null) return NotFound();
            Choices(); return View("Form", model);
        }
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,FullName,CompanyName,Email,Phone")] Customer model)
        {
            if (id != model.Id) return BadRequest();
            var existing = Db.Customers.Find(id);
            if (existing == null) return NotFound();
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }
            existing.FullName = model.FullName;
            existing.CompanyName = model.CompanyName;
            existing.Email = model.Email;
            existing.Phone = model.Phone;
            if (!SaveAudited("Updated Customer", "Customer #" + id)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Changes saved.";
            return RedirectToAction(nameof(Details), new { id });
        }
        public IActionResult Delete(int id)
        {
            var model = Db.Customers.Find(id);
            if (model == null) return NotFound();
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var model = Db.Customers.Find(id);
            if (model == null) return NotFound();
            // Keep related billing and activity history intact instead of deleting parent records.
            if (Db.Subscriptions.Any(x => x.CustomerId == id)) { ModelState.AddModelError("", "This record is in use. Remove or reassign its dependent records first."); return View("Delete", model); }
            var profile = Db.BillingProfiles.Find(id); if (profile != null) Db.BillingProfiles.Remove(profile);
            Db.Customers.Remove(model);
            if (!SaveAudited("Deleted Customer", "Customer #" + id)) return View("Delete", model);
            TempData["Success"] = "Customer deleted."; return RedirectToAction(nameof(Index));
        }
        private void Validate(Customer model) { if (Db.Customers.Any(x => x.Email == model.Email && x.Id != model.Id)) ModelState.AddModelError("Email", "A customer already uses this email."); }
        private void Choices()
        {
            ViewBag.Customers = Db.Customers.OrderBy(x => x.CompanyName).ToList();
            ViewBag.Plans = Db.Plans.OrderBy(x => x.Name).ToList();
            ViewBag.Discounts = Db.Discounts.Where(x => x.Enabled).OrderBy(x => x.Name).ToList();
            ViewBag.Subscriptions = Db.Subscriptions.Include(x => x.Customer).ToList();
        }
    }
}
