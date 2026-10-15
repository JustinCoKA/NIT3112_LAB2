using JustinLee_Lab2.Data;
using JustinLee_Lab2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JustinLee_Lab2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews(options =>
            {
                // Session 8: verify the hidden antiforgery token on all modifying requests.
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                var messages = options.ModelBindingMessageProvider;
                messages.SetAttemptedValueIsInvalidAccessor((value, field) => "Enter a valid value for " + field + ".");
                messages.SetNonPropertyAttemptedValueIsInvalidAccessor(value => "Enter a valid value in this field.");
                messages.SetValueMustBeANumberAccessor(field => "Enter a valid number for " + field + ".");
                messages.SetNonPropertyValueMustBeANumberAccessor(() => "Enter a valid number in this field.");
                messages.SetValueMustNotBeNullAccessor(value => "Enter a value in this field.");
                messages.SetValueIsInvalidAccessor(value => "Enter a valid value in this field.");
                messages.SetMissingBindRequiredValueAccessor(field => "Provide " + field + ".");
                messages.SetMissingKeyOrValueAccessor(() => "Provide both required values.");
                messages.SetUnknownValueIsInvalidAccessor(field => "Enter a valid value for " + field + ".");
                messages.SetNonPropertyUnknownValueIsInvalidAccessor(() => "Enter a valid value in this field.");
            });
            // Session 7: resolve relative database paths against the project content root.
            var connection = new SqliteConnectionStringBuilder(builder.Configuration.GetConnectionString("QuTech") ?? "Data Source=qutech.db");
            if (connection.DataSource != ":memory:" && !Path.IsPathRooted(connection.DataSource))
                connection.DataSource = Path.Combine(builder.Environment.ContentRootPath, connection.DataSource);
            builder.Services.AddDbContext<QuTechContext>(options => options.UseSqlite(connection.ToString()));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 12;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            }).AddEntityFrameworkStores<QuTechContext>().AddDefaultTokenProviders();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
            });
            var app = builder.Build();
            // Session 7 slide 26: migrations make a fresh extracted submission ready to run.
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<QuTechContext>();
                await db.Database.MigrateAsync();
                await SeedData.InitializeAccountsAsync(scope.ServiceProvider);
            }
            if (!app.Environment.IsDevelopment()) app.UseExceptionHandler("/Home/Error");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
