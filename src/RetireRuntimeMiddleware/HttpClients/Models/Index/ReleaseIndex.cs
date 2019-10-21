using System.Collections.Generic;
using Newtonsoft.Json;

namespace RetireRuntimeMiddleware.HttpClients.Models.Index
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
