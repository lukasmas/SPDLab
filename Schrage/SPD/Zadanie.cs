using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD
{
    class Zadanie
    {
        public Zadanie(int id, int[] arr, int timeAll=0)
        {
            this.id = id;
            this.arr = arr;
            this.timeAll = timeAll;

        }
        public int id { get; set; }
        public int [] arr { get; set; }
        public int timeAll { get; set; }
    }

    class Obiekt
    {
        public Obiekt(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
           
        }
            public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public int U { get; set; }
        public int UB { get; set; }
        public int LB { get; set; }
        public int[,] orderFromSchrage;
    }
    }


