using Microsoft.Xrm.Sdk;
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Web.Script.Serialization;
using TipoCambio.BE;

namespace TipoCambio.DA
{
    public  class TipoCambioDA
    {

        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 13/06/2019
        //Notas: clase con los metodos para crea y actualiza los tipo de cambio 
        #endregion

        #region Variables
        IOrganizationService servicio = ConexionCrmDA.ObtenerConexion();
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
       int dolar=int.Parse( ConfigurationManager.AppSettings["Dolar"]);
        int uf=int.Parse(ConfigurationManager.AppSettings["UF"]);

        DolarBe dolarbe = new DolarBe();
        UfBE ufBe = new UfBE();
        FuncionesDA fun = new FuncionesDA();
        string creado = "";
        #endregion


        #region Metodo
        /// <summary>
        /// Metodo que consume una api para obtener el valor del dolar y uf
        /// </summary>
        public string  CargaDeTasa()
        {
            try
            {
                DataTable validar = new DataTable();
               

                string url = "https://mindicador.cl/api";
                string jason = new WebClient().DownloadString(url);
                JavaScriptSerializer js = new JavaScriptSerializer();
                var d = js.Deserialize<dynamic>(jason);

                //Obtiene los valores para dolares
                var dolares = d["dolar"];
                var valores = dolares["valor"];
                dolarbe.FechaDolar = Convert.ToDateTime(dolares["fecha"]);
                int Dolar = (int)valores;
                dolarbe.ValorDolar = Dolar + 5;
                dolarbe.MonedaDolar = dolar;

                //Obtiene los  valores para uf
                var ufs = d["uf"];
                ufBe.ValorUf = Convert.ToInt32(ufs["valor"]);
                ufBe.FechaUf = Convert.ToDateTime(ufs["fecha"]);
                ufBe.MonedaUf = uf;

                DateTime fechaActual =DateTime.Today;

                validar = fun.ObtenerTasa();

                if (validar.Rows.Count> 0)
                {
                  ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se actualizaron los datos ");

                    string idDo = fun.IdTasa(dolar);
                    if (idDo == null)
                    {
                        CreaTasaCambioDolar();
                    }
                    string idUf = fun.IdTasa(uf);
                    if (idUf == null)
                    {
                        CreaTasaCambioUf();
                    }

                    creado = Actualizar( idDo, idUf);
                }
                else
                {
                  ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se crean los registros");
                  CreaTasaCambioDolar();
                  CreaTasaCambioUf();
                  
                }
                return creado;
            }
            catch (Exception ex)
            {

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
            } 
        }

        /// <summary>
        /// Metodo que crea los registro de la tasa de cambio Dolar
        /// </summary>
        public void CreaTasaCambioDolar()
        {
            try
            {
                EntidadesCRM.zth_tipodecambio TMoneda = new EntidadesCRM.zth_tipodecambio();
                TMoneda.zth_name = "Dolar";
                TMoneda.zth_Moneda = new OptionSetValue(dolarbe.MonedaDolar);
                TMoneda.zth_Fecha = Convert.ToDateTime(dolarbe.FechaDolar);
                TMoneda.zth_Valor = dolarbe.ValorDolar;

                creado = servicio.Create(TMoneda).ToString();
 
            }
            catch (Exception ex)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
            }
        }

