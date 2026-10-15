using JustinLee_Lab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JustinLee_Lab2.Data
{
    public static class SeedData
    {
        // Session 7: deterministic HasData values become part of the migration.
        // All organisations, contact details and activity records below are fictional.
        public static void Configure(ModelBuilder model)
        {
            string[] names = { "Mia Chen", "Oliver Patel", "Amelia Brooks", "Noah Wilson", "Ava Singh" };
            string[] companies = { "Harbour Studio", "Summit Logistics", "Greenfield Retail", "Coastal Design", "Riverbank Consulting" };
            string[] emails = { "mia", "oliver", "amelia", "noah", "ava" };
            string[] admins = { "Alex Morgan", "Taylor Kim", "Jordan Ellis", "Casey Tran", "Riley Park" };
            string[] adminEmails = { "alex", "taylor", "jordan", "casey", "riley" };
            string[] departments = { "Platform operations", "Billing support", "Customer support", "Service configuration", "Compliance" };
            string[] plans = { "Starter", "Growth", "Scale", "Enterprise", "Community" };
            decimal[] prices = { 49m, 149m, 399m, 799m, 29m };
            int[] calls = { 10000, 50000, 200000, 500000, 5000 };
            string[] modules = { "Workspace", "Billing", "Analytics", "Audit", "Integrations" };
            string[] moduleDescriptions = { "Manage customer workspaces and service settings.", "Generate invoices and track payment status.", "Review subscription revenue and API usage.", "Review accountable administrator activity.", "Connect business applications through API access." };
            string[] discountNames = { "Welcome", "Loyalty", "Education", "Community", "Seasonal" };
            decimal[] percentages = { 10m, 15m, 20m, 25m, 5m };
            int[] selectedPlans = { 2, 3, 1, 4, 5 };
            string[] statuses = { "Active", "Active", "Trial", "Active", "Paused" };
            int[] usage = { 32400, 142600, 7200, 185000, 2100 };
            for (int n = 0; n < 5; n++)
            {
                int id = n + 1;
                model.Entity<Customer>().HasData(new Customer { Id = id, FullName = names[n], CompanyName = companies[n], Email = emails[n] + "@example.com", Phone = "+61 2 5550 010" + id });
                model.Entity<BillingProfile>().HasData(new BillingProfile { CustomerId = id, BillingName = companies[n], BillingEmail = "accounts." + emails[n] + "@example.com", Address = (id * 10) + " Example Street, Sydney NSW 2000" });
                model.Entity<Administrator>().HasData(new Administrator { Id = 101 + n, FullName = admins[n], Email = adminEmails[n] + "@example.com", Phone = "+61 2 5550 020" + id, Department = departments[n] });
                model.Entity<Plan>().HasData(new Plan { Id = id, Name = plans[n], Description = plans[n] + " subscription for QuTech customer workspaces with " + calls[n] + " monthly API calls.", MonthlyPrice = prices[n], IncludedApiCalls = calls[n] });
                model.Entity<Module>().HasData(new Module { Id = id, Name = modules[n], Description = moduleDescriptions[n] });
                model.Entity<Discount>().HasData(new Discount { Id = id, Name = discountNames[n], Description = discountNames[n] + " programme: " + percentages[n] + "% off eligible monthly subscription charges.", Percentage = percentages[n], StartsOn = new DateTime(2026, 1, 1), EndsOn = new DateTime(2027, 12, 31), Enabled = true });
                model.Entity<Subscription>().HasData(new Subscription { Id = id, CustomerId = id, PlanId = selectedPlans[n], DiscountId = id, StartDate = new DateTime(2026, 9, 1), Status = statuses[n] });
                model.Entity<UsageRecord>().HasData(new UsageRecord { Id = id, SubscriptionId = id, PeriodStart = new DateTime(2026, 10, 1), ApiCalls = usage[n] });
                decimal subtotal = prices[selectedPlans[n] - 1];
                decimal reduction = Math.Round(subtotal * percentages[n] / 100m, 2, MidpointRounding.AwayFromZero);
                model.Entity<Invoice>().HasData(new Invoice { Id = id, Reference = "QT-2026-10-00" + id, SubscriptionId = id, PeriodStart = new DateTime(2026, 10, 1), IssuedOn = new DateTime(2026, 10, 1), DueOn = new DateTime(2026, 10, 15), Subtotal = subtotal, DiscountAmount = reduction, Amount = subtotal - reduction, Status = n % 2 == 0 ? "Paid" : "Unpaid" });
                model.Entity<AuditEntry>().HasData(new AuditEntry { Id = id, AdministratorId = 101 + n, OccurredAt = new DateTime(2026, 10, 1, 9, n * 5, 0), Action = "Seeded demonstration subscription and October invoice", Target = "Subscription #" + id });
            }
            for (int plan = 1; plan <= 5; plan++)
            {
                int count = plan == 5 ? 1 : Math.Min(plan + 1, 5);
                for (int module = 1; module <= count; module++)
                    model.Entity<PlanModule>().HasData(new PlanModule { PlanId = plan, ModuleId = module, Enabled = true });
            }
        }

        // Session 9: UserManager hashes passwords; no plaintext passwords are stored in SQLite.
        // These known credentials are ONLY for this local assessment demonstration.
        public static async Task InitializeAccountsAsync(IServiceProvider services)
        {
            var roles = services.GetRequiredService<RoleManager<IdentityRole>>();
            var users = services.GetRequiredService<UserManager<ApplicationUser>>();
            if (!await roles.RoleExistsAsync("Admin"))
            {
                var roleResult = await roles.CreateAsync(new IdentityRole("Admin"));
                if (!roleResult.Succeeded) throw new InvalidOperationException("Could not create the demonstration administrator role.");
            }
            string[] emails = { "alex", "taylor", "jordan", "casey", "riley" };
            for (int n = 0; n < emails.Length; n++)
            {
                string email = emails[n] + "@example.com";
                var user = await users.FindByEmailAsync(email);
                if (user == null)
                {
                    // Do not recreate a deliberately deleted administrator on a subsequent launch.
                    var db = services.GetRequiredService<QuTechContext>();
                    if (!await db.Administrators.AnyAsync(a => a.Id == 101 + n) ||
                        await db.Users.AnyAsync(u => u.AdministratorId == 101 + n)) continue;
                    user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true, AdministratorId = 101 + n };
                    var result = await users.CreateAsync(user, "QuTechLab3!2026");
                    if (!result.Succeeded) throw new InvalidOperationException("Could not create the demonstration administrator account.");
                    var assignment = await users.AddToRoleAsync(user, "Admin");
                    if (!assignment.Succeeded) throw new InvalidOperationException("Could not assign the demonstration administrator role.");
                }
            }
        }
    }
}
