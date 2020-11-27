using System;
using System.Text.Json.Serialization;

namespace RetireNet.Runtimes.Core.HttpClients.Models.Index
{
    internal class Channel
    {
        [JsonPropertyName("releases.json")]
        public Uri ReleasesUrl { get; set; }
    }
}
