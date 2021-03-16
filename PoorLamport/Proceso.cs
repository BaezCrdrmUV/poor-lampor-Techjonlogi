using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoorLamport
{
    public class Process
    {
        public int Identificador { get; set; }
        public int Clock { get; set; }

        public Process(int id)
        {
            this.Identificador = id;
            Clock = 0;
        }
    }
}
