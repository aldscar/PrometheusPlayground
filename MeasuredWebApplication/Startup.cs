using Prometheus;
using Serilog;
using SharedClasses;

namespace MeasuredWebApplication
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHealthChecks()
                .AddCheck<SampleHealthCheck>("WebApp")
                .ForwardToPrometheus();
        }

        public void Configure(IApplicationBuilder app)
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapMetrics();
            });
        }
    }
}
