using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Serilog;
using SharedClasses;

namespace StatisticsHostedService
{
    public class Startup
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TenantsStatistics>();
            services.AddHostedService<WorkloadService>();

            services.AddRouting();
            services.AddHealthChecks()
                .AddCheck<SampleHealthCheck>("AppHealthCheck")
                .ForwardToPrometheus();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
            app.UseMetricServer();
            app.UseRouting();
            app.UseHealthChecks("/health");

            app.UseHttpMetrics(config =>
            {
                config.RequestDuration.Enabled = true;
                config.RequestCount.Enabled = true;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Go to /health to see the health status");
            });
        }
    }
}