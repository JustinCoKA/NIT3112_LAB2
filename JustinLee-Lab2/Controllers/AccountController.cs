using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JustinLee_Lab2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly QuTechContext context;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, QuTechContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            // Only routes within this application may be used after login.
            if (!string.IsNullOrEmpty(returnUrl) && !Url.IsLocalUrl(returnUrl))
                return BadRequest("The requested return address is not a local application page.");

            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!string.IsNullOrEmpty(model.ReturnUrl) && !Url.IsLocalUrl(model.ReturnUrl))
                return BadRequest("The requested return address is not a local application page.");

            if (!ModelState.IsValid)
                return View(model);

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null && await userManager.IsInRoleAsync(user, "Admin"))
            {
                // Identity verifies the stored hash and counts unsuccessful attempts for lockout.
                var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    context.AuditEntries.Add(new AuditEntry
                    {
                        AdministratorId = user.AdministratorId,
                        OccurredAt = DateTime.UtcNow,
                        Action = "Administrator signed in.",
                        Target = "Administrator #" + user.AdministratorId
                    });
                    await context.SaveChangesAsync();
                    await signInManager.SignInAsync(user, isPersistent: false);
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                        return LocalRedirect(model.ReturnUrl);
                    return RedirectToAction("Index", "Home");
                }
            }

            // Do not reveal whether an email exists, lacks a role, or is temporarily locked.
            ModelState.AddModelError(string.Empty,
                "Unable to sign in. Check your administrator credentials or try again later.");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                context.AuditEntries.Add(new AuditEntry
                {
                    AdministratorId = user.AdministratorId,
                    OccurredAt = DateTime.UtcNow,
                    Action = "Administrator signed out.",
                    Target = "Administrator #" + user.AdministratorId
                });
                await context.SaveChangesAsync();
            }
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = StatusCodes.Status403Forbidden;
            return View();
        }
    }
}
