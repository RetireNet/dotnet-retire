using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RetireNet.Runtimes.Core.HttpClients.Models.Index
{
    internal class ReleaseIndex
    {
        public ReleaseIndex()
        {
            Channels = new List<Channel>();
        }

        [JsonPropertyName("releases-index")]
        public IEnumerable<Channel> Channels { get; set; }
    }
}
