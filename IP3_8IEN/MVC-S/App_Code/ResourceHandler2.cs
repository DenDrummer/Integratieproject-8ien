using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;

namespace IP_8IEN.UI.MVC_S.App_Code
{
    [ComVisible(true)]
    public class ResourceHandler2
    {
        //
        // Summary:
        //     Holds the number used to identify resource files.
        public static readonly int MagicNumber;
        //
        // Summary:
        //     Specifies the version of resource file headers that the current implementation
        //     of System.Resources.ResourceManager can interpret and produce.
        public static readonly int HeaderVersionNumber;
        //
        // Summary:
        //     Specifies the root name of the resource files that the System.Resources.ResourceManager
        //     searches for resources.
        protected string BaseNameField;
        //
        // Summary:
        //     Contains a System.Collections.Hashtable that returns a mapping from cultures
        //     to System.Resources.ResourceSet objects.
        [Obsolete("call InternalGetResourceSet instead")]
        protected Hashtable ResourceSets;
        //
        // Summary:
        //     Specifies the main assembly that contains the resources.
        protected Assembly MainAssembly;


    }
}