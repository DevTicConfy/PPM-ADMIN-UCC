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
    public partial class ReporteDePatios : Form
    {
        public ReporteDePatios()
        {
            InitializeComponent();
        }

        private void ReporteDePatios_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'dataSetReportes.P_ReportePatios' Puede moverla o quitarla según sea necesario.
            this.p_ReportePatiosTableAdapter.Fill(this.dataSetReportes.P_ReportePatios);
            this.reportViewer1.RefreshReport();
            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {

            this.Hide();
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
