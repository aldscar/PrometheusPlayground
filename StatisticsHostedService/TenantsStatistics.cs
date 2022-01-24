
namespace StatisticsHostedService
{
    /// <summary>
    /// Yields fake data
    /// </summary>
    class TenantsStatistics
    {
        private Int32 numberOfCompanies = 5;

        private TenantData[] tenants;

        public TenantsStatistics()
        {
            tenants = Enumerable.Range(0, numberOfCompanies)
                .Select(x => new TenantData())
                .ToArray();
        }

        public TenantData[] LoadTenantStatistics()
        {
            if (Random.Shared.Next(100) == 0)
            {
                tenants = tenants
                    .Concat(new[] { new TenantData() })
                    .ToArray();
            }

            static int getRandom() => Random.Shared.Next(5) - Random.Shared.Next(2);

            foreach (var tenant in tenants)
            {
                tenant.DraftDocuments += getRandom();
                if (tenant.DraftDocuments < 0) tenant.DraftDocuments = 0;

                tenant.PublishedDocuments += getRandom();
                if (tenant.PublishedDocuments < 0) tenant.PublishedDocuments = 0;

                tenant.ApprovedDocuments += getRandom();
                if (tenant.ApprovedDocuments < 0) tenant.ApprovedDocuments = 0;

                tenant.ObsoleteDocuments += getRandom();
                if (tenant.ObsoleteDocuments < 0) tenant.ObsoleteDocuments = 0;
            }

            return tenants;
        }

        public class TenantData
        {
            public String Name { get; set; } = Guid.NewGuid().ToString();
            public Int32 DraftDocuments { get; set; }
            public Int32 PublishedDocuments { get; set; }
            public Int32 ApprovedDocuments { get; set; }
            public Int32 ObsoleteDocuments { get; set; }
        }
    }
}