namespace BlockAndPass.PPMWinform
{
    partial class RegistroManualPopUp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistroManualPopUp));
            this.cbMotivo = new System.Windows.Forms.ComboBox();
            this.tbObservacion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Cancel = new FontAwesome.Sharp.IconButton();
            this.btn_Ok = new FontAwesome.Sharp.IconButton();
            this.label4 = new System.Windows.Forms.Label();
            this.tbFechaYHoraActual = new System.Windows.Forms.TextBox();
            this.trmHoraActual = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.grupoFrm = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cboMinuto = new System.Windows.Forms.ComboBox();
            this.cboHora = new System.Windows.Forms.ComboBox();
            this.tbFechaEntrada = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbTotal = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbMensualidad = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbTarjeta = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbParqueo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbPlaca = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rdoMoto = new System.Windows.Forms.RadioButton();
            this.rdoCarro = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.grupoFrm.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbMotivo
            // 
            this.cbMotivo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMotivo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMotivo.FormattingEnabled = true;
            this.cbMotivo.Location = new System.Drawing.Point(137, 60);
            this.cbMotivo.Name = "cbMotivo";
            this.cbMotivo.Size = new System.Drawing.Size(347, 32);
            this.cbMotivo.TabIndex = 3;
            // 
            // tbObservacion
            // 
            this.tbObservacion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbObservacion.Location = new System.Drawing.Point(17, 505);
            this.tbObservacion.Multiline = true;
            this.tbObservacion.Name = "tbObservacion";
            this.tbObservacion.Size = new System.Drawing.Size(474, 96);
            this.tbObservacion.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(61, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Motivo:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 482);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Observacion";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(180)))), ((int)(((byte)(77)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(515, 70);
            this.panel1.TabIndex = 51;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 33);
            this.label1.TabIndex = 6;
            this.label1.Text = "Registros manuales";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(64)))), ((int)(((byte)(97)))));
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.ForeColor = System.Drawing.Color.White;
            this.btn_Cancel.Icon = FontAwesome.Sharp.IconChar.Times;
            this.btn_Cancel.IconColor = System.Drawing.Color.White;
            this.btn_Cancel.IconSize = 20;
            this.btn_Cancel.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cancel.Image")));
            this.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Cancel.Location = new System.Drawing.Point(270, 694);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(90, 35);
            this.btn_Cancel.TabIndex = 53;
            this.btn_Cancel.Text = "Cerrar";
            this.btn_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Cancel.UseVisualStyleBackColor = false;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Ok
            // 
            this.btn_Ok.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(134)))), ((int)(((byte)(165)))), ((int)(((byte)(64)))));
            this.btn_Ok.FlatAppearance.BorderSize = 0;
            this.btn_Ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Ok.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Ok.ForeColor = System.Drawing.Color.White;
            this.btn_Ok.Icon = FontAwesome.Sharp.IconChar.Check;
            this.btn_Ok.IconColor = System.Drawing.Color.White;
            this.btn_Ok.IconSize = 20;
            this.btn_Ok.Image = ((System.Drawing.Image)(resources.GetObject("btn_Ok.Image")));
            this.btn_Ok.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Ok.Location = new System.Drawing.Point(389, 694);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(102, 35);
            this.btn_Ok.TabIndex = 52;
            this.btn_Ok.Text = "Confirmar";
            this.btn_Ok.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Ok.UseVisualStyleBackColor = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(5, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 24);
            this.label4.TabIndex = 55;
            this.label4.Text = "Fecha y hora:";
            // 
            // tbFechaYHoraActual
            // 
            this.tbFechaYHoraActual.Enabled = false;
            this.tbFechaYHoraActual.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFechaYHoraActual.Location = new System.Drawing.Point(205, 12);
            this.tbFechaYHoraActual.Name = "tbFechaYHoraActual";
            this.tbFechaYHoraActual.Size = new System.Drawing.Size(209, 31);
            this.tbFechaYHoraActual.TabIndex = 54;
            this.tbFechaYHoraActual.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // trmHoraActual
            // 
            this.trmHoraActual.Tick += new System.EventHandler(this.trmHoraActual_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(2, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 24);
            this.label5.TabIndex = 56;
            this.label5.Text = "Tipo vehiculo:";
            // 
            // grupoFrm
            // 
            this.grupoFrm.Controls.Add(this.label12);
            this.grupoFrm.Controls.Add(this.cboMinuto);
            this.grupoFrm.Controls.Add(this.cboHora);
            this.grupoFrm.Controls.Add(this.tbFechaEntrada);
            this.grupoFrm.Controls.Add(this.label11);
            this.grupoFrm.Controls.Add(this.groupBox1);
            this.grupoFrm.Controls.Add(this.tbPlaca);
            this.grupoFrm.Controls.Add(this.label6);
            this.grupoFrm.Controls.Add(this.rdoMoto);
            this.grupoFrm.Controls.Add(this.rdoCarro);
            this.grupoFrm.Controls.Add(this.label2);
            this.grupoFrm.Controls.Add(this.cbMotivo);
            this.grupoFrm.Controls.Add(this.label3);
            this.grupoFrm.Controls.Add(this.tbFechaYHoraActual);
            this.grupoFrm.Controls.Add(this.tbObservacion);
            this.grupoFrm.Controls.Add(this.label5);
            this.grupoFrm.Controls.Add(this.label4);
            this.grupoFrm.Location = new System.Drawing.Point(0, 76);
            this.grupoFrm.Name = "grupoFrm";
            this.grupoFrm.Size = new System.Drawing.Size(503, 612);
            this.grupoFrm.TabIndex = 59;
            this.grupoFrm.TabStop = false;
            this.grupoFrm.Enter += new System.EventHandler(this.grupoFrm_Enter);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(354, 144);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(15, 24);
            this.label12.TabIndex = 67;
            this.label12.Text = ":";
            // 
            // cboMinuto
            // 
            this.cboMinuto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMinuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMinuto.FormattingEnabled = true;
            this.cboMinuto.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32",
            "33",
            "34",
            "35",
            "36",
            "37",
            "38",
            "39",
            "40",
            "41",
            "42",
            "43",
            "44",
            "45",
            "46",
            "47",
            "48",
            "49",
            "50",
            "51",
            "52",
            "53",
            "54",
            "55",
            "56",
            "57",
            "58",
            "59"});
            this.cboMinuto.Location = new System.Drawing.Point(372, 142);
            this.cboMinuto.Name = "cboMinuto";
            this.cboMinuto.Size = new System.Drawing.Size(42, 28);
            this.cboMinuto.TabIndex = 66;
            // 
            // cboHora
            // 
            this.cboHora.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHora.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboHora.FormattingEnabled = true;
            this.cboHora.Items.AddRange(new object[] {
            "00",
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.cboHora.Location = new System.Drawing.Point(310, 142);
            this.cboHora.Name = "cboHora";
            this.cboHora.Size = new System.Drawing.Size(42, 28);
            this.cboHora.TabIndex = 65;
            // 
            // tbFechaEntrada
            // 
            this.tbFechaEntrada.CustomFormat = "";
            this.tbFechaEntrada.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbFechaEntrada.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tbFechaEntrada.Location = new System.Drawing.Point(172, 145);
            this.tbFechaEntrada.Name = "tbFechaEntrada";
            this.tbFechaEntrada.Size = new System.Drawing.Size(132, 26);
            this.tbFechaEntrada.TabIndex = 64;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(2, 145);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(164, 24);
            this.label11.TabIndex = 63;
            this.label11.Text = "Fecha de entrada:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbTotal);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbMensualidad);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.tbTarjeta);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbParqueo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.groupBox1.Location = new System.Drawing.Point(19, 236);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 243);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tipo Pago";
            // 
            // tbTotal
            // 
            this.tbTotal.Enabled = false;
            this.tbTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTotal.Location = new System.Drawing.Point(145, 198);
            this.tbTotal.Name = "tbTotal";
            this.tbTotal.Size = new System.Drawing.Size(209, 31);
            this.tbTotal.TabIndex = 67;
            this.tbTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(68, 202);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 24);
            this.label10.TabIndex = 68;
            this.label10.Text = "Total:";
            // 
            // tbMensualidad
            // 
            this.tbMensualidad.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbMensualidad.Location = new System.Drawing.Point(145, 121);
            this.tbMensualidad.Name = "tbMensualidad";
            this.tbMensualidad.Size = new System.Drawing.Size(209, 31);
            this.tbMensualidad.TabIndex = 65;
            this.tbMensualidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbMensualidad.TextChanged += new System.EventHandler(this.tbMensualidad_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 24);
            this.label9.TabIndex = 66;
            this.label9.Text = "Mensualidad:";
            // 
            // tbTarjeta
            // 
            this.tbTarjeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTarjeta.Location = new System.Drawing.Point(145, 84);
            this.tbTarjeta.Name = "tbTarjeta";
            this.tbTarjeta.Size = new System.Drawing.Size(209, 31);
            this.tbTarjeta.TabIndex = 63;
            this.tbTarjeta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbTarjeta.TextChanged += new System.EventHandler(this.tbTarjeta_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(52, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 24);
            this.label8.TabIndex = 64;
            this.label8.Text = "Tarjeta:";
            // 
            // tbParqueo
            // 
            this.tbParqueo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbParqueo.Location = new System.Drawing.Point(145, 47);
            this.tbParqueo.Name = "tbParqueo";
            this.tbParqueo.Size = new System.Drawing.Size(209, 31);
            this.tbParqueo.TabIndex = 61;
            this.tbParqueo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbParqueo.TextChanged += new System.EventHandler(this.tbParqueo_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(37, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 24);
            this.label7.TabIndex = 62;
            this.label7.Text = "Parqueo:";
            // 
            // tbPlaca
            // 
            this.tbPlaca.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPlaca.Location = new System.Drawing.Point(172, 199);
            this.tbPlaca.Name = "tbPlaca";
            this.tbPlaca.Size = new System.Drawing.Size(209, 31);
            this.tbPlaca.TabIndex = 59;
            this.tbPlaca.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(105, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 24);
            this.label6.TabIndex = 60;
            this.label6.Text = "Placa:";
            // 
            // rdoMoto
            // 
            this.rdoMoto.AutoSize = true;
            this.rdoMoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoMoto.Location = new System.Drawing.Point(261, 103);
            this.rdoMoto.Name = "rdoMoto";
            this.rdoMoto.Size = new System.Drawing.Size(70, 28);
            this.rdoMoto.TabIndex = 58;
            this.rdoMoto.TabStop = true;
            this.rdoMoto.Text = "Moto";
            this.rdoMoto.UseVisualStyleBackColor = true;
            // 
            // rdoCarro
            // 
            this.rdoCarro.AutoSize = true;
            this.rdoCarro.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoCarro.Location = new System.Drawing.Point(153, 103);
            this.rdoCarro.Name = "rdoCarro";
            this.rdoCarro.Size = new System.Drawing.Size(74, 28);
            this.rdoCarro.TabIndex = 57;
            this.rdoCarro.TabStop = true;
            this.rdoCarro.Text = "Carro";
            this.rdoCarro.UseVisualStyleBackColor = true;
            // 
            // RegistroManualPopUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(515, 730);
            this.ControlBox = false;
            this.Controls.Add(this.grupoFrm);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RegistroManualPopUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.RegistroManualPopUp_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grupoFrm.ResumeLayout(false);
            this.grupoFrm.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMotivo;
        private System.Windows.Forms.TextBox tbObservacion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconButton btn_Cancel;
        private FontAwesome.Sharp.IconButton btn_Ok;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbFechaYHoraActual;
        private System.Windows.Forms.Timer trmHoraActual;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox grupoFrm;
        private System.Windows.Forms.RadioButton rdoMoto;
        private System.Windows.Forms.RadioButton rdoCarro;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tbPlaca;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbTotal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbMensualidad;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbTarjeta;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbParqueo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker tbFechaEntrada;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cboMinuto;
        private System.Windows.Forms.ComboBox cboHora;
    }
}