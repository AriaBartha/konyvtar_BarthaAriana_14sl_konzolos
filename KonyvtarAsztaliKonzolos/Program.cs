using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonyvtarAsztaliKonzolos
{
    internal class Program
    {
        
        public static Statisztika adatok = null;
        static void Main(string[] args)
        {
            adatok = new Statisztika();
            Statisztika.beolvas();
            //Statisztika.konyvListazas();
            Statisztika.otszaznalHosszabb();
            Statisztika.regiKonyv();
            Statisztika.leghosszabb();
            Statisztika.legtobbKonyvSzerzoje();
            Console.WriteLine("\nProgram vége.");
            Console.ReadLine();
        }
    }
}
