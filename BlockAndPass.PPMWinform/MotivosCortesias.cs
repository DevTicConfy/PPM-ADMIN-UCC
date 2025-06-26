using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BlockAndPass.PPMWinform.ByPServices;


namespace BlockAndPass.PPMWinform
{
    public partial class MotivosCortesias : Form
    {
        public MotivosCortesias(string documentoUsuario, long idEstacionamiento, long idTransaccion, DateTime fechaEntrada)
        {
            _DocumentoUsuario = documentoUsuario;
            _IdEstacionamiento = idEstacionamiento;
            _IdTransaccion = idTransaccion;
            _FechaEntrada = fechaEntrada;
            InitializeComponent();
        }


        private DateTime _FechaEntrada = DateTime.Now;
        public DateTime FechaEntrada
        {
            get { return _FechaEntrada; }
            set { _FechaEntrada = value; }
        }

        private int _IdMotivoCortesia = 1;
        public int IdMotivoCortesia
        {
            get { return _IdMotivoCortesia; }
            set { _IdMotivoCortesia = value; }
        }

        private string _DocumentoUsuario = string.Empty;
        public string DocumentoUsuario
        {
            get { return _DocumentoUsuario; }
            set { _DocumentoUsuario = value; }
        }

        private long _IdEstacionamiento = 1;
        public long IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        private long _IdTransaccion = 1;
        public long IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        ServicesByP cliente = new ServicesByP();


