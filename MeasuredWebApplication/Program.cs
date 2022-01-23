using Prometheus;
using Serilog;
using SharedClasses;

await Host.CreateDefaultBuilder(args)
    .UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration))
    .ConfigureWebHostDefaults(builder =>
    {
        builder.ConfigureServices(services =>
        {
            services.AddControllers();
            services.AddHealthChecks()
                .AddCheck<SampleHealthCheck>("WebApp")
                .ForwardToPrometheus();
        });
        builder.Configure((IApplicationBuilder app) =>
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
        });
    })
    .RunConsoleAsync();
