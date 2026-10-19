using System.Globalization;

namespace JustinLee_QuTech;

internal static class Program
{
    private static void Main() => new AdminPortal().Run();
}

// Immutable catalogue entry; decimal avoids binary floating-point money errors.
public sealed class Plan
{
    public int Id { get; }
    public string Name { get; }
    public decimal MonthlyPrice { get; }
    public Plan(int id, string name, decimal monthlyPrice)
    {
        Id = id;
        Name = name;
        MonthlyPrice = monthlyPrice;
    }
}

public sealed class Subscription
{
    public const decimal DiscountRate = 0.15m;
    public string BusinessName { get; }
    public Plan Plan { get; }
    public int Users { get; }
    public bool HasDiscount { get; }
    public decimal Subtotal => Plan.MonthlyPrice * Users;
    public decimal DiscountAmount => HasDiscount ? Subtotal * DiscountRate : 0m;
    public decimal PricePerUser => Plan.MonthlyPrice * (HasDiscount ? 1m - DiscountRate : 1m);
    public decimal TotalPrice => Subtotal - DiscountAmount;

    public Subscription(string businessName, Plan plan, int users, bool hasDiscount)
    {
        if (string.IsNullOrWhiteSpace(businessName) || businessName.Length > 60 || businessName.Any(char.IsControl))
            throw new ArgumentException("Business name must contain 1-60 printable characters.");
        if (users < 1 || users > 100) throw new ArgumentOutOfRangeException(nameof(users));
        BusinessName = businessName.Trim();
        Plan = plan ?? throw new ArgumentNullException(nameof(plan));
        Users = users;
        HasDiscount = hasDiscount;
    }
}

public sealed class AdminPortal
{
    private const string AdministratorName = "Justin Lee";
    private const string DiscountCode = "JUST";
    private readonly Plan[] plans =
    {
        new(0, "Startup", 10m), new(1, "Standard", 20m),
        new(2, "Premium", 30m), new(3, "Enterprise", 40m)
    };
    private readonly List<Subscription> subscriptions = new();
    private static string Money(decimal value) => "$" + value.ToString("F2", CultureInfo.InvariantCulture);

    public void Run()
    {
        Console.WriteLine("******** Welcome to QuTech Admin Portal ********");
        Console.WriteLine($"Logged in as: {AdministratorName} (Administrator)");
        // EOF also exits cleanly, including EOF partway through an unsaved entry.
        try
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("\nA. See all Plans\nB. Create a new Subscription\nC. Show all Subscriptions\nD. Quit");
                switch (Read("Your option: ").ToUpperInvariant())
                {
                    case "A": ShowPlans(); break;
                    case "B": CreateSubscription(); break;
                    case "C": ShowSubscriptions(); break;
                    case "D": running = false; break;
                    default: Console.WriteLine("Invalid option. Choose A, B, C or D."); break;
                }
            }
        }
        catch (EndOfStreamException) { Console.WriteLine("\nInput ended. Any incomplete entry was not saved."); }
        Console.WriteLine("Goodbye, Justin Lee. Session data is cleared on exit.");
    }

    private static string Read(string prompt)
    {
        Console.Write(prompt);
        string value = Console.ReadLine() ?? throw new EndOfStreamException();
        return value.Trim();
    }

    private void ShowPlans()
    {
        Console.WriteLine("\nPlan ID | Plan Name    | Monthly price per user");
        foreach (Plan plan in plans)
            Console.WriteLine($"{plan.Id,-7} | {plan.Name,-12} | {Money(plan.MonthlyPrice)}");
    }

    private static int ReadInteger(string prompt, int minimum, int maximum)
    {
        while (true)
        {
            if (int.TryParse(Read(prompt), out int value) && value >= minimum && value <= maximum)
                return value;
            Console.WriteLine($"Enter a whole number from {minimum} to {maximum}.");
        }
    }

    private void CreateSubscription()
    {
        string businessName;
        while (true)
        {
            businessName = Read("Enter business name (1-60 characters): ");
            if (businessName.Length is >= 1 and <= 60 && !businessName.Any(char.IsControl)) break;
            Console.WriteLine("Business name must contain 1-60 printable characters.");
        }
        ShowPlans();
        int planId = ReadInteger("Plan choice (0-3): ", 0, 3);
        int users = ReadInteger("Enter number of users (1-100): ", 1, 100);
        bool hasDiscount;
        while (true)
        {
            string code = Read("Enter discount code or No: ");
            if (code.Equals(DiscountCode, StringComparison.OrdinalIgnoreCase)) { hasDiscount = true; break; }
            if (code.Equals("No", StringComparison.OrdinalIgnoreCase)) { hasDiscount = false; break; }
            Console.WriteLine("Invalid discount code. Enter JUST for 15% off, or No.");
        }
        // Add only after every field is valid; no partial entry reaches the summary.
        var subscription = new Subscription(businessName, plans[planId], users, hasDiscount);
        subscriptions.Add(subscription);
        Console.WriteLine($"\nSaved subscription for {subscription.BusinessName}");
        Console.WriteLine($"Plan: {subscription.Plan.Name} | Users: {subscription.Users}");
        Console.WriteLine($"Price per user: {Money(subscription.PricePerUser)} per month");
        Console.WriteLine($"Subtotal: {Money(subscription.Subtotal)} | Discount: {Money(subscription.DiscountAmount)}");
        Console.WriteLine($"Total monthly price: {Money(subscription.TotalPrice)} | Discount: {(hasDiscount ? "Y" : "N")}");
    }

    private void ShowSubscriptions()
    {
        Console.WriteLine("\nAll Subscriptions");
        if (subscriptions.Count == 0) { Console.WriteLine("No subscriptions yet. Choose B to create one."); return; }
        int width = Math.Max(13, subscriptions.Max(s => s.BusinessName.Length));
        Console.WriteLine($"{"Business name".PadRight(width)} | {"Plan",-12} | {"Users",5} | {"Total Price",11} | Discount");
        Console.WriteLine(new string('-', width + 54));
        foreach (Subscription s in subscriptions)
            Console.WriteLine($"{s.BusinessName.PadRight(width)} | {s.Plan.Name,-12} | {s.Users,5} | {Money(s.TotalPrice),11} | {(s.HasDiscount ? "Y" : "N")}");
        Console.WriteLine($"Subscriptions: {subscriptions.Count} | Combined monthly total: {Money(subscriptions.Sum(s => s.TotalPrice))}");
    }
}
