using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD
{
    public class Task
    {
        public Task(int idConstruct, int Time1, int Time2, int Time3)
        {
            id = idConstruct;
            machineTime1 = Time1;
            machineTime2 = Time2;
            machineTime3 = Time3;
        }
        public Task(int idConstruct, int Time1, int Time2)
        {
            id = idConstruct;
            machineTime1 = Time1;
            machineTime2 = Time2;
            machineTime3 = 0;
        }
        public int id { get; set; }
       public int machineTime1 { get; set; }
        public int machineTime2 { get; set; }
        public int machineTime3 { get; set; }
    }
}
