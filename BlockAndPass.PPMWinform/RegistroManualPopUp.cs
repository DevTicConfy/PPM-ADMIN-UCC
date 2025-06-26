using BlockAndPass.PPMWinform.ByPServices;
using BlockAndPass.PPMWinform.Tickets;
using Microsoft.Reporting.WinForms;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class RegistroManualPopUp : Form
    {
        ServicesByP cliente = new ServicesByP();

        private string _imgQR = string.Empty;
        public string imgQR
        {
            get { return _imgQR; }
            set { _imgQR = value; }
        }
        private string _Observacion = string.Empty;
        public string Observacion
        {
            get { return _Observacion; }
            set { _Observacion = value; }
        }
        private int _Motivo = 0;
        public int Motivo
        {
            get { return _Motivo; }
            set { _Motivo = value; }
        }
        private int _FechaYHora = 0;
        public int FechaYHora
        {
            get { return _FechaYHora; }
            set { _FechaYHora = value; }
        }
        private int _IdTipoVehiculo = 0;
        public int IdTipoVehiculo
        {
            get { return _IdTipoVehiculo; }
            set { _IdTipoVehiculo = value; }
        }
        private long _IdTransaccion = 0;
        public long IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        private int _IdTipoPago = 0;
        public int IdTipoPago
        {
            get { return _IdTipoPago; }
            set { _IdTipoPago = value; }
        }

        private string _IdEstacionamiento = string.Empty;
        public string IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        private string _IdModulo = string.Empty;
        public string IdModulo
        {
            get { return _IdModulo; }
            set { _IdModulo = value; }
        }

        private string _DocumentoUsuario = string.Empty;
        public string DocumentoUsuario
        {
            get { return _DocumentoUsuario; }
            set { _DocumentoUsuario = value; }
        }
        private string _NitCliente = string.Empty;
        public string NitCliente
        {
            get { return _NitCliente; }
            set { _NitCliente = value; }
        }


        List<InfoPagoRegistroManualResponse> listaDatos = new List<InfoPagoRegistroManualResponse>();
        InfoPagoRegistroManualResponse nuevoRegistro = new InfoPagoRegistroManualResponse();
        InfoPagoRegistroManualResponse RegistroParqueo = new InfoPagoRegistroManualResponse();
        InfoPagoRegistroManualResponse RegistroTarjeta = new InfoPagoRegistroManualResponse();


        public RegistroManualPopUp(string idEstacionamiento, string idModulo, string documentoUsuario)
        {


            InitializeComponent();

            _IdEstacionamiento = idEstacionamiento;
            _IdModulo = idModulo;
            _DocumentoUsuario = documentoUsuario;

            trmHoraActual.Start();
            trmHoraActual.Interval = 1000;
            trmHoraActual.Tick += trmHoraActual_Tick;
            MotivosRegistroManuales oInfo = cliente.ObtenerListaMovitosRegistroManuales(idEstacionamiento);
            if (!oInfo.Exito)
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show("No encontro motivos asociados al estacionamiento id = " + idEstacionamiento, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                var dataSource = new List<Object>();
                foreach (var item in oInfo.LstMotivosRegistrosManuales)
                {
                    dataSource.Add(new { Name = item.Display, Value = item.Value });
                }

                //Setup data binding
                this.cbMotivo.DataSource = dataSource;
                this.cbMotivo.DisplayMember = "Name";
                this.cbMotivo.ValueMember = "Value";
            }
        }

        private void RegistroManualPopUp_Load(object sender, EventArgs e)
        {
            tbParqueo.Text = "0";
            tbTarjeta.Text = "0";
            tbMensualidad.Text = "0";
        }

        #region Botones y eventos

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (tbObservacion.Text != "" && tbObservacion.Text.Length > 5)
            {
                if (tbTotal.Text != "$0")
                {
                    if (tbPlaca.Text != "" || tbPlaca.Text != string.Empty)
                    {
                        if ((!rdoMoto.Checked && rdoCarro.Checked) || (!rdoCarro.Checked && rdoMoto.Checked))
                        {
                            if (cboHora.Text != "")
                            {

                                //DialogResult result3 = MessageBox.Show("¿Está seguro de confirmar el registro manual por un valor de: " + tbTotal.Text + " ?", "PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                                MensajePopUp popUp = new MensajePopUp(tbTotal.Text.Trim());
                                popUp.ShowDialog();
                                if (popUp.DialogResult == DialogResult.OK)
                                {
                                    if (GuardarRegistro())
                                    {
                                        this.DialogResult = DialogResult.OK;
                                        this.Close();
                                        this.Hide();
                                        Limpiar();
                                    }

                                }
                                else
                                {
                                    this.DialogResult = DialogResult.None;
                                    this.Close();

                                }
                            }
                            else
                            {
                                MessageBox.Show("Seleccione una hora de entrada", "Facturación Electronica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.DialogResult = DialogResult.Cancel;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Seleccione el tipo de vehículo", "Facturación Electronica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.DialogResult = DialogResult.Cancel;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Por favor ingrese una placa", "Facturación Electronica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.DialogResult = DialogResult.Cancel;
                    }
                }
                else
                {
                    MessageBox.Show("No se puede generar la factura con un valor de $0", "Facturación Electronica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                }
            }
            else
            {
                MessageBox.Show("Es necesario que ingrese una observación válida", "Facturación Electronica", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
            }

        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        void trmHoraActual_Tick(object sender, EventArgs e)
        {
            tbFechaYHoraActual.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            tbFechaYHoraActual.Update();
            tbFechaYHoraActual.Update();
        }

        private void grupoFrm_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbParqueo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int64 valor = Convert.ToInt64(tbParqueo.Text.Replace("$", "").Replace(".", ""));

                if (valor > 0)
                {
                    tbMensualidad.Enabled = false;
                    RegistroParqueo.IdTipoPago = 1;
                }
                else
                {
                    tbMensualidad.Enabled = true;
                }

                tbParqueo.Text = "$" + string.Format("{0:#,##0.##}", valor);

                CalcularTotal();
            }
            catch (Exception exe)
            {
            }

            tbParqueo.SelectionStart = tbParqueo.Text.Length;
            tbParqueo.SelectionLength = 0;
        }

        private void tbTarjeta_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int64 valor = Convert.ToInt64(tbTarjeta.Text.Replace("$", "").Replace(".", ""));

                if (valor > 0)
                {
                    RegistroTarjeta.IdTipoPago = 3;
                }

                tbTarjeta.Text = "$" + string.Format("{0:#,##0.##}", valor);

                CalcularTotal();
            }
            catch (Exception exe)
            {
            }

            tbTarjeta.SelectionStart = tbTarjeta.Text.Length;
            tbTarjeta.SelectionLength = 0;
        }

        private void tbMensualidad_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int64 valor = Convert.ToInt64(tbMensualidad.Text.Replace("$", "").Replace(".", ""));

                if (valor > 0)
                {
                    nuevoRegistro.IdTipoPago = 2;
                    tbParqueo.Enabled = false;
                }
                else
                {
                    tbParqueo.Enabled = true;
                }

                tbMensualidad.Text = "$" + string.Format("{0:#,##0.##}", valor);

                CalcularTotal();
            }
            catch (Exception exe)
            {
            }

            tbMensualidad.SelectionStart = tbMensualidad.Text.Length;
            tbMensualidad.SelectionLength = 0;
        }
        #endregion

        #region Funciones
        public void CapturaRutaCodigoQR()
        {


            string Placa = string.Empty;
            string rutaBarras = string.Empty;

            rutaBarras = ConfigurationManager.AppSettings["RutaCodigoQR"].ToString();
            _imgQR = rutaBarras;

        }
        public void EliminarRutaCodigoQR()
        {
            if (File.Exists(_imgQR))
            {
                File.Delete(_imgQR);
            }
        }
        private string CalcularCUDE(
string numeroDocumento, string fechaEmision, string horaEmision,
decimal totalSinImpuestos, decimal totalImpuesto1, decimal totalImpuesto2, decimal totalImpuesto3, decimal totalConImpuestos, string identificacionEmisor, string identificacionAdquiriente, string SoftwarePin, string tipoAmbiente)
        {

            bool pruebas = Convert.ToBoolean(ConfigurationManager.AppSettings["AmbientePruebas"]);

            if (pruebas)
            {
                tipoAmbiente = "2";
            }
            else
            {
                tipoAmbiente = "1";
            }
            // Cadena de identificación
            var cadenaCufe = String.Concat(
                numeroDocumento,
                fechaEmision, horaEmision,
                totalSinImpuestos.ToString("0.00", CultureInfo.InvariantCulture),
                "01", totalImpuesto1.ToString("0.00", CultureInfo.InvariantCulture),
                "04", totalImpuesto2.ToString("0.00", CultureInfo.InvariantCulture),
                "03", totalImpuesto3.ToString("0.00", CultureInfo.InvariantCulture),
                totalConImpuestos.ToString("0.00", CultureInfo.InvariantCulture),
                identificacionEmisor,
                identificacionAdquiriente,
                SoftwarePin, tipoAmbiente);

            // Conversión del hash a String con hexadecimales
            string rta = ComputeSha384Hash(cadenaCufe);
            return rta;
        }
        static string ComputeSha384Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA384 sha384Hash = SHA384.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha384Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public bool GuardarRegistro()
        {
            bool ok = true;
            try
            {
                int idtipoVehiculo = 0;
                if (rdoCarro.Checked)
                {
                    idtipoVehiculo = 1;
                }
                else if (rdoMoto.Checked)
                {
                    idtipoVehiculo = 2;
                }
                if (cboMinuto.Text == "")
                {
                    cboMinuto.Text = "00";
                }
                string fechaEntrada = tbFechaEntrada.Text + " " + cboHora.SelectedItem.ToString().Trim() + ":" + cboMinuto.SelectedItem.ToString().Trim() + ":00";

                string idTransaccionEnFecha = Convert.ToDateTime(tbFechaYHoraActual.Text.Trim()).ToString("yyyyMMddHHmmss") + IdEstacionamiento;
                _IdTransaccion = Convert.ToInt64(idTransaccionEnFecha);
                Int64 valorTotal = Convert.ToInt64(tbTotal.Text.Replace("$", "").Replace(".", ""));

                //MENSUALIDAD
                Int64 valor = Convert.ToInt64(tbMensualidad.Text.Replace("$", "").Replace(".", ""));

                if (valor > 500)
                {
                    nuevoRegistro.IdTipoPago = 2;
                    double subtotal;
                    double iva;
                    double total;
                    subtotal = Math.Round(valor / 1.19, 0);
                    iva = valor - subtotal;
                    total = valor;
                    nuevoRegistro.IdMotivo = Convert.ToInt32(cbMotivo.SelectedValue);
                    nuevoRegistro.Subtotal = subtotal;
                    nuevoRegistro.Iva = iva;
                    nuevoRegistro.Total = total;
                    nuevoRegistro.IdTransaccion = Convert.ToInt64(_IdTransaccion);
                    nuevoRegistro.Observacion = tbObservacion.Text.Trim();
                    nuevoRegistro.Placa = tbPlaca.Text.Trim();
                    listaDatos.Add(nuevoRegistro);
                }

                //PARQUEO Y TARJETA


                Int64 valorParqueo = Convert.ToInt64(tbParqueo.Text.Replace("$", "").Replace(".", ""));

                if (valorParqueo > 500)
                {
                    RegistroParqueo.IdTipoPago = 1;
                    double subtotal;
                    double iva;
                    double total;
                    subtotal = Math.Round(valorParqueo / 1.19, 0);
                    iva = valorParqueo - subtotal;
                    total = valorParqueo;
                    RegistroParqueo.IdMotivo = Convert.ToInt32(cbMotivo.SelectedValue);
                    RegistroParqueo.Subtotal = subtotal;
                    RegistroParqueo.Iva = iva;
                    RegistroParqueo.Total = total;
                    RegistroParqueo.IdTransaccion = Convert.ToInt64(_IdTransaccion);
                    RegistroParqueo.Observacion = tbObservacion.Text.Trim();
                    RegistroParqueo.Placa = tbPlaca.Text.Trim();
                    listaDatos.Add(RegistroParqueo);
                }



                Int64 valorTarjeta = Convert.ToInt64(tbTarjeta.Text.Replace("$", "").Replace(".", ""));

                if (valorTarjeta > 500)
                {
                    RegistroTarjeta.IdTipoPago = 3;
                    double subtotal;
                    double iva;
                    double total;
                    subtotal = Math.Round(valorTarjeta / 1.19, 0);
                    iva = valorTarjeta - subtotal;
                    total = valorTarjeta;
                    RegistroTarjeta.IdMotivo = Convert.ToInt32(cbMotivo.SelectedValue);
                    RegistroTarjeta.Subtotal = subtotal;
                    RegistroTarjeta.Iva = iva;
                    RegistroTarjeta.Total = total;
                    RegistroTarjeta.IdTransaccion = Convert.ToInt64(_IdTransaccion);
                    RegistroTarjeta.Observacion = tbObservacion.Text.Trim();
                    RegistroTarjeta.Placa = tbPlaca.Text.Trim();
                    listaDatos.Add(RegistroTarjeta);
                }



                InfoPagoRegistroManualResponse[] arrayDatos = listaDatos.ToArray();
                DialogResult result3 = MessageBox.Show("¿Requiere factura electrónica?", "PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    FacturaElectronica popPup = new FacturaElectronica();
                    popPup.ShowDialog();
                    if (popPup.DialogResult == DialogResult.OK)
                    {
                        if (popPup.SolicitudFacturaElectronica == true)
                        {
                            if (popPup.Nit != 0)
                            {
                                if (Convert.ToInt64(tbTotal.Text.Replace("$", "").Replace(".", "")) > 0)
                                {
                                    _NitCliente = popPup.Nit.ToString();
                                }

                            }
                            else
                            {
                                _NitCliente = "222222222";
                            }
                        }
                        else
                        {
                            _NitCliente = "222222222";
                        }
                    }
                    else
                    {
                        _NitCliente = "222222222";

                    }
                }

                else
                {
                    _NitCliente = "222222222";

                }

                string fechaPago = DateTime.Now.ToString();

                InfoPagoRegistroManualResponse oInfoPago = cliente.RegistrarPagoRegistroManual(IdEstacionamiento, IdModulo, _DocumentoUsuario, valorTotal.ToString().Trim(), _IdTransaccion.ToString(), fechaPago, 10, Convert.ToInt32(_NitCliente), tbPlaca.Text.Trim(), idtipoVehiculo, fechaEntrada, arrayDatos);

                if (oInfoPago.Exito)
                {
                    //ImprimirPagoNormal(_IdTransaccion.ToString());
                    ok = true;
                }
                else
                {
                    ok = false;

                }
            }
            catch (Exception)
            {

                throw;
                ok = false;
            }
            return ok;

        }

        private void CalcularTotal()
        {
            try
            {
                Int64 pagarParqueo = Convert.ToInt64(tbParqueo.Text.Replace("$", "").Replace(".", ""));
                Int64 pagarTarjeta = Convert.ToInt64(tbTarjeta.Text.Replace("$", "").Replace(".", ""));
                Int64 pagarMensualidad = Convert.ToInt64(tbMensualidad.Text.Replace("$", "").Replace(".", ""));

                Int64 sumaTotal = pagarParqueo + pagarTarjeta + pagarMensualidad; // Agrega más valores si es necesario

                tbTotal.Text = "$" + string.Format("{0:#,##0.##}", sumaTotal);
            }
            catch (Exception exe)
            {

            }
        }

        public void Limpiar()
        {
            tbTotal.Text = "$0";
            tbTarjeta.Text = "$0";
            tbMensualidad.Text = "$0";
            tbParqueo.Text = "$0";
            rdoCarro.Checked = false;
            rdoMoto.Checked = false;
            tbObservacion.Text = "";
            tbPlaca.Text = "";
        }

        #endregion

        #region Impresión
        private void ImprimirPagoNormal(string idTransaccion)
        {
            InfoFacturaResponse oInfoFacturaResponse = cliente.ObtenerDatosFactura(idTransaccion);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicket(oInfoFacturaResponse.LstItems.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool PrintTicket(List<InfoItemsFacturaResponse> datos)
        {
            bool bPrint = false;

            try
            {

                List<List<InfoItemsFacturaResponse>> facturas = new List<List<InfoItemsFacturaResponse>>();
                foreach (InfoItemsFacturaResponse item in datos)
                {
                    bool find = false;
                    if (facturas.Count > 0)
                    {
                        foreach (List<InfoItemsFacturaResponse> item2 in facturas)
                        {
                            if (item2[0].NumeroFactura == item.NumeroFactura)
                            {
                                find = true;
                                item2.Add(item);
                            }
                        }

                        if (!find)
                        {
                            List<InfoItemsFacturaResponse> otraFactura = new List<InfoItemsFacturaResponse>();
                            otraFactura.Add(item);
                            facturas.Add(otraFactura);
                        }
                        find = false;
                    }
                    else
                    {
                        List<InfoItemsFacturaResponse> primeraFactura = new List<InfoItemsFacturaResponse>();
                        primeraFactura.Add(item);
                        facturas.Add(primeraFactura);
                    }
                }



                if (facturas.Count > 0)
                {
                    foreach (var item in facturas)
                    {
                        ReportDataSource datasource = new ReportDataSource();
                        LocalReport oLocalReport = new LocalReport();

                        datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPago(item).Tables[0]);
                        oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketPago"));
                        ReportParameter urlImage = new ReportParameter("codigoQR", new Uri(Convert.ToString(_imgQR)).AbsoluteUri);
                        oLocalReport.EnableExternalImages = true;
                        oLocalReport.SetParameters(new ReportParameter[] { urlImage });
                        oLocalReport.DataSources.Add(datasource);
                        oLocalReport.Refresh();

                        ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                        ore.PrintController = new StandardPrintController();
                        ore.Print();

                        oLocalReport.Dispose();
                        oLocalReport = null;
                        ore.Dispose();
                        ore = null;
                    }
                }





                bPrint = true;
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }

        private DataSetTicketPago GenerarTicketPago(List<InfoItemsFacturaResponse> infoTicket)
        {
            DataSetTicketPago facturacion = new DataSetTicketPago();

            double total = 0;
            double subtotal = 0;
            double iva = 0;
            foreach (var item in infoTicket)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                subtotal += Convert.ToDouble(item.Subtotal);
            }
            foreach (var item in infoTicket)
            {
                iva += Convert.ToDouble(item.Iva);
            }
            foreach (var item in infoTicket)
            {
                DataSetTicketPago.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                //rowDatosFactura.Cambio = Convert.ToDouble(item.Cambio);
                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha.ToString();
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Placa = item.Placa;
                //rowDatosFactura.Recibido = Convert.ToDouble(item.ValorRecibido);
                rowDatosFactura.Resolucion = item.NumeroResolucion;
                rowDatosFactura.Rut = "NIT 900.554.696 -8";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.Fecha2 = item.FechaEntrada.ToString();
                rowDatosFactura.Vehiculo = item.TipoVehiculo;
                //rowDatosFactura.VigenciaFactura = item.Vigencia;

                rowDatosFactura.DocCliente = _NitCliente.ToString();

                InfoClienteFacturaElectronicaResponse oInfoCliente = cliente.ValidarClientePorNit(Convert.ToInt32(_NitCliente));
                if (oInfoCliente.Exito)
                {
                    rowDatosFactura.RazonSocial = oInfoCliente.Nombre;
                }

                string emailIdentificacion = cliente.ObtenerValorParametroxNombre("EmiIdentificacion", IdEstacionamiento);
                string softwarePin = cliente.ObtenerValorParametroxNombre("SoftwarePin", IdEstacionamiento);
                string claveTecnica = cliente.ObtenerValorParametroxNombre("ClaveTecnica", IdEstacionamiento);
                string cufeCalculado = CalcularCUDE(item.NumeroFactura.Replace("-", ""), Convert.ToDateTime(item.Fecha).ToString("yyyy-MM-dd"), Convert.ToDateTime(item.Fecha).ToString("HH:mm:ss") + "-05:00", Convert.ToDecimal(subtotal), Convert.ToDecimal(0), Convert.ToDecimal(0), Convert.ToDecimal(0), Convert.ToDecimal(total), emailIdentificacion, _NitCliente.ToString(), claveTecnica, "1");
                rowDatosFactura.CUFE = cufeCalculado;

                #region QR
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(_imgQR + cufeCalculado, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrCode.GetGraphic(50);
                string nombreQR = _IdTransaccion + ".png";
                string directorio = System.IO.Path.GetDirectoryName(ConfigurationManager.AppSettings["RutaCodigoQR"]);
                if (!System.IO.Directory.Exists(directorio))
                {
                    System.IO.Directory.CreateDirectory(directorio);
                }
                string nombreQrFinal = directorio + "\\" + nombreQR;
                qrCodeImage.Save(nombreQrFinal, System.Drawing.Imaging.ImageFormat.Png);
                _imgQR = nombreQrFinal;
                qrCodeImage.Dispose();


                #endregion

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }
        #endregion

        public class RegistroDatos
        {
            private string _Observacion = string.Empty;
            public string Observacion
            {
                get { return _Observacion; }
                set { _Observacion = value; }
            }
            private int _IdMotivo = 0;
            public int IdMotivo
            {
                get { return _IdMotivo; }
                set { _IdMotivo = value; }
            }
            private string _FechaYHora = string.Empty;
            public string FechaYHora
            {
                get { return _FechaYHora; }
                set { _FechaYHora = value; }
            }
            private int _IdTipoVehiculo = 0;
            public int IdTipoVehiculo
            {
                get { return _IdTipoVehiculo; }
                set { _IdTipoVehiculo = value; }
            }
            private int _ValorParqueo = 0;
            public int ValorParqueo
            {
                get { return _ValorParqueo; }
                set { _ValorParqueo = value; }
            }
            private int _ValorTarjeta = 0;
            public int ValorTarjeta
            {
                get { return _ValorTarjeta; }
                set { _ValorTarjeta = value; }
            }
            private int _ValorMensualidad = 0;
            public int ValorMensualidad
            {
                get { return _ValorMensualidad; }
                set { _ValorMensualidad = value; }
            }
            private long _IdTransaccion = 0;
            public long IdTransaccion
            {
                get { return _IdTransaccion; }
                set { _IdTransaccion = value; }
            }

            private int _IdTipoPago = 0;
            public int IdTipoPago
            {
                get { return _IdTipoPago; }
                set { _IdTipoPago = value; }
            }

            private string _IdModulo = string.Empty;
            public string IdModulo
            {
                get { return _IdModulo; }
                set { _IdModulo = value; }
            }
            private double _Iva = 0;
            public double Iva
            {
                get { return _Iva; }
                set { _Iva = value; }
            }
            private double _Total = 0;
            public double Total
            {
                get { return _Total; }
                set { _Total = value; }
            }
            private double _Subtotal = 0;
            public double Subtotal
            {
                get { return _Subtotal; }
                set { _Subtotal = value; }
            }

        }

        private void tbHoraEntrada_TextChanged(object sender, EventArgs e)
        {
           
            
        }
    }


    
}
