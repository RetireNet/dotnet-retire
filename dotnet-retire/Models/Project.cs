using System.Collections.Generic;

namespace dotnet_retire
{
    public class Project
    {
        public Project()
        {
            Assets = new List<Asset>();
        }

        public List<Asset> Assets { get; set; }
    }
}