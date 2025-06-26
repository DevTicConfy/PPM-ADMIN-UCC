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
    public partial class FacturaContingencia : Form
    {

      ServicesByP cliente = new ServicesByP();

        private int _HorasCarro = 0;
        public int HorasCarro
        {
            get { return _HorasCarro; }
            set { _HorasCarro = value; }
        }
        private int _HorasMoto = 0;
        public int HorasMoto
        {
            get { return _HorasMoto; }
            set { _HorasMoto = value; }
        }
        private int _Tarjeta = 0;
        public int Tarjeta
        {
            get { return _Tarjeta; }
            set { _Tarjeta = value; }
        }
        private int _MensualidadesCarro = 0;
        public int MensualidadesCarro
        {
            get { return _MensualidadesCarro; }
            set { _MensualidadesCarro = value; }
        }
        private int _MensualidadesMoto = 0;
        public int MensualidadesMoto
        {
            get { return _MensualidadesMoto; }
            set { _MensualidadesMoto = value; }
        }

        private int _IdTipoVehiculo = 0;
        public int IdTipoVehiculo
        {
            get { return IdTipoVehiculo; }
            set { IdTipoVehiculo = value; }
        }

        private int _IdTipoPago = 0;
        public int IdTipoPago
        {
            get { return IdTipoPago; }
            set { IdTipoPago = value; }
        }


        public string Observaciones = string.Empty;
        public string _Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
    
        public FacturaContingencia()
        {
            InitializeComponent();
        }

        private void CalcularTotal()
        {
            try
            {
                Int64 pagarCarro = Convert.ToInt64(tbHorasCarro.Text.Replace("$", "").Replace(".", ""));
                Int64 pagarMoto = Convert.ToInt64(tbHorasMoto.Text.Replace("$", "").Replace(".", ""));
                Int64 pagarMensualidadesCarro = Convert.ToInt64(tbMensualidadesCarro.Text.Replace("$", "").Replace(".", ""));
                Int64 pagarMensualidadesMoto = Convert.ToInt64(tbMensualidadesMoto.Text.Replace("$", "").Replace(".", ""));
                Int64 pagarReposicion = Convert.ToInt64(tbReposicion.Text.Replace("$", "").Replace(".", ""));

                Int64 sumaTotal = pagarReposicion + pagarCarro + pagarMoto + pagarMensualidadesCarro + pagarMensualidadesMoto; // Agrega más valores si es necesario

                tbTotal.Text = "$" + string.Format("{0:#,##0.##}", sumaTotal);
            }
            catch (Exception exe)
            {
               
            }
        }

        private void tbHorasCarro_TextChanged(object sender, EventArgs e)
        {

            try
            {
                Int64 valor = Convert.ToInt64(tbHorasCarro.Text.Replace("$", "").Replace(".", ""));

                tbHorasCarro.Text = "$" + string.Format("{0:#,##0.##}", valor);




                CalcularTotal();

            }
            catch (Exception exe)
            {
            }

            tbHorasCarro.SelectionStart = tbHorasCarro.Text.Length;
            tbHorasCarro.SelectionLength = 0;

            if (tbHorasCarro.Text.Length > 0 && tbHorasCarro.Text != "$0")
            {

                tbHorasMoto.Enabled = false;
                tbReposicion.Enabled = false;
                tbMensualidadesCarro.Enabled = false;
                tbMensualidadesMoto.Enabled = false;
            }
            else
            {
                if (tbHorasCarro.Text.Length <= 0 && tbHorasCarro.Text == "")
                {
                    tbHorasMoto.Enabled = true;
                    tbReposicion.Enabled = true;
                    tbMensualidadesCarro.Enabled = true;
                    tbMensualidadesMoto.Enabled = true;
                    tbHorasCarro.Text = "$" + string.Format("{0:#,##0.##}", 0);

                }
            }
        }

        private void tbHorasMoto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int64 valor = Convert.ToInt64(tbHorasMoto.Text.Replace("$", "").Replace(".", ""));

                tbHorasMoto.Text = "$" + string.Format("{0:#,##0.##}", valor);

                CalcularTotal();
            }
            catch (Exception exe)
            {
            }

            tbHorasMoto.SelectionStart = tbHorasMoto.Text.Length;
            tbHorasMoto.SelectionLength = 0;

            if (tbHorasMoto.Text.Length > 0 && tbHorasMoto.Text != "$0")
            {

                tbHorasCarro.Enabled = false;
                tbReposicion.Enabled = false;
                tbMensualidadesCarro.Enabled = false;
                tbMensualidadesMoto.Enabled = false;
            }
            else
            {
                if (tbHorasMoto.Text.Length <= 0 && tbHorasMoto.Text == "")
                {
                    tbHorasCarro.Enabled = true;
                    tbReposicion.Enabled = true;
                    tbMensualidadesCarro.Enabled = true;
                    tbMensualidadesMoto.Enabled = true;
                    tbHorasMoto.Text = "$" + string.Format("{0:#,##0.##}", 0);

                }
            }
        }

        private void tbReposicion_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int64 valor = Convert.ToInt64(tbReposicion.Text.Replace("$", "").Replace(".", ""));

                tbReposicion.Text = "$" + string.Format("{0:#,##0.##}", valor);

                CalcularTotal();
            }
            catch (Exception exe)
            {
            }

            tbReposicion.SelectionStart = tbReposicion.Text.Length;
            tbReposicion.SelectionLength = 0;

            if (tbReposicion.Text.Length > 0 && tbReposicion.Text != "$0")
            {

                tbHorasCarro.Enabled = false;
                tbHorasMoto.Enabled = false;
                tbMensualidadesCarro.Enabled = false;
                tbMensualidadesMoto.Enabled = false;
            }
            else
            {
                if (tbReposicion.Text.Length <= 0 && tbReposicion.Text == "")
                {
                    tbHorasCarro.Enabled = true;
                    tbHorasMoto.Enabled = true;
                    tbMensualidadesCarro.Enabled = true;
                    tbMensualidadesMoto.Enabled = true;
                    tbReposicion.Text = "$" + string.Format("{0:#,##0.##}", 0);

                }
            }
        }

        private void tbMensualidadesCarro_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int64 valor = Convert.ToInt64(tbMensualidadesCarro.Text.Replace("$", "").Replace(".", ""));

                tbMensualidadesCarro.Text = "$" + string.Format("{0:#,##0.##}", valor);

                CalcularTotal();
            }
            catch (Exception exe)
            {
            }

            tbMensualidadesCarro.SelectionStart = tbMensualidadesCarro.Text.Length;
            tbMensualidadesCarro.SelectionLength = 0;

            if (tbMensualidadesCarro.Text.Length > 0 && tbMensualidadesCarro.Text != "$0")
            {

                tbHorasCarro.Enabled = false;
                tbHorasMoto.Enabled = false;
                tbReposicion.Enabled = false;
                tbMensualidadesMoto.Enabled = false;
            }
            else
            {
                if (tbMensualidadesCarro.Text.Length <= 0 && tbMensualidadesCarro.Text == "")
                {
                    tbHorasCarro.Enabled = true;
                    tbHorasMoto.Enabled = true;
                    tbReposicion.Enabled = true;
                    tbMensualidadesMoto.Enabled = true;
                    tbMensualidadesCarro.Text = "$" + string.Format("{0:#,##0.##}", 0);

                }
            }
        }

        private void tbMensualidadesMoto_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int64 valor = Convert.ToInt64(tbMensualidadesMoto.Text.Replace("$", "").Replace(".", ""));

                tbMensualidadesMoto.Text = "$" + string.Format("{0:#,##0.##}", valor);

                CalcularTotal();
            }
            catch (Exception exe)
            {
            }

            tbMensualidadesMoto.SelectionStart = tbMensualidadesMoto.Text.Length;
            tbMensualidadesMoto.SelectionLength = 0;

            if (tbMensualidadesMoto.Text.Length > 0 && tbMensualidadesMoto.Text != "$0")
            {

                tbHorasCarro.Enabled = false;
                tbHorasMoto.Enabled = false;
                tbReposicion.Enabled = false;
                tbMensualidadesCarro.Enabled = false;
            }
            else
            {
                if (tbMensualidadesMoto.Text.Length <= 0 && tbMensualidadesMoto.Text == "")
                {
                    tbHorasCarro.Enabled = true;
                    tbHorasMoto.Enabled = true;
                    tbReposicion.Enabled = true;
                    tbMensualidadesCarro.Enabled = true;
                    tbMensualidadesMoto.Text = "$" + string.Format("{0:#,##0.##}", 0);

                }
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            _Observaciones = tbObservacion.Text.ToString();
            _HorasCarro = Convert.ToInt32(tbHorasCarro.Text.Replace("$", "").Replace(".", ""));
            _HorasMoto = Convert.ToInt32(tbHorasMoto.Text.Replace("$", "").Replace(".", ""));
            _MensualidadesCarro = Convert.ToInt32(tbMensualidadesCarro.Text.Replace("$", "").Replace(".", ""));
            _MensualidadesMoto = Convert.ToInt32(tbMensualidadesMoto.Text.Replace("$", "").Replace(".", ""));
            _Tarjeta = Convert.ToInt32(tbReposicion.Text.Replace("$", "").Replace(".", ""));
            if (HorasCarro > 0)
            {
                _IdTipoVehiculo = 1;
                _IdTipoPago = 1;
            }
            if (HorasMoto > 0)
            {
                _IdTipoVehiculo = 2;
                _IdTipoPago = 1;
            }
            if (MensualidadesCarro > 0)
            {
                _IdTipoVehiculo = 1;
                _IdTipoPago = 2;
            }
            if (MensualidadesMoto > 0)
            {
                _IdTipoVehiculo = 2;
                _IdTipoPago = 2;
            }
            if (Tarjeta > 0)
            {
                _IdTipoPago = 3;
                _IdTipoVehiculo = 6;
            }

            int total = HorasCarro + HorasMoto + MensualidadesCarro + MensualidadesMoto + Tarjeta;
            if (Observaciones != string.Empty)
            {
                if (total > 0)
                {
                    DialogResult result3 = MessageBox.Show("¿Desea generar la factura de contingencia con un total de: " + tbTotal.Text + " ?", "Facturación Electronica", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result3 == DialogResult.Yes)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                    }
                }
                else
                {
                    MessageBox.Show("No se puede generar la factura con un valor de $0", "Facturación Electronica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.None;
                }
            }
            else
            {
                MessageBox.Show("Es necesario que llene el campo observaciones", "Facturación Electronica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbObservacion.Focus();
                this.DialogResult = DialogResult.None;
            }
        }

        private void FacturaContingencia_Load(object sender, EventArgs e)
        {
            tbHorasMoto.Text = "0";
            tbHorasCarro.Text = "0";
            tbMensualidadesMoto.Text = "0";
            tbMensualidadesCarro.Text = "0";
            tbReposicion.Text = "0";
            tbObservacion.Text = "";
        }
    }
}
