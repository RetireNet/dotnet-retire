using System;

namespace RetireNet.Packages.Tool.Services
{
    public class RetireServiceOptions
    {
        private string path;

        public Uri RootUrl { get; set; }

        public string Path
        {
            get => path;
            set => path = value?.Replace('/', System.IO.Path.DirectorySeparatorChar).Replace('\\', System.IO.Path.DirectorySeparatorChar);
        }

        public bool AlwaysExitWithZero { get; set; }
    }
}
