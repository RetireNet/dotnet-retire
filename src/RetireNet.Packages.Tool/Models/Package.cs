namespace RetireNet.Packages.Tool.Models
{
    public class Package
    {
        public string Id { get; set; }
        public string Affected { get; set; }
        public string Fix { get; set; }

        public override string ToString()
        {
            return $"{Id}/{Affected} => {Fix}";
        }
    }
}