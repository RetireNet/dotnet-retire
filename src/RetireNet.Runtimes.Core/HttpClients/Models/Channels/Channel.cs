namespace RetireNet.Runtimes.Core.HttpClients.Models.Channels;

internal class Channel
{
    public Channel()
    {
        Releases = new List<Release>();
    }
    public List<Release> Releases { get; set; }
}