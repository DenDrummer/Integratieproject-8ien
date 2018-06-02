using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace IP_8IEN.UI.ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            //Versie1();
            Versie2();
            Console.ReadKey();
        }

        private static void Versie1()
        {
            ResourceHandler rh = ResourceHandler.Instance;
            rh.WriteString("Personen", "Personen");
            rh.ChangeResource("PolitiekeBarometer");
            //rh.WriteString("Persoon", "Politicus");
            Console.WriteLine(rh.ReadString("Personen"));
        }

        private static void Versie2()
        {
            throw new NotImplementedException();
        }

        static void Test()
        {
            CultureAndRegionInfoBuilder builder = new CultureAndRegionInfoBuilder("nl-BE-brand1", CultureAndRegionModifiers.None);
            CultureInfo parentCI = new CultureInfo("en-US");
            RegionInfo parentRI = new RegionInfo("en-US");
            builder.LoadDataFromCultureInfo(parentCI);
            builder.LoadDataFromRegionInfo(parentRI);
            builder.Parent = parentCI;
            // set other properties of the custom culture (CultureEnglishName, CultureNativeName, possibly other ones)
            // ...
            builder.Register();
        }
    }
}
