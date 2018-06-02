using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Web;

namespace IP_8IEN.UI.MVC_S.App_Code
{
    [ComVisible(true)]
    public class ResourceHandler2 : ResourceManager
    {
        public void SomeMethod()
        {
            CultureAndRegionInfoBuilder builder = new CultureAndRegionInfoBuilder("nl-NL-default", CultureAndRegionModifiers.None);
            CultureInfo parentCI = new CultureInfo("nl-NL");
            RegionInfo parentRI = new RegionInfo("nl-NL");
            builder.LoadDataFromCultureInfo(parentCI);
            builder.LoadDataFromRegionInfo(parentRI);
        }
    }
}