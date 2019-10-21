using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace RetireNet.Packages.Tool.Services
{
    public static class HttpService
    {
        public static HttpClient HttpClient = new HttpClient();

        public static T Get<T>(Uri uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = HttpClient.SendAsync(request).GetAwaiter().GetResult();
            var str = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
