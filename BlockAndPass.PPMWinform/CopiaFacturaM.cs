using BlockAndPass.PPMWinform.ByPServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class CopiaFacturaM : Form
    {
        string _NumFact = string.Empty;
        string _IdModulo = string.Empty;
        bool btnMensualidad = false;
        long _IdEstacionamiento = 0;
        ServicesByP cliente = new ServicesByP();
        public CopiaFacturaM(string NUM, string idModulo, bool activado, long idEstacionamiento)
        {
            _NumFact = NUM;
            _IdModulo = idModulo;
            btnMensualidad = activado;
            _IdEstacionamiento = idEstacionamiento;
            InitializeComponent();

            ListarModulos(_IdEstacionamiento);           
        }
        private void CopiaFacturaM_Load(object sender, EventArgs e)
        {
            if (btnMensualidad)
            {
                chkMensualidad.Checked = true;
                cboIdModulo.DisplayMember = _IdModulo;
                tbnumerofactura.Text = _NumFact;
            }
        }

        public void ListarModulos(long IdEstacionamiento)
        {
            ModulosResponse oModulos = cliente.ListarModulosPagos(IdEstacionamiento.ToString());

            if (oModulos.Exito)
            {
                var datasource = new List<Object>();
                foreach (var item in oModulos.ListInfoModuloResponse)
                {
                    datasource.Add(new
                    {
                        Name = item.Display,
                        Value = item.Value
                    });

                    cboIdModulo.DataSource = datasource;
                    cboIdModulo.ValueMember = "Name";
                    cboIdModulo.DisplayMember = "Name";
                }
            }
            else
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show("No se encontraron modulos de pagos activos para el id = " + IdEstacionamiento, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void cboIdModulo_Click(object sender, EventArgs e)
        {
            ListarModulos(_IdEstacionamiento);
        }

        private void btn_Ok_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.dataTable2TableAdapter.Fill(this.dataSetCopia.DataTable2, _NumFact, _IdModulo);
                //this.DATA

                this.reportViewer1.RefreshReport();
            }
            catch (Exception)
            {
                
              this.reportViewer1.RefreshReport();

            }
    
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void tbnumerofactura_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
