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
}
