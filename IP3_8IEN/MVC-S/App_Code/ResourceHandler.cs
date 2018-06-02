using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Web.Hosting;

namespace IP3_8IEN.UI.MVC_S.App_Code
{
    public sealed class ResourceHandler //: ResourceManager
    {

        public static ResourceHandler Instance { get; } = new ResourceHandler();

        private static int currentResource;
        private static int defaultResource;
        private static List<string> resourceSets;
        private static string resourceFolder;

        #region constructors
        static ResourceHandler()
        {
            //hier moet denk ik niets
        }
        private ResourceHandler()
        {
            string defaultResourceString = "Resources";

            #region load existing resource files
            string currentDir = HostingEnvironment.ApplicationPhysicalPath;
            resourceFolder = $"{currentDir}App_GlobalResources\\";
            DirectoryInfo d = new DirectoryInfo($@"{resourceFolder}");

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

        public static void WriteString(string key, string stringValue)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            if (File.Exists(ConvertToPath(GetResource(currentResource))))
            {
                ResourceSet rs = new ResourceSet(ConvertToPath(GetResource(currentResource)));
                foreach (DictionaryEntry de in rs)
                {
                    d.Add((string)de.Key, (string)de.Value);
                }
                rs.Close();
            }
            d.Add(key, stringValue);

            ResourceWriter rw = new ResourceWriter(ConvertToPath(GetResource(currentResource)));
            foreach(KeyValuePair<string, string> kvp in d)
            {
                rw.AddResource(kvp.Key, kvp.Value);
            }
            rw.Generate();
            rw.Close();
        }

        public static string ReadString(string key)
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
                if (!File.Exists(ConvertToPath(GetResource(defaultResource))))
                {
                    Initialize();
                }
                ResourceSet rs = new ResourceSet(ConvertToPath(GetResource(defaultResource)));
                string value = rs.GetString(key);
                rs.Close();
                return value;
            }
        }

        private static string GetResource(int index) => resourceSets.ElementAt(index);

        public static void ChangeResource(string resource)
        {
            if (!resourceSets.Exists(r => r.Equals(resource)))
            {
                resourceSets.Add(resource);
            }
            currentResource = resourceSets.FindIndex(r => r.Equals(resource));
        }

        private static string ConvertToPath(string resource) => $"{resourceFolder}{resource}.resx";

        public static void Initialize()
        {
            int current = currentResource;

            #region default
            ChangeResource(resourceSets.ElementAt(defaultResource));

            WriteString("District", "District");
            WriteString("Organisatie", "Organisatie");
            WriteString("Organisaties", "Organisaties");
            WriteString("Persoon", "Persoon");
            WriteString("Personen", "Personen");
            #endregion

            #region Politieke Barometer
            ChangeResource("PolitiekeBarometer");

            WriteString("District", "Kieskring");
            WriteString("Organisatie", "Partij");
            WriteString("Organisaties", "Partijen");
            WriteString("Persoon", "Politicus");
            WriteString("Personen", "Politici");
            #endregion

            //change back to current resources
            ChangeResource(resourceSets.ElementAt(current));
        }
    }
}