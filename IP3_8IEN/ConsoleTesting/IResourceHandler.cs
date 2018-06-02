namespace IP_8IEN.UI.ConsoleTesting
{
    public interface IResourceHandler
    {
        void WriteString(string key, string stringValue);
        string ReadString(string key);
        void ChangeResource(string resource);
    }
}