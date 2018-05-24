using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;

namespace IP_8IEN.UI.ConsoleTesting
{
    public sealed class ResourceHandler //: ResourceManager
    {

        public static ResourceHandler Instance { get; } = new ResourceHandler();

        private int currentResource;
        private int defaultResource;
        private List<string> resourceSets;
        private string resourceFolder;

        #region constructors
        static ResourceHandler()
        {
            //hier moet denk ik niets
        }
        private ResourceHandler()
        {
            string defaultResourceString = "Resources";
            resourceFolder = "";

            #region load existing resource files
            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo d = new DirectoryInfo($@"{currentDir}{resourceFolder}");
            FileInfo[] files = d.GetFiles("*.resx");
            resourceSets = new List<string>();
            foreach (FileInfo f in files)
            {
                resourceSets.Add(f.Name);
            }
            #endregion
            if (resourceSets.FindIndex(r => r.Equals(defaultResourceString)) < 0)
            {
                resourceSets.Add(defaultResourceString);
            }
            defaultResource = resourceSets.FindIndex(r => r.Equals(defaultResourceString));
            currentResource = defaultResource;
        }
        #endregion

        public void WriteString(string key, string stringValue)
        {
            ResourceWriter rw = new ResourceWriter(ConvertToPath(GetResource(currentResource)));
            rw.AddResource(key, stringValue);
            rw.Close();
        }

        public string ReadString(string key)
        {
            try
            {
                ResourceSet rs = new ResourceSet(ConvertToPath(GetResource(currentResource)));
                string value = rs.GetString(key);
                rs.Close();
                if ((value == null || value.Equals("")) && currentResource != defaultResource)
                {
                    rs = new ResourceSet(ConvertToPath(GetResource(defaultResource)));
                    value = rs.GetString(key);
                    rs.Close();
                }
                return value;
            }
            catch (FileNotFoundException fnfe)
            {
                ResourceSet rs = new ResourceSet(ConvertToPath(GetResource(defaultResource)));
                string value = rs.GetString(key);
                rs.Close();
                return value;
            }
        }

        private string GetResource(int index) => resourceSets.ElementAt(index);

        public void ChangeResource(string resource)
        {
            if (!resourceSets.Exists(r => r.Equals(resource)))
            {
                resourceSets.Add(resource);
            }
            currentResource = resourceSets.FindIndex(r => r.Equals(resource));
        }

        private string ConvertToPath(string resource)
        {
            string path = $"{resourceFolder}{resource}.resx";
            return path;
        }
    }
}