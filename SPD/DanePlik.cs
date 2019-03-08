using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SPD
{
    class DanePlik
    {
        string nazwa;
        int x { get; set; }
        int y { get; set; }
        int[] array;

        public DanePlik(string nazwa_plik, int h, int w)
        {
            nazwa = nazwa_plik;
            x = w;
            y = h;
            array = new int[x * y];
        }

        



        
    }
}
