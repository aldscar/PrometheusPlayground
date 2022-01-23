using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;

internal class WorkloadService : BackgroundService
{
    private readonly TenantsStatistics tenantsStatistics = new TenantsStatistics();
    private readonly ILogger<WorkloadService> logger;

    private static readonly Gauge TenantsTotal =
        Metrics.CreateGauge("business_tenants_total", "Number of tenants registred in db.");

    private static readonly Gauge Documents =
        Metrics.CreateGauge("business_documents_number",
                            "number of documents registred on a tenant",
                            new GaugeConfiguration() { LabelNames = new[] { "tenant", "status" } });

    public WorkloadService(ILogger<WorkloadService> logger)
    {
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var tenants = await tenantsStatistics.LoadTenantStatistics();

            TenantsTotal.Set(tenants.Count());

            foreach (var tenant in tenants)
            {
                Documents.WithLabels(tenant.Name, "Draft").Set(tenant.DraftDocuments);
                Documents.WithLabels(tenant.Name, "Published").Set(tenant.PublishedDocuments);
                Documents.WithLabels(tenant.Name, "Obsolete").Set(tenant.ObsoleteDocuments);
                Documents.WithLabels(tenant.Name, "Approved").Set(tenant.ApprovedDocuments);
            }
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
