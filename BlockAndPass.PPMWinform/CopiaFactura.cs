
using BlockAndPass.PPMWinform.ByPServices;
using BlockAndPass.PPMWinform.Tickets;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class CopiaFactura : Form
    {
        ServicesByP cliente = new ServicesByP();
        long _IdEstacionamiento = 0;
        public static SqlConnection conexionSQL = new SqlConnection();
        public static SqlCommand comandoSQL = new SqlCommand();
        public static string sSerial
        {
            get
            {
                string sIdeModulo = ConfigurationManager.AppSettings["serial"];
                if (!string.IsNullOrEmpty(sIdeModulo))
                {
                    return sIdeModulo;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public CopiaFactura(string idEstacionamiento)
        {
            InitializeComponent();

            ReportNumeroFactura.Visible = false;
            ReportPlacaEntrada.Visible = false;
            ReportMensualidad.Visible = false;
            ReportMensualidadPorPlaca.Visible = false;


            _IdEstacionamiento = Convert.ToInt64(idEstacionamiento);
            ModulosResponse oModulos = cliente.ListarModulosPagos(idEstacionamiento);

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
                MessageBox.Show("No se encontraron modulos de pagos activos para el id = " + idEstacionamiento, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void CopiaFactura_Load(object sender, EventArgs e)
        {


        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void btn_Ok_Click(object sender, EventArgs e)
        {

            #region Old
            //if (tbnumerofactura.Text != "")
            //{
            //    if (chkMensualidad.Checked)
            //    {
            //        CopiaFacturaM popUp = new CopiaFacturaM(tbnumerofactura.Text.Trim(), cboIdModulo.Text.ToString(), true, _IdEstacionamiento);
            //        popUp.ShowDialog();
            //        if (popUp.DialogResult == DialogResult.Cancel)
            //        {
            //            this.Hide();
            //            this.DialogResult = DialogResult.Cancel;
            //        }

            //    }
            //    else
            //    {

            //        InfoFacturaResponse oInfoFactura = cliente.ObtenerDatosCopiaFactura(tbnumerofactura.Text.Trim(), cboIdModulo.Text.ToString());
            //        if (oInfoFactura.Exito)
            //        {

            //            string numeroFactura = tbnumerofactura.Text.Trim();
            //            string idModulo = cboIdModulo.Text.ToString();
            //            //bool resultado = PrintTicket(oInfoFactura.LstItems.ToList());
            //            //if (!resultado)
            //            //{
            //            //    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            //}
            //            this.dataTable3TableAdapter.Fill(this.dataSetCopia.DataTable3, numeroFactura, idModulo);
            //            //this.DATA

            //            this.ReportNumeroFactura.RefreshReport();

            //        }
            //    }
            //}
            #endregion

            GenerarCopiaFactura();

        }


        public void GenerarCopiaFactura()
        {
            if (tbnumerofactura.Text != "" && chkMensualidad.Checked == false && tbPlacaBuscar.Text=="")
            {
                InfoFacturaResponse oInfoFactura = cliente.ObtenerDatosCopiaFactura(tbnumerofactura.Text.Trim(), cboIdModulo.Text.ToString());
                if (oInfoFactura.Exito)
                {

                    ReportPlacaEntrada.Visible = false;
                    ReportMensualidad.Visible = false;
                    ReportNumeroFactura.Visible = true;
                    ReportMensualidad.Visible = false;
                    ReportMensualidadPorPlaca.Visible = false;


                    string numeroFactura = tbnumerofactura.Text.Trim();
                    string idModulo = cboIdModulo.Text.ToString();
                    //bool resultado = PrintTicket(oInfoFactura.LstItems.ToList());
                    //if (!resultado)
                    //{
                    //    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}

                    ReportNumeroFactura.Size =new Size(539, 599);
                    ReportNumeroFactura.Location = new Point(5,3);

                    this.dataTable3TableAdapter.Fill(this.dataSetCopia.DataTable3, numeroFactura, idModulo);
                    //this.DATA

                    this.ReportNumeroFactura.RefreshReport();
                    

                }

            }
            else if (tbnumerofactura.Text == "" && chkMensualidad.Checked == false && tbPlacaBuscar.Text != "")
            {
                ReportPlacaEntrada.Visible = true;
                ReportMensualidad.Visible = false;
                ReportNumeroFactura.Visible = false;
                ReportMensualidad.Visible = false;
                ReportMensualidadPorPlaca.Visible = false;


                string numeroFactura = tbnumerofactura.Text.Trim();
                string idModulo = cboIdModulo.Text.ToString();
                string placaEntrada = tbPlacaBuscar.Text.Trim();

                //bool resultado = PrintTicket(oInfoFactura.LstItems.ToList());
                //if (!resultado)
                //{
                //    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                ReportPlacaEntrada.Size = new Size(539, 599);
                ReportPlacaEntrada.Location = new Point(5,3);

                this.dataTable4TableAdapter.Fill(this.dataSetCopia.DataTable4, placaEntrada, idModulo);
                //this.DATA

                this.ReportPlacaEntrada.RefreshReport();
            }
            else if (tbnumerofactura.Text != "" && tbPlacaBuscar.Text == "" && chkMensualidad.Checked == true)
            {
                ReportPlacaEntrada.Visible = false;
                ReportMensualidad.Visible = false;
                ReportNumeroFactura.Visible = false;
                ReportMensualidad.Visible = true;
                ReportMensualidadPorPlaca.Visible = false;

                string numeroFactura = tbnumerofactura.Text.Trim();
                string idModulo = cboIdModulo.Text.ToString();
                string placaEntrada = tbPlacaBuscar.Text.Trim();

                //bool resultado = PrintTicket(oInfoFactura.LstItems.ToList());
                //if (!resultado)
                //{
                //    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                ReportMensualidad.Size = new Size(539, 599);
                ReportMensualidad.Location = new Point(5, 3);

                this.dataTable2TableAdapter.Fill(this.dataSetCopia.DataTable2, numeroFactura, idModulo);

                //this.DATA

                this.ReportMensualidad.RefreshReport();
            }
            else if (tbnumerofactura.Text == "" && tbPlacaBuscar.Text != "" && chkMensualidad.Checked == true)
            {
                ReportPlacaEntrada.Visible = false;
                ReportMensualidad.Visible = false;
                ReportNumeroFactura.Visible = false;
                ReportMensualidad.Visible = false;
                ReportMensualidadPorPlaca.Visible = true;

                string numeroFactura = tbnumerofactura.Text.Trim();
                string idModulo = cboIdModulo.Text.ToString();
                string placaEntrada = tbPlacaBuscar.Text.Trim();

                //bool resultado = PrintTicket(oInfoFactura.LstItems.ToList());
                //if (!resultado)
                //{
                //    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}

                string idTransaccion = string.Empty;
                AutorizadoxPlacaResponse infoAutorizado = cliente.BuscarAutorizadoxPlaca(placaEntrada);
                if (infoAutorizado.Exito)
                {
                    idTransaccion = infoAutorizado.Documento;
                }

                ReportMensualidadPorPlaca.Size = new Size(539, 599);
                ReportMensualidadPorPlaca.Location = new Point(5, 3);

                this.dataTable5TableAdapter.Fill(this.dataSetCopia.DataTable5, idTransaccion, idModulo);

                //this.DATA

                this.ReportMensualidadPorPlaca.RefreshReport();
            }

        }

        private void tbPlacaBuscar_TextChanged(object sender, EventArgs e)
        {
            if (tbPlacaBuscar.Text.Length > 0)
            {
                tbnumerofactura.Text = "";
                tbnumerofactura.Enabled = false;
            }
            else if (tbPlacaBuscar.Text.Length <= 0)
            {
                tbnumerofactura.Text = "";
                tbnumerofactura.Enabled = true;
            }
        }

        private void tbnumerofactura_TextChanged(object sender, EventArgs e)
        {
            if (tbnumerofactura.Text.Length > 0)
            {
                tbPlacaBuscar.Text = "";
                tbPlacaBuscar.Enabled = false;
            }
            else if (tbnumerofactura.Text.Length <= 0)
            {
                tbPlacaBuscar.Text = "";
                tbPlacaBuscar.Enabled = true;
            }
        }


        #region Old


        //string numfact = tbnumerofactura.text;

        //string data = @"" + sserial + "";
        //conexionsql.connectionstring = data;

        //string tipo = string.empty;

        //conexionsql.open();

        ////formar la sentencia sql, un select en este caso
        //sqldatareader myreader1 = null;
        //string strcadsql1 = "select idtipopago from  t_pagos where  numerofactura = '" + numfact + "'";
        //sqlcommand mycommand1 = new sqlcommand(strcadsql1, conexionsql);

        ////ejecutar el comando sql
        //myreader1 = mycommand1.executereader();

        //while (myreader1.read())
        //{
        //    tipo = myreader1["idtipopago"].tostring();
        //}

        //conexionsql.close();

        //if (tipo == "1")
        //{

        //    this.datatable1tableadapter1.fill(this.datasetcopia.datatable1, numfact);
        //    //this.data

        //    this.reportviewer1.refreshreport();
        //}
        //else
        //{
        //    copiafacturam popup = new copiafacturam(tbnumerofactura.text);
        //    popup.showdialog();
        //    if (popup.dialogresult == dialogresult.ok)
        //    {
        //        this.close();
        //    }
        //    else if (popup.dialogresult == system.windows.forms.dialogresult.cancel)
        //    {
        //        this.dialogresult = dialogresult.cancel;
        //        this.close();
        //    }
        //}
    }

    //public bool PrintTicket(List<InfoItemsFacturaResponse> datos)
    //{
    //    bool bPrint = false;

    //    try
    //    {

    //        List<List<InfoItemsFacturaResponse>> facturas = new List<List<InfoItemsFacturaResponse>>();
    //        foreach (InfoItemsFacturaResponse item in datos)
    //        {
    //            bool find = false;
    //            if (facturas.Count > 0)
    //            {
    //                foreach (List<InfoItemsFacturaResponse> item2 in facturas)
    //                {
    //                    if (item2[0].NumeroFactura == item.NumeroFactura)
    //                    {
    //                        find = true;
    //                        item2.Add(item);
    //                    }
    //                }

    //                if (!find)
    //                {
    //                    List<InfoItemsFacturaResponse> otraFactura = new List<InfoItemsFacturaResponse>();
    //                    otraFactura.Add(item);
    //                    facturas.Add(otraFactura);
    //                }
    //                find = false;
    //            }
    //            else
    //            {
    //                List<InfoItemsFacturaResponse> primeraFactura = new List<InfoItemsFacturaResponse>();
    //                primeraFactura.Add(item);
    //                facturas.Add(primeraFactura);
    //            }
    //        }



    //        if (facturas.Count > 0)
    //        {
    //            foreach (var item in facturas)
    //            {
    //                ReportDataSource datasource = new ReportDataSource();
    //                LocalReport oLocalReport = new LocalReport();

    //                datasource = new ReportDataSource("DataSetCopia", (DataTable)GenerarTicketCopia(item).Tables[0]);
    //                oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "CopiaFactura"));
    //                oLocalReport.DataSources.Add(datasource);
    //                oLocalReport.Refresh();
    //                ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
    //                ore.PrintController = new StandardPrintController();
    //                ore.Print();
    //                oLocalReport.Dispose();
    //                oLocalReport = null;
    //                ore.Dispose();
    //                ore = null;
    //            }
    //        }





    //        bPrint = true;
    //    }
    //    catch (Exception e)
    //    {
    //        bPrint = false;
    //    }
    //    return bPrint;
    //}

    //private DataSetCopia GenerarTicketCopia(List<InfoItemsFacturaResponse> infoTicket)
    //{
    //    DataSetCopia facturacion = new DataSetCopia();

    //    double total = 0;
    //    foreach (var item in infoTicket)
    //    {
    //        total += Convert.ToDouble(item.Total);
    //    }

    //    foreach (var item in infoTicket)
    //    {
    //        DataSetCopia.DataTable3Row rowDatosFactura = facturacion.DataTable3.NewDataTable3Row();

    //        rowDatosFactura.Nombre = item.Nombre;
    //        rowDatosFactura.Direccion = item.Direccion;
    //        //rowDatosFactura.Fecha = Convert.ToDateTime(item.Fecha).ToString("yyyy/MM/dd HH:mm tt");
    //        rowDatosFactura.FechaPago = item.Fecha.ToString();
    //        rowDatosFactura.IdTransaccion = item.IdTransaccion;
    //        //rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
    //        //rowDatosFactura.Modulo = item.Modulo;
    //        rowDatosFactura.NumeroFactura = item.NumeroFactura;
    //        rowDatosFactura.PlacaEntrada = item.Placa;
    //        //rowDatosFactura.Recibido = Convert.ToDouble(item.ValorRecibido);
    //        rowDatosFactura.NumeroResolucion = item.NumeroResolucion;
    //        rowDatosFactura.NIT = "NIT 900.554.696 -8";
    //        rowDatosFactura.TelefonoContacto = item.Telefono;
    //        //rowDatosFactura.Total = total;
    //        rowDatosFactura.Total = item.Fecha.ToString();
    //        //rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
    //        //rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
    //        if (Convert.ToInt32(item.Tipo) == 1) 
    //        {
    //            rowDatosFactura.IdTipoPago = item.Tipo;
    //        }
    //        //rowDatosFactura.IdTipoPago = item.Tipo;
    //        rowDatosFactura.FechaEntrada = item.Fecha.ToString();
    //        //rowDatosFactura.Vehiculo = item.TipoVehiculo;
    //        //rowDatosFactura. = item.Vigencia;

    //        facturacion.DataTable3.AddDataTable3Row(rowDatosFactura);
    //    }

    //    return facturacion;
    //}
        #endregion
}

