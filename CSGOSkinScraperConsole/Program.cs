
// File: CSGOSkinScraperConsole/Program.cs

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CSGOSkinApp.Data;
using CSGOSkinApp.Services;

using IHost host = CreateHostBuilder(args).Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var consoleService = services.GetRequiredService<IMyConsoleService>();
        await consoleService.RunAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

await host.RunAsync();

IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        })
        .ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(hostContext.Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<SkinScraper>();
            services.AddTransient<IMyConsoleService, MyConsoleService>();
        });

public interface IMyConsoleService
{
    Task RunAsync();
}

public class MyConsoleService : IMyConsoleService
{
    private readonly SkinScraper _skinScraper;

    public MyConsoleService(SkinScraper skinScraper)
    {
        _skinScraper = skinScraper;
    }

    public async Task RunAsync()
    {
        await _skinScraper.ScrapeSkins();
        Console.WriteLine("Scraping completed successfully.");
    }
}