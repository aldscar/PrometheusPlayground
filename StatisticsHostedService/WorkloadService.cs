using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StatisticsHostedService
{
    internal class WorkloadService : BackgroundService
    {
        private readonly TimeSpan refreshPeriod = TimeSpan.FromSeconds(30);

        private readonly TenantsStatistics tenantsStatistics;
        private readonly ILogger<WorkloadService> logger;


        public WorkloadService(ILogger<WorkloadService> logger,
                               TenantsStatistics tenantsStatistics)
        {
            this.logger = logger;
            this.tenantsStatistics = tenantsStatistics;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Never-ending loop for updating metrics
            //Ends only if hosted services stops.
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Reload tenant statistics");

                var tenants = tenantsStatistics.LoadTenantStatistics();

                StatisticsMetrics.TenantsTotal.Set(tenants.Length);

                foreach (var tenant in tenants)
                {
                    StatisticsMetrics.DocumentsTotalPerTenant
                        .WithLabels(tenant.Name, "Draft").Set(tenant.DraftDocuments);

                    StatisticsMetrics.DocumentsTotalPerTenant
                        .WithLabels(tenant.Name, "Published").Set(tenant.PublishedDocuments);

                    StatisticsMetrics.DocumentsTotalPerTenant
                        .WithLabels(tenant.Name, "Obsolete").Set(tenant.ObsoleteDocuments);

                    StatisticsMetrics.DocumentsTotalPerTenant
                        .WithLabels(tenant.Name, "Approved").Set(tenant.ApprovedDocuments);
                }

                await Task.Delay(refreshPeriod, stoppingToken);
            }
        }
    }
}