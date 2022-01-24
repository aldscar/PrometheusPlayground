using Prometheus;

namespace StatisticsHostedService
{
    internal static class StatisticsMetrics
    {
        public static readonly Gauge TenantsTotal =
            Metrics.CreateGauge("business_tenants_total", "Number of tenants registred in db.");

        public static readonly Gauge DocumentsTotalPerTenant =
            Metrics.CreateGauge("business_documents_number",
                                "number of documents registred on a tenant",
                                new GaugeConfiguration() { LabelNames = new[] { "tenant", "status" } });

    }
}