        public bool GuardarCortesia(string nombresApellidos, string cedula, string placa, string tipoNovedad, string descripcion, string DocumentoAutorizaCortesia)
        {
            bool OK = false;
            try
            {
                if (nombresApellidos == "" && cedula == "")
                {

                    AplicarEventoResponse oInfoCortesia = cliente.AplicarLaCortesiaNoParquea(Convert.ToString(IdEstacionamiento), descripcion, Convert.ToString(_IdMotivoCortesia), Convert.ToString(IdTransaccion), DocumentoUsuario);
                    if (oInfoCortesia.Exito)
                    {
                        OK = true;

                    }
                }
                else
                {
                    AplicarCortesiaResponse oInfoCortesia = cliente.AplicarLaCortesia(Convert.ToString(IdEstacionamiento), descripcion, Convert.ToString(_IdMotivoCortesia), Convert.ToString(IdTransaccion), DocumentoUsuario, nombresApellidos, cedula, DocumentoAutorizaCortesia, tipoNovedad);
                    if (oInfoCortesia.Exito)
                    {
                        OK = true;

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString() + "\n Informar a Tecnología", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OK = false;
            }
            return OK;

        }


        private void MotivosCortesias_Load(object sender, EventArgs e)
        {
            TabPrincipal.SelectedTab = TabMotivosCortesias;
            TabPrincipal.Appearance = TabAppearance.FlatButtons;
            TabPrincipal.ItemSize = new Size(0, 1);
            TabPrincipal.SizeMode = TabSizeMode.Fixed;
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            TabPrincipal.SelectedTab = TabClientes;
            _IdMotivoCortesia = 8;
            lblTitulo.Text = "Cortesías - " + btnClientes.Text;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (txtNombresApellidosClientes.Text != "")
            {
                if (txtCedulaClientes.Text != "")
                {
                    if (txtPlacaClientes.Text != "")
                    {
                        if (cboTipoNovedadClientes.Text != "")
                        {
                            if (txtDescripcionClientes.Text != "")
                            {
                                if (cboTipoNovedadClientes.Text == "Mensualidad")
                                {
                                    AutorizadoxPlacaResponse oInfoAutorizado = cliente.BuscarAutorizadoxPlaca(txtPlacaClientes.Text.Trim());
                                    if (oInfoAutorizado.Exito)
                                    {

                                        if (oInfoAutorizado.Documento == txtCedulaClientes.Text.Trim())
                                        {

                                            if (GuardarCortesia(txtNombresApellidosClientes.Text.Trim(), txtCedulaClientes.Text.Trim(), txtPlacaClientes.Text.Trim(), cboTipoNovedadClientes.Text.Trim(), txtDescripcionClientes.Text.Trim(), string.Empty))
                                            {
                                                DialogResult = DialogResult.OK;
                                                this.Hide();
                                            }
                                            else
                                            {
                                                MessageBox.Show("Ocurrió un error el momento de registrar la cortesía\n Por favor informar a Tecnología", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                this.Hide();

                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("El documento no pertenece a la placa ingresada\n Por favor verifique", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            txtCedulaClientes.Focus();
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("La placa ingresada no pertenece a un autorizado", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtPlacaClientes.Focus();
                                    }
                                }
                                else
                                {
                                    if (GuardarCortesia(txtNombresApellidosClientes.Text.Trim(), txtCedulaClientes.Text.Trim(), txtPlacaClientes.Text.Trim(), cboTipoNovedadClientes.Text.Trim(), txtDescripcionClientes.Text.Trim(), string.Empty))
                                    {
                                        DialogResult = DialogResult.OK;
                                        this.Hide();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Ocurrió un error el momento de registrar la cortesía\n Por favor informar a Tecnología", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        this.Hide();

                                    }
                                }

                            }
                            else
                            {
                                MessageBox.Show("Es necesario que ingrese una descripción", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtDescripcionClientes.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Por favor seleccione el tipo de novedad", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            cboTipoNovedadClientes.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es necesario que ingrese la placa del cliente", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPlacaClientes.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("El documento del cliente es obligatorio", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCedulaClientes.Focus();
                }
            }
            else
            {
                MessageBox.Show("Es necesario que ingrese nombres o apellidos", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombresApellidosClientes.Focus();
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void btn_Ok_Proveedores_Click(object sender, EventArgs e)
        {
            if (txtNombreApellidosPro.Text != "")
            {
                if (txtCedulaPro.Text != "")
                {
                    //if (cboAutorizados.SelectedText != "")
                    //{
                    if (txtDescripcionPro.Text != "")
                    {
                        if (txtNombreApellidosPro.Text.Length >= 5)
                        {
                            if (GuardarCortesia(txtNombreApellidosPro.Text.Trim(), txtCedulaPro.Text.Trim(), txtPlacaPro.Text.Trim(), string.Empty, txtDescripcionPro.Text.Trim(), cboAutorizados.SelectedValue.ToString()))
                            {
                                DialogResult = DialogResult.OK;
                                this.Hide();
                            }

                        }
                        else
                        {
                            MessageBox.Show("Nombres y apellidos no válidos", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es necesario que ingrese una descripción", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtDescripcionPro.Focus();
                    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Por favor seleccione quien autoriza la cortesía", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    //}
                }
                else
                {
                    MessageBox.Show("Es necesario que ingrese el documento", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCedulaPro.Focus();

                }
            }
            else
            {
                MessageBox.Show("Es necesario que ingrese Nombres y Apellidos", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNombreApellidosPro.Focus();
            }
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
            TabPrincipal.SelectedTab = TabProveedores;
            _IdMotivoCortesia = 9;
            lblTitulo.Text = "Cortesías - " + btnProveedor.Text;

            cboAutorizados.DataSource = cliente.ListarUsuariosAutorizaCortesia(Convert.ToInt64(_IdEstacionamiento));
            cboAutorizados.ValueMember = "Documento";
            cboAutorizados.DisplayMember = "Nombres";
        }

        private void btnNoParquea_Click(object sender, EventArgs e)
        {
            TabPrincipal.SelectedTab = TabNoParquea;
            _IdMotivoCortesia = 10;
            lblTitulo.Text = "Cortesías - " + btnNoParquea.Text;
        }

        private void btn_OkNoParquea_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaActual = DateTime.Now;

                TimeSpan calculoTiempo = fechaActual - FechaEntrada;

                string tiempo = cliente.ObtenerValorParametroxNombre("TiempoNoParquea", IdEstacionamiento.ToString());

                if (Convert.ToInt32(calculoTiempo.Minutes) < Convert.ToInt32(tiempo))
                {


                    if (txtPlacaNoParquea.Text != "")
                    {
                        if (txtDescripcionNoParquea.Text != "")
                        {

                            if (GuardarCortesia("", "", txtPlacaNoParquea.Text.Trim(), "", txtDescripcionNoParquea.Text.Trim(), string.Empty))
                            {
                                DialogResult = DialogResult.OK;
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Ocurrió un error el momento de registrar la cortesía\n Por favor informar a Tecnología", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Hide();

                            }

                        }
                        else
                        {
                            MessageBox.Show("Es necesario que ingrese una descripción", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtDescripcionClientes.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Es necesario que ingrese la placa el vehículo", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtPlacaClientes.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("No se puede aplicar la cortesía\n El vehículo lleva mas de "+tiempo+" minutos dentro del sistema", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.Cancel;
                    this.Hide();
                }
            }
            catch (Exception)
            {
                
            }
   
        }

        private void btn_CancelNoParquea_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Hide();
        }
    }
}
