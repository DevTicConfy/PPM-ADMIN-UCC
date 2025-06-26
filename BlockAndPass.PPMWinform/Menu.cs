using BlockAndPass.PPMWinform.ByPServices;
using BlockAndPass.PPMWinform.Tickets;
using EGlobalT.Device.SmartCard;
using EGlobalT.Device.SmartCardReaders;
using EGlobalT.Device.SmartCardReaders.Entities;
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
    public partial class Menu : Form
    {
        #region Definiciones


        private string _imgQR = string.Empty;
        public string imgQR
        {
            get { return _imgQR; }
            set { _imgQR = value; }
        }



        private bool _PagoEfectivo = true;
        public bool PagoEfectivo
        {
            get { return _PagoEfectivo; }
            set { _PagoEfectivo = value; }
        }

        private int _FormaPago = 0;
        public int FormaPago
        {
            get { return _FormaPago; }
            set { _FormaPago = value; }
        }
        private bool isButtonPressed = false;
        private string _DocumentoUsuario = string.Empty;
        public string DocumentoUsuario
        {
            get { return _DocumentoUsuario; }
            set { _DocumentoUsuario = value; }
        }

        private string _IdTransaccion = string.Empty;
        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        private string _InfoIdTransaccion = string.Empty;
        public string InfoIdTransaccion
        {
            get { return _InfoIdTransaccion; }
            set { _InfoIdTransaccion = value; }
        }

        private string _CargoUsuario = string.Empty;
        public string CargoUsuario
        {
            get { return _CargoUsuario; }
            set { _CargoUsuario = value; }
        }

        private int _IdSede = 0;
        public int IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
        private int _IdEstacionamiento = 0;
        public int IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        private int _IdTipoVehiculo = 0;
        public int IdTipoVehiculo
        {
            get { return _IdTipoVehiculo; }
            set { _IdTipoVehiculo = value; }
        }

        private int _IdCarrilEntrada = 0;
        public int IdCarrilEntrada
        {
            get { return _IdCarrilEntrada; }
            set { _IdCarrilEntrada = value; }
        }

        private string _IdTarjeta = string.Empty;

        private int _IdAutorizacion = 0;
        public int IdAutorizacion
        {
            get { return _IdAutorizacion; }
            set { _IdAutorizacion = value; }
        }

        private string _NiCliente = string.Empty;
        public string NiCliente
        {
            get { return _NiCliente; }
            set { _NiCliente = value; }
        }

        private string _IdModuloEntrada = string.Empty;
        public string IdModuloEntrada
        {
            get { return _IdModuloEntrada; }
            set { _IdModuloEntrada = value; }
        }

        private string _imgUrl = string.Empty;
        public string imgUrl
        {
            get { return _imgUrl; }
            set { _imgUrl = value; }
        }

        private bool _FacturaElectronica = false;
        public bool FacturaElectronica
        {
            get { return _FacturaElectronica; }
            set { _FacturaElectronica = value; }
        }

        string documentoUsuario = string.Empty;
        string nombresUsuario = string.Empty;
        ServicesByP cliente = new ServicesByP();
        LiquidacionService liquidacion = new LiquidacionService();
        System.Windows.Forms.Timer tmrHora = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer clickTimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tmrTimeOutPago = new System.Windows.Forms.Timer();
        CardResponse oCardResponse = new CardResponse();
        int cnt = 0;
        string rta = string.Empty;
        string moduloEntrada = string.Empty;

        #endregion

        public Menu(string sDocumento, string sCargo, InfoPPMService oInfoPPMService)
        {
            InitializeComponent();

            _DocumentoUsuario = sDocumento;
            _CargoUsuario = sCargo;

            tmrHora.Start();
            tmrHora.Interval = 1000;
            tmrHora.Tick += tmrHora_Tick;

            tmrTimeOutPago.Interval = 1000;
            tmrTimeOutPago.Tick += tmrTimeOutPago_Tick;

            clickTimer = new System.Windows.Forms.Timer();
            clickTimer.Interval = 5000;
            clickTimer.Tick += (s, e) =>
            {
                if (txtPlacaBuscar.Text.Length > 0)
                {
                    tmrTimeOutPago.Stop();
                }
                else
                {
                    LimpiarDatosCobrar();
                    tmrTimeOutPago.Start();
                    clickTimer.Stop();
                }
            };

            //FUNCIONES

            CargaImagenes();
            CargarUsuario();


            //DATOS ESTACIONAMIENTOS 
            var dataSource = new List<Object>();
            dataSource.Add(new { Name = oInfoPPMService.Sede, Value = oInfoPPMService.IdSede });

            //Setup data binding
            this.cbSede.DataSource = dataSource;
            this.cbSede.DisplayMember = "Name";
            this.cbSede.ValueMember = "Value";

            var dataSource2 = new List<Object>();
            dataSource2.Add(new { Name = oInfoPPMService.Estacionamiento, Value = oInfoPPMService.IdEstacionamiento });
            _IdEstacionamiento = Convert.ToInt32(oInfoPPMService.IdEstacionamiento.ToString());

            //Setup data binding
            this.cbEstacionamiento.DataSource = dataSource2;
            this.cbEstacionamiento.DisplayMember = "Name";
            this.cbEstacionamiento.ValueMember = "Value";

            var dataSource3 = new List<Object>();
            dataSource3.Add(new { Name = oInfoPPMService.Modulo, Value = oInfoPPMService.Modulo });

            //Setup data binding
            this.cbPPM.DataSource = dataSource3;
            this.cbPPM.DisplayMember = "Name";
            this.cbPPM.ValueMember = "Value";

        }

        private void Menu_Load(object sender, EventArgs e)
        {
            pnlTablaEventos.Visible = false;
            tabPrincipal.SelectedTab = tabMenuPrincipal;
            tabPrincipal.Appearance = TabAppearance.FlatButtons;
            tabPrincipal.ItemSize = new Size(0, 1);
            tabPrincipal.SizeMode = TabSizeMode.Fixed;
            ReestablecerBotonesLateralIzquierdo();
            ReestablecerBotonesLateralDerechoPrincipal();
            ReestablecerBotonInferior();
            btn_Principal.BackgroundImage = Image.FromFile(@"Media\Png\btn_PrincipalPresionado.png");
            if (_CargoUsuario == "AUXILIAR DE SERVICIOS" || _CargoUsuario == "ENCARGADO")
            {
                btn_SaldoEnLinea.Visible = false;
                btn_ReportePatios.Visible = true;
                btn_Mensualidades.Visible = false;
                btn_Cortesia.Visible = true;


            }
            else if (_CargoUsuario == "CONTROL INTERNO")
            {

            }

        }

        #region Eventos
        void tmrHora_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss tt");
            lblFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Update();
            lblFecha.Update();


            lblTotalCarrosPrincipal.Text = Convert.ToString(ObtenerCantidadVehiculosActuales());
            lblTotalCarrosPrincipal.Update();
            lblTotalMotosPrincipal.Text = Convert.ToString(ObtenerCantidadMotosActuales());
            lblTotalMotosPrincipal.Update();

            dtpFechaIngreso.Value = DateTime.Now;
        }
        void tmrTimeOutPago_Tick(object sender, EventArgs e)
        {

            ConsultarDatosCobrar();

        }
        private void tbPlaca_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
                tbPlaca.Focus();
            }
            if (e.KeyChar == (char)13)
            {
                btn_ConfirmaIngreso_Click(btn_ConfirmaIngreso, EventArgs.Empty);
            }
        }
        private void tbRecibidoCobrar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbRecibidoCobrar.Text.Length >= 3)
                {
                    tmrTimeOutPago.Stop();

                    //MessageBox.Show(tbCambio.Text);
                    Int64 pagar = Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", ""));
                    //Int64 recibido = Convert.ToInt64(tbRecibido.Text.Replace("$", ""));
                    Int64 cambio = Convert.ToInt64(tbCambioCobrar.Text.Replace("$", "").Replace(".", ""));

                    Int64 recibido = 0;
                    try
                    {
                        if (Int64.TryParse(tbRecibidoCobrar.Text.Replace("$", "").Replace(".", ""), out recibido))
                        {
                            tbRecibidoCobrar.Text = "$" + string.Format("{0:#,##0.##}", recibido);

                            if (recibido > pagar)
                            {
                                //MessageBox.Show(recibido + " " + pagar);
                                tbCambioCobrar.Text = "$" + string.Format("{0:#,##0.##}", (recibido - pagar));
                            }
                            else
                            {
                                tbCambioCobrar.Text = "$0";
                            }
                        }
                        else
                        {
                            tbCambioCobrar.Text = string.Empty;
                        }
                    }
                    catch (Exception exe)
                    {
                        //MessageBox.Show(exe.InnerException.ToString() + " " + exe.Message + " " + );
                    }

                    tbRecibidoCobrar.SelectionStart = tbRecibidoCobrar.Text.Length; // add some logic if length is 0
                    tbRecibidoCobrar.SelectionLength = 0;
                }
                else
                {
                    tmrTimeOutPago.Start();
                    //MessageBox.Show(tbCambio.Text);
                    Int64 pagar = Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", ""));
                    //Int64 recibido = Convert.ToInt64(tbRecibido.Text.Replace("$", ""));
                    Int64 cambio = Convert.ToInt64(tbCambioCobrar.Text.Replace("$", "").Replace(".", ""));

                    Int64 recibido = 0;
                    try
                    {
                        if (Int64.TryParse(tbRecibidoCobrar.Text.Replace("$", "").Replace(".", ""), out recibido))
                        {
                            tbRecibidoCobrar.Text = "$" + string.Format("{0:#,##0.##}", recibido);

                            if (recibido > pagar)
                            {
                                //MessageBox.Show(recibido + " " + pagar);
                                tbCambioCobrar.Text = "$" + string.Format("{0:#,##0.##}", (recibido - pagar));
                            }
                            else
                            {
                                tbCambioCobrar.Text = "$0";
                            }
                        }
                        else
                        {
                            tbCambioCobrar.Text = string.Empty;
                        }
                    }
                    catch (Exception exe)
                    {
                        //MessageBox.Show(exe.InnerException.ToString() + " " + exe.Message + " " + );
                    }

                    tbRecibidoCobrar.SelectionStart = tbRecibidoCobrar.Text.Length; // add some logic if length is 0
                    tbRecibidoCobrar.SelectionLength = 0;
                }


            }
            catch (Exception EX)
            {


            }

        }
        private void txtPlacaBuscar_Click(object sender, EventArgs e)
        {
            tmrTimeOutPago.Stop();
            clickTimer.Start();
        }
        private void txtPlacaBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (ckMensualidadDocumento.Checked == true && txtPlacaBuscar.Text != string.Empty)
                {
                    string sPlacaAuto = txtPlacaBuscar.Text.Trim();

                    AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoxPlaca(sPlacaAuto);

                    if (oInfoAuto.Exito)
                    {
                        liquidacion = cliente.ConsultarValorPagar(true, false, 1, oInfoAuto.IdTarjeta, oInfoAuto.IdTarjeta);
                        double sumTotalPagar = 0;

                        foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                        {
                            sumTotalPagar += item.Total;
                        }

                        if (sumTotalPagar > 0)
                        {
                            tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                            tbCambioCobrar.Text = "$0";
                            tbRecibidoCobrar.Text = "$0";

                            tbValorAPagarCobrar.Update();
                            tbCambioCobrar.Update();
                            tbRecibidoCobrar.Update();



                            tbNombreAutorizadoCobrar.Text = oInfoAuto.NombresApelldidos;
                            tbNombreAutorizadoCobrar.Update();
                        }

                    }
                    else
                    {
                        MessageBox.Show("Ocurrió un error el momento de consultar el valor a pagar.", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }

            }

        }
        private void btn_Cascos_Click(object sender, EventArgs e)
        {
            if (txtPlacaBuscar.Text != string.Empty)
            {
                if (oCardResponse.idCard != string.Empty)
                {
                    tmrTimeOutPago.Stop();
                    ReestablecerBotonesLateralDerechoEntradas();
                    btn_Cascos.BackgroundImage = Image.FromFile(@"Media\Png\btn_CascosPresionado.png");

                    CasilleroCasco popup = new CasilleroCasco(cbEstacionamiento.SelectedValue.ToString());
                    popup.ShowDialog();

                    if (popup.DialogResult == DialogResult.OK)
                    {
                        MessageBox.Show("Tarifa casco creada con EXITO", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ReestablecerBotonesLateralDerechoCobro();
                        tmrTimeOutPago.Start();
                    }
                    else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        ReestablecerBotonesLateralDerechoCobro();
                        tmrTimeOutPago.Start();
                    }
                    else
                    {
                        ReestablecerBotonesLateralDerechoCobro();
                        tmrTimeOutPago.Start();
                    }
                }
                else
                {
                    ReestablecerBotonesLateralDerechoCobro();
                }
            }
            else
            {
                ReestablecerBotonesLateralDerechoCobro();
            }
        }
        private void ckMensualidadDocumento_Click(object sender, EventArgs e)
        {
            if (ckMensualidadDocumento.Checked)
            {
                ckMensualidadDocumento.Checked = true;
                tmrTimeOutPago.Stop();
                clickTimer.Start();
            }
            else
            {
                ckMensualidadDocumento.Checked = false;
            }
        }
        private void tbRecibidoCobrar_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b' && e.KeyChar != '.' && e.KeyChar != '-' && e.KeyChar != (char)13)
                {
                    e.Handled = true;

                    MessageBox.Show("Error al intentar procesar el dato ingresado", "Error Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btn_Entrada_Click(btn_Entrada, EventArgs.Empty);
                }
                else
                {
                    if (e.KeyChar == (char)13)
                    {
                        if (Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", "")) > 0)
                        {
                            btn_ConfirmarCobro_Click(btn_ConfirmarCobro, EventArgs.Empty);

                        }
                    }
                }

            }
            catch (Exception)
            {

            }


        }
        #endregion

        #region Funciones
        public int ObtenerCantidadVehiculosActuales()
        {
            int totalCarros = 0;
            InfoCantidadVehiculosActualesResponse cantidad = cliente.ObtenerCantidadVehiculosActuales();

            totalCarros = cantidad.Cantidad;
            return totalCarros;

        }
        public int ObtenerCantidadMotosActuales()
        {
            int totalMotos = 0;
            InfoCantidadMotosActualesResponse cantidad = cliente.ObtenerCantidadMotosActuales();

            totalMotos = cantidad.Cantidad;
            return totalMotos;

        }
        public bool CargaImagenes()
        {
            bool ok = false;
            try
            {
                Imagen_Logo.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\Logo.png"));
                Imagen_Principal.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\Principal.png"));
                Imagen_Principal.BackgroundImageLayout = ImageLayout.Stretch;
                Imagen_FondoCobrar.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\ImagenFondoCobrar.png"));
                Imagen_FondoEntrada.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\ImagenFondoIngreso.png"));

                //Botones


            }
            catch (Exception ex)
            {

                ok = false;
            }
            return ok;
        }
        private void CargarUsuario()
        {
            InfoUsuarioResponse response = cliente.ObtenerInformacionUsuario(_DocumentoUsuario);
            if (response.Exito)
            {
                tbUsuario.Text = response.Usuario;
                tbUsuario.Update();
                documentoUsuario = response.Documento;
                tbNombreUsuario.Text = response.Nombres;
                nombresUsuario = response.Usuario;
                tbNombreUsuario.Update();
            }
            else
            {
                tbUsuario.Text = "Usuario Desconocido";
            }
        }
        public void LimpiarDatosEntrada()
        {
            tbPlaca.Text = "";
            tbPlaca.Focus();
            tbNombreAutortizado.Text = "";
            chbAutorizado.Checked = false;
            btn_ConfirmaIngreso.BackgroundImage = Image.FromFile(@"Media\Png\btn_ConfirmarEntrada.png");
            btn_ConfirmaIngreso.Text = "";
            btn_ConfirmaIngreso.BackgroundImageLayout = ImageLayout.Stretch;
            _IdTipoVehiculo = 1;
            //tbPlaca.Enabled = true;
            tmrHora.Start();
        }
        public string ValidarIngreso()
        {
            try
            {
                CarrilEntradaXEntradaResponse oInfo = cliente.ObtenerListaCarrilEntradaxEstacionamiento(Convert.ToInt32(cbSede.SelectedValue), Convert.ToInt32(cbEstacionamiento.SelectedValue));
                if (!oInfo.Exito)
                {
                    rta = "No encontro entradas asociadas al estacionamiento id = " + Convert.ToInt32(cbEstacionamiento.SelectedValue) + "";
                }
                else
                {
                    var dataSource = new List<Object>();
                    foreach (var item in oInfo.LstCarrillesEntrada)
                    {
                        dataSource.Add(new { Name = item.Display, Value = item.Value });
                        if (item.Display == "ADM01")
                        {
                            moduloEntrada = item.Display;
                        }
                    }

                    //Setup data binding
                    this.cbEntrada.DataSource = dataSource;
                    this.cbEntrada.DisplayMember = "Name";
                    this.cbEntrada.ValueMember = "Value";

                    rta = "OK";
                }
            }
            catch (Exception ex)
            {

                rta = "Error " + ex.ToString();
            }

            return rta;


        }
        public void LimpiarDatosCobrar()
        {
            try
            {
                _IdTransaccion = "";
                InfoIdTransaccion = string.Empty;
                btn_ConfirmarCobro.BackgroundImage = Image.FromFile(@"Media\Png\btn_Confirmar.png");
                txtPlacaBuscar.Text = "";
                tbTipoVehiculoCobrar.Text = "";
                tbNombreAutorizadoCobrar.Text = "";
                tbConvenioCobrar.Text = "";
                tbCasilleroCobrar.Text = "";
                tbFechaIngresoCobrar.Text = "";
                tbModuloIngresoCobrar.Text = "";
                tbTiempoCobrar.Text = "";
                tbValorAPagarCobrar.Text = "";
                tbRecibidoCobrar.Text = "$0";
                tbCambioCobrar.Text = "$0";
                ckMensualidadDocumento.Checked = false;
                ckMensualidadDocumento.Enabled = true;
                tbCodigo.Text = "";
                tbCodigo.Focus();
                _FacturaElectronica = false;
                _NiCliente = string.Empty; 
                btn_Motos.Visible = false;
                btn_Carros.Visible = false;
                txtPlacaBuscar.Enabled = true;
                //TABLA EVENTOS
                pnlTablaEventos.Visible = false;
                dvgListadoEventos.DataSource = null;
                dvgListadoEventos.Rows.Clear();
                dvgListadoEventos.Visible = false;


                //Card
                oCardResponse.fechaPago = null;
                oCardResponse.fechEntrada = null;
                oCardResponse.idCard = null;
                oCardResponse.moduloEntrada = null;
                oCardResponse.moduloPago = null;
                oCardResponse.placa = null;
                oCardResponse.reposicion = false;
                oCardResponse.tipoTarjeta = null;
                oCardResponse.tipoVehiculo = null;
                oCardResponse.cortesia = false;
                oCardResponse.codeAutorizacion1 = 0;
                oCardResponse.cicloActivo = false;
            }
            catch (Exception EX)
            {

                throw EX;
            }


        }
        public void ConsultarDatosCobrar()
        {
            try
            {
                string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                if (clave != string.Empty)
                {
                    oCardResponse = GetCardInfo(clave);
                    if (!oCardResponse.error)
                    {
                        bool bContinuarLiquidacion = true;

                        if (oCardResponse.cicloActivo)
                        {
                            if (oCardResponse.tipoTarjeta == "AUTHORIZED_PARKING")
                            {
                                DialogResult result3 = MessageBox.Show("¿Desea crear una salida de autorizado? \n PRESIONE NO PARA CONTINUAR CON LA LIQUIDACION.", "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result3 == DialogResult.Yes)
                                {

                                    DialogResult result4 = MessageBox.Show("¿Esta seguro que desea crear la salida para la placa: " + oCardResponse.placa, "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                    if (result4 == DialogResult.Yes)
                                    {
                                        bContinuarLiquidacion = false;
                                        CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
                                        if (oCarrilxIdModuloResponse.Exito)
                                        {
                                            string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                                            CardResponse oCardResponseExit = new CardResponse();
                                            oCardResponseExit = ExitCardAutho(clave, oCardResponse.idCard);
                                            if (!oCardResponseExit.error)
                                            {
                                                CreaSalidaResponse resp = cliente.CrearSalida2(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.placa, sIdTransaccion, oCarrilxIdModuloResponse.Carril.ToString(), oCardResponse.moduloEntrada, oCardResponse.idCard);

                                                if (resp.Exito)
                                                {
                                                    MessageBox.Show("Salida creada con EXITO", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                }
                                                else
                                                {
                                                    this.DialogResult = DialogResult.None;
                                                    MessageBox.Show(resp.ErrorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(oCardResponseExit.errorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }

                                        }
                                        else
                                        {
                                            MessageBox.Show("No fue posible encontrar el carril asociado al modulo.", "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }

                            if (bContinuarLiquidacion)
                            {
                                CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
                                if (oCarrilxIdModuloResponse.Exito)
                                {
                                    string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                                    //tbIdTarjeta.Text = oCardResponse.idCard;
                                    InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                                    InfoTransaccionService lstInfo = cliente.ConsultarCascosxId(sIdTransaccion);

                                    if (oInfoTransaccionService.Exito)
                                    {

                                        if (oInfoTransaccionService.IdTipoVehiculo == 1)
                                        {
                                            btn_Carros.Visible = false;
                                            btn_Motos.Visible = true;
                                        }
                                        else
                                        {
                                            btn_Carros.Visible = true;
                                            btn_Motos.Visible = false;
                                        }

                                        ListarEventosAplicadosPorTransaccion(oInfoTransaccionService.IdTransaccion);

                                        if (oInfoTransaccionService.IdTransaccion != string.Empty)
                                        {
                                            _IdTransaccion = oInfoTransaccionService.IdTransaccion;
                                            //tbIdTransaccion.Text = oInfoTransaccionService.IdTransaccion;

                                            txtPlacaBuscar.Text = oInfoTransaccionService.PlacaEntrada;
                                            txtPlacaBuscar.Update();
                                            if (oCardResponse.codeAutorizacion1 != 0)
                                            {
                                                cliente.AplicarConvenios(oInfoTransaccionService.IdTransaccion, oCardResponse.codeAutorizacion1, oCardResponse.codeAutorizacion2, oCardResponse.codeAutorizacion3);
                                            }

                                            DateTime dtDespues = DateTime.Now;
                                            DateTime dtAntes = new DateTime();


                                            oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("a. m.", "a.m.");
                                            oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("p. m.", "p.m.");

                                            //try
                                            //{
                                            //    if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy hh':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                            //    {
                                            //        if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "d'/'MM'/'yyyy hh':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                            //        {
                                            //            if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy h':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                            //            {
                                            //                if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "d'/'MM'/'yyyy h':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                            //                {
                                            //                    if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy H':'mm':'ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                            //                    {
                                            //                        dtAntes = DateTime.ParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy HH':'mm':'ss", CultureInfo.CurrentCulture);
                                            //                    }
                                            //                }
                                            //            }
                                            //        }
                                            //    }
                                            //}
                                            //catch (Exception exe)
                                            //{
                                            //    MessageBox.Show("Tiempo de permanencia no disponible, continue con el pago e informe al desarrollador->" + exe.Message + " / " + oInfoTransaccionService.HoraTransaccion, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            //}

                                            DateTime? ntes = oCardResponse.fechEntrada;
                                            dtAntes = Convert.ToDateTime(ntes);


                                            tbFechaIngresoCobrar.Text = oInfoTransaccionService.FechaEntrada.ToString("dd'/'MM'/'yyyy HH':'mm':'ss");
                                            tbFechaIngresoCobrar.Update();
                                            tbModuloIngresoCobrar.Text = oInfoTransaccionService.ModuloEntrada.Trim();
                                            tbModuloIngresoCobrar.Update();

                                            if (oInfoTransaccionService.IdTipoVehiculo == 1)
                                            {
                                                tbTipoVehiculoCobrar.Text = "CARRO";
                                                tbTipoVehiculoCobrar.Update();
                                            }
                                            if (oInfoTransaccionService.IdTipoVehiculo == 2)
                                            {
                                                tbTipoVehiculoCobrar.Text = "MOTO";
                                                tbTipoVehiculoCobrar.Update();
                                            }

                                            if (oInfoTransaccionService.IdTipoVehiculo == 3)
                                            {
                                                tbTipoVehiculoCobrar.Text = "BICICLETA";
                                                tbTipoVehiculoCobrar.Update();
                                            }

                                            //tbHoraPago.Text = dtDespues.ToString("dd'/'MM'/'yyyy HH':'mm':'ss");

                                            if (lstInfo.LstTransac.Length == 1)
                                            {
                                                tbCasilleroCobrar.Text = lstInfo.LstTransac[0].Casillero;
                                            }
                                            else if (lstInfo.LstTransac.Length == 2)
                                            {
                                                tbCasilleroCobrar.Text = lstInfo.LstTransac[0].Casillero + " y " + lstInfo.LstTransac[1].Casillero;
                                            }
                                            else
                                            {
                                                tbCasilleroCobrar.Text = string.Empty;
                                            }

                                            TimeSpan ts = dtDespues - dtAntes;
                                            tbTiempoCobrar.Text = Convert.ToInt32(ts.TotalMinutes).ToString() + " minutos";
                                            tbTiempoCobrar.Update();



                                            liquidacion = cliente.ConsultarValorPagar(false, oCardResponse.reposicion, oInfoTransaccionService.IdTipoVehiculo, oInfoTransaccionService.IdTransaccion, oCardResponse.idCard);
                                            double sumTotalPagar = 0;

                                            #region Estacionamiento

                                            if (liquidacion.Exito)
                                            {
                                                if (liquidacion.LstLiquidacion.Length > 0)
                                                {
                                                    foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                    {
                                                        sumTotalPagar += item.Total;
                                                    }

                                                    tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                    tbCambioCobrar.Text = "$0";
                                                    tbRecibidoCobrar.Text = "$0";

                                                    tbValorAPagarCobrar.Update();
                                                    tbCambioCobrar.Update();
                                                    tbRecibidoCobrar.Update();

                                                    tbRecibidoCobrar.Focus();
                                                    ckMensualidadDocumento.Enabled = false;
                                                    //panelPagar.Enabled = true;
                                                    txtPlacaBuscar.Enabled = false;

                                                    //Este me sirve para el evento de leer tarjeta
                                                    //tmrTimeOutPago.Start();

                                                    //chbEstacionamiento.Checked = true;
                                                    //chbMensualidad.Checked = false;

                                                    //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";

                                                }
                                                else
                                                {
                                                    //MessageBox.Show("No obtiene valor a pagar idTransaccion = " + oInfoTransaccionService.IdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", oCardResponse.idCard);
                                                    sumTotalPagar = 0;
                                                    if (liquidacion.Exito)
                                                    {
                                                        foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                        {
                                                            sumTotalPagar += item.Total;
                                                        }

                                                        tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                        tbCambioCobrar.Text = "$0";
                                                        tbRecibidoCobrar.Text = "$0";

                                                        tbValorAPagarCobrar.Update();
                                                        tbCambioCobrar.Update();
                                                        tbRecibidoCobrar.Update();
                                                        tbRecibidoCobrar.Focus();
                                                        txtPlacaBuscar.Enabled = false;

                                                        //panelPagar.Enabled = true;
                                                        //tmrTimeOutPago.Start();

                                                        //chbEstacionamiento.Checked = false;
                                                        //chbMensualidad.Checked = true;

                                                        //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    }
                                                }
                                            }

                                            #endregion

                                            #region Mensualidad

                                            else
                                            {
                                                //MessageBox.Show("No obtiene valor a pagar idTransaccion = " + oInfoTransaccionService.IdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", oCardResponse.idCard);
                                                sumTotalPagar = 0;
                                                if (liquidacion.Exito)
                                                {
                                                    foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                    {
                                                        sumTotalPagar += item.Total;
                                                    }

                                                    if (sumTotalPagar > 0)
                                                    {
                                                        tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                        tbCambioCobrar.Text = "$0";
                                                        tbRecibidoCobrar.Text = "$0";

                                                        tbValorAPagarCobrar.Update();
                                                        tbCambioCobrar.Update();
                                                        tbRecibidoCobrar.Update();
                                                        tbRecibidoCobrar.Focus();
                                                        txtPlacaBuscar.Enabled = false;

                                                        //Cargando(false);
                                                        //panelPagar.Enabled = true;

                                                        //chbEstacionamiento.Checked = false;
                                                        //chbMensualidad.Checked = true;

                                                        //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                                    }

                                                    #region Old
                                                    //else
                                                    //{
                                                    //    DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                                    //    if (result5 == DialogResult.Yes)
                                                    //    {
                                                    //        ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                                    //        if (resp.Exito)
                                                    //        {
                                                    //            MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            this.DialogResult = DialogResult.None;
                                                    //            MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    //        }
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        Cargando(false);
                                                    //    }
                                                    //    Cargando(false);

                                                    //}

                                                    #endregion
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Ocurrió un error el momento de consultar el valor a pagar.", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                            }

                                            #endregion

                                        }
                                    }
                                    #region Mensualidad
                                    else
                                    {
                                        //if (ckMensualidadDocumento.Checked == true)
                                        //{
                                        pnlTablaEventos.Visible = false;

                                        string idTarjeta = oCardResponse.idCard;

                                        AutorizadoxPlacaResponse oInfoAutorizado = cliente.BuscarAutorizadoPorIdTarjeta(idTarjeta);
                                        if (oInfoAutorizado.Exito)
                                        {
                                            tbNombreAutorizadoCobrar.Text = oInfoAutorizado.NombresApelldidos;
                                            tbNombreAutorizadoCobrar.Update();
                                        }

                                        //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", "0EEEC6CB");
                                        //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", oCardResponse.idCard);
                                        liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, idTarjeta, oCardResponse.idCard);

                                        double sumTotalPagar = 0;
                                        if (liquidacion.Exito)
                                        {
                                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                            {
                                                sumTotalPagar += item.Total;
                                            }

                                            if (sumTotalPagar > 0)
                                            {
                                                tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                tbCambioCobrar.Text = "$0";
                                                tbRecibidoCobrar.Text = "$0";

                                                tbValorAPagarCobrar.Update();
                                                tbCambioCobrar.Update();
                                                tbRecibidoCobrar.Update();
                                                txtPlacaBuscar.Enabled = false;

                                                //Cargando(false);
                                                //panelPagar.Enabled = true;
                                                //tmrTimeOutPago.Start();

                                                //chbEstacionamiento.Checked = false;
                                                //chbMensualidad.Checked = true;

                                                //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                            }
                                            else
                                            {
                                                tmrTimeOutPago.Stop();
                                                DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                                if (result5 == DialogResult.Yes)
                                                {
                                                    ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                                    if (resp.Exito)
                                                    {
                                                        MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    }
                                                    else
                                                    {
                                                        this.DialogResult = DialogResult.None;
                                                        MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    }

                                                    tmrTimeOutPago.Start();
                                                }
                                                else
                                                {
                                                    LimpiarDatosCobrar();
                                                    tmrTimeOutPago.Start();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        //}
                                    }
                                    #endregion


                                }
                            }
                        }
                        #region Mensualidad
                        else
                        {
                            pnlTablaEventos.Visible = false;

                            string idTarjeta = oCardResponse.idCard;

                            AutorizadoxPlacaResponse oInfoAutorizado = cliente.BuscarAutorizadoPorIdTarjeta(idTarjeta);
                            if (oInfoAutorizado.Exito)
                            {
                                tbNombreAutorizadoCobrar.Text = oInfoAutorizado.NombresApelldidos;
                                tbNombreAutorizadoCobrar.Update();
                            }

                            //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", "0EEEC6CB");
                            //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", oCardResponse.idCard);
                            liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, idTarjeta, oCardResponse.idCard);

                            double sumTotalPagar = 0;
                            if (liquidacion.Exito)
                            {
                                foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                {
                                    sumTotalPagar += item.Total;
                                }

                                if (sumTotalPagar > 0)
                                {
                                    tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                    tbCambioCobrar.Text = "$0";
                                    tbRecibidoCobrar.Text = "$0";

                                    tbValorAPagarCobrar.Update();
                                    tbCambioCobrar.Update();
                                    tbRecibidoCobrar.Update();
                                    txtPlacaBuscar.Enabled = false;

                                    //Cargando(false);
                                    //panelPagar.Enabled = true;
                                    //tmrTimeOutPago.Start();

                                    //chbEstacionamiento.Checked = false;
                                    //chbMensualidad.Checked = true;

                                    //lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                }
                                else
                                {
                                    tmrTimeOutPago.Stop();
                                    DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                    if (result5 == DialogResult.Yes)
                                    {
                                        ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                        if (resp.Exito)
                                        {
                                            MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                        else
                                        {
                                            this.DialogResult = DialogResult.None;
                                            MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }

                                        tmrTimeOutPago.Start();
                                    }
                                    else
                                    {
                                        LimpiarDatosCobrar();
                                        tmrTimeOutPago.Start();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        #endregion
                    }
                    else if (ckMensualidadDocumento.Checked == true && txtPlacaBuscar.Text != string.Empty)
                    {
                        string sPlacaAuto = txtPlacaBuscar.Text.Trim();

                        AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoxPlaca(sPlacaAuto);

                        if (oInfoAuto.Exito)
                        {
                            liquidacion = cliente.ConsultarValorPagar(true, false, 1, oInfoAuto.IdTarjeta, oInfoAuto.IdTarjeta);
                            double sumTotalPagar = 0;

                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                            {
                                sumTotalPagar += item.Total;
                            }

                            if (sumTotalPagar > 0)
                            {
                                tbValorAPagarCobrar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                tbCambioCobrar.Text = "$0";
                                tbRecibidoCobrar.Text = "$0";

                                tbValorAPagarCobrar.Update();
                                tbCambioCobrar.Update();
                                tbRecibidoCobrar.Update();



                                tbNombreAutorizadoCobrar.Text = oInfoAuto.NombresApelldidos;
                                tbNombreAutorizadoCobrar.Update();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Ocurrió un error el momento de consultar el valor a pagar.", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                    else
                    {
                        LimpiarDatosCobrar();
                    }
                }
                else
                {
                    LimpiarDatosCobrar();
                }



            }
            catch (Exception)
            {

            }


        }
        public void ListarEventosAplicadosPorTransaccion(string idTransaccionEvento)
        {

            try
            {
                dvgListadoEventos.DataSource = cliente.ListarEventosAplicadosPorTransaccion(idTransaccionEvento);
                if (dvgListadoEventos.Rows.Count <= 1)
                {
                    pnlTablaEventos.Visible = false;
                }
                else
                {
                    pnlTablaEventos.Visible = true;
                    dvgListadoEventos.Visible = true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void ConfirmarCobro()
        {
            try
            {
                if (!_FacturaElectronica)
                {
                    _NiCliente = cliente.ObtenerValorParametroxNombre("ConsumidorFinal", cbEstacionamiento.SelectedValue.ToString());
                }

                if (Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", "")) > 0)
                {
                    tmrTimeOutPago.Stop();

                    string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                    if (clave != string.Empty && oCardResponse.idCard.ToString() != "")
                    {
                        //AutorizadoxPlacaResponse oInfoAutorizado = cliente.BuscarAutorizadoPorIdTarjeta(oCardResponse.idCard.ToString());
                        //if (!oInfoAutorizado.Exito)
                        //{

                        if (oCardResponse.tipoTarjeta != "AUTHORIZED_PARKING" && oCardResponse.tipoTarjeta != null)
                        {
                            string pagosFinal = "";
                            double sumTotalPagar = 0;
                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                            {
                                if (pagosFinal == string.Empty)
                                {
                                }
                                else
                                {
                                    pagosFinal += ',';
                                }
                                sumTotalPagar += item.Total;
                                pagosFinal += item.Tipo + "-" + item.SubTotal + "-" + item.Iva + "-" + item.Total;
                            }

                            InfoPagoNormalService pagoNormal = cliente.PagarClienteParticular(pagosFinal, cbEstacionamiento.SelectedValue.ToString(), Convert.ToString(_IdTransaccion), cbPPM.SelectedValue.ToString(), DateTime.Now.ToString(), sumTotalPagar.ToString(), _DocumentoUsuario, _FormaPago, _NiCliente);

                            if (pagoNormal.Exito)
                            {
                                //DialogResult oDialogResult = MessageBox.Show("¿Desea imprimir la factura?", "Cobro PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                //if (oDialogResult == DialogResult.Yes)
                                //{
                                ImprimirPagoNormal(_IdTransaccion.ToString());
                                tmrTimeOutPago.Stop();
                                btn_Entrada_Click(btn_Entrada, EventArgs.Empty);
                                LimpiarDatosCobrar();

                                //}
                                oCardResponse = PayCard(clave, oCardResponse.idCard, cbPPM.SelectedValue.ToString(), DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), "dd'/'MM'/'yyyy HH':'mm':'ss", CultureInfo.CurrentCulture));
                                if (!oCardResponse.error)
                                {
                                    //LimpiarDatosCobrar();
                                    //tmrTimeOutPago.Start();
                                }
                                else
                                {
                                    MessageBox.Show(oCardResponse.errorMessage, "Error Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show(pagoNormal.ErrorMessage, "Error Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        //}
                        else if (ckMensualidadDocumento.Checked == true && txtPlacaBuscar.Text != string.Empty)
                        {
                            string sPlacaAuto = txtPlacaBuscar.Text.Trim();

                            AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoxPlaca(sPlacaAuto);

                            if (oInfoAuto.Exito)
                            {
                                //Mensualidad
                                string pagosFinal = "";
                                double sumTotalPagar = 0;
                                foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                {
                                    sumTotalPagar += item.Total;
                                    pagosFinal += item.Tipo + "-" + item.SubTotal + "-" + item.Iva + "-" + item.Total;
                                }

                                InfoPagoMensualidadService pagoNormal = cliente.PagarMensualidad(pagosFinal, cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), DateTime.Now.ToString(), sumTotalPagar.ToString(), oInfoAuto.IdTarjeta, _DocumentoUsuario, _FormaPago, _NiCliente);

                                if (pagoNormal.Exito)
                                {
                                    ////CardResponse oCardResponse = new CardResponse();
                                    //oCardResponse = LimpiarReposicion(clave);
                                    //if (!oCardResponse.error)
                                    //{
                                    //DialogResult oDialogResult = MessageBox.Show("¿Desea imprimir la factura?", "Cobro PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    //if (oDialogResult == DialogResult.Yes)
                                    //{
                                    ImprimirPagoMensualidad(pagoNormal.IdTranaccion, pagoNormal.IdAutorizacion);
                                    tmrTimeOutPago.Stop();
                                    btn_Entrada_Click(btn_Entrada, EventArgs.Empty);

                                    //}
                                    LimpiarDatosCobrar();
                                    //tmrTimeOutPago.Start();
                                    //}
                                }
                                else
                                {
                                    MessageBox.Show(pagoNormal.ErrorMessage, "Error Pagar Mensualidad PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No fué posible consultar los datos de la mensualidad", "Error Pagar Mensualidad PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }

                        }
                        else
                        {
                            //Mensualidad
                            oCardResponse = GetCardInfo(clave);

                            string pagosFinal = "";
                            double sumTotalPagar = 0;
                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                            {
                                sumTotalPagar += item.Total;
                                pagosFinal += item.Tipo + "-" + item.SubTotal + "-" + item.Iva + "-" + item.Total;
                            }

                            InfoPagoMensualidadService pagoNormal = cliente.PagarMensualidad(pagosFinal, cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), DateTime.Now.ToString(), sumTotalPagar.ToString(), oCardResponse.idCard, _DocumentoUsuario, _FormaPago, _NiCliente);

                            if (pagoNormal.Exito)
                            {
                                ////CardResponse oCardResponse = new CardResponse();
                                //oCardResponse = LimpiarReposicion(clave);
                                //if (!oCardResponse.error)
                                //{
                                //DialogResult oDialogResult = MessageBox.Show("¿Desea imprimir la factura?", "Cobro PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                //if (oDialogResult == DialogResult.Yes)
                                //{
                                ImprimirPagoMensualidad(pagoNormal.IdTranaccion, pagoNormal.IdAutorizacion);
                                tmrTimeOutPago.Stop();
                                btn_Entrada_Click(btn_Entrada, EventArgs.Empty);

                                //}
                                LimpiarDatosCobrar();
                                //tmrTimeOutPago.Start();
                                //}
                            }
                            else
                            {
                                MessageBox.Show(pagoNormal.ErrorMessage, "Error Pagar Mensualidad PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }

            }
            catch (Exception)
            {
                if (ckMensualidadDocumento.Checked == true && txtPlacaBuscar.Text != string.Empty)
                {
                    string sPlacaAuto = txtPlacaBuscar.Text.Trim();

                    AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoxPlaca(sPlacaAuto);

                    if (oInfoAuto.Exito)
                    {
                        //Mensualidad
                        string pagosFinal = "";
                        double sumTotalPagar = 0;
                        foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                        {
                            sumTotalPagar += item.Total;
                            pagosFinal += item.Tipo + "-" + item.SubTotal + "-" + item.Iva + "-" + item.Total;
                        }

                        InfoPagoMensualidadService pagoNormal = cliente.PagarMensualidad(pagosFinal, cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), DateTime.Now.ToString(), sumTotalPagar.ToString(), oInfoAuto.IdTarjeta, _DocumentoUsuario, _FormaPago, _NiCliente);

                        if (pagoNormal.Exito)
                        {
                            ////CardResponse oCardResponse = new CardResponse();
                            //oCardResponse = LimpiarReposicion(clave);
                            //if (!oCardResponse.error)
                            //{
                            //DialogResult oDialogResult = MessageBox.Show("¿Desea imprimir la factura?", "Cobro PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            //if (oDialogResult == DialogResult.Yes)
                            //{
                            ImprimirPagoMensualidad(pagoNormal.IdTranaccion, pagoNormal.IdAutorizacion);
                            tmrTimeOutPago.Stop();
                            btn_Entrada_Click(btn_Entrada, EventArgs.Empty);

                            //}
                            LimpiarDatosCobrar();
                            //tmrTimeOutPago.Start();
                            //}
                        }
                        else
                        {
                            MessageBox.Show(pagoNormal.ErrorMessage, "Error Pagar Mensualidad PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No fué posible consultar los datos de la mensualidad", "Error Pagar Mensualidad PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                }

            }
        }
        public CardResponse ReplaceCard(string clave, string placa, string modulo, string idTransaccion, string idTipoVehiculo, string password, string horaTransaccion)
        {
            CardResponse oCardResponse = new CardResponse();
            //string password = HttpContext.Current.Request.Params["password"];

            //string plate = HttpContext.Current.Request.Params["plate"];
            //string modulo = HttpContext.Current.Request.Params["modulo"];

            //string dia = HttpContext.Current.Request.Params["dia"];
            //string mes = HttpContext.Current.Request.Params["mes"];
            //string anho = HttpContext.Current.Request.Params["anho"];

            //string hora = HttpContext.Current.Request.Params["hora"];
            //string min = HttpContext.Current.Request.Params["min"];
            //string seg = HttpContext.Current.Request.Params["seg"];



            //string tipov = HttpContext.Current.Request.Params["tipov"];

            string ano = idTransaccion.Substring(0, 4);
            string mes = idTransaccion.Substring(4, 2);
            string dia = idTransaccion.Substring(6, 2);
            string hora = idTransaccion.Substring(8, 2);
            string min = idTransaccion.Substring(10, 2);
            string seg = idTransaccion.Substring(12, 2);
            //oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("a. m.", "a.m.");
            //oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("p. m.", "p.m.");

            //string HoraTransaccion = horaTransaccion.Replace("a. m.", "a.m.");
            //    HoraTransaccion = horaTransaccion.Replace("p. m.","p.m");
            //int horaFinal = 0;


            //if (HoraTransaccion=="p.m.")
            //{
            //    horaFinal = Convert.ToInt32(hora) + 12;
            //}
            //else
            //{
            //    horaFinal = Convert.ToInt32(hora);
            //}

            //if (Convert.ToInt32(hora) > 12)
            //{
            //    horaFinal = Convert.ToInt32(hora) + 12;
            //}
            //else
            //{
            //    horaFinal = Convert.ToInt32(hora);
            //}

            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {

                            Rspsta_BorrarTarjeta_LECTOR respBorrar = lectora.BorrarTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);

                            if (respBorrar.TarjetaBorrada)
                            {
                                SMARTCARD_PARKING_V1 oTarjeta = new SMARTCARD_PARKING_V1();
                                oTarjeta.TypeCard = TYPE_TARJETAPARKING_V1.VISITOR;
                                if (idTipoVehiculo == "1")
                                {
                                    oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE;
                                }
                                else if (idTipoVehiculo == "2")
                                {
                                    oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.MOTORCYCLE;
                                }
                                oTarjeta.Replacement = true;
                                oTarjeta.CodeCard = resp.CodigoTarjeta;
                                oTarjeta.ActiveCycle = true;

                                DateTime fecha = new DateTime();

                                try
                                {
                                    //fecha = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(mes), Convert.ToInt32(dia), horaFinal, Convert.ToInt32(min), Convert.ToInt32(seg));
                                    fecha = Convert.ToDateTime(horaTransaccion);
                                }
                                catch (Exception fex)
                                {
                                    // fecha = new DateTime(Convert.ToInt32(anho), Convert.ToInt32(dia), Convert.ToInt32(mes), horaFinal, Convert.ToInt32(min), 0);
                                    //fecha = new DateTime(Convert.ToInt32(ano), Convert.ToInt32(dia), Convert.ToInt32(mes), horaFinal, Convert.ToInt32(min), Convert.ToInt32(seg));
                                    fecha = Convert.ToDateTime(horaTransaccion);
                                }



                                oTarjeta.DateTimeEntrance = fecha;
                                oTarjeta.EntranceModule = modulo;
                                oTarjeta.EntrancePlate = placa;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(oTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                    oCardResponse.idCard = resp.CodigoTarjeta;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No borra tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse CreateEntry(string password, string plate, string modulo, DateTime dtFecha, string tipov)
        {
            dtFecha = new DateTime(dtFecha.Year, dtFecha.Month, dtFecha.Day, dtFecha.Hour, dtFecha.Minute, dtFecha.Second);
            CardResponse oCardResponse = new CardResponse();


            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_BorrarTarjeta_LECTOR respBorrar = lectora.BorrarTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);

                            if (respBorrar.TarjetaBorrada)
                            {
                                SMARTCARD_PARKING_V1 oTarjeta = new SMARTCARD_PARKING_V1();
                                oTarjeta.TypeCard = TYPE_TARJETAPARKING_V1.VISITOR;
                                if (tipov == "1")
                                {
                                    oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE;
                                }
                                else if (tipov == "2")
                                {
                                    oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.MOTORCYCLE;
                                }
                                oTarjeta.Replacement = false;
                                oTarjeta.CodeCard = resp.CodigoTarjeta;
                                oTarjeta.ActiveCycle = true;
                                oTarjeta.DateTimeEntrance = dtFecha;
                                oTarjeta.EntranceModule = modulo;
                                oTarjeta.EntrancePlate = plate;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(oTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.idCard = resp.CodigoTarjeta;
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No borra tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse CreateAuthEntry(string password, string plate, string modulo, DateTime dtFecha, string tipov, string sIdTarjeta)
        {
            dtFecha = new DateTime(dtFecha.Year, dtFecha.Month, dtFecha.Day, dtFecha.Hour, dtFecha.Minute, dtFecha.Second);
            CardResponse oCardResponse = new CardResponse();


            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;

                                if (!Convert.ToBoolean(myTarjeta.ActiveCycle))
                                {

                                    if (tipov == "1")
                                    {
                                        myTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE;
                                    }
                                    else if (tipov == "2")
                                    {
                                        myTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.MOTORCYCLE;
                                    }
                                    myTarjeta.Replacement = false;
                                    myTarjeta.CodeCard = sIdTarjeta;
                                    myTarjeta.ActiveCycle = true;
                                    myTarjeta.DateTimeEntrance = dtFecha;
                                    myTarjeta.EntranceModule = modulo;
                                    myTarjeta.EntrancePlate = plate;

                                    Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                    if (resp4.TarjetaEscrita)
                                    {
                                        oCardResponse.error = false;
                                    }
                                    else
                                    {
                                        oCardResponse.error = true;
                                        oCardResponse.errorMessage = "No escribe tarjeta.";
                                    }
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "Tarjeta con entrada registrada.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse GetCardInfo(string sPass)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        oCardResponse.idCard = resp.CodigoTarjeta;
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(sPass);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                oCardResponse.error = false;
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                oCardResponse.cicloActivo = myTarjeta.ActiveCycle != null ? (bool)myTarjeta.ActiveCycle : false;
                                oCardResponse.codeAutorizacion1 = myTarjeta.CodeAgreement1 != null ? (int)myTarjeta.CodeAgreement1 : 0;
                                oCardResponse.codeAutorizacion2 = myTarjeta.CodeAgreement2 != null ? (int)myTarjeta.CodeAgreement2 : 0;
                                oCardResponse.codeAutorizacion3 = myTarjeta.CodeAgreement3 != null ? (int)myTarjeta.CodeAgreement3 : 0;
                                oCardResponse.cortesia = myTarjeta.Courtesy != null ? (bool)myTarjeta.Courtesy : false;
                                oCardResponse.fechEntrada = myTarjeta.DateTimeEntrance;
                                oCardResponse.moduloEntrada = myTarjeta.EntranceModule;
                                oCardResponse.placa = myTarjeta.EntrancePlate;
                                oCardResponse.fechaPago = myTarjeta.PaymentDateTime.ToString();
                                oCardResponse.moduloPago = myTarjeta.PaymentModule;
                                oCardResponse.reposicion = myTarjeta.Replacement != null ? (bool)myTarjeta.Replacement : false;
                                oCardResponse.tipoTarjeta = myTarjeta.TypeCard.ToString();
                                oCardResponse.tipoVehiculo = myTarjeta.TypeVehicle.ToString();
                                oCardResponse.valet = myTarjeta.ValetParking != null ? (bool)myTarjeta.ValetParking : false;
                                //myTarjeta.

                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }

            return oCardResponse;
        }
        public CardResponse ExitCardAutho(string password, string idTarjeta)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();

            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                //Thread.Sleep(200);
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    //Thread.Sleep(200);
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        //Thread.Sleep(200);
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            //Thread.Sleep(200);
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                Thread.Sleep(2000);
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.Replacement = false;
                                myTarjeta.ActiveCycle = false;
                                myTarjeta.DateTimeEntrance = null;
                                myTarjeta.EntranceModule = string.Empty;
                                myTarjeta.EntrancePlate = string.Empty;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse PayCard(string password, string idTarjeta, string moduloPago, DateTime dtFechaPago)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();

            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                //Thread.Sleep(200);
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    //Thread.Sleep(200);
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        //Thread.Sleep(200);
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            //Thread.Sleep(200);
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                //Thread.Sleep(2000);
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.PaymentModule = moduloPago;
                                myTarjeta.PaymentDateTime = dtFechaPago;
                                //myTarjeta.CodeCard = idTarjeta;
                                myTarjeta.Replacement = false;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    //Thread.Sleep(2000);
                                    //Rspsta_Leer_Tarjeta_LECTOR respFinal = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                                    //if (respFinal.TarjetaLeida)
                                    //{
                                    oCardResponse.error = false;
                                    //    SMARTCARD_PARKING_V1 myTarjetaFinal = (SMARTCARD_PARKING_V1)respFinal.Tarjeta;
                                    //    oCardResponse.cicloActivo = myTarjetaFinal.ActiveCycle != null ? (bool)myTarjetaFinal.ActiveCycle : false;
                                    //    oCardResponse.codeAutorizacion1 = myTarjetaFinal.CodeAgreement1 != null ? (int)myTarjetaFinal.CodeAgreement1 : 0;
                                    //    oCardResponse.codeAutorizacion2 = myTarjetaFinal.CodeAgreement2 != null ? (int)myTarjetaFinal.CodeAgreement2 : 0;
                                    //    oCardResponse.codeAutorizacion3 = myTarjetaFinal.CodeAgreement3 != null ? (int)myTarjetaFinal.CodeAgreement3 : 0;
                                    //    oCardResponse.cortesia = myTarjetaFinal.Courtesy != null ? (bool)myTarjetaFinal.Courtesy : false;
                                    //    oCardResponse.fechEntrada = myTarjetaFinal.DateTimeEntrance;
                                    //    oCardResponse.moduloEntrada = myTarjetaFinal.EntranceModule;
                                    //    oCardResponse.placa = myTarjetaFinal.EntrancePlate;
                                    //    oCardResponse.fechaPago = myTarjetaFinal.PaymentDateTime.ToString();
                                    //    oCardResponse.moduloPago = myTarjetaFinal.PaymentModule;
                                    //    oCardResponse.reposicion = myTarjetaFinal.Replacement != null ? (bool)myTarjetaFinal.Replacement : false;
                                    //    oCardResponse.tipoTarjeta = myTarjetaFinal.TypeCard.ToString();
                                    //    oCardResponse.tipoVehiculo = myTarjetaFinal.TypeVehicle.ToString();
                                    //    oCardResponse.valet = myTarjetaFinal.ValetParking != null ? (bool)myTarjetaFinal.ValetParking : false;
                                    //}
                                    //else
                                    //{
                                    //    oCardResponse.error = true;
                                    //    oCardResponse.errorMessage = "No lee tarjeta.";
                                    //}
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse LimpiarReposicion(string password)
        {
            CardResponse oCardResponse = new CardResponse();

            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.Replacement = false;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse AplicarCortesia(string password)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.Courtesy = true;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }
        public CardResponse AplicarConvenio(string password, string idConvenio)
        {
            CardResponse oCardResponse = new CardResponse();

            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.CodeAgreement1 = Convert.ToInt32(idConvenio);
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }

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
decimal totalSinImpuestos, decimal totalImpuesto1, decimal totalImpuesto2, decimal totalImpuesto3, decimal totalConImpuestos,
string identificacionEmisor, string identificacionAdquiriente,
string SoftwarePin, string tipoAmbiente)
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
        #endregion

        #region FuncionesBotones
        public void ReestablecerBotonesLateralIzquierdo()
        {
            btn_Principal.BackgroundImage = Image.FromFile(@"Media\Png\btn_Principal.png");
            btn_Principal.Text = "";
            btn_Principal.BackgroundImageLayout = ImageLayout.Stretch;
            btn_Entrada.BackgroundImage = Image.FromFile(@"Media\Png\btn_Entrada.png");
            btn_Entrada.Text = "";
            btn_Entrada.BackgroundImageLayout = ImageLayout.Stretch;
            btn_Cobrar.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cobrar.png");
            btn_Cobrar.Text = "";
            btn_Cobrar.BackgroundImageLayout = ImageLayout.Stretch;

            //TABLA EVENTOS
            pnlTablaEventos.Visible = false;
            dvgListadoEventos.DataSource = null;
            dvgListadoEventos.Rows.Clear();


        }
        public void ReestablecerBotonesLateralDerechoPrincipal()
        {
            btn_Arqueo.BackgroundImage = Image.FromFile(@"Media\Png\btn_Arqueo.png");
            btn_Arqueo.Text = "";
            btn_Arqueo.BackgroundImageLayout = ImageLayout.Stretch;

            btn_SaldoEnLinea.BackgroundImage = Image.FromFile(@"Media\Png\btn_SaldoEnLinea.png");
            btn_SaldoEnLinea.Text = "";
            btn_SaldoEnLinea.BackgroundImageLayout = ImageLayout.Stretch;

            btn_FacturaContingencia.BackgroundImage = Image.FromFile(@"Media\Png\btn_FacturaContingencia.png");
            btn_FacturaContingencia.Text = "";
            btn_FacturaContingencia.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Mensualidades.BackgroundImage = Image.FromFile(@"Media\Png\btn_Mensualidades.png");
            btn_Mensualidades.Text = "";
            btn_Mensualidades.BackgroundImageLayout = ImageLayout.Stretch;

            btn_ReportePatios.BackgroundImage = Image.FromFile(@"Media\Png\btn_ReportePatios.png");
            btn_ReportePatios.Text = "";
            btn_ReportePatios.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Copia.BackgroundImage = Image.FromFile(@"Media\Png\btn_copia.png");
            btn_Copia.Text = "";
            btn_Copia.BackgroundImageLayout = ImageLayout.Stretch;

            btn_RegistroManual.BackgroundImage = Image.FromFile(@"Media\Png\btn_RegistroManual.png");
            btn_RegistroManual.Text = "";
            btn_RegistroManual.BackgroundImageLayout = ImageLayout.Stretch;

        }
        public void ReestablecerBotonesLateralDerechoCobro()
        {
            btn_Cortesia.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cortesia.png");
            btn_Cortesia.Text = "";
            btn_Cortesia.BackgroundImageLayout = ImageLayout.Stretch;

            btn_TarifasEspeciales.BackgroundImage = Image.FromFile(@"Media\Png\btn_TarifasEspeciales.png");
            btn_TarifasEspeciales.Text = "";
            btn_TarifasEspeciales.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Convenios.BackgroundImage = Image.FromFile(@"Media\Png\btn_Convenios.png");
            btn_Convenios.Text = "";
            btn_Convenios.BackgroundImageLayout = ImageLayout.Stretch;

            btn_FacturaElectronica.BackgroundImage = Image.FromFile(@"Media\Png\btn_FacturaElectronica.png");
            btn_FacturaElectronica.Text = "";
            btn_FacturaElectronica.BackgroundImageLayout = ImageLayout.Stretch;


            btn_Reposicion.BackgroundImage = Image.FromFile(@"Media\Png\btn_Reposicion.png");
            btn_Reposicion.Text = "";
            btn_Reposicion.BackgroundImageLayout = ImageLayout.Stretch;


            btn_Cascos.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cascos.png");
            btn_Cascos.Text = "";
            btn_Cascos.BackgroundImageLayout = ImageLayout.Stretch;

            btn_ConfirmarCobro.BackgroundImage = Image.FromFile(@"Media\Png\btn_Confirmar.png");
            btn_ConfirmarCobro.Text = "";
            btn_ConfirmarCobro.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Carros.BackgroundImage = Image.FromFile(@"Media\Png\btn_Carro.png");
            btn_Carros.Text = "";
            btn_Carros.BackgroundImageLayout = ImageLayout.Stretch;

            btn_Motos.BackgroundImage = Image.FromFile(@"Media\Png\btn_Moto.png");
            btn_Motos.Text = "";
            btn_Motos.BackgroundImageLayout = ImageLayout.Stretch;

            btn_RegistroManual.BackgroundImage = Image.FromFile(@"Media\Png\btn_RegistroManual.png");
            btn_RegistroManual.Text = "";
            btn_RegistroManual.BackgroundImageLayout = ImageLayout.Stretch;

            //btn_Otros_Cobrar.BackgroundImage = Image.FromFile(@"Media\Png\btn_Otro.png");
            //btn_Otros_Cobrar.Text = "";
            //btn_Otros_Cobrar.BackgroundImageLayout = ImageLayout.Stretch;

        }
        public void ReestablecerBotonesLateralDerechoEntradas()
        {



            //btn_Otros.BackgroundImage = Image.FromFile(@"Media\Png\btn_Otro.png");
            //btn_Otros.Text = "";
            //btn_Otros.BackgroundImageLayout = ImageLayout.Stretch;

            btn_ConfirmaIngreso.BackgroundImage = Image.FromFile(@"Media\Png\btn_ConfirmarEntrada.png");
            btn_ConfirmaIngreso.Text = "";
            btn_ConfirmaIngreso.BackgroundImageLayout = ImageLayout.Stretch;


        }
        public void ReestablecerBotonInferior()
        {
            btn_Cerrar.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cerrar.png");
            btn_Cerrar.Text = "";
            btn_Cerrar.BackgroundImageLayout = ImageLayout.Stretch;

            btn_CerrarPrincipal.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cerrar.png");
            btn_CerrarPrincipal.Text = "";
            btn_CerrarPrincipal.BackgroundImageLayout = ImageLayout.Stretch;

            btn_CerrarCobrar.BackgroundImage = Image.FromFile(@"Media\Png\btn_Cerrar.png");
            btn_CerrarCobrar.Text = "";
            btn_CerrarCobrar.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void btn_Arqueo_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_Arqueo.BackgroundImage = Image.FromFile(@"Media\Png\btn_ArqueoPresionado.png");

            DialogResult oDialogResult = MessageBox.Show("¿Esta seguro que desea realizar el arqueo?", "Arqueo PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (oDialogResult == DialogResult.Yes)
            {
                RegistrarArqueoResponse rgis = cliente.RegistrarElArqueo(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), _DocumentoUsuario);
                if (rgis.Exito)
                {
                    ArqueoPopUp popup = new ArqueoPopUp(rgis.IdArqueo.ToString());
                    popup.ShowDialog();
                    if (popup.DialogResult == DialogResult.OK)
                    {
                        ConfirmarArqueoResponse confirmacionArqueo = cliente.ConfirmarElArqueo(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), rgis.IdArqueo.ToString(), popup.Valor.ToString(), documentoUsuario);
                        if (confirmacionArqueo.Exito)
                        {
                            ImprimirArqueo(rgis.IdArqueo.ToString());
                            ReestablecerBotonesLateralDerechoPrincipal();
                            Login oLogin = new Login();
                            this.Hide();
                            oLogin.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show(confirmacionArqueo.ErrorMessage, "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            StringBuilder sb = new StringBuilder();
                            sb.Append(confirmacionArqueo.ErrorMessage);
                            File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt"), sb.ToString());
                            sb.Clear();
                        }
                    }
                    else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        ReestablecerBotonesLateralDerechoPrincipal();
                    }
                    else
                    {
                        MessageBox.Show("Error al procesar ventana carga", "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else
                {
                    MessageBox.Show(rgis.ErrorMessage, "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ReestablecerBotonesLateralDerechoPrincipal();

                }
            }
            else
            {
                ReestablecerBotonesLateralDerechoPrincipal();

            }
        }
        private void btn_SaldoEnLinea_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_SaldoEnLinea.BackgroundImage = Image.FromFile(@"Media\Png\btn_SaldoEnLineaPresionado.png");
            //tabPrincipal.SelectedTab = tabSaldoEnLinea;
            SaldoEnLinea popup = new SaldoEnLinea();
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.Cancel)
            {
                ReestablecerBotonesLateralDerechoPrincipal();
            }
        }
        private void btn_ReportePatios_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_ReportePatios.BackgroundImage = Image.FromFile(@"Media\Png\btn_ReportePatiosPresionado.png");
            //tabPrincipal.SelectedTab = tabReportePatios;

            ReporteDePatios popup = new ReporteDePatios();
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.Cancel)
            {
                ReestablecerBotonesLateralDerechoPrincipal();
            }
        }
        private void btn_FacturaContingencia_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_FacturaContingencia.BackgroundImage = Image.FromFile(@"Media\Png\btn_FacturaContingenciaPresionado.png");
            FacturaContingencia popup = new FacturaContingencia();
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {

                List<string> pagosFinal = new List<string>();

                #region Definiciones
                int horasCarro = 0;
                double subtotalHorasCarro = 0;
                double ivaHorasCarro = 0;
                double totalHorasCarro = 0;

                int horasMoto = 0;
                double subtotalHorasMoto = 0;
                double ivaHorasMoto = 0;
                double totalHorasMoto = 0;

                int reposiciones = 0;
                double subtotalReposiciones = 0;
                double ivaReposiciones = 0;
                double totalReposiciones = 0;

                int mensualidadesMoto = 0;
                double subtotalMensualidadesMoto = 0;
                double ivaMensualidadesMoto = 0;
                double totalMensualidadesMoto = 0;

                int mensualidadesCarro = 0;
                double subtotalMensualidadesCarro = 0;
                double ivaMensualidadesCarro = 0;
                double totalMensualidadesCarro = 0;

                int idTipoPago = 0;
                int idTipoVehiculo = 0;
                string nitCliente = cliente.ObtenerValorParametroxNombre("ConsumidorFinal", _IdEstacionamiento.ToString());
                string observaciones = string.Empty;

                #endregion

                horasCarro = popup.HorasCarro;
                horasMoto = popup.HorasMoto;
                reposiciones = popup.Tarjeta;
                mensualidadesCarro = popup.MensualidadesCarro;
                mensualidadesMoto = popup.MensualidadesMoto;
                observaciones = popup.Observaciones;
                // OPERACIONES HORAS CARRO
                subtotalHorasCarro = Math.Round(horasCarro / 1.19, 0);
                ivaHorasCarro = horasCarro - subtotalHorasCarro;
                totalHorasCarro = horasCarro;

                // OPERACIONES HORAS MOTO
                subtotalHorasMoto = Math.Round(horasMoto / 1.19, 0);
                ivaHorasMoto = horasMoto - subtotalHorasMoto;
                totalHorasMoto = horasMoto;

                // OPERACIONES MENSUALIDADES MOTO
                subtotalMensualidadesMoto = Math.Round(mensualidadesMoto / 1.19, 0);
                ivaMensualidadesMoto = mensualidadesMoto - subtotalMensualidadesMoto;
                totalMensualidadesMoto = mensualidadesMoto;

                // OPERACIONES MENSUALIDADES CARRO
                subtotalMensualidadesCarro = Math.Round(mensualidadesCarro / 1.19, 0);
                ivaMensualidadesCarro = mensualidadesCarro - subtotalMensualidadesCarro;
                totalMensualidadesCarro = mensualidadesCarro;

                // OPERACIONES REPOSICIONES              
                subtotalReposiciones = Math.Round(reposiciones / 1.19, 0);
                ivaReposiciones = reposiciones - subtotalReposiciones;
                totalReposiciones = reposiciones;


                if (horasCarro > 0)
                {
                    //string registro = "IdTipoPago: 1, IdTipoVehiculo: 1, Valor: " + horasCarro;
                    string registro = "1,1," + subtotalHorasCarro + "," + ivaHorasCarro + "," + horasCarro + "";
                    pagosFinal.Add(registro);
                }

                if (horasMoto > 0)
                {
                    string registro = "1,2," + subtotalHorasMoto + "," + ivaHorasMoto + "," + horasMoto + "";
                    pagosFinal.Add(registro);
                }
                if (reposiciones > 0)
                {
                    string registro = "3,1," + subtotalReposiciones + "," + ivaReposiciones + "," + reposiciones + "";
                    pagosFinal.Add(registro);
                }
                if (mensualidadesCarro > 0)
                {
                    string registro = "2,1," + subtotalMensualidadesCarro + "," + ivaMensualidadesCarro + "," + mensualidadesCarro + "";
                    pagosFinal.Add(registro);
                }

                if (mensualidadesMoto > 0)
                {
                    string registro = "2,2," + subtotalMensualidadesMoto + "," + ivaMensualidadesMoto + "," + mensualidadesMoto + "";
                    pagosFinal.Add(registro);
                }

                string[] arrayPagos = pagosFinal.ToArray();

                InfoPagoNormalService pagoNormalContingencia = cliente.PagarFacturasContingencia(arrayPagos, cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), ConfigurationManager.AppSettings["PrefijoFacContingencia"].ToString(), nitCliente, Convert.ToInt32(_DocumentoUsuario), observaciones);
                if (pagoNormalContingencia.Exito)
                {
                    ImprimirPagoContingencia(cbPPM.SelectedValue.ToString(), pagoNormalContingencia.NumeroFactura);
                    ReestablecerBotonesLateralDerechoPrincipal();
                }
                else
                {
                    MessageBox.Show(pagoNormalContingencia.ErrorMessage, "Error Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                ReestablecerBotonesLateralDerechoPrincipal();
            }
        }
        private void btn_Copia_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_Copia.BackgroundImage = Image.FromFile(@"Media\Png\btn_copiaPresionado.png");
            CopiaFactura popup = new CopiaFactura(Convert.ToString(_IdEstacionamiento));
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                ReestablecerBotonesLateralDerechoPrincipal();

            }
            else
            {
                ReestablecerBotonesLateralDerechoPrincipal();


            }
        }
        private void btn_Principal_Click(object sender, EventArgs e)
        {
            tmrTimeOutPago.Stop();
            tmrHora.Start();
            ReestablecerBotonesLateralDerechoPrincipal();
            ReestablecerBotonesLateralIzquierdo();


            btn_Principal.BackgroundImage = Image.FromFile(@"Media\Png\btn_PrincipalPresionado.png");
            tabPrincipal.SelectedTab = tabMenuPrincipal;


        }
        private void btn_Entrada_Click(object sender, EventArgs e)
        {
            tmrTimeOutPago.Stop();
            tmrHora.Stop();
            LimpiarDatosEntrada();
            ReestablecerBotonInferior();
            ReestablecerBotonesLateralIzquierdo();
            ReestablecerBotonesLateralDerechoEntradas();


            btn_Entrada.BackgroundImage = Image.FromFile(@"Media\Png\btn_EntradaPresionado.png");
            tabPrincipal.SelectedTab = tabEntrada;


            rta = ValidarIngreso();
            if (rta.Equals("OK"))
            {
                LimpiarDatosEntrada();
                tbModuloIngreso.Text = cbPPM.SelectedValue.ToString();
            }
            tmrTimeOutPago.Stop();


        }
        private void tbPlaca_TextChanged(object sender, EventArgs e)
        {
            string texto = tbPlaca.Text;

            try
            {
                if (texto.Length <= 6 && char.IsLetter(texto[texto.Length - 1]))
                {
                    tbTipoVehiculo.Text = "Moto";
                    _IdTipoVehiculo = 2;
                }

                else if (texto.Length >= 6 && char.IsDigit(texto[texto.Length - 1]))
                {
                    tbTipoVehiculo.Text = "Carro";
                    _IdTipoVehiculo = 1;
                }
            }
            catch (Exception ex)
            {
                tbTipoVehiculo.Text = "";
                LimpiarDatosEntrada();
                tbPlaca.Focus();

            }
        }
        private void btn_ConfirmaIngreso_Click(object sender, EventArgs e)
        {
            if (tbPlaca.Text != string.Empty || tbPlaca.Text != "")
            {

                string placaEntrada = tbPlaca.Text.Trim();
                AutorizadoxPlacaResponse oInfoAutorizado = cliente.BuscarAutorizadoxPlaca(placaEntrada);
                if (oInfoAutorizado.Exito)
                {
                    tbNombreAutortizado.Text = oInfoAutorizado.NombresApelldidos;
                    tbNombreAutortizado.Update();

                    VerificaVigenciaAutorizadoResponse resp = cliente.VerificarVigenciaAutorizado(oInfoAutorizado.IdTarjeta);
                    if (resp.Exito)
                    {
                        AutorizadoRecargaxPlacaResponse infoClienteRecarga = cliente.BuscarAutorizadoRecargaxPlaca(placaEntrada);
                        if (infoClienteRecarga.Exito)
                        {
                            if (infoClienteRecarga.IdPago == "3")
                            {

                                VerificaVigenciaAutorizadoRecargaResponse infoDiasDisponibles = cliente.VerificarVigenciaAutorizadoRecarga(infoClienteRecarga.IdTarjeta);
                                if (infoDiasDisponibles.CantidadDias > 0)
                                {
                                    //AUTORIZADO NORMAL
                                    VerificaTransaccionAbiertaAutorizadoResponse infoSalida = cliente.VerificarTransaccionAbiertaAutorizado(oInfoAutorizado.IdTarjeta);
                                    if (!infoSalida.Exito)
                                    {
                                        CreaEntradaRecargaResponse oInfoEntrada = cliente.CrearEntradaRecarga(_IdEstacionamiento.ToString(), oInfoAutorizado.IdTarjeta, moduloEntrada, placaEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), oInfoAutorizado.IdAutorizacion);
                                        if (oInfoEntrada.Exito)
                                        {
                                            MessageBox.Show("Bienvenido Sr/Sra " + oInfoAutorizado.NombresApelldidos + "", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            LimpiarDatosEntrada();
                                        }
                                    }
                                    else
                                    {
                                        DialogResult result = MessageBox.Show("La mensualidad ya se encuentra en uso con placa: " + infoSalida.PlacaEntrada + "\n\n Presione SI: Para registrar la salida\n\n Presione NO: Para registrar por horas", "Leer PPM", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                        if (result == DialogResult.Yes)
                                        {
                                            CreaSalidaResponse oInfoSalida = cliente.CrearSalida(_IdEstacionamiento.ToString(), placaEntrada, oInfoAutorizado.IdTarjeta);
                                            if (oInfoSalida.Exito)
                                            {
                                                MessageBox.Show("Salida creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                LimpiarDatosEntrada();
                                            }
                                            else
                                            {
                                                MessageBox.Show("Error al crear la salida", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                LimpiarDatosEntrada();
                                            }
                                        }
                                        else if (result == DialogResult.No)
                                        {
                                            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());

                                            CardResponse oCardResponse = CreateEntry(clave, tbPlaca.Text, moduloEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString());
                                            if (!oCardResponse.error)
                                            {
                                                AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoPorIdTarjeta(oCardResponse.idCard);
                                                if (oInfoAuto.Exito)
                                                {
                                                    MessageBox.Show("Por favor coloque una tarjeta de visitante", "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                                else
                                                {
                                                    CreaEntradaResponse oInfoEntrada = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oCardResponse.idCard, moduloEntrada, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), string.Empty);
                                                    if (oInfoEntrada.Exito)
                                                    {
                                                        MessageBox.Show("Entrada creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        LimpiarDatosEntrada();
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show(oInfoEntrada.ErrorMessage, "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        LimpiarDatosEntrada();
                                                    }
                                                }


                                            }
                                            else
                                            {
                                                MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                LimpiarDatosEntrada();
                                            }

                                        }
                                        else
                                        {
                                            LimpiarDatosEntrada();
                                        }

                                    }
                                }
                                else
                                {
                                    DialogResult result = MessageBox.Show("El autorizado tiene la mensualidad vencida \n Por favor coloque una tarjeta en la lectora", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    if (result == DialogResult.OK)
                                    {
                                        string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());

                                        CardResponse oCardResponse = CreateEntry(clave, tbPlaca.Text, moduloEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString());
                                        if (!oCardResponse.error)
                                        {
                                            AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoPorIdTarjeta(oCardResponse.idCard);
                                            if (oInfoAuto.Exito)
                                            {
                                                MessageBox.Show("Por favor coloque una tarjeta de visitante", "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            else
                                            {
                                                CreaEntradaResponse oInfoEntrada = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oCardResponse.idCard, moduloEntrada, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), string.Empty);
                                                if (oInfoEntrada.Exito)
                                                {
                                                    MessageBox.Show("Entrada creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    LimpiarDatosEntrada();
                                                }
                                                else
                                                {
                                                    MessageBox.Show(oInfoEntrada.ErrorMessage, "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    LimpiarDatosEntrada();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LimpiarDatosEntrada();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //AUTORIZADO NORMAL
                                VerificaTransaccionAbiertaAutorizadoResponse infoSalida = cliente.VerificarTransaccionAbiertaAutorizado(oInfoAutorizado.IdTarjeta);
                                if (!infoSalida.Exito)
                                {
                                    CreaEntradaResponse oInfoEntrada = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oInfoAutorizado.IdTarjeta, moduloEntrada, placaEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), oInfoAutorizado.IdAutorizacion);
                                    if (oInfoEntrada.Exito)
                                    {
                                        MessageBox.Show("Bienvenido Sr/Sra " + oInfoAutorizado.NombresApelldidos + "", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LimpiarDatosEntrada();
                                    }
                                }
                                else
                                {
                                    DialogResult result = MessageBox.Show("La mensualidad ya se encuentra en uso con placa: " + infoSalida.PlacaEntrada + "\n\n Presione SI: Para registrar la salida\n\n Presione NO: Para registrar por horas", "Leer PPM", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                    if (result == DialogResult.Yes)
                                    {
                                        CreaSalidaResponse oInfoSalida = cliente.CrearSalida(_IdEstacionamiento.ToString(), placaEntrada, oInfoAutorizado.IdTarjeta);
                                        if (oInfoSalida.Exito)
                                        {
                                            MessageBox.Show("Salida creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            LimpiarDatosEntrada();
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error al crear la salida", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LimpiarDatosEntrada();
                                        }
                                    }
                                    else if (result == DialogResult.No)
                                    {
                                        string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());

                                        CardResponse oCardResponse = CreateEntry(clave, tbPlaca.Text, moduloEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString());
                                        if (!oCardResponse.error)
                                        {
                                            AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoPorIdTarjeta(oCardResponse.idCard);
                                            if (oInfoAuto.Exito)
                                            {
                                                MessageBox.Show("Por favor coloque una tarjeta de visitante", "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                            else
                                            {
                                                CreaEntradaResponse oInfoEntrada = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oCardResponse.idCard, moduloEntrada, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), string.Empty);
                                                if (oInfoEntrada.Exito)
                                                {
                                                    MessageBox.Show("Entrada creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    LimpiarDatosEntrada();
                                                }
                                                else
                                                {
                                                    MessageBox.Show(oInfoEntrada.ErrorMessage, "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    LimpiarDatosEntrada();
                                                }
                                            }


                                        }
                                        else
                                        {
                                            MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            LimpiarDatosEntrada();
                                        }

                                    }
                                    else
                                    {
                                        LimpiarDatosEntrada();
                                    }

                                }
                            }
                        }
                        else
                        {
                            //AUTORIZADO NORMAL
                            VerificaTransaccionAbiertaAutorizadoResponse infoSalida = cliente.VerificarTransaccionAbiertaAutorizado(oInfoAutorizado.IdTarjeta);
                            if (!infoSalida.Exito)
                            {
                                CreaEntradaResponse oInfoEntrada = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oInfoAutorizado.IdTarjeta, moduloEntrada, placaEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), oInfoAutorizado.IdAutorizacion);
                                if (oInfoEntrada.Exito)
                                {
                                    MessageBox.Show("Bienvenido Sr/Sra " + oInfoAutorizado.NombresApelldidos + "", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LimpiarDatosEntrada();
                                }
                            }
                            else
                            {
                                DialogResult result = MessageBox.Show("La mensualidad ya se encuentra en uso con placa: " + infoSalida.PlacaEntrada + "\n\n Presione SI: Para registrar la salida\n\n Presione NO: Para registrar por horas", "Leer PPM", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result == DialogResult.Yes)
                                {
                                    CreaSalidaResponse oInfoSalida = cliente.CrearSalida(_IdEstacionamiento.ToString(), placaEntrada, oInfoAutorizado.IdTarjeta);
                                    if (oInfoSalida.Exito)
                                    {
                                        MessageBox.Show("Salida creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LimpiarDatosEntrada();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error al crear la salida", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        LimpiarDatosEntrada();
                                    }
                                }
                                else if (result == DialogResult.No)
                                {
                                    string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());

                                    CardResponse oCardResponse = CreateEntry(clave, tbPlaca.Text, moduloEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString());
                                    if (!oCardResponse.error)
                                    {
                                        AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoPorIdTarjeta(oCardResponse.idCard);
                                        if (oInfoAuto.Exito)
                                        {
                                            MessageBox.Show("Por favor coloque una tarjeta de visitante", "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        else
                                        {
                                            CreaEntradaResponse oInfoEntrada = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oCardResponse.idCard, moduloEntrada, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), string.Empty);
                                            if (oInfoEntrada.Exito)
                                            {
                                                MessageBox.Show("Entrada creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                LimpiarDatosEntrada();
                                            }
                                            else
                                            {
                                                MessageBox.Show(oInfoEntrada.ErrorMessage, "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                LimpiarDatosEntrada();
                                            }
                                        }


                                    }
                                    else
                                    {
                                        MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        LimpiarDatosEntrada();
                                    }

                                }
                                else
                                {
                                    LimpiarDatosEntrada();
                                }

                            }
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("El autorizado tiene la mensualidad vencida \n Por favor coloque una tarjeta en la lectora", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (result == DialogResult.OK)
                        {
                            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());

                            CardResponse oCardResponse = CreateEntry(clave, tbPlaca.Text, moduloEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString());
                            if (!oCardResponse.error)
                            {
                                AutorizadoxPlacaResponse oInfoAuto = cliente.BuscarAutorizadoPorIdTarjeta(oCardResponse.idCard);
                                if (oInfoAuto.Exito)
                                {
                                    MessageBox.Show("Por favor coloque una tarjeta de visitante", "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    CreaEntradaResponse oInfoEntrada = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oCardResponse.idCard, moduloEntrada, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), string.Empty);
                                    if (oInfoEntrada.Exito)
                                    {
                                        MessageBox.Show("Entrada creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        LimpiarDatosEntrada();
                                    }
                                    else
                                    {
                                        MessageBox.Show(oInfoEntrada.ErrorMessage, "Error Crear Entrada PPM Cliente Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        LimpiarDatosEntrada();
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                LimpiarDatosEntrada();
                            }
                        }
                    }
                }
                else
                {
                    string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());

                    oCardResponse = GetCardInfo(clave);
                    AutorizadoxPlacaResponse oInfoAutoIdTarjeta = cliente.BuscarAutorizadoPorIdTarjeta(oCardResponse.idCard);
                    if (!oInfoAutoIdTarjeta.Exito)
                    {
                        oCardResponse = CreateEntry(clave, tbPlaca.Text, moduloEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString());
                        if (!oCardResponse.error)
                        {

                            CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oCardResponse.idCard, moduloEntrada, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), string.Empty);

                            if (oInfo.Exito)
                            {
                                MessageBox.Show("Entrada creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LimpiarDatosEntrada();
                            }
                            else
                            {
                                MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                LimpiarDatosEntrada();

                            }


                        }
                        else
                        {
                            MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LimpiarDatosEntrada();

                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Esta tarjeta le pertenece a un autorizado,\n ¿Desea registrar la entrada sobre esta tarjeta?", "Leer PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (result == DialogResult.Yes)
                        {
                            oCardResponse = CreateEntry(clave, tbPlaca.Text, moduloEntrada, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString());
                            if (!oCardResponse.error)
                            {

                                CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oCardResponse.idCard, moduloEntrada, tbPlaca.Text, dtpFechaIngreso.Value, _IdTipoVehiculo.ToString(), string.Empty);

                                if (oInfo.Exito)
                                {
                                    MessageBox.Show("Entrada creada con EXITO", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    LimpiarDatosEntrada();
                                }
                                else
                                {
                                    MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    LimpiarDatosEntrada();

                                }


                            }
                            else
                            {
                                MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                LimpiarDatosEntrada();

                            }
                        }
                        else
                        {
                            MessageBox.Show("Por favor coloque una tarjeta de visitante", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                tbPlaca.Focus();
            }
        }
        private void btn_Cobrar_Click(object sender, EventArgs e)
        {

            tmrHora.Stop();
            ReestablecerBotonesLateralIzquierdo();
            ReestablecerBotonInferior();
            ReestablecerBotonesLateralDerechoCobro();
            LimpiarDatosCobrar();
            btn_Cobrar.BackgroundImage = Image.FromFile(@"Media\Png\btn_CobrarPresionado.png");
            tabPrincipal.SelectedTab = tabCobrar;
            tmrTimeOutPago.Start();

        }
        private void btn_ConfirmarCobro_Click(object sender, EventArgs e)
        {
            try
            {
                if (_PagoEfectivo)
                {
                    _FormaPago = 10;
                }
                else
                {
                    _FormaPago = 47;
                }


                if (Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", "")) > 0)
                {
                    ConfirmarCobro();
                }
                else if ((Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", "")) == 0))
                {
                    string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                    if (clave != string.Empty)
                    {
                        oCardResponse = GetCardInfo(clave);
                        if (!oCardResponse.error)
                        {
                            if (Convert.ToInt32(oCardResponse.codeAutorizacion1) > 0)
                            {

                                VerificaTransaccionAbiertaAutorizadoResponse oInfoSalida = cliente.ValidarSalida(Convert.ToInt64(IdTransaccion));
                                if (oInfoSalida.Exito)
                                {
                                    CreaSalidaResponse oInfoRegistrarSalida = cliente.CrearSalida(IdEstacionamiento.ToString(), oCardResponse.placa.ToString(), oCardResponse.idCard);
                                    if (oInfoRegistrarSalida.Exito)
                                    {
                                        MessageBox.Show("Salida creada con exito", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    }
                                    else
                                    {
                                        MessageBox.Show("Ocurrió un error en el momento de registrar la salida\n Informar al area de Tecnología", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Tarjeta sin registro de entrada", "Parquearse Tecnología", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {

            }

        }
        private void btn_Cortesia_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPlacaBuscar.Text != string.Empty && tbValorAPagarCobrar.Text != string.Empty)
                {
                    if (Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", "")) > 0)
                    {
                        ReestablecerBotonesLateralDerechoCobro();
                        btn_Cortesia.BackgroundImage = Image.FromFile(@"Media\Png\btn_CortesiaPresionado.png");
                        MotivosCortesias popup = new MotivosCortesias(documentoUsuario.ToString(), Convert.ToInt64(cbEstacionamiento.SelectedValue.ToString()), Convert.ToInt64(IdTransaccion), Convert.ToDateTime(oCardResponse.fechEntrada));
                        tmrTimeOutPago.Stop();
                        popup.ShowDialog();
                        if (popup.DialogResult == DialogResult.OK)
                        {
                            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                            if (clave != string.Empty)
                            {
                                oCardResponse = GetCardInfo(clave);
                                if (!oCardResponse.error)
                                {

                                    if (oCardResponse.reposicion)
                                    {
                                        MessageBox.Show("NO se puede aplicar cortesía a una REPOSICION", "Error Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
                                        if (oCarrilxIdModuloResponse.Exito)
                                        {
                                            string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();
                                            InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                                            if (oInfoTransaccionService.Exito)
                                            {
                                                //AplicarCortesiaResponse oAplicarCortesiaResponse = cliente.AplicarLaCortesia(cbEstacionamiento.SelectedValue.ToString(), popup.Observacion, popup.Motivo.ToString(), oInfoTransaccionService.IdTransaccion, _DocumentoUsuario);
                                                //if (oAplicarCortesiaResponse.Exito)
                                                //{
                                                //    oCardResponse = AplicarCortesia(clave);
                                                //    AplicaCascoResponse oInfo = cliente.LiberarCasco(sIdTransaccion);
                                                //    if (!oCardResponse.error)
                                                //    {

                                                MessageBox.Show("Cortesia aplicada exitosamente", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                ReestablecerBotonesLateralDerechoCobro();
                                                tmrTimeOutPago.Start();

                                                //    }
                                                //    else
                                                //    {
                                                //        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    MessageBox.Show(oAplicarCortesiaResponse.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                //}
                                            }
                                            else
                                            {
                                                MessageBox.Show(oInfoTransaccionService.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                            }
                            else
                            {
                                MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                        {
                            ReestablecerBotonesLateralDerechoCobro();
                            tmrTimeOutPago.Start();
                        }
                        else
                        {
                            ReestablecerBotonesLateralDerechoCobro();
                            tmrTimeOutPago.Start();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void btn_Convenios_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoCobro();
            btn_Convenios.BackgroundImage = Image.FromFile(@"Media\Png\btn_ConveniosPresionado.png");
            ConvenioPopUp popup = new ConvenioPopUp(cbEstacionamiento.SelectedValue.ToString(), _DocumentoUsuario);
            tmrTimeOutPago.Stop();
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                if (clave != string.Empty)
                {
                    CardResponse oCardResponse = AplicarConvenio(clave, popup.Convenio.ToString());
                    if (!oCardResponse.error)
                    {
                        SaveConveniosResponse oInfo = new SaveConveniosResponse();
                        oInfo = cliente.SaveConvenio(cbEstacionamiento.SelectedValue.ToString(), Convert.ToInt64(popup.Convenio), popup.NameConvenio.ToString());
                        MessageBox.Show("Convenio aplicado exitosamente", "Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ReestablecerBotonesLateralDerechoCobro();
                        tmrTimeOutPago.Start();
                    }
                    else
                    {
                        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ReestablecerBotonesLateralDerechoCobro();
                        tmrTimeOutPago.Start();


                    }
                }
                else
                {
                    MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ReestablecerBotonesLateralDerechoCobro();
                    tmrTimeOutPago.Start();
                }
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                ReestablecerBotonesLateralDerechoCobro();
                tmrTimeOutPago.Start();


            }
            else
            {
                ReestablecerBotonesLateralDerechoCobro();
                tmrTimeOutPago.Start();


            }
        }
        private void btn_TarifasEspeciales_Click(object sender, EventArgs e)
        {
            if (txtPlacaBuscar.Text != string.Empty)
            {
                if (oCardResponse.idCard != string.Empty)
                {
                    tmrTimeOutPago.Stop();
                    ReestablecerBotonesLateralDerechoCobro();
                    btn_TarifasEspeciales.BackgroundImage = Image.FromFile(@"Media\Png\btn_TarifasEspecialesPresionado.png");

                    string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                    if (clave != string.Empty)
                    {
                        oCardResponse = GetCardInfo(clave);
                        if (!oCardResponse.error)
                        {
                            CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
                            if (oCarrilxIdModuloResponse.Exito)
                            {
                                string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                                InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                                if (oInfoTransaccionService.Exito)
                                {
                                    EventoPopUp popup = new EventoPopUp(cbEstacionamiento.SelectedValue.ToString(), _DocumentoUsuario, oInfoTransaccionService.IdTipoVehiculo);
                                    popup.ShowDialog();
                                    if (popup.DialogResult == DialogResult.OK)
                                    {
                                        AplicarEventoResponse oAplicarEventoResponse = cliente.AplicarElEvento(cbEstacionamiento.SelectedValue.ToString(), sIdTransaccion, _DocumentoUsuario, oCardResponse.idCard, popup.Evento.ToString());
                                        if (oAplicarEventoResponse.Exito)
                                        {
                                            MessageBox.Show("Evento aplicado exitosamente", "Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            ReestablecerBotonesLateralDerechoCobro();
                                            tmrTimeOutPago.Start();
                                        }
                                        else
                                        {
                                            MessageBox.Show(oAplicarEventoResponse.ErrorMessage, "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            ReestablecerBotonesLateralDerechoCobro();
                                            tmrTimeOutPago.Start();
                                        }

                                    }
                                    else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                                    {
                                        ReestablecerBotonesLateralDerechoCobro();
                                        tmrTimeOutPago.Start();
                                    }
                                    else
                                    {
                                        ReestablecerBotonesLateralDerechoCobro();
                                        tmrTimeOutPago.Start();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(oInfoTransaccionService.ErrorMessage, "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    ReestablecerBotonesLateralDerechoCobro();
                                    tmrTimeOutPago.Start();
                                }
                            }
                            else
                            {
                                MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                ReestablecerBotonesLateralDerechoCobro();
                                tmrTimeOutPago.Start();
                            }
                        }
                        else
                        {
                            MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ReestablecerBotonesLateralDerechoCobro();
                            tmrTimeOutPago.Start();
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ReestablecerBotonesLateralDerechoCobro();
                        tmrTimeOutPago.Start();
                    }


                }
            }



        }
        private void btn_Cerrar_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }
        private void btn_CerrarCobrar_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }
        private void btn_CerrarPrincipal_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }
        private void btn_Reposicion_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoCobro();
            btn_Reposicion.BackgroundImage = Image.FromFile(@"Media\Png\btn_ReposicionPresionado.png");
            ReposicionPopup popUp = new ReposicionPopup();
            tmrTimeOutPago.Stop();
            popUp.ShowDialog();
            if (popUp.DialogResult == DialogResult.OK)
            {
                string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());
                oCardResponse = GetCardInfo(clave);
                AutorizadoxPlacaResponse oInfoAutoIdTarjeta = cliente.BuscarAutorizadoPorIdTarjeta(oCardResponse.idCard);
                if (!oInfoAutoIdTarjeta.Exito)
                {
                    InfoTransaccionService oInfoTra = cliente.ConsultarInfoTransaccionxId(popUp.IdTransaccion);
                    if (oInfoTra.Exito)
                    {
                        //INACTVAR LA TARJETA ACTUAL 
                        TarjetasResponse oInfoTarjeta = cliente.InactivarTarjeta(oCardResponse.idCard, Convert.ToInt64(_IdEstacionamiento), Convert.ToInt64(popUp.IdTransaccion));
                        if (oInfoTarjeta.Exito)
                        {
                            oCardResponse = ReplaceCard(clave, oInfoTra.PlacaEntrada, oInfoTra.ModuloEntrada.ToString(), popUp.IdTransaccion, oInfoTra.IdTipoVehiculo.ToString(), clave, oInfoTra.HoraTransaccion);
                            if (!oCardResponse.error)
                            {
                                LimpiarDatosCobrar();
                                ReestablecerBotonesLateralDerechoCobro();
                                tmrTimeOutPago.Start();
                            }
                            else
                            {
                                MessageBox.Show(oCardResponse.errorMessage, "Reposición-PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                tmrTimeOutPago.Start();
                                ReestablecerBotonesLateralDerechoCobro();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error al momento de actualizar el registro", "Reposición-PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            tmrTimeOutPago.Start();
                            ReestablecerBotonesLateralDerechoCobro();
                        }

                    }
                    else
                    {
                        MessageBox.Show("No se encontró información de la transacción", "Reposición-PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tmrTimeOutPago.Start();
                        ReestablecerBotonesLateralDerechoCobro();
                    }
                }
                else
                {
                    MessageBox.Show("La tarjeta le pertenece a un autorizado\n Por favor coloque una tarjeta de visitante", "Reposición-PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tmrTimeOutPago.Start();
                    ReestablecerBotonesLateralDerechoCobro();
                }
            }
            else
            {
                ReestablecerBotonesLateralDerechoCobro();
                tmrTimeOutPago.Start();
            }
        }
        private void btn_Carros_Click(object sender, EventArgs e)
        {
            CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
            if (oCarrilxIdModuloResponse.Exito)
            {
                string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                //tbIdTarjeta.Text = oCardResponse.idCard;
                InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                if (oInfoTransaccionService.Exito)
                {
                    if (oInfoTransaccionService.IdTipoVehiculo == 2)
                    {
                        tmrTimeOutPago.Stop();
                        ReestablecerBotonesLateralDerechoCobro();
                        btn_Carros.BackgroundImage = Image.FromFile(@"Media\Png\btn_CarroPresionado.png");
                        AplicarMotoResponse oAplicarMotoResponse = cliente.AplicarEtiquetaCarro(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                        if (oAplicarMotoResponse.Exito)
                        {
                            MessageBox.Show("Etiqueta carro aplicada exitosamente a = " + cbEstacionamiento.SelectedValue.ToString() + "/" + oCardResponse.idCard + "/" + oCardResponse.moduloEntrada, "Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ReestablecerBotonesLateralDerechoCobro();
                            tmrTimeOutPago.Start();
                        }
                        else
                        {
                            MessageBox.Show(oAplicarMotoResponse.ErrorMessage, "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ReestablecerBotonesLateralDerechoCobro();
                            tmrTimeOutPago.Start();

                        }
                    }
                    else
                    {
                        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ReestablecerBotonesLateralDerechoCobro();
                        tmrTimeOutPago.Start();

                    }


                }
                else
                {
                    ReestablecerBotonesLateralDerechoCobro();
                    tmrTimeOutPago.Start();
                }
            }

        }
        private void btn_Motos_Click(object sender, EventArgs e)
        {
            CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
            if (oCarrilxIdModuloResponse.Exito)
            {
                string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                //tbIdTarjeta.Text = oCardResponse.idCard;
                InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                if (oInfoTransaccionService.Exito)
                {
                    if (oInfoTransaccionService.IdTipoVehiculo == 1)
                    {
                        tmrTimeOutPago.Stop();
                        ReestablecerBotonesLateralDerechoCobro();
                        btn_Carros.BackgroundImage = Image.FromFile(@"Media\Png\btn_CarroPresionado.png");
                        AplicarMotoResponse oAplicarMotoResponse = cliente.AplicarEtiquetaMoto(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                        if (oAplicarMotoResponse.Exito)
                        {
                            MessageBox.Show("Etiqueta moto aplicada exitosamente a = " + cbEstacionamiento.SelectedValue.ToString() + "/" + oCardResponse.idCard + "/" + oCardResponse.moduloEntrada, "Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ReestablecerBotonesLateralDerechoCobro();
                            tmrTimeOutPago.Start();
                        }
                        else
                        {
                            MessageBox.Show(oAplicarMotoResponse.ErrorMessage, "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            ReestablecerBotonesLateralDerechoCobro();
                            tmrTimeOutPago.Start();

                        }
                    }
                    else
                    {
                        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ReestablecerBotonesLateralDerechoCobro();
                        tmrTimeOutPago.Start();

                    }


                }
                else
                {
                    ReestablecerBotonesLateralDerechoCobro();
                    tmrTimeOutPago.Start();
                }
            }
        }
        private void btn_FacturaElectronica_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", "")) > 0)
            {
                ReestablecerBotonesLateralDerechoCobro();
                btn_FacturaContingencia.BackgroundImage = Image.FromFile(@"Media\Png\btn_FacturaElectronicaPresionado.png");
                FacturaElectronica popPup = new FacturaElectronica();
                tmrTimeOutPago.Stop();
                popPup.ShowDialog();
                if (popPup.DialogResult == DialogResult.OK)
                {
                    if (popPup.SolicitudFacturaElectronica == true)
                    {
                        if (popPup.Nit != "")
                        {
                            if (Convert.ToInt64(tbValorAPagarCobrar.Text.Replace("$", "").Replace(".", "")) > 0)
                            {
                                FacturaElectronica = true;
                                _NiCliente = popPup.Nit;
                                tmrTimeOutPago.Start();
                            }

                        }
                        else
                        {
                            DialogResult result3 = MessageBox.Show("No fué posible encontrar la información del cliente,\n ¿Desea imprimir la factura POS?", "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                            if (result3 == DialogResult.Yes)
                            {
                                tmrTimeOutPago.Start();
                                btn_ConfirmarCobro_Click(btn_ConfirmarCobro, EventArgs.Empty);
                            }
                            else
                            {
                                tmrTimeOutPago.Start();
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Print
        private void ImprimirPagoMensualidadFE(string idTransaccion, string idAutorizacion)
        {
            InfoFacturaResponseFE oInfoFacturaResponse = cliente.ObtenerDatosFacturaMensualidadFE(idTransaccion, idAutorizacion);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketMensualidadFE(oInfoFacturaResponse.LstItemsMensualidad.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket Mensualidad", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {

                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool PrintTicketMensualidadFE(List<InfoItemsFacturaMensualidadResponse> datos)
        {
            bool bPrint = false;

            try
            {

                ReportDataSource datasource = new ReportDataSource();
                LocalReport oLocalReport = new LocalReport();

                datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPagoMensualidadFE(datos).Tables[0]);
                oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketPagoMFE"));


                oLocalReport.DataSources.Add(datasource);
                oLocalReport.Refresh();

                ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                ore.PrintController = new StandardPrintController();
                ore.Print();

                oLocalReport.Dispose();
                oLocalReport = null;
                ore.Dispose();
                ore = null;

                bPrint = true;
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketPagoMensualidad GenerarTicketPagoMensualidadFE(List<InfoItemsFacturaMensualidadResponse> infoTicket)
        {
            DataSetTicketPagoMensualidad facturacion = new DataSetTicketPagoMensualidad();

            double total = 0;
            foreach (var item in infoTicket)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                DataSetTicketPagoMensualidad.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Resolucion = item.NumeroResolucion;
                rowDatosFactura.Rut = "NIT 900.554.696 -8";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.NombreAutorizacion = item.NombreAutorizacion;
                rowDatosFactura.Documento = item.Documento;
                rowDatosFactura.VigenciaFactura = item.Vigencia;

                //NEW FIELDS
                //rowDatosFactura.Nit = item.NombreEmpresa;
                //rowDatosFactura.NombreEmpresa = item.Nit;
                //

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }
        private void ImprimirArqueo(string idArqueo)
        {
            InfoArqueoResponse oInfoFacturaResponse = cliente.ObtenerDatosComprobanteArqueo(idArqueo);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketArqueo(oInfoFacturaResponse.LstInfoArqueos.ToList());
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
        private bool PrintTicketArqueo(List<InfoItemsArqueoResponse> datos)
        {
            bool bPrint = false;

            try
            {
                foreach (InfoItemsArqueoResponse item in datos)
                {
                    ReportDataSource datasource = new ReportDataSource();
                    LocalReport oLocalReport = new LocalReport();

                    datasource = new ReportDataSource("DataSetTicketArqueo", (DataTable)GenerarTicketArqueo(item).Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketArqueo"));


                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();

                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    ore.PrintController = new StandardPrintController();
                    ore.Print();

                    oLocalReport.Dispose();
                    oLocalReport = null;
                    ore.Dispose();
                    ore = null;

                    bPrint = true;
                }
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketArqueo GenerarTicketArqueo(InfoItemsArqueoResponse infoTicket)
        {
            DataSetTicketArqueo facturacion = new DataSetTicketArqueo();

            DataSetTicketArqueo.TablaTicketArqueoRow rowDatosFactura = facturacion.TablaTicketArqueo.NewTablaTicketArqueoRow();

            rowDatosFactura.Valor = infoTicket.Valor != "" ? Convert.ToDouble(infoTicket.Valor) : 0;
            rowDatosFactura.Producido = Convert.ToDouble(infoTicket.Producido);
            rowDatosFactura.Direccion = infoTicket.Direccion;
            rowDatosFactura.Fecha = infoTicket.Fecha;
            rowDatosFactura.IdArqueo = infoTicket.IdArqueo;
            rowDatosFactura.Modulo = infoTicket.Modulo;
            rowDatosFactura.Nombre = infoTicket.Nombre;
            rowDatosFactura.Telefono = infoTicket.Telefono;
            rowDatosFactura.Cantidad = infoTicket.CantTransacciones;
            rowDatosFactura.IdUsuario = infoTicket.IdUsuario;
            rowDatosFactura.Conteo = Convert.ToDouble(infoTicket.Conteo);


            facturacion.TablaTicketArqueo.AddTablaTicketArqueoRow(rowDatosFactura);

            return facturacion;
        }
        private void ImprimirPagoContingencia(string idModulo, int numeroFactura)
        {
            InfoFacturaResponseFE oInfoFacturaResponse = cliente.ObtenerDatosFacturaContingenciaFE(idModulo, numeroFactura);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketContingenciaFE(oInfoFacturaResponse.LstItems.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //RestablecerPPM();
            }
            else
            {
                //RestablecerPPM();
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool PrintTicketContingenciaFE(List<InfoItemsFacturaResponse> datos)
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

                        datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPagoContingenciaFE(item).Tables[0]);
                        oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "TicketContingenciaFE"));


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
        private DataSetTicketPago GenerarTicketPagoContingenciaFE(List<InfoItemsFacturaResponse> infoTicket)
        {
            DataSetTicketPago facturacion = new DataSetTicketPago();

            double total = 0;
            foreach (var item in infoTicket)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                DataSetTicketPago.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                //rowDatosFactura.Cambio = Convert.ToDouble(item.Cambio);
                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                //rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                //rowDatosFactura.Placa = item.Placa;
                //rowDatosFactura.Recibido = Convert.ToDouble(item.ValorRecibido);
                //rowDatosFactura.Resolucion = item.NumeroResolucion;
                rowDatosFactura.Rut = "NIT 900.554.696 -8";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                //rowDatosFactura.Fecha2 = item.FechaEntrada;
                rowDatosFactura.Vehiculo = item.TipoVehiculo;
                //rowDatosFactura.VigenciaFactura = item.Vigencia;

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }
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

                if (File.Exists(_imgQR))
                {
                    File.Delete(_imgQR);
                }
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private void ImprimirPagoNormalFE(string idTransaccion)
        {
            InfoFacturaResponseFE oInfoFacturaResponse = cliente.ObtenerDatosFacturaFE(idTransaccion);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketFE(oInfoFacturaResponse.LstItems.ToList());
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
        public bool PrintTicketFE(List<InfoItemsFacturaResponse> datos)
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

                        datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPagoFE(item).Tables[0]);
                        oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketPagoFE"));


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
        private DataSetTicketPago GenerarTicketPagoFE(List<InfoItemsFacturaResponse> infoTicket)
        {
            DataSetTicketPago facturacion = new DataSetTicketPago();

            double total = 0;
            foreach (var item in infoTicket)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                DataSetTicketPago.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Cambio = Convert.ToDouble(item.Cambio);
                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Placa = item.Placa;
                rowDatosFactura.Recibido = Convert.ToDouble(item.ValorRecibido);
                rowDatosFactura.Resolucion = item.NumeroResolucion;
                rowDatosFactura.Rut = "NIT 900.554.696 -8";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.Fecha2 = item.FechaEntrada;
                rowDatosFactura.Vehiculo = item.TipoVehiculo;
                //rowDatosFactura.VigenciaFactura = item.Vigencia;

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
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

                rowDatosFactura.Cambio = Convert.ToDouble(item.Cambio);
                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha.ToString();
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Placa = item.Placa;
                rowDatosFactura.Recibido = Convert.ToDouble(item.ValorRecibido);
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

                rowDatosFactura.DocCliente = _NiCliente.ToString();

                InfoClienteFacturaElectronicaResponse oInfoCliente = cliente.ValidarClientePorNit(_NiCliente);
                if (oInfoCliente.Exito)
                {
                    rowDatosFactura.RazonSocial = oInfoCliente.Nombre;
                }

                string emailIdentificacion = cliente.ObtenerValorParametroxNombre("EmiIdentificacion", cbEstacionamiento.SelectedValue.ToString());
                string softwarePin = cliente.ObtenerValorParametroxNombre("SoftwarePin", cbEstacionamiento.SelectedValue.ToString());
                string claveTecnica = cliente.ObtenerValorParametroxNombre("ClaveTecnica" + cbPPM.SelectedValue.ToString() + "", cbEstacionamiento.SelectedValue.ToString());
                string cufeCalculado = CalcularCUDE(item.NumeroFactura.Replace("-", ""), Convert.ToDateTime(item.Fecha).ToString("yyyy-MM-dd"), Convert.ToDateTime(item.Fecha).ToString("HH:mm:ss") + "-05:00", Convert.ToDecimal(subtotal), Convert.ToDecimal(0), Convert.ToDecimal(0), Convert.ToDecimal(0), Convert.ToDecimal(total), emailIdentificacion, _NiCliente.ToString(), claveTecnica, "1");
                rowDatosFactura.CUFE = cufeCalculado;

                #region QR
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(_imgQR + cufeCalculado, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrCode.GetGraphic(50);
                string nombreQR = _IdTransaccion + ".png";
                string directorio = Path.Combine(Application.StartupPath, "codigoQR");
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
        private void ImprimirPagoMensualidad(string idTransaccion, string idAutorizacion)
        {
            InfoFacturaResponse oInfoFacturaResponse = cliente.ObtenerDatosFacturaMensualidad(idTransaccion, idAutorizacion);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketMensualidad(oInfoFacturaResponse.LstItemsMensualidad.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket Mensualidad", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool PrintTicketMensualidad(List<InfoItemsFacturaMensualidadResponse> datos)
        {
            bool bPrint = false;

            try
            {

                ReportDataSource datasource = new ReportDataSource();
                LocalReport oLocalReport = new LocalReport();

                datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPagoMensualidad(datos).Tables[0]);
                oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketPagoM"));
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

                bPrint = true;

                if (File.Exists(_imgQR))
                {
                    File.Delete(_imgQR);
                }
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketPagoMensualidad GenerarTicketPagoMensualidad(List<InfoItemsFacturaMensualidadResponse> infoTicket)
        {
            DataSetTicketPagoMensualidad facturacion = new DataSetTicketPagoMensualidad();

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
                DataSetTicketPagoMensualidad.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Resolucion = item.NumeroResolucion;
                rowDatosFactura.Rut = "NIT 900.554.696 -8";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.NombreAutorizacion = item.NombreAutorizacion;
                rowDatosFactura.Documento = item.Documento;
                //rowDatosFactura.VigenciaFactura = item.Vigencia;

                //NEW FIELDS

                rowDatosFactura.Nit = item.NombreEmpresa;
                rowDatosFactura.NombreEmpresa = item.Nit;

                if (rowDatosFactura.Nit == "null")
                {
                    rowDatosFactura.Nit = "";
                }

                if (rowDatosFactura.NombreEmpresa == "null")
                {
                    rowDatosFactura.NombreEmpresa = "";
                }

                rowDatosFactura.DocCliente = _NiCliente.ToString();

                InfoClienteFacturaElectronicaResponse oInfoCliente = cliente.ValidarClientePorNit(_NiCliente);
                if (oInfoCliente.Exito)
                {
                    rowDatosFactura.RazonSocial = oInfoCliente.Nombre;
                }

                string emailIdentificacion = cliente.ObtenerValorParametroxNombre("EmiIdentificacion", cbEstacionamiento.SelectedValue.ToString());
                string softwarePin = cliente.ObtenerValorParametroxNombre("SoftwarePin", cbEstacionamiento.SelectedValue.ToString());
                string claveTecnica = cliente.ObtenerValorParametroxNombre("ClaveTecnica" + cbPPM.SelectedValue.ToString() + "", cbEstacionamiento.SelectedValue.ToString());
                string cufeCalculado = CalcularCUDE(item.NumeroFactura.Replace("-", ""), Convert.ToDateTime(item.Fecha).ToString("yyyy-MM-dd"), Convert.ToDateTime(item.Fecha).ToString("HH:mm:ss") + "-05:00", Convert.ToDecimal(subtotal), Convert.ToDecimal(0), Convert.ToDecimal(0), Convert.ToDecimal(0), Convert.ToDecimal(total), emailIdentificacion, _NiCliente.ToString(), claveTecnica, "1");
                rowDatosFactura.CUFE = cufeCalculado;

                #region QR
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(_imgQR + cufeCalculado, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeImage = qrCode.GetGraphic(50);
                string nombreQR = _IdTransaccion + ".png";
                string directorio = Path.Combine(Application.StartupPath, "codigoQR");
                if (!System.IO.Directory.Exists(directorio))
                {
                    System.IO.Directory.CreateDirectory(directorio);
                }
                string nombreQrFinal = directorio + "\\" + nombreQR;
                qrCodeImage.Save(nombreQrFinal, System.Drawing.Imaging.ImageFormat.Png);
                _imgQR = nombreQrFinal;
                qrCodeImage.Dispose();

                #endregion

                //

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }
        #endregion

        private void dvgListadoEventos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                tmrTimeOutPago.Stop();

                if (e.ColumnIndex == dvgListadoEventos.Columns["Eliminar"].Index)
                {
                    DataGridViewCheckBoxCell ckhEliminar = (DataGridViewCheckBoxCell)dvgListadoEventos.Rows[e.RowIndex].Cells["Eliminar"];
                    ckhEliminar.Value = !Convert.ToBoolean(ckhEliminar.Value);
                    if (Convert.ToBoolean(ckhEliminar.Value) == true)
                    {
                        if (Convert.ToBoolean(ckhEliminar.Value))
                        {
                            int idEvento = Convert.ToInt32(dvgListadoEventos.Rows[e.RowIndex].Cells["IdEvento"].Value);
                            long idTransaccion = Convert.ToInt64(dvgListadoEventos.Rows[e.RowIndex].Cells["IdTransaccion"].Value);

                            AplicarEventoResponse oAplicarEvento = cliente.EliminarEvento(IdEstacionamiento.ToString(), idTransaccion.ToString(), idEvento.ToString());

                            if (oAplicarEvento.Exito)
                            {
                                tmrTimeOutPago.Start();
                            }

                        }
                    }

                }
            }
            catch (Exception)
            {

                MessageBox.Show("Ocurrió un error al momento de seleccionar el evento a eliminar", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tmrTimeOutPago.Start();


            }

        }

        private void btn_RegistroManual_Click(object sender, EventArgs e)
        {
            ReestablecerBotonesLateralDerechoPrincipal();
            btn_RegistroManual.BackgroundImage = Image.FromFile(@"Media\Png\btn_RegistroManualPresionado.png");
            RegistroManualPopUp popUp = new RegistroManualPopUp(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), DocumentoUsuario);
            tmrTimeOutPago.Stop();
            popUp.ShowDialog();
            if (popUp.DialogResult == DialogResult.OK)
            {
                ReestablecerBotonesLateralDerechoCobro();
            }
            else if (popUp.DialogResult == DialogResult.Cancel)
            {
                ReestablecerBotonesLateralDerechoCobro();
            }
        }

    }

}



