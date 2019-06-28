using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using TipoCambio.DA;

namespace TipoCambio.BL
{
  public  class TipoCambioBL
    {


        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 13/06/2019
        //Notas: clase que llama a los metodos de la clase tipoCambio.DA
        #endregion

        #region Variables


        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        TipoCambioDA da = new TipoCambioDA();
        #endregion



        #region Metodo

        /// <summary>
        /// Metodo que llama al metodo carga tasa 
        /// </summary>
        /// <returns>Retorna el id de la tasa de cambio registro en el crm </returns>
        public string generarTasa()
        {
            try
            {
              return  da.CargaDeTasa();
            }
            catch (Exception ex)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
            }
        }
        #endregion

    }
}
