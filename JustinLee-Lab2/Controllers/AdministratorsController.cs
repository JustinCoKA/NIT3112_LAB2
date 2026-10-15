using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JustinLee_Lab2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorsController : Controller
    {
        private readonly QuTechContext context;
        private readonly UserManager<ApplicationUser> users;

        public AdministratorsController(QuTechContext context, UserManager<ApplicationUser> users)
        {
            this.context = context;
            this.users = users;
        }

        public IActionResult Index() => View(context.Administrators.Include(a => a.AuditEntries).OrderBy(a => a.FullName).ToList());

        public IActionResult Details(int id)
        {
            var model = context.Administrators.Include(a => a.AuditEntries).FirstOrDefault(a => a.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        public IActionResult Create() => View(new Administrator());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Email,Phone,Department")] Administrator model, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("password", "Enter an initial administrator password.");
            if (model.Email != null && await users.FindByEmailAsync(model.Email) != null)
                ModelState.AddModelError("Email", "An account already uses this email address.");
            if (!ModelState.IsValid) return View(model);

            // Profile, Identity credentials and role membership must all succeed together.
            using var transaction = await context.Database.BeginTransactionAsync();
            context.Administrators.Add(model);
            await context.SaveChangesAsync();
            var account = new ApplicationUser { UserName = model.Email, Email = model.Email, AdministratorId = model.Id };
            var result = await users.CreateAsync(account, password);
            if (result.Succeeded) result = await users.AddToRoleAsync(account, "Admin");
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                AddAccountErrors(result);
                model.Id = 0;
                return View(model);
            }
            await RecordAudit("Created administrator account.", model.Id);
            await transaction.CommitAsync();
            TempData["Success"] = "Administrator and login account created.";
            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        public IActionResult Edit(int id)
        {
            var model = context.Administrators.Find(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Phone,Department")] Administrator model)
        {
            if (id != model.Id) return BadRequest();
            var existing = await context.Administrators.FindAsync(id);
            if (existing == null) return NotFound();
            var account = await context.Users.FirstOrDefaultAsync(u => u.AdministratorId == id);
            if (account == null) ModelState.AddModelError(string.Empty, "The linked login account could not be found.");
            var duplicate = model.Email == null ? null : await users.FindByEmailAsync(model.Email);
            if (duplicate != null && duplicate.Id != account?.Id)
                ModelState.AddModelError("Email", "An account already uses this email address.");
            if (!ModelState.IsValid) return View(model);

            using var transaction = await context.Database.BeginTransactionAsync();
            account.Email = model.Email;
            account.UserName = model.Email;
            var result = await users.UpdateAsync(account);
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                AddAccountErrors(result);
                return View(model);
            }
            existing.FullName = model.FullName;
            existing.Email = model.Email;
            existing.Phone = model.Phone;
            existing.Department = model.Department;
            await RecordAudit("Updated administrator profile and account email.", id);
            await transaction.CommitAsync();
            TempData["Success"] = "Administrator details saved.";
            return RedirectToAction(nameof(Details), new { id });
        }

        public IActionResult Delete(int id)
        {
            var model = context.Administrators.Include(a => a.AuditEntries).FirstOrDefault(a => a.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var model = await context.Administrators.Include(a => a.AuditEntries).FirstOrDefaultAsync(a => a.Id == id);
            if (model == null) return NotFound();
            var actor = await users.GetUserAsync(User);
            if (actor?.AdministratorId == id)
                ModelState.AddModelError(string.Empty, "You cannot delete your own administrator account.");
            if (model.AuditEntries.Count > 0)
                ModelState.AddModelError(string.Empty, "This administrator has audit history and must be retained for accountability.");
            if (!ModelState.IsValid) return View("Delete", model);

            using var transaction = await context.Database.BeginTransactionAsync();
            var account = await context.Users.FirstOrDefaultAsync(u => u.AdministratorId == id);
            if (account != null)
            {
                var result = await users.DeleteAsync(account);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    AddAccountErrors(result);
                    return View("Delete", model);
                }
            }
            context.Administrators.Remove(model);
            await RecordAudit("Deleted unused administrator account.", id);
            await transaction.CommitAsync();
            TempData["Success"] = "Administrator account deleted.";
            return RedirectToAction(nameof(Index));
        }

        private async Task RecordAudit(string action, int targetId)
        {
            var actor = await users.GetUserAsync(User);
            context.AuditEntries.Add(new AuditEntry
            {
                AdministratorId = actor.AdministratorId,
                OccurredAt = DateTime.UtcNow,
                Action = action,
                Target = "Administrator #" + targetId
            });
            await context.SaveChangesAsync();
        }

        private void AddAccountErrors(IdentityResult result)
        {
            // Translate Identity codes into coursework-specific messages, never echo a password.
            foreach (var error in result.Errors)
            {
                var message = error.Code switch
                {
                    "DuplicateEmail" or "DuplicateUserName" => "An account already uses this email address.",
                    "InvalidEmail" or "InvalidUserName" => "Enter an email address suitable for an administrator login.",
                    "PasswordTooShort" => "Choose a password with at least 12 characters.",
                    "PasswordRequiresDigit" => "Include at least one number in the administrator password.",
                    "PasswordRequiresLower" => "Include at least one lowercase letter in the administrator password.",
                    "PasswordRequiresUpper" => "Include at least one uppercase letter in the administrator password.",
                    "PasswordRequiresNonAlphanumeric" => "Include at least one symbol in the administrator password.",
                    _ => "The account could not be saved. Review the details and try again."
                };
                ModelState.AddModelError(string.Empty, message);
            }
        }
    }
}
