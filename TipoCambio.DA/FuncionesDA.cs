using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipoCambio.DA
{
  public  class FuncionesDA
    {

        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 13/06/2019
        //Notas: metodos para crear los registros para la entidad
        #endregion


        #region Variables

        IOrganizationService servicio = ConexionCrmDA.ObtenerConexion();
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        
        #endregion


        #region metodos


        /// <summary>
        /// metodo que obtiene los datos desde el crm en en el caso que exista ,sino devuelve un null
        /// </summary>
        /// <returns>Retorna los datos que exitan en el crm ,sino null</returns>
        public DataTable ObtenerTasa()
        {
            try
            {

                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("zth_tipodecambio", ref servicio);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_tipodecambioid", ZthFetchXml365.zthFetch.TipoRetorno.Key);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_fecha", ZthFetchXml365.zthFetch.TipoRetorno.CrmDateTime);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_valor", ZthFetchXml365.zthFetch.TipoRetorno.CrmDecimal);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_moneda", ZthFetchXml365.zthFetch.TipoRetorno.PicklistName);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_name", ZthFetchXml365.zthFetch.TipoRetorno.String);

                //fetch.AgregarFiltroPlano("zth_tipodecambio", ZthFetchXml365.zthFetch.TipoFiltro.and, "zth_fecha",
                //ZthFetchXml365.zthFetch.TipoComparacionFiltro.FechaIgualQue, "" +fecha.Month +"/"+fecha.Day+"/"+fecha.Year+"" );

                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);

               // string empresa;
                if (Dato.Rows.Count > 0)
                {
                    return  Dato ;
                }
                else
                {
                    return Dato  ;
                } 
            }

            catch (Exception ex)
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener los datos de licecia csp: " + ex.Message.ToString());
                return null;
            }
        }


        /// <summary>
        /// Metodo que obtiene el guid del tipo de cambio 
        /// </summary>
        /// <param name="moneda">Parametro con el valor de la moneda</param>
        /// <returns>returna el guid del tipo cambio filtrado </returns>
        public string IdTasa( int moneda)
        {
            try
            {

                ZthFetchXml365.zthFetch fetch = new ZthFetchXml365.zthFetch("zth_tipodecambio", ref servicio);

                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_tipodecambioid", ZthFetchXml365.zthFetch.TipoRetorno.Key);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_fecha", ZthFetchXml365.zthFetch.TipoRetorno.CrmDateTime);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_valor", ZthFetchXml365.zthFetch.TipoRetorno.CrmDecimal);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_moneda", ZthFetchXml365.zthFetch.TipoRetorno.PicklistValue);
                fetch.AgregarCampoRetorno("zth_tipodecambio", "zth_name", ZthFetchXml365.zthFetch.TipoRetorno.String);

             

                fetch.AgregarFiltroPlano("zth_tipodecambio", ZthFetchXml365.zthFetch.TipoFiltro.and, "zth_moneda",
                ZthFetchXml365.zthFetch.TipoComparacionFiltro.Igual,moneda.ToString()  );

                DataTable Dato = new DataTable();
                Dato = fetch.GeneraTblconFetchResult(false);

                string id;
                if (Dato.Rows.Count > 0)
                {
                    return id= Dato.Rows[0]["zth_tipodecambioid"].ToString();
                }
                else
                {
                    return id=null;
                }
            }

            catch (Exception ex)
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al obtener los datos de licecia csp: " + ex.Message.ToString());
                return null;
            }
        }




        #endregion



    }
}
