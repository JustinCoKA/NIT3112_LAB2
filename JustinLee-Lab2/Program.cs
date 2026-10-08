namespace JustinLee_Lab2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Session 3, slide 6: register MVC and use the conventional route.
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            app.Run();
        }
    }
}
