using System;
using Newtonsoft.Json;

namespace DotNet.Retire.Runtimes.Core.HttpClients.Models.Index
{
    internal class Channel
    {
        [JsonProperty("releases.json")]
        public Uri ReleasesUrl { get; set; }
    }
}
