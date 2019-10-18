using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNetCore.ReleaseMetadata.HttpClient.Models
{
    public class ReleaseIndex
    {
        public ReleaseIndex()
        {
            Channels = new List<Channel>();
        }

        [JsonProperty("releases-index")]
        public IEnumerable<Channel> Channels { get; set; }
    }
}