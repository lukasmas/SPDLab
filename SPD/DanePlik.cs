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
        public List<string> permutacje = new List<string>();
        public List<int> czasy_permutacje = new List<int>();
        public int cMin { get; set; }
        public string bestOpt { get; set; }
        public string bestOptJ { get; set; }


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
                    bestOptJ += (element.id+1).ToString();
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
                    bestOptJ += (element.id + 1).ToString();

                    count++;
                }
                
                return tablica;

            }
        }

        public int[,] JohnsonNaSztywno()
        {
            
            return ListMadeOfTable(Johnson());
            

        }

        public DanePlik(string nazwa_plik, int h, int w, int[,] vs)
        {
            nazwa = nazwa_plik;
            maszyny = w;
            zadania = h;
            cMin = 999999;

            czasy = new int[zadania, maszyny];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    czasy[i, j] = vs[i, j];
                }
            }
            if (maszyny == 3)
                for (int i = 0; i < h; i++)
                {
                    tasksList.Add(new Task(i, vs[i, 0], vs[i, 1], vs[i, 2]));
                }
            if (maszyny == 2)
                for (int i = 0; i < h; i++)
                {
                    tasksList.Add(new Task(i, vs[i, 0], vs[i, 1]));
                }


        }

        public void Permutacja()
        {

            
            string temp = "";
            for (int i = 0; i < zadania; i++)
            {
                temp += (i+1).ToString();
            }
            string sekwencja = temp;

            
            int n = sekwencja.Length;
            char[] chars = new char[n];
            string permutation;

            
            int[] pozycja = new int[n];
            bool[] used = new bool[n];
            bool last;

            
            for (int i = 0; i < n; i++)
                pozycja[i] = i;

            do
            {
                
                for (int i = 0; i < n; i++)
                {
                    chars[i] = sekwencja[pozycja[i]];
                }
                permutation = new string(chars);
                
                permutacje.Add(permutation);

                // recalculate positions
                last = false;
                int k = n - 2;
                while (k >= 0)
                {
                    if (pozycja[k] < pozycja[k + 1])
                    {
                        for (int i = 0; i < n; i++)
                            used[i] = false;
                        for (int i = 0; i < k; i++)
                            used[pozycja[i]] = true;
                        do pozycja[k]++; while (used[pozycja[k]]);
                        used[pozycja[k]] = true;
                        for (int i = 0; i < n; i++)
                            if (!used[i]) pozycja[++k] = i;
                        break;
                    }
                    else k--;
                }
                last = (k < 0);
            } while (!last);
           
        }



        public int Czas(int [,] arr)
        {
            int[,] temp = arr;
            int t_czas;

            int[] t_zwolnienia = new int[maszyny];



            for (int z = 0; z < zadania; z++)
            {
                t_czas = 0;


                for (int i = 0; i < maszyny; i++)
                {


                    t_czas = (temp[z, i]);

                   
                    if (i == 0)
                    {
                        t_zwolnienia[i] += t_czas;
                    }
                    else
                    {
                        if (t_zwolnienia[i] >= t_zwolnienia[i - 1])
                            t_zwolnienia[i] += (t_czas);
                        else
                            t_zwolnienia[i] = t_zwolnienia[i - 1] + (t_czas);
                    }



                }
            }
            int cmax;
            cmax = (t_zwolnienia[maszyny - 1]);
            return cmax;
            
        }

        public void PermutacjaCzas()
        {
            
            foreach (var item in permutacje)
            {
                int[,] temp = new int[zadania, maszyny];

                string klucz = item;
                for (int i = 0; i < zadania; i++)
                {   

                    int x = (int)klucz[i] - 49; // 1 w kod ascii = 49
                    for (int j = 0; j < maszyny; j++)
                    {
                        temp[i, j] = czasy[x, j];
                    }
                }

                int t_czas = Czas(temp);
                czasy_permutacje.Add(t_czas);
                if (t_czas < cMin)
                {
                    cMin = t_czas;
                    bestOpt = item;
                    prez_wy = temp;
                }
               
            }
            
        }
        
    }
    
}

