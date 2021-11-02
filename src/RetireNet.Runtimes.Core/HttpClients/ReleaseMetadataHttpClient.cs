using System.Runtime.CompilerServices;
using System.Text.Json;
using RetireNet.Runtimes.Core.HttpClients.Models.Channels;

[assembly: InternalsVisibleTo("RetireNet.Runtimes.Core.Tests")]
namespace RetireNet.Runtimes.Core.HttpClients;

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

    public async Task<IEnumerable<Channel>> GetAllChannelsAsync()
    {
        var index = await GetIndexAsync();

        var tasks = new List<Task<Channel>>();

        foreach (var singleChannel in index.Channels)
        {
            tasks.Add(GetChannel(singleChannel.ReleasesUrl));
        }

        return await Task.WhenAll(tasks);

    }

    public async Task<Channel> GetChannel(Uri url)
    {
        return await Get<Channel>(url);
    }

    private async Task<Models.Index.ReleaseIndex> GetIndexAsync()
    {
        return await Get<Models.Index.ReleaseIndex>("/dotnet/release-metadata/releases-index.json");
    }

    private async Task<T> Get<T>(string url)
    {
        var getResult = await _httpClient.GetAsync(url);
        var getResultContent = await getResult.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(getResultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
    }

    private async Task<T> Get<T>(Uri url)
    {
        var getResult = await _httpClient.GetAsync(url);
        var getResultContent = await getResult.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(getResultContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
    }
}
