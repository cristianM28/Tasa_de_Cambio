using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipoCambio.BE
{
   public  class DolarBe
    {



        private int _valorDolar;

        public int ValorDolar
        {
            get { return _valorDolar; }
            set { _valorDolar = value; }
        }
        private DateTime _fechaDolar;

        public DateTime FechaDolar
        {
            get { return _fechaDolar; }
            set { _fechaDolar = value; }
        }

        private int monedaDolar;

        public int MonedaDolar
        {
            get { return monedaDolar; }
            set { monedaDolar = value; }
        }


    }
}
