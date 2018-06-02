using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_8IEN.UI.ConsoleTesting
{
    public sealed class ResourceHandler2 : IResourceHandler
    {
        public static ResourceHandler2 Instance { get; } = new ResourceHandler2();

        private readonly string DefaultResource = "Resources";

        #region  constructors
        static ResourceHandler2() { }

        private ResourceHandler2()
        {

        }
        #endregion

        public void WriteString(string key, string stringValue)
        {
            throw new NotImplementedException();
        }

        public string ReadString(string key)
        {
            throw new NotImplementedException();
        }

        public void ChangeResource(string resource)
        {
            throw new NotImplementedException();
        }
    }
}
