using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using ZthSeguridad;
using System.Net;

namespace TipoCambio.DA
{
   public  class ConexionCrmDA
    {

        static IOrganizationService _orgService;

        static public string connection = "";
        static public string usuarioCRM = Metodos.Desencriptar(ConfigurationManager.AppSettings["Usuario_CRM"]);
        static public string claveCRM = Metodos.Desencriptar(ConfigurationManager.AppSettings["Clave_CRM"]);
        static public string deviceId = "";
        static public string deviceClave = "";
        static public string urlCRM = Metodos.Desencriptar(ConfigurationManager.AppSettings["URL_CRM"]);
        static CrmServiceClient conn = null;

        public static IOrganizationService ObtenerConexion()
        {

            try
            {

                connection = string.Format(ConfigurationManager.AppSettings["Connection_CRM"], urlCRM, usuarioCRM, claveCRM);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                conn = new CrmServiceClient(connection);

                //Validamos si se pudo realizar la Conexión
                if (conn.IsReady)
                {

                    _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)
                    conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
                    conn.Dispose();
                    conn = null;
                    return _orgService;

                }
                else
                {

                    conn.Dispose();
                    conn = null;
                    return _orgService;

                }

            }
            catch (Exception ex)
            {
                string ruta = ConfigurationManager.AppSettings["PathLogServicio"];

                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw new Exception(ex.Message.ToString());

            }

        }

    }
}
