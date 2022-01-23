using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SharedClasses
{
    public class SampleHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var val = Random.Shared.Next(100);
            var result = val < 75
                ? HealthCheckResult.Healthy("I'm ok!")
                : val < 95
                ? HealthCheckResult.Degraded("It is hard!")
                : HealthCheckResult.Unhealthy("I failed!");

            return Task.FromResult(result);
        }
    }
}