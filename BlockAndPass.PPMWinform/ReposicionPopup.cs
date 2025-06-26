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
    public partial class ReposicionPopup : Form
    {


        ServicesByP cliente = new ServicesByP();

        #region Definiciones

        private string _PlacaEntrada = string.Empty;

        public string PlacaEntrada
        {
            get { return _PlacaEntrada; }
            set { _PlacaEntrada = value; }
        }

        private string _ModuloEntrada = string.Empty;

        public string ModuloEntrada
        {
            get { return _ModuloEntrada; }
            set { _ModuloEntrada = value; }
        }

        private string _IdTransaccion = string.Empty;

        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _ModuloEntrada = value; }
        }

        private int _IdTipoVehiculo = 0;

        public int IdTipoVehiculo
        {
            get { return _IdTipoVehiculo; }
            set { _IdTipoVehiculo = value; }
        }




        #endregion

        public ReposicionPopup()
        {
            InitializeComponent();

            //DialogResult result3 = MessageBox.Show("¿La reposición es para una mensualidad?", "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //if (result3 == DialogResult.Yes)
            //{
            //    ReposicionMensualidadPopUp popUp = new ReposicionMensualidadPopUp();
            //    popUp.ShowDialog();
            //    if (popUp.DialogResult == DialogResult.Cancel || popUp.DialogResult == DialogResult.Yes) 
            //    {
            //        this.Close();
            //        this.DialogResult = DialogResult.Cancel;
            //    }
            //}

        }

        public void ConsultarTransaccionPorPlacaEntrada(string placaEntrada)
        {
            try
            {
                DataTable tabla = cliente.ListarTransaccionesPorPlacaEntrada(placaEntrada);
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string placaBuscar = tbPlacaBuscar.Text.Trim();
            try
            {
                if (tbPlacaBuscar.Text.Length > 0)
                {
                    ConsultarTransaccionPorPlacaEntrada(placaBuscar);

                }
                else
                {
                    dvgListadoTransacciones.DataSource = null;
                    dvgListadoTransacciones.Rows.Clear();
                    dvgListadoTransacciones.Columns[0].Visible = false;

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void dvgListadoTransacciones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dvgListadoTransacciones.Columns["Reposicion"].Index)
            {
                DataGridViewCheckBoxCell chkAplicarReposicion = (DataGridViewCheckBoxCell)dvgListadoTransacciones.Rows[e.RowIndex].Cells["Reposicion"];
                chkAplicarReposicion.Value = !Convert.ToBoolean(chkAplicarReposicion.Value);
            }
            foreach (DataGridViewRow item in dvgListadoTransacciones.Rows)
            {
                if (Convert.ToBoolean(item.Cells[0].Value) == true)
                {
                    try
                    {
                        DialogResult result = MessageBox.Show("¿Desea aplicar una reposición para la placa "+item.Cells[5].Value.ToString().Trim()+"", "Reposición - PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (result == DialogResult.Yes)
                        {
                            _IdTransaccion = item.Cells[1].Value.ToString();
                            _ModuloEntrada = item.Cells[2].Value.ToString();
                            _PlacaEntrada = item.Cells[4].Value.ToString();
                            btn_Ok_Click(btn_Ok, EventArgs.Empty);


                        }
                        else if (result == DialogResult.No)
                        {
                            dvgListadoTransacciones.DataSource = null;
                            dvgListadoTransacciones.Rows.Clear();
                            dvgListadoTransacciones.Columns[0].Visible = false;
                            this.Close();
                        }

                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                else
                {
                    
                }

            }

        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.PlacaEntrada = _PlacaEntrada;
            this.IdTransaccion = _IdTransaccion;
            this.IdTipoVehiculo = _IdTipoVehiculo;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
