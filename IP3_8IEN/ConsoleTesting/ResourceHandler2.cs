using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;

namespace IP3_8IEN.UI.ConsoleTesting
{
    public sealed class ResourceHandler2 : IResourceHandler
    {
        public static ResourceHandler2 Instance { get; } = new ResourceHandler2();

        //folder
        private string ResourceFolder = @"..\..\Resources";
        //deelplatform
        private string CurrentResource;
        private readonly string DefaultResource = "Resources";
        //taal
        //private string CurrentCulture;

        private List<string> ResourceStrings = new List<string>();

        #region  constructors
        static ResourceHandler2() { }

        private ResourceHandler2()
        {
            CurrentResource = DefaultResource;
            ResourceStrings.Add(DefaultResource);
        }
        #endregion

        public void WriteString(string key, string value)
        {
            string path = ConvertToPath(CurrentResource);
            #region read existing entries
            List<DictionaryEntry> entries = ReadExistingEntries();
            #endregion

            using (ResXResourceWriter rw = new ResXResourceWriter(path))
            {
                #region write new entry
                rw.AddResource(key, value);
                #endregion

                #region write existing entries if they're not overwritten
                foreach (DictionaryEntry de in entries)
                {
                    if (!key.Equals((string)de.Key))
                    {
                        rw.AddResource((string)de.Key, (string)de.Value);
                    }
                }
                #endregion
            }
        }

        public List<DictionaryEntry> ReadExistingEntries()
        {
            string path = ConvertToPath(CurrentResource);
            List<DictionaryEntry> entries = new List<DictionaryEntry>();
            try
            {
                using (ResXResourceReader rr = new ResXResourceReader(path))
                {
                    foreach (DictionaryEntry de in rr)
                    {
                        entries.Add(de);
                    }
                }
            }
            catch(FileNotFoundException fnfe)
            {

            }

            return entries;
        }

        public string ReadString(string key)
        {
            try
            {
                string value;
                using (ResXResourceSet rs = new ResXResourceSet(ConvertToPath(CurrentResource)))
                {
                    value = rs.GetString(key);
                }
                if ((value == null || value.Equals("")) && !CurrentResource.Equals(DefaultResource))
                {
                    using (ResXResourceSet rs = new ResXResourceSet(ConvertToPath(DefaultResource)))
                    {
                        value = rs.GetString(key);
                    }
                }
                return value;
            }
            catch (FileNotFoundException fnfe)
            {
                using (ResXResourceSet rs = new ResXResourceSet(ConvertToPath(DefaultResource)))
                {
                    return rs.GetString(key);
                }
            }
        }

        public void ChangeResource(string resource = "Resources")
        {
            if (!ResourceStrings.Exists(r => r.Equals(resource)))
            {
                ResourceStrings.Add(resource);
            }
            CurrentResource = resource;
        }

        private string ConvertToPath(string resource)
        {
            string path = $@"{ResourceFolder}\{resource}.resx";
            return path;
        }

        public void WriteStrings(List<KeyValuePair<string, string>> kvpList)
        {
            string path = ConvertToPath(CurrentResource);
            #region read existing entries
            List<DictionaryEntry> entries = ReadExistingEntries();
            #endregion

            using (ResXResourceWriter rw = new ResXResourceWriter(path))
            {
                #region write new entry
                foreach (KeyValuePair<string, string> kvp in kvpList)
                {
                    rw.AddResource(kvp.Key, kvp.Value);
                }
                #endregion

                #region write existing entries if they're not overwritten
                foreach (DictionaryEntry de in entries)
                {
                    foreach (KeyValuePair<string, string> kvp in kvpList)
                    {
                        if (!kvp.Key.Equals((string)de.Key))
                        {
                            rw.AddResource((string)de.Key, (string)de.Value);
                        }
                    }
                }
                #endregion
            }
        }

        public string GetCurrentResource() => CurrentResource;

        public void Initialize()
        {
            string currentResource = CurrentResource;
            ChangeResource();
            List<KeyValuePair<string, string>> resources = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Personen","Personen"),
                new KeyValuePair<string, string>("Persoon","Persoon"),
                new KeyValuePair<string, string>("District","District"),
                new KeyValuePair<string, string>("Organisatie","Organisatie"),
                new KeyValuePair<string, string>("Organisaties","Organisaties")
            };
            WriteStrings(resources);

            ChangeResource("PolitiekeBarometer");
            List<KeyValuePair<string, string>> politiekeBarometer = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Personen","Politici"),
                new KeyValuePair<string, string>("Persoon","Politicus"),
                new KeyValuePair<string, string>("District","Kieskring"),
                new KeyValuePair<string, string>("Organisatie","Partij"),
                new KeyValuePair<string, string>("Organisaties","Partijen")
            };
            WriteStrings(politiekeBarometer);
            ChangeResource(currentResource);
        }
    }
}
