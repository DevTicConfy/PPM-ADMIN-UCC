using System;
using BlockAndPass.PPMWinform.ByPServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class ReposicionMensualidadPopUp : Form
    {


        ServicesByP cliente = new ServicesByP();

        public ReposicionMensualidadPopUp()
        {
            InitializeComponent();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            ConsultarInformacionAutorizadoPorPlaca(textBox1.Text.Trim());
        }

        public void ConsultarInformacionAutorizadoPorPlaca(string sPlaca)
        {

            try
            {
                DataTable tabla = cliente.BuscarAutorizadoxPlacaReposicion(sPlaca);
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = tabla;

                dvgListadoTransacciones.DataSource = tabla;

                if (dvgListadoTransacciones.Rows.Count <= 1)
                {
                    dvgListadoTransacciones.Columns[0].Visible = false;
                    dvgListadoTransacciones.DataSource = null;
                    dvgListadoTransacciones.Rows.Clear();
                    dvgListadoTransacciones.Columns[0].Visible = false;
                }
                else
                {
                    dvgListadoTransacciones.Columns[0].Visible = true;

                }

            }
            catch (Exception ex )
            {

                throw ex;
            }

        }
    }
}
