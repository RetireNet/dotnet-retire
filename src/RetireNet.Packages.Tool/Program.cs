namespace RetireNet.Packages.Tool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var h = new Host())
            {
                h.Build(args).Run();
            }
        }
    }
}
