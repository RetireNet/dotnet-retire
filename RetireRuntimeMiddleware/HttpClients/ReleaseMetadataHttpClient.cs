using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RetireRuntimeMiddleware.HttpClients.Models;

[assembly: InternalsVisibleTo("Tests")]
namespace RetireRuntimeMiddleware.HttpClients
{
    internal class ReleaseMetadataHttpClient
    {
        private readonly HttpClient _httpClient;

        public ReleaseMetadataHttpClient()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10),
                BaseAddress = new Uri("https://dotnetcli.blob.core.windows.net")
            };
        }

        public async Task<ReleaseIndex> GetIndexAsync()
        {
            return await Get<ReleaseIndex>("/dotnet/release-metadata/releases-index.json");
        }

        public async Task<ReleasesContainer> GetReleases(Uri url)
        {
            return await Get<ReleasesContainer>(url);
        }

        private async Task<T> Get<T>(string url)
        {
            var getResult = await _httpClient.GetAsync(url);
            var getResultContent = await getResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(getResultContent);
        }

        private async Task<T> Get<T>(Uri url)
        {
            var getResult = await _httpClient.GetAsync(url);
            var getResultContent = await getResult.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(getResultContent);
        }
    }
}
