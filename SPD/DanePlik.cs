using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SPD
{
    class DanePlik : MainWindow
    {
        public string nazwa { get; set; }
        public int maszyny { get; set; } // ilość maszyn
        public int zadania { get; set; } // ilość zadań
        public int[,] czasy { get; set; }
        public int[,] prez_wy { get; set; }
        List<string> permutacje = new List<string>();
        public int cMin { get; set; }
        public string bestOpt { get; set; }
        


        

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


        public void Permutacja()
        {
            // our sequence of characters
            char[] temp = new char[zadania];
            for (int i = 0; i < zadania; i++)
            {
                temp[i] = (char) i;
            }
            string sequence = new string(temp);

            // variables aiding us in char[] <-> string conversion
            int n = sequence.Length;
            char[] chars = new char[n];
            string permutation;

            // variables necessary for our algorithm
            int[] positions = new int[n];
            bool[] used = new bool[n];
            bool last;

            // initialize positions
            for (int i = 0; i < n; i++)
                positions[i] = i;

            do
            {
                // make permutation according to positions
                for (int i = 0; i < n; i++)
                    chars[i] = sequence[positions[i]];
                permutation = new string(chars);

                // output it
                Console.WriteLine(permutation);

                // recalculate positions
                last = false;
                int k = n - 2;
                while (k >= 0)
                {
                    if (positions[k] < positions[k + 1])
                    {
                        for (int i = 0; i < n; i++)
                            used[i] = false;
                        for (int i = 0; i < k; i++)
                            used[positions[i]] = true;
                        do positions[k]++; while (used[positions[k]]);
                        used[positions[k]] = true;
                        for (int i = 0; i < n; i++)
                            if (!used[i]) positions[++k] = i;
                        break;
                    }
                    else k--;
                }
                last = (k < 0);
            } while (!last);
        }

        public void Czas()
        {

        }

      
        
    }
}
