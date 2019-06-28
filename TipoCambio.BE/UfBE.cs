using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipoCambio.BE
{
  public  class UfBE
    {


        private int _valorUf;

        public int ValorUf
        {
            get { return _valorUf; }
            set { _valorUf = value; }
        }

        private DateTime _fechaUf;  

        public DateTime FechaUf
        {
            get { return _fechaUf; }
            set { _fechaUf = value; }
        }

        private int _monedaUf;

        public int MonedaUf
        {
            get { return _monedaUf; }
            set { _monedaUf = value; }
        }


    }
}
