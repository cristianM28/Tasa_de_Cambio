using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TipoCambio.BL;
namespace TipoCambio.UI
{
    public partial class Form1 : Form
    {

        #region HISTORIA
        //Autor: Cristian M .
        //Fecha: 13/06/2019
        //Notas: form que llama al metodo generartasa 
        #endregion

        #region Variable
        string ruta = ConfigurationManager.AppSettings["PathLogServicio"];
        TipoCambioBL t = new TipoCambioBL();
 
        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GenerarTsaCambio();
            txtMensaje.Text = "Proceso Terminado";
        }

          /// <summary>
          /// Metodo que crea los registros de la tasa de cambio en el crm 
          /// </summary>
        public void GenerarTsaCambio()
        {
            try
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "=========Inicio de la carga de datos=========");
             
                string creado = t.generarTasa();
                if (creado==null)
                {
                    ZthMetodosVarios.Metodos.GuardarLog(ruta, "Error al cargar los datos");
                }


                ZthMetodosVarios.Metodos.GuardarLog(ruta, "==============Proceso Terminado==============");

            }
            catch (Exception ex)
            {
                ZthMetodosVarios.Metodos.GuardarLog(ruta, "Se ha producido el siguiente error: " + ex.Message.ToString());
                throw;
            }
           
        }
    }
}
