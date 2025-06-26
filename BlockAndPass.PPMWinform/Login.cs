using BlockAndPass.PPMWinform.ByPServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class Login : Form
    {
        ServicesByP cliente = new ServicesByP();

        public Login()
        {
            InitializeComponent();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            #region Old
            //LoginResponse oLogin = cliente.Loguearse(tbUser.Text);
            //if (oLogin.Exito)
            //{
            //    if (Decrypt(oLogin.Clave) == tbClave.Text)
            //    {
            //        InfoPPMService oInfoPPMService = cliente.ObtenerDatosPPMxMac(GetLocalMACAddress());

            //        if (oInfoPPMService.Exito)
            //        {
            //            PPM df = new PPM(oLogin.Documento, oInfoPPMService);

            //            df.Show();

            //            this.Close();
            //        }
            //        else
            //        {
            //            MessageBox.Show("No se reconoce punto de pago asociado para la MAC = " + GetLocalMACAddress(), "Error PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            this.Close();
            //            Application.Exit();
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Clave incorrecta", "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        this.Close();
            //        Application.Exit();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show(oLogin.ErrorMessage, "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //    Application.Exit();
            //}
            #endregion

            LoginResponse oLogin = cliente.Loguearse(tbUser.Text);
            if (oLogin.Exito)
            {
                if (Decrypt(oLogin.Clave) == tbClave.Text)
                {
                    InfoPPMService oInfoPPMService = cliente.ObtenerDatosPPMxMac(GetLocalMACAddress());

                    if (oInfoPPMService.Exito)
                    {
                        Menu df = new Menu(oLogin.Documento, oLogin.Cargo, oInfoPPMService);

                        df.Show();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se reconoce punto de pago asociado para la MAC = " + GetLocalMACAddress(), "Error PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("Clave incorrecta", "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                }
            }
            else
            {
                MessageBox.Show(oLogin.ErrorMessage, "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                Application.Exit();
            }
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private void tbClave_Enter(object sender, EventArgs e)
        {

        }

        private string GetLocalMACAddress()
        {
            return GetMACAddress();
        }

        private string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            //return "00251161D626";
            return sMacAddress;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Imgen_FondoLogin.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Media\Png\ImagenFondoLogin.Png"));

            //Copyright

            lblFooter.Text = "© " + DateTime.Now.Year + " - Parquearse Tecnología";
        }

        private void btn_Ok_Click_1(object sender, EventArgs e)
        {
            LoginResponse oLogin = cliente.Loguearse(tbUser.Text);
            if (oLogin.Exito)
            {
                if (Decrypt(oLogin.Clave) == tbClave.Text)
                {
                    InfoPPMService oInfoPPMService = cliente.ObtenerDatosPPMxMac(GetLocalMACAddress());

                    if (oInfoPPMService.Exito)
                    {
                        Menu df = new Menu(oLogin.Documento, oLogin.Cargo, oInfoPPMService);

                        df.Show();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se reconoce punto de pago asociado para la MAC = " + GetLocalMACAddress(), "Error PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("Clave incorrecta", "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                }
            }
            else
            {
                MessageBox.Show(oLogin.ErrorMessage, "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                Application.Exit();
            }
        }

        private void tbClave_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btn_Ok_Click(btn_Ok, EventArgs.Empty);
            }
        }

        private void btn_Cancel_Click_1(object sender, EventArgs e)
        {
            btn_Cancel.BackgroundImage = Image.FromFile(@"Media\Png\btn_CancelarPresionado.png");
            btn_Cancel.Text = "";
            btn_Cancel.BackgroundImageLayout = ImageLayout.Stretch;
            this.Close();
            Application.Exit();
        }
    }
}
