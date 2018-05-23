using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Resources;

namespace MVC_S.App_Code
{
    public class ResourceHandler //: ResourceManager
    {
        public static readonly ResourceHandler rh;
        private ResourceWriter rw;
        private ResourceReader rr;
        private int currentResource;
        private string defaultResource;
        List<string> resourceSets;

        public ResourceHandler()
        {
            defaultResource = "Resources.resx";
            ResourceSet defaultSet = new ResourceSet("Resources.resx");
            resourceSets = new List<string>()
            {
                defaultResource
            };
            rw = new ResourceWriter(defaultResource);
            rr = new ResourceReader(defaultResource);
            resourceSets.ElementAt(0);
        }

        public void WriteString(string key, string stringValue)
        {

        }
    }
}