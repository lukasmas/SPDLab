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

        public List<Task> tasksList= new List<Task>();
        public List<Task> Johnson()
        {
            List<Task> wirtualneZadania = new List<Task>();
            List<Task> lista1 = new List<Task>();
            List<Task> lista2 = new List<Task>();
            List<Task> listaostateczna = new List<Task>();
            int count = 0;
            if (maszyny == 3)
            {
                foreach (Task element in tasksList)
                {

                    wirtualneZadania.Add(new Task(count, (element.machineTime1 + element.machineTime2), (element.machineTime2 + element.machineTime3)));
                    count++;
                }
            }
            else if (maszyny==2)
            {
                count = 0;
                foreach (Task element in tasksList)
                {
                    wirtualneZadania.Add(new Task(count, element.machineTime1, element.machineTime2));
                        count++;
                }
           }
            while (wirtualneZadania.Count>0)
                {

                count = 0;
            int min1=999999; //daj na koniec listy 1
            int min2 = 999999; //2 czas najmniejszy na poczatek lisy 2
                Task taskMin1=wirtualneZadania[0];
                Task taskMin2=wirtualneZadania[0];
                foreach (Task element in wirtualneZadania)
                {
                    
                    if (min1>wirtualneZadania[count].machineTime1)
                    {
                        min1 = wirtualneZadania[count].machineTime1;
                        taskMin1 = element;
                    }
                    count++;
                }
                count = 0;
                foreach (Task elementy in wirtualneZadania)
                {
                    
                    if (min2 > wirtualneZadania[count].machineTime2)
                    {
                        min2 = wirtualneZadania[count].machineTime2;
                        taskMin2 = elementy;
                    }
                    count++;
                }
            
                if (min2<=min1)
                {
                    lista2.Insert(0, taskMin2);
                    wirtualneZadania.Remove(taskMin2);
                }
                else
                {
                    lista1.Add(taskMin1);
                    wirtualneZadania.Remove(taskMin1);
                }
            }
            lista1.AddRange(lista2);
         

           
            foreach (Task element in lista1)
            {
                Task result = tasksList.Find(x => x.id == element.id);

                listaostateczna.Add(result);
                
                    
                    }
         
            

            return listaostateczna;
        }

        public int[,] ListMadeOfTable(List<Task> tasklist)
        {
            int dlugosc = tasklist.Count();
            int count = 0;
            if (maszyny == 3)
            {
                int[,] tablica = new int[dlugosc, 3];
                foreach (Task element in tasklist)
                {
                    tablica[count, 0] = element.machineTime1;
                    tablica[count, 1] = element.machineTime2;
                    tablica[count, 2] = element.machineTime3;

                    count++;
                }
                return tablica;
            }
            else 
            {
                int[,] tablica = new int[dlugosc, 2];
                foreach (Task element in tasklist)
                {
                    tablica[count, 0] = element.machineTime1;
                    tablica[count, 1] = element.machineTime2;
                    

                    count++;
                }

                return tablica;

            }
        }

        public int[,] JohnsonNaSztywno()
        {
            
            return ListMadeOfTable(Johnson());
            

        }
        
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
            if (maszyny==3)
            for (int i = 0; i < h; i++)
            {
                tasksList.Add(new Task(i,vs[i,0],vs[i,1],vs[i,2]));
            }
            if (maszyny==2)
                for (int i = 0; i < h; i++)
                {
                tasksList.Add(new Task(i, vs[i, 0], vs[i, 1]));
            }


        }
       


    }
}
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
