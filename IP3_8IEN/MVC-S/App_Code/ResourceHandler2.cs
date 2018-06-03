using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace IP3_8IEN.UI.MVC_S.App_Code
{
    public sealed class ResourceHandler2
    {
        public static ResourceHandler2 Instance { get; } = new ResourceHandler2();

        //pad naar folder
        private readonly string ResourceFolder = @"~\App_GlobalResources";
        //huidig en standaard deelplatform
        private string CurrentResource;
        private readonly string DefaultResource = "Resources";


        private List<string> ResourceStrings = new List<string>();

        #region constructors
        static ResourceHandler2() { }

        private ResourceHandler2()
        {
            Initialize();
        }
        #endregion

        #region initialize
        private void Initialize()
        {
            #region load existing resource files
            DirectoryInfo di = new DirectoryInfo($@"{Directory.GetCurrentDirectory()}");
            FileInfo[] files = di.GetFiles("*.resx");
            foreach (FileInfo fi in files)
            {
                //gebruik de methode ipv rechtstreeks om dubbele resources te vermijden
                ChangeResource(fi.Name);
            }
            #endregion

            //switch to default resource
            ChangeResource();

            InitializeDeelplatformen();
            ChangeResource();
        }

        private void InitializeDeelplatformen()
        {
            #region default
            List<KeyValuePair<string, string>> resources = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Personen","Personen"),
                new KeyValuePair<string, string>("Persoon","Persoon"),
                new KeyValuePair<string, string>("District","District"),
                new KeyValuePair<string, string>("Organisatie","Organisatie"),
                new KeyValuePair<string, string>("Organisaties","Organisaties")
            };
            WriteStrings(resources);
            #endregion

            #region Politieke Barometer
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
            #endregion
        }
        #endregion

        #region writing
        public void WriteString(string key, string value)
        {
            string path = ConvertToPath(CurrentResource);

            using (ResXResourceWriter rw = new ResXResourceWriter(path))
            {
                rw.AddResource(key, value);

                #region write existing entries if they're not overwritten
                foreach (DictionaryEntry de in ReadExistingEntries())
                {
                    if (!key.Equals((string)de.Key))
                    {
                        rw.AddResource((string)de.Key, (string)de.Value);
                    }
                }
                #endregion
            }
        }

        public void WriteStrings(List<KeyValuePair<string, string>> kvpList)
        {
            string path = ConverToPath(CurrentResource);
            using (ResXResourceWriter rw = new ResXResourceWriter)
            {
                #region write new entries
                foreach (KeyValuePair<string, string> kvp in kvpList)
                {
                    rw.AddResource(kvp.Key, kvp.Value);
                }
                #endregion

                #region write existing entries if they're not overwritten
                foreach (DictionaryEntry de in ReadExistingEntries())
                {
                    bool exists = false;
                    foreach (KeyValuePair<string, string> kvp in kvpList)
                    {
                        if (!kvp.Key.Equals((string)de.Key) && !exists)
                        {
                            exists = true;
                        }
                    }
                    if (!exists)
                    {
                        rw.AddResource((string)de.Key, (string)de.Value);
                    }
                }
                #endregion
            }
        }
        #endregion

        #region reading
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
                using (System.Resources.ResourceSet rs = new ResXResourceSet(ConvertToPath(DefaultResource)))
                {
                    return rs.GetString(key);
                }
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
            catch (FileNotFoundException fnfe)
            {

            }

            return entries;
        }
        #endregion

        #region helpers
        public string GetCurrentResource()
        {
            return CurrentResource;
        }

        private string ConvertToPath(string resource)
        {
            string path = $@"{ResourceFolder}\{resource}.resx";
            return path;
        }

        public void ChangeResource(string resource = "Resources")
        {
            if (!ResourceStrings.Exists(r => r.Equals(resource)))
            {
                ResourceStrings.Add(resource);
            }
            CurrentResource = resource;
        }
        #endregion
    }
}