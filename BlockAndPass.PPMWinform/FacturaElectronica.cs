using BlockAndPass.PPMWinform.ByPServices;
using System;
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
    public partial class FacturaElectronica : Form
    {


        ServicesByP cliente = new ServicesByP();

        public string _Nit = "";
        public string Nit
        {
            get { return _Nit; }
            set { _Nit = value; }
        }

        private bool _SolicitudFacturaElectronica = false;
        public bool SolicitudFacturaElectronica
        {
            get { return _SolicitudFacturaElectronica; }
            set { _SolicitudFacturaElectronica = value; }
        }


        public FacturaElectronica()
        {
            InitializeComponent();
            lblRtaCliente.Visible = false;

        }


        public void ValidarInformacionCliente(string nitCliente)
        {
            try
            {
                InfoClienteFacturaElectronicaResponse oInfoCliente = cliente.ValidarClientePorNit(nitCliente);
                if (oInfoCliente.Exito)
                {
                    lblRtaCliente.Visible = true;
                    lblRtaCliente.Text = oInfoCliente.Nombre.Trim();
                    lblRtaCliente.ForeColor = Color.Green;
                    lblRtaCliente.Update();
                    _SolicitudFacturaElectronica = true;
                    _Nit = bNitCliente.Text.Trim();
                }
                else
                {
                    lblRtaCliente.Visible = true;
                    lblRtaCliente.Text = "El cliente no está registrado";
                    lblRtaCliente.ForeColor = Color.Red;
                    lblRtaCliente.Update();
                    _SolicitudFacturaElectronica = false;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Facturación Electrónica - PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bNitCliente_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string texto = bNitCliente.Text;

                if (char.IsLetter(texto[texto.Length - 1]))
                {
                    bNitCliente.Text = "";
                }

                if (texto.Length > 0)
                {
                    ValidarInformacionCliente((texto.Trim()));
                }
                else
                {
                    lblRtaCliente.Text = "";
                    lblRtaCliente.Visible = false;
                }
            }
            catch (Exception)
            {

                bNitCliente.Focus();
            }

        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void bNitCliente_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }



    }
}
