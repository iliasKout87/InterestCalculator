using InterestCalculator.Models.Types;
using InterestCalculator.Models.InterestCalculation;
using Microsoft.Extensions.DependencyInjection;
using InterestCalculator.Persistence.InterestCalculation;
using InterestCalculator.Application.Persistence;
using InterestCalculator.Persistence.WebScraping;
using InterestCalculator.Infrastructure.WebScraping;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

// Helper methods for input validation
static decimal GetCapitalAmount()
{
    while (true)
    {
        Console.Write("Enter capital amount (e.g. 1000.00): ");
        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
        {
            return amount;
        }
        Console.WriteLine("Invalid input. Please enter a positive number.");
    }
}

static DateOnly GetDate(string prompt)
{
    while (true)
    {
        Console.Write($"Enter {prompt} (dd/MM/yyyy): ");
        var input = Console.ReadLine() ?? "";
        
        // Try parsing with flexible date formats
        var formats = new[] { "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MM/yyyy" };
        if (DateOnly.TryParseExact(input, formats, null, System.Globalization.DateTimeStyles.None, out DateOnly date))
        {
            return date;
        }
        Console.WriteLine("Invalid date format. Please use dd/MM/yyyy format.");
    }
}

static (DateOnly startDate, DateOnly endDate) GetValidDateRange()
{
    while (true)
    {
        var startDate = GetDate("start date");
        var endDate = GetDate("end date");

        if (endDate > startDate)
        {
            return (startDate, endDate);
        }
        Console.WriteLine("End date must be after start date. Please try again.");
    }
}

static bool ContinueCalculating()
{
    while (true)
    {
        Console.Write("\nWould you like to calculate another interest schedule? (y/n): ");
        var response = Console.ReadLine()?.ToLower().Trim();
        
        if (response == "y" || response == "yes")
            return true;
        if (response == "n" || response == "no")
            return false;
            
        Console.WriteLine("Please enter 'y' for yes or 'n' for no.");
    }
}

// Main program
var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var appSettingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        config.AddJsonFile(appSettingsPath, optional: false, reloadOnChange: true);
        config.AddEnvironmentVariables();
        config.AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
		//TODO: verify registration scope
        services.ConfigureOptions<BankInterestRateOptionsSetup>();
        services.AddSingleton<IReadOnlyRepository<BankInterestRateSchedule>, BankInterestRateScheduleRepository>();
        services.AddSingleton<IWebScraping, HtmlAgilityWebScraping>();
    })
    .Build();

var bankInterestRateScheduleRepository = host.Services.GetRequiredService<IReadOnlyRepository<BankInterestRateSchedule>>();
var bankInterestRateSchedule = await bankInterestRateScheduleRepository.GetAsync();

Console.WriteLine("Welcome to Interest Calculator!");
Console.WriteLine("-------------------------------");

do
{
    Console.Clear();
    Console.WriteLine("Interest Calculator");
    Console.WriteLine("------------------");

    // Get user input
    var capitalAmount = GetCapitalAmount();
    var (startDate, endDate) = GetValidDateRange();

    var interestSchedule = bankInterestRateSchedule.ToInterestSchedule(
        new DatePeriod(startDate, endDate),
        new Capital(new Money(capitalAmount, new Currency("EUR")))
    );

    Console.WriteLine("\nCalculated Interest Schedule:");
    Console.WriteLine("Start Date   End Date     Days  Rate    Interest Amount");
    Console.WriteLine("------------------------------------------------");

    foreach (var interest in interestSchedule.Interests)
    {
        Console.WriteLine("{0}  {1}  {2,4}  {3,4:0.00}%  {4,14:0.00}",
            interest.Period.Period.StartDate.ToString("dd/MM/yyyy"),
            interest.Period.Period.EndDate.ToString("dd/MM/yyyy"),
            interest.Period.Period.DurationInDays,
            interest.Period.InterestRate.Rate * 100,
            interest.Amount.Money.Amount);
    }

    Console.WriteLine("------------------------------------------------");
    var totalInterest = interestSchedule.Interests.Sum(i => Math.Round(i.Amount.Money.Amount, 2));
    Console.WriteLine("Capital Amount: {0,33:0.00}", capitalAmount);
    Console.WriteLine("Total Interest: {0,33:0.00}", totalInterest);
    Console.WriteLine("Total Amount:   {0,33:0.00}", capitalAmount + totalInterest);

} while (ContinueCalculating());

Console.WriteLine("\nThank you for using Interest Calculator!");
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();



