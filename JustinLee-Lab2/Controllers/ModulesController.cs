using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
namespace JustinLee_Lab2.Controllers
{
    public class ModulesController : QuTechAdminController
    {
        public ModulesController(QuTechContext db) : base(db) { }
        public IActionResult Index() => View(Db.Modules.OrderBy(x => x.Id).ToList());
        public IActionResult Details(int id)
        {
            var model = Db.Modules.FirstOrDefault(x => x.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        public IActionResult Create() { Choices(); return View("Form", new Module()); }
        [HttpPost]
        public IActionResult Create([Bind("Name,Description")] Module model)
        {
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }

            Db.Modules.Add(model);
            if (!SaveAudited("Created Module", "Module: " + model.Name)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Module created and saved.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }
        public IActionResult Edit(int id)
        {
            var model = Db.Modules.Find(id);
            if (model == null) return NotFound();
            Choices(); return View("Form", model);
        }
        [HttpPost]
        public IActionResult Edit(int id, [Bind("Id,Name,Description")] Module model)
        {
            if (id != model.Id) return BadRequest();
            var existing = Db.Modules.Find(id);
            if (existing == null) return NotFound();
            Validate(model);
            if (!ModelState.IsValid) { Choices(); return View("Form", model); }
            existing.Name = model.Name;
            existing.Description = model.Description;
            if (!SaveAudited("Updated Module", "Module #" + id)) { Choices(); return View("Form", model); }
            TempData["Success"] = "Changes saved.";
            return RedirectToAction(nameof(Details), new { id });
        }
        public IActionResult Delete(int id)
        {
            var model = Db.Modules.Find(id);
            if (model == null) return NotFound();
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var model = Db.Modules.Find(id);
            if (model == null) return NotFound();
            // Keep related billing and activity history intact instead of deleting parent records.
            if (Db.PlanModules.Any(x => x.ModuleId == id)) { ModelState.AddModelError("", "This record is in use. Remove or reassign its dependent records first."); return View("Delete", model); }

            Db.Modules.Remove(model);
            if (!SaveAudited("Deleted Module", "Module #" + id)) return View("Delete", model);
            TempData["Success"] = "Module deleted."; return RedirectToAction(nameof(Index));
        }
        private void Validate(Module model) { if (Db.Modules.Any(x => x.Name == model.Name && x.Id != model.Id)) ModelState.AddModelError("Name", "A module already uses this name."); }
        private void Choices()
        {
            ViewBag.Customers = Db.Customers.OrderBy(x => x.CompanyName).ToList();
            ViewBag.Plans = Db.Plans.OrderBy(x => x.Name).ToList();
            ViewBag.Discounts = Db.Discounts.Where(x => x.Enabled).OrderBy(x => x.Name).ToList();
            ViewBag.Subscriptions = Db.Subscriptions.Include(x => x.Customer).ToList();
        }
    }
}
