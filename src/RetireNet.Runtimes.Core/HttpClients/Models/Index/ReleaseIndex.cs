using System.Collections.Generic;
using Newtonsoft.Json;

namespace RetireNet.Runtimes.Core.HttpClients.Models.Index
{
    internal class ReleaseIndex
    {
        public ReleaseIndex()
        {
            Channels = new List<Channel>();
        }

        [JsonProperty("releases-index")]
        public IEnumerable<Channel> Channels { get; set; }
    }
}