        /// <summary>
        /// Metodo que crea los registro de la tasa de cambio UF
        /// </summary>
        public void CreaTasaCambioUf()
        {
            try
            {
                EntidadesCRM.zth_tipodecambio TMoneda = new EntidadesCRM.zth_tipodecambio();

                TMoneda.zth_name = "Uf";
                TMoneda.zth_Moneda = new OptionSetValue(ufBe.MonedaUf);
                TMoneda.zth_Fecha = Convert.ToDateTime(ufBe.FechaUf);
                TMoneda.zth_Valor = ufBe.ValorUf;

                creado = servicio.Create(TMoneda).ToString();
            }
            catch (Exception ex)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
            }
        }



        /// <summary>
        /// Metodo que actualiza los campos de la entidad TIPO CAMBIO 
        /// </summary>
        /// <param name="idDo">Parametro guid del dolar </param>
        /// <param name="idUf">Parametro guid de la uf </param>
        public string Actualizar(string idDo,string idUf)
        {
            try
            {
                 
                string creado="";
                // se eliman el registro por el guid del dolar
                EntidadesCRM.zth_tipodecambio TMoneda = new EntidadesCRM.zth_tipodecambio();

                string idDol = fun.IdTasa(dolar);
                if (idDol!=null)
                {
                    servicio.Delete(EntidadesCRM.zth_tipodecambio.EntityLogicalName, Guid.Parse(idDol));

                    
                    // se crea el registro para el dolar
                    TMoneda.zth_name = "Dolar";
                    TMoneda.zth_Moneda = new OptionSetValue(dolarbe.MonedaDolar);
                    TMoneda.zth_Fecha = Convert.ToDateTime(dolarbe.FechaDolar);
                    TMoneda.zth_Valor = dolarbe.ValorDolar;
                    creado = servicio.Create(TMoneda).ToString();

                }

                string idUfl = fun.IdTasa(uf);
                if (idUfl!=null)
                {

                    //se elimina el registro por el guid del uf
                    servicio.Delete(EntidadesCRM.zth_tipodecambio.EntityLogicalName, Guid.Parse(idUfl));
                    // se crea el registro para uf
                    TMoneda.zth_name = "Uf";
                    TMoneda.zth_Moneda = new OptionSetValue(ufBe.MonedaUf);
                    TMoneda.zth_Fecha = Convert.ToDateTime(ufBe.FechaUf);
                    TMoneda.zth_Valor = ufBe.ValorUf;
                    creado = servicio.Create(TMoneda).ToString();
                }



                return creado;
            }
            catch (Exception ex)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
            }

        }

        #region prueba consumo api indicadores economicos 

        /// <summary>
        /// Metodo que consume una API Indicadores economicos para obtener el valor del dolar actual
        /// </summary>
        /// <returns>Retorna el valor del dolar </returns>
        //public int Dolar()
        //{
        //    try
        //    {
        //        #region api_prueba
        //        //  string url = "https://api.desarrolladores.datos.gob.cl/indicadores-financieros/v1/dolar/hoy.json/?auth_key=ef88f15c94123fba357b17765210f096a6f4edfb";
        //        ////  string url = "https://api.sbif.cl/api-sbifv3/recursos_api/dolar?apikey=099dd5965160111e705aa021b9fca409e34b9d9a&formato=json";
        //        #endregion

        //        string url = "https://mindicador.cl/api";

        //        string jason = new WebClient().DownloadString(url);

        //        JavaScriptSerializer js = new JavaScriptSerializer();
        //        var d = js.Deserialize<dynamic>(jason);
        //        var dolares = d["dolar"];


        //        var valores = dolares["valor"];
        //        var facha = dolares["fecha"];
        //        int Dolar = (int)valores;
        //        int ValorDolar = Dolar + 5;
        //        return ValorDolar;
        //    }
        //    catch (Exception ex)
        //    {
        //        ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
        //        throw;
        //    }
        //}


        /// <summary>
        /// Metodo que consume una API Indicadores economicos para obtener el valor del uf actual
        /// </summary>
        /// <returns>Retorna el valor del uf </returns>
        //public int UF()
        //{
        //    try
        //    {
        //        #region api_prueba
        //        //  string url = " https://api.desarrolladores.datos.gob.cl/indicadores-financieros/v1/uf/hoy.json/?auth_key=2943d93a3b9001139056e0f106ddda5cc4a5bdbc ";
        //        #endregion

        //        string url = "https://mindicador.cl/api";
        //        string jason = new WebClient().DownloadString(url);

        //        JavaScriptSerializer js = new JavaScriptSerializer();
        //        var d = js.Deserialize<dynamic>(jason);
        //        var dolares = d["uf"];
        //        var valores = dolares["valor"];
        //        int UF = (int)valores;
        //        //int ValorDolar = Dolar + 5;
        //        return UF;
        //    }
        //    catch (Exception ex)
        //    {
        //        ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
        //        throw;
        //    }




        //}


        #endregion

        #endregion

    }
}
