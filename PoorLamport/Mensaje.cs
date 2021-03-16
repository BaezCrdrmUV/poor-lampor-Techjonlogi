using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoorLamport
{
    public class Mensaje
    {
        public Process Remitente { get; set; }
        public Process Emisor { get; set; }

        public int Tiempo { get; set; }

        public String Señal { get; set; }

        public Mensaje()
        {
            Tiempo = 0;
        }
    }
}
