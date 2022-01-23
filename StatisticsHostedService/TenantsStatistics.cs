using System.Net.Http.Headers;
using System.Text.Json;

class TenantsStatistics
{
    private Int32 numberOfCompanies = 5;

    private TenantData[] tenants;

    public async Task<TenantData[]> LoadTenantStatistics()
    {
        if (tenants == null)
        {
            tenants = await GetTenants(numberOfCompanies);
        }

        if (Random.Shared.Next(100) == 0)
        {
            var newCompanies = await GetTenants(1);
            tenants = tenants.Concat(newCompanies).ToArray();
        }

        foreach (var tenant in tenants)
            RefreshValues(tenant);

        return tenants;
    }

    private void RefreshValues(TenantData tenant)
    {
        tenant.DraftDocuments += Random.Shared.Next(10);
        tenant.DraftDocuments -= Random.Shared.Next(tenant.DraftDocuments);
        if (tenant.DraftDocuments < 0) tenant.DraftDocuments = 0;

        tenant.PublishedDocuments += Random.Shared.Next(5) - Random.Shared.Next(2);
        if (tenant.PublishedDocuments < 0) tenant.PublishedDocuments = 0;

        tenant.ApprovedDocuments += Random.Shared.Next(5) - Random.Shared.Next(2);
        if (tenant.ApprovedDocuments < 0) tenant.ApprovedDocuments = 0;

        tenant.ObsoleteDocuments += Random.Shared.Next(5) - Random.Shared.Next(2);
        if (tenant.ObsoleteDocuments < 0) tenant.ObsoleteDocuments = 0;
    }

    private static async Task<TenantData[]?> GetTenants(Int32 size)
    {
        using var client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var url = $"https://fakerapi.it/api/v1/custom?_quantity={size}&customfield1=company_name";
        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {

            var unparsed = await response.Content.ReadAsStringAsync();
            var parsed = JsonSerializer.Deserialize<FakeData>(unparsed);
            return parsed.data.Select(x => new TenantData() { Name = x.customfield1 }).ToArray();
        }
        return null;
    }

    private class FakeData
    {
        public class FakeCompany
        {
            public String customfield1 { get; set; }
        }

        public FakeCompany[] data { get; set; }
    }

    
    public class TenantData
    {
        public String Name { get; set; }
        public Int32 DraftDocuments { get; set; }
        public Int32 PublishedDocuments { get; set; }
        public Int32 ApprovedDocuments { get; set; }
        public Int32 ObsoleteDocuments { get; set; }
    }
}