using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace IP3_8IEN.UI.ConsoleTesting
{
    public sealed class ResourceHandler2 : IResourceHandler
    {
        public static ResourceHandler2 Instance { get; } = new ResourceHandler2();

        private string CurrentResource;
        private readonly string DefaultResource = "Resources";
        private string ResourceFolder = @".";

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
            List<DictionaryEntry> entries = ReadExistingEntries(path);
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

        private static List<DictionaryEntry> ReadExistingEntries(string path)
        {
            List<DictionaryEntry> entries = new List<DictionaryEntry>();
            using (ResXResourceReader rr = new ResXResourceReader(path))
            {
                foreach (DictionaryEntry de in rr)
                {
                    entries.Add(de);
                }
            }

            return entries;
        }

        public string ReadString(string key)
        {
            try
            {
                using (ResXResourceSet rs = new ResXResourceSet(ConvertToPath(CurrentResource)))
                {

                }
                return null;
            }
            catch(FileNotFoundException fnfe)
            {
                return null;
            }
        }

        public void ChangeResource(string resource)
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
            List<DictionaryEntry> entries = ReadExistingEntries(path);
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
    }
}
