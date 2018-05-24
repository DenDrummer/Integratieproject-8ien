using System;

namespace IP_8IEN.UI.ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            ResourceHandler rh = ResourceHandler.Instance;
            rh.WriteString("Personen", "Personen");
            rh.ChangeResource("PolitiekeBarometer");
            //rh.WriteString("Persoon", "Politicus");
            Console.WriteLine(rh.ReadString("Personen"));
            Console.ReadKey();
        }
    }
}
