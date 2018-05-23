using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace MVC_S.App_Code
{
    public sealed class ResourceHandler //: ResourceManager
    {
        private static readonly ResourceHandler instance;

        public static  ResourceHandler Instance
        {
            get
            {
                return Instance;
            }
        }
        private int currentResource;
        private int defaultResource;
        private List<string> resourceSets;
        
        static ResourceHandler()
        {
        }

        private ResourceHandler()
        {
            string defaultResourceString = "Resources.resx";
            
            resourceSets = new List<string>()
            {
                defaultResourceString
            };
            defaultResource = resourceSets.FindIndex(r => r.Equals(defaultResourceString));
            currentResource = defaultResource;
        }

        public void WriteString(string key, string stringValue)
        {
            ResourceWriter rw = new ResourceWriter(ConvertToPath(GetCurrentResource()));
            rw.AddResource(key, stringValue);
            rw.Generate();
        }

        public string ReadString(string key)
        {
            return "Sorry maar dit werkt nog niet";
        }

        private string GetCurrentResource() => resourceSets.ElementAt(currentResource);

        public void ChangeResource(string resource)
        {
            if (!resourceSets.Exists(r => r.Equals(resource)))
            {
                resourceSets.Add(resource);
            }
            currentResource = resourceSets.FindIndex(r => r.Equals(resource));
        }

        private string ConvertToPath(string resource) => $"~/App_GlobalResources/{resource}.resx";
    }
}