﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
        public string bestOptJ1 { get; set; }
        public string bestOptN1 { get; set; }
        public string bestOptA { get; set; }
        public string bestOptA1 { get; set; }


        public List<Task> tasksList= new List<Task>();
        public List<Zadanie> zadaniesList = new List<Zadanie>();

        

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
            bestOptJ = string.Empty;
            bestOptJ1 = string.Empty;
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
                    bestOptJ1 +=" " +(element.id + 1).ToString();
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
                    bestOptJ1 += " " + (element.id + 1).ToString();

                    count++;
                }
                
                return tablica;

            }
        }

        public int[,] JohnsonNaSztywno()
        {
            
            return ListMadeOfTable(Johnson());
            

        }
        public void Insert(List<Zadanie> zadanies)
        {
            Random rand = new Random();
            
            int randomnumber = rand.Next(0, zadanies.Count - 1);
            int anotherrandomnumber = rand.Next(0, zadanies.Count - 1);
            while (randomnumber == anotherrandomnumber)
            {
                randomnumber = rand.Next(0, zadanies.Count - 1);
                anotherrandomnumber = rand.Next(0, zadanies.Count - 1);
            }
            Zadanie chosenTask = zadanies.ElementAt(randomnumber);

            zadanies.RemoveAt(randomnumber);
            zadanies.Insert(anotherrandomnumber, chosenTask);
            
        }
       
        
        public void Swap2(List<Zadanie> zadanies)
        {
            
        }
        public void Swap(List<Zadanie> zadanies)
        {
            Random rand = new Random();
            int randomnumber = 0;
            int anotherrandomnumber = 0;
            while (randomnumber==anotherrandomnumber)
            {
                 randomnumber = rand.Next(0, zadanies.Count - 1);
                 anotherrandomnumber = rand.Next(0, zadanies.Count - 1);
            }
            Zadanie chosenTask = zadanies.ElementAt(randomnumber);
            Zadanie anotherchosenTask = zadanies.ElementAt(anotherrandomnumber);
            zadanies.RemoveAt(anotherrandomnumber);
            zadanies.Insert(anotherrandomnumber, chosenTask);
            zadanies.RemoveAt(randomnumber);
           
          zadanies.Insert(randomnumber, anotherchosenTask);
            
        }
       

            public int [,] Annealing(double temperature)
        {
            
            bestOptA = string.Empty;
            bestOptA1 = string.Empty;
            int iterator = 0;
            Random rand = new Random();
            double probablity = 0.00;
            double tempRand =0.00;
            int[,] tableOfTimes = new int[zadania, maszyny];
            while (temperature > 0.00000000000001)
            {
               

                iterator= 0;
                foreach (Zadanie zadanie in zadaniesList)
                {
                    var tempTable = zadanie.arr;
                    for (int m = 0; m < maszyny; m++)
                    {
                        tableOfTimes[iterator, m] = tempTable[m];
                    }
                    iterator++;
                   
                }
                
                int czas = Czas(tableOfTimes);

                List<Zadanie> temporary = new List<Zadanie>(zadaniesList);

                int randomnumber = 0;
                int anotherrandomnumber = 0;
                while (randomnumber == anotherrandomnumber)
                {
                    randomnumber = rand.Next(0, temporary.Count - 1);
                    anotherrandomnumber = rand.Next(0, temporary.Count - 1);
                }
                
                Swap(temporary);//albo insert
                iterator = 0;
                foreach (Zadanie zadanie in temporary)
                {
                    var tempTable = zadanie.arr;
                    for (int m = 0; m < maszyny; m++)
                    {
                        tableOfTimes[iterator, m] = tempTable[m];
                    }
                    iterator++;

                }
                int tempCzas = Czas(tableOfTimes);

                if(tempCzas>=czas)
                {
                    probablity = Math.Exp(Convert.ToDouble((czas - tempCzas) / temperature));
                }
                else
                {
                    probablity = 1;
                }
                temperature *= 0.999;

                 tempRand = rand.Next(1000000) * 0.000001;

                if (probablity >= tempRand)
                {
                    zadaniesList = new List<Zadanie>(temporary);
                }
            }


             iterator = 0;
            foreach (Zadanie zadanie in zadaniesList)
            {
                var tempTable = zadanie.arr;
                for (int m = 0; m < maszyny; m++)
                {
                    tableOfTimes[iterator, m] = tempTable[m];
                }
                iterator++;
                bestOptA += (zadanie.id + 1).ToString();
                bestOptA1 += " " + (zadanie.id + 1).ToString();
            }

            return tableOfTimes;


            }
        public int[,] AnnealingTakingList(double temperature,List<Zadanie> zadanies)
        {
            // Neh();
            bestOptA = string.Empty;
            bestOptA1 = string.Empty;
            int iterator = 0;
            Random rand = new Random();
            double probablity = 0.00;
            double tempRand = 0.00;
            int[,] tableOfTimes = new int[zadania, maszyny];
            while (temperature > 0.00000000000001)
            {


                iterator = 0;
                foreach (Zadanie zadanie in zadanies)
                {
                    var tempTable = zadanie.arr;
                    for (int m = 0; m < maszyny; m++)
                    {
                        tableOfTimes[iterator, m] = tempTable[m];
                    }
                    iterator++;

                }

                int czas = Czas(tableOfTimes);

                List<Zadanie> temporary = new List<Zadanie>(zadanies);

                int randomnumber = 0;
                int anotherrandomnumber = 0;
                while (randomnumber == anotherrandomnumber)
                {
                    randomnumber = rand.Next(0, temporary.Count - 1);
                    anotherrandomnumber = rand.Next(0, temporary.Count - 1);
                }

                Swap(temporary);//albo insert
                iterator = 0;
                foreach (Zadanie zadanie in temporary)
                {
                    var tempTable = zadanie.arr;
                    for (int m = 0; m < maszyny; m++)
                    {
                        tableOfTimes[iterator, m] = tempTable[m];
                    }
                    iterator++;

                }
                int tempCzas = Czas(tableOfTimes);

                if (tempCzas >= czas)
                {
                    probablity = Math.Exp(Convert.ToDouble((czas - tempCzas) / temperature));
                }
                else
                {
                    probablity = 1;
                }
                temperature *= 0.999;

                tempRand = rand.Next(1000000) * 0.000001;

                if (probablity >= tempRand)
                {
                    zadanies = new List<Zadanie>(temporary);
                }
            }


            iterator = 0;
            foreach (Zadanie zadanie in zadanies)
            {
                var tempTable = zadanie.arr;
                for (int m = 0; m < maszyny; m++)
                {
                    tableOfTimes[iterator, m] = tempTable[m];
                }
                iterator++;
                bestOptA += (zadanie.id + 1).ToString();
                bestOptA1 += " " + (zadanie.id + 1).ToString();
            }

            return tableOfTimes;


        }

        public int[,] Neh()
        {
            bestOptJ = string.Empty;
            bestOptN1 = string.Empty;
            List<Zadanie> temporary = new List<Zadanie>();
            temporary = zadaniesList;
            List<Zadanie> sortedList = temporary.OrderBy(o => o.timeAll).ToList();
            List<Zadanie> zadanieOstateczne = new List<Zadanie>();
            int numer = 0;
            int[,] czasyy1 = new int[zadania, maszyny];

            for (int i = 0; i < zadania; i++)
            {

                int czas = 999999999;
                if (sortedList.Count > 0)
                {
                    for (int h = 0; h < i + 1; h++)
                    {
                        zadanieOstateczne.Insert(h, sortedList[sortedList.Count - 1]);
                        int pih = 0;
                        foreach (Zadanie zadanie in zadanieOstateczne)
                        {

                            var jp = zadanie.arr;
                            for (int m = 0; m < maszyny; m++)
                            {
                                czasyy1[pih, m] = jp[m];

                            }
                            pih++;
                        }
                        int tmpczas = Czas(czasyy1);

                        if (tmpczas < czas)
                        {
                            czas = tmpczas;
                            numer = h;
                            zadanieOstateczne.Remove(sortedList[sortedList.Count - 1]);
                        }
                        else
                        {
                            zadanieOstateczne.Remove(sortedList[sortedList.Count - 1]);
                        }
                    }

                    zadanieOstateczne.Insert(numer, sortedList[sortedList.Count - 1]);
                    sortedList.RemoveAt(sortedList.Count - 1);
                }
            }
            int ih = 0;
            foreach (Zadanie zadanie in zadanieOstateczne)
            {
                var jp = zadanie.arr;
                for (int m = 0; m < maszyny; m++)
                {
                    czasyy1[ih, m] = jp[m];
                }
                ih++;
                bestOptJ += (zadanie.id + 1).ToString();
                bestOptN1 += " " + (zadanie.id + 1).ToString();
            }
            return czasyy1;
        }
        public List<Zadanie> NehReturningList()
        {
            bestOptJ = string.Empty;
            bestOptN1 = string.Empty;
            List<Zadanie> temporary = new List<Zadanie>();
            temporary = zadaniesList;
            List<Zadanie> sortedList = temporary.OrderBy(o => o.timeAll).ToList();
            List<Zadanie> zadanieOstateczne = new List<Zadanie>();
            int numer = 0;
            int[,] czasyy1 = new int[zadania, maszyny];

            for (int i = 0; i < zadania; i++)
            {

                int czas = 999999999;
                if (sortedList.Count > 0)
                {
                    for (int h = 0; h < i + 1; h++)
                    {
                        zadanieOstateczne.Insert(h, sortedList[sortedList.Count - 1]);
                        int pih = 0;
                        foreach (Zadanie zadanie in zadanieOstateczne)
                        {

                            var jp = zadanie.arr;
                            for (int m = 0; m < maszyny; m++)
                            {
                                czasyy1[pih, m] = jp[m];

                            }
                            pih++;
                        }
                        int tmpczas = Czas(czasyy1);

                        if (tmpczas < czas)
                        {
                            czas = tmpczas;
                            numer = h;
                            zadanieOstateczne.Remove(sortedList[sortedList.Count - 1]);
                        }
                        else
                        {
                            zadanieOstateczne.Remove(sortedList[sortedList.Count - 1]);
                        }
                    }

                    zadanieOstateczne.Insert(numer, sortedList[sortedList.Count - 1]);
                    sortedList.RemoveAt(sortedList.Count - 1);
                }
            }
            int ih = 0;
            foreach (Zadanie zadanie in zadanieOstateczne)
            {
                var jp = zadanie.arr;
                for (int m = 0; m < maszyny; m++)
                {
                    czasyy1[ih, m] = jp[m];
                }
                ih++;
                bestOptJ += (zadanie.id + 1).ToString();
                bestOptN1 += " " + (zadanie.id + 1).ToString();
            }
           return zadanieOstateczne;
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
            for (int i = 0; i < h; i++)
            {
                int [] arrk = new int[maszyny];
                int totalTime=0;

                
                for (int j = 0; j < w; j++)
                {
                    
                        arrk[j] = vs[i, j];
                        totalTime += arrk[j];
                    
                    
                }
                
                zadaniesList.Add(new Zadanie(i, arrk,totalTime));
                arrk = null;
                
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
                        if (t_zwolnienia[i] >= t_zwolnienia[i - 1]) //tu zmienione, zabrane rowna sie
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

