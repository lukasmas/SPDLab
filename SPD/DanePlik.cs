using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SPD
{
    class DanePlik
    {
        string nazwa { get; set; }
        int maszyny { get; set; } // ilość maszyn
        int zadania { get; set; } // ilość zadań
        int[,] czasy;

        public DanePlik(string nazwa_plik, int h, int w, int [,] vs)
        {
            nazwa = nazwa_plik;
            maszyny = w;
            zadania = h;
            czasy = new int[zadania, maszyny];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    czasy[i, j] = vs[i, j];
                }
            }


        }

        



        
    }
}
