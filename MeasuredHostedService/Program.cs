using MeasuredHostedService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SharedClasses;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration))
                .ConfigureServices((ctx, config) =>
                {
                    config.AddHostedService<WorkloadService>();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseKestrel()
                        .UseStartup<BaseHostedServiceStartup>();
                })
                .RunConsoleAsync();
    }
}