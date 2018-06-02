using System;
using System.Collections;

namespace IP3_8IEN.UI.ConsoleTesting
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
            ResourceHandler2 rh = ResourceHandler2.Instance;
            //rh.Initialize();

            Console.WriteLine("PolitiekeBarometer");
            foreach (DictionaryEntry de in rh.ReadExistingEntries())
            {
                Console.WriteLine($"-{(string)de.Key}: {(string)de.Value}");
            }
            rh.ChangeResource("PolitiekeBarometer");
            Console.WriteLine();
            Console.WriteLine("Resources");
            foreach (DictionaryEntry de in rh.ReadExistingEntries())
            {
                Console.WriteLine($"-{(string)de.Key}: {(string)de.Value}");
            }
        }
    }
}
