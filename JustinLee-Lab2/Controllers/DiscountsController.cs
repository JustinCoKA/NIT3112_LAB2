using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class DiscountsController : QuTechAdminController
    {
        public DiscountsController(QuTechContext db) : base(db) { }
        public IActionResult Index() => View(Db.Discounts.OrderBy(x => x.Id).ToList());
        public IActionResult Details(int id)
        {
            var model = Db.Discounts.FirstOrDefault(x => x.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create() { Choices(); return View("Form", new Discount()); }
        [HttpPost]
        public IActionResult Create([Bind("Name,Description,Percentage,StartsOn,EndsOn,Enabled")] Discount model)
        {
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }

            Db.Discounts.Add(model);
            if (!SaveAudited("Created Discount", "Discount: " + model.Name)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Discount created and saved.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        public IActionResult Edit(int id)
        {
            var model = Db.Discounts.Find(id);
            if (model == null) return NotFound();
            Choices(); return View("Form", model);
        }
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Name,Description,Percentage,StartsOn,EndsOn,Enabled")] Discount model)
        {
            if (id != model.Id) return BadRequest();
            var existing = Db.Discounts.Find(id);
            if (existing == null) return NotFound();
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }
            existing.Name = model.Name;
            existing.Description = model.Description;
            existing.Percentage = model.Percentage;
            existing.StartsOn = model.StartsOn;
            existing.EndsOn = model.EndsOn;
            existing.Enabled = model.Enabled;
            if (!SaveAudited("Updated Discount", "Discount #" + id)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Changes saved.";
            return RedirectToAction(nameof(Details), new { id });
        }
        public IActionResult Delete(int id)
        {
            var model = Db.Discounts.Find(id);
            if (model == null) return NotFound();
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var model = Db.Discounts.Find(id);
            if (model == null) return NotFound();
            // Keep related billing and activity history intact instead of deleting parent records.
            if (Db.Subscriptions.Any(x => x.DiscountId == id)) { ModelState.AddModelError("", "This record is in use. Remove or reassign its dependent records first."); return View("Delete", model); }

            Db.Discounts.Remove(model);
            if (!SaveAudited("Deleted Discount", "Discount #" + id)) return View("Delete", model);
            TempData["Success"] = "Discount deleted."; return RedirectToAction(nameof(Index));
        }
        private void Validate(Discount model)
        {
            if (model.Percentage < 0 || model.Percentage > 100) ModelState.AddModelError("Percentage", "Enter a percentage from 0 to 100.");
            if (model.EndsOn < model.StartsOn) ModelState.AddModelError("EndsOn", "The end date must be on or after the start date.");
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
