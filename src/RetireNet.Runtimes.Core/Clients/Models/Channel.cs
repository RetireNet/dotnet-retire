namespace RetireNet.Runtimes.Core.Clients.Models;

public class Channel
{
    public Channel()
    {
        Releases = new List<Release>();
    }

    public List<Release> Releases { get; set; }

}