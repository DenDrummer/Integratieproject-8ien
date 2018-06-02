using System.Collections.Generic;

namespace IP3_8IEN.UI.ConsoleTesting
{
    public interface IResourceHandler
    {
        void WriteString(string key, string value);
        void WriteStrings(List<KeyValuePair<string, string>> kvpList);
        string ReadString(string key);
        void ChangeResource(string resource);
    }
}