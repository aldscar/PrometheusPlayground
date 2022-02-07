using MeasuredWebApplication;
using Serilog;

await Host.CreateDefaultBuilder(args)
    .UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration))
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .RunConsoleAsync();
