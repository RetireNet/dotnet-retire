using System;
using Newtonsoft.Json;

namespace DotNetCore.ReleaseMetadata.HttpClient.Models
{
    public class Channel
    {
        [JsonProperty("releases.json")]
        public Uri ReleasesUrl { get; set; }
    }
}