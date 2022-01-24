using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MeasuredHostedService
{
    /// <summary>
    /// Emulates memory load
    /// </summary>
    internal class WorkloadService : BackgroundService
    {
        private readonly ILogger<WorkloadService> logger;
        private readonly DateTime started = DateTime.Now;
        private readonly Int32 InstanceDelay = Random.Shared.Next(3000);

        public WorkloadService(ILogger<WorkloadService> logger)
        {
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var x = ((DateTime.Now - started).TotalSeconds / Math.PI);
                var y = 1024 * (3 + Math.Sin(x) + Math.Sin(x / 2) + Math.Sin(x * 2));
                var size = (Int32)y;
                var int32Array = new Int32[size];

                logger.LogInformation("Hello from {service}", nameof(WorkloadService));

                await Task.Delay(InstanceDelay, stoppingToken);
            }
        }
    }
}