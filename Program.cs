using PowerTradeJob;
using PowerTradeJob.Models;
using PowerTradeJob.Services;

public class Program
{
    public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

    public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureLogging(logging =>
           {
               logging.ClearProviders();
               logging.AddConsole();
               logging.AddEventLog();
           })
          .UseWindowsService()
           .ConfigureServices(configureServices);

    private static void configureServices(HostBuilderContext context, IServiceCollection services)
    {

        services.AddLogging();
        services.AddTransient<IPowerTradeService, PowerTradeService>();
        services.AddHostedService<PowerTradeWorker>();
       
    }
}

