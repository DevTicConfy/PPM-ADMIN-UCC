namespace BlockAndPass.PPMWinform
{
    partial class CopiaFactura
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopiaFactura));
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.dataTable3BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetCopia = new BlockAndPass.PPMWinform.Tickets.DataSetCopia();
            this.dataTable5BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataTable2BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetCopiaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataTable4BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tbnumerofactura = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.iconButton1 = new FontAwesome.Sharp.IconButton();
            this.btn_Ok = new FontAwesome.Sharp.IconButton();
            this.cboIdModulo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ReportNumeroFactura = new Microsoft.Reporting.WinForms.ReportViewer();
            this.label3 = new System.Windows.Forms.Label();
            this.chkMensualidad = new System.Windows.Forms.CheckBox();
            this.pnlCopiaFactura = new System.Windows.Forms.Panel();
            this.ReportMensualidadPorPlaca = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ReportMensualidad = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ReportPlacaEntrada = new Microsoft.Reporting.WinForms.ReportViewer();
            this.tbPlacaBuscar = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dataTable3TableAdapter = new BlockAndPass.PPMWinform.Tickets.DataSetCopiaTableAdapters.DataTable3TableAdapter();
            this.dataTable4TableAdapter = new BlockAndPass.PPMWinform.Tickets.DataSetCopiaTableAdapters.DataTable4TableAdapter();
            this.dataTable2TableAdapter = new BlockAndPass.PPMWinform.Tickets.DataSetCopiaTableAdapters.DataTable2TableAdapter();
            this.dataTable5TableAdapter = new BlockAndPass.PPMWinform.Tickets.DataSetCopiaTableAdapters.DataTable5TableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable3BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCopia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable5BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable2BindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCopiaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable4BindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlCopiaFactura.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataTable3BindingSource
            // 
            this.dataTable3BindingSource.DataMember = "DataTable3";
            this.dataTable3BindingSource.DataSource = this.dataSetCopia;
            // 
            // dataSetCopia
            // 
            this.dataSetCopia.DataSetName = "DataSetCopia";
            this.dataSetCopia.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataTable5BindingSource
            // 
            this.dataTable5BindingSource.DataMember = "DataTable5";
            this.dataTable5BindingSource.DataSource = this.dataSetCopia;
            // 
            // dataTable2BindingSource
            // 
            this.dataTable2BindingSource.DataMember = "DataTable2";
            this.dataTable2BindingSource.DataSource = this.dataSetCopiaBindingSource;
            // 
            // dataSetCopiaBindingSource
            // 
            this.dataSetCopiaBindingSource.DataSource = this.dataSetCopia;
            this.dataSetCopiaBindingSource.Position = 0;
            // 
            // dataTable4BindingSource
            // 
            this.dataTable4BindingSource.DataMember = "DataTable4";
            this.dataTable4BindingSource.DataSource = this.dataSetCopiaBindingSource;
            // 
            // tbnumerofactura
            // 
            this.tbnumerofactura.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbnumerofactura.Location = new System.Drawing.Point(233, 558);
            this.tbnumerofactura.Name = "tbnumerofactura";
            this.tbnumerofactura.Size = new System.Drawing.Size(134, 23);
            this.tbnumerofactura.TabIndex = 19;
            this.tbnumerofactura.TextChanged += new System.EventHandler(this.tbnumerofactura_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(237, 538);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 18;
            this.label1.Text = "Numero de factura";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(180)))), ((int)(((byte)(77)))));
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(523, 63);
            this.panel1.TabIndex = 52;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Control;
            this.label4.Location = new System.Drawing.Point(12, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(228, 33);
            this.label4.TabIndex = 5;
            this.label4.Text = "Copia de factura";
            // 
            // iconButton1
            // 
            this.iconButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(64)))), ((int)(((byte)(97)))));
            this.iconButton1.FlatAppearance.BorderSize = 0;
            this.iconButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButton1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconButton1.ForeColor = System.Drawing.Color.White;
            this.iconButton1.Icon = FontAwesome.Sharp.IconChar.Times;
            this.iconButton1.IconColor = System.Drawing.Color.White;
            this.iconButton1.IconSize = 20;
            this.iconButton1.Image = ((System.Drawing.Image)(resources.GetObject("iconButton1.Image")));
            this.iconButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButton1.Location = new System.Drawing.Point(314, 587);
            this.iconButton1.Name = "iconButton1";
            this.iconButton1.Size = new System.Drawing.Size(90, 35);
            this.iconButton1.TabIndex = 55;
            this.iconButton1.Text = "Cerrar";
            this.iconButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButton1.UseVisualStyleBackColor = false;
            this.iconButton1.Click += new System.EventHandler(this.iconButton1_Click);
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
            this.btn_Ok.Location = new System.Drawing.Point(410, 587);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(102, 35);
            this.btn_Ok.TabIndex = 54;
            this.btn_Ok.Text = "Confirmar";
            this.btn_Ok.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Ok.UseVisualStyleBackColor = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // cboIdModulo
            // 
            this.cboIdModulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIdModulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.cboIdModulo.FormattingEnabled = true;
            this.cboIdModulo.Items.AddRange(new object[] {
            "PUB",
            "DES",
            "PPE"});
            this.cboIdModulo.Location = new System.Drawing.Point(99, 558);
            this.cboIdModulo.Name = "cboIdModulo";
            this.cboIdModulo.Size = new System.Drawing.Size(121, 24);
            this.cboIdModulo.TabIndex = 57;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(134, 538);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 18);
            this.label2.TabIndex = 56;
            this.label2.Text = "Modulo";
            // 
            // ReportNumeroFactura
            // 
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.dataTable3BindingSource;
            this.ReportNumeroFactura.LocalReport.DataSources.Add(reportDataSource1);
            this.ReportNumeroFactura.LocalReport.ReportEmbeddedResource = "BlockAndPass.PPMWinform.Tickets.CopiaFactura.rdlc";
            this.ReportNumeroFactura.Location = new System.Drawing.Point(5, 3);
            this.ReportNumeroFactura.Name = "ReportNumeroFactura";
            this.ReportNumeroFactura.Size = new System.Drawing.Size(396, 107);
            this.ReportNumeroFactura.TabIndex = 58;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(12, 538);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 18);
            this.label3.TabIndex = 59;
            this.label3.Text = "Mensualidad";
            // 
            // chkMensualidad
            // 
            this.chkMensualidad.AutoSize = true;
            this.chkMensualidad.Location = new System.Drawing.Point(47, 567);
            this.chkMensualidad.Name = "chkMensualidad";
            this.chkMensualidad.Size = new System.Drawing.Size(15, 14);
            this.chkMensualidad.TabIndex = 60;
            this.chkMensualidad.UseVisualStyleBackColor = true;
            // 
            // pnlCopiaFactura
            // 
            this.pnlCopiaFactura.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCopiaFactura.Controls.Add(this.ReportMensualidadPorPlaca);
            this.pnlCopiaFactura.Controls.Add(this.ReportMensualidad);
            this.pnlCopiaFactura.Controls.Add(this.ReportPlacaEntrada);
            this.pnlCopiaFactura.Controls.Add(this.ReportNumeroFactura);
            this.pnlCopiaFactura.Location = new System.Drawing.Point(12, 69);
            this.pnlCopiaFactura.Name = "pnlCopiaFactura";
            this.pnlCopiaFactura.Size = new System.Drawing.Size(511, 454);
            this.pnlCopiaFactura.TabIndex = 61;
            // 
            // ReportMensualidadPorPlaca
            // 
            reportDataSource2.Name = "DataSet1";
            reportDataSource2.Value = this.dataTable5BindingSource;
            this.ReportMensualidadPorPlaca.LocalReport.DataSources.Add(reportDataSource2);
            this.ReportMensualidadPorPlaca.LocalReport.ReportEmbeddedResource = "BlockAndPass.PPMWinform.Tickets.CopiaFacturaM.rdlc";
            this.ReportMensualidadPorPlaca.Location = new System.Drawing.Point(5, 336);
            this.ReportMensualidadPorPlaca.Name = "ReportMensualidadPorPlaca";
            this.ReportMensualidadPorPlaca.Size = new System.Drawing.Size(396, 96);
            this.ReportMensualidadPorPlaca.TabIndex = 61;
            // 
            // ReportMensualidad
            // 
            reportDataSource3.Name = "DataSet1";
            reportDataSource3.Value = this.dataTable2BindingSource;
            this.ReportMensualidad.LocalReport.DataSources.Add(reportDataSource3);
            this.ReportMensualidad.LocalReport.ReportEmbeddedResource = "BlockAndPass.PPMWinform.Tickets.CopiaFacturaM.rdlc";
            this.ReportMensualidad.Location = new System.Drawing.Point(5, 231);
            this.ReportMensualidad.Name = "ReportMensualidad";
            this.ReportMensualidad.Size = new System.Drawing.Size(396, 96);
            this.ReportMensualidad.TabIndex = 60;
            // 
            // ReportPlacaEntrada
            // 
            reportDataSource4.Name = "DataSet1";
            reportDataSource4.Value = this.dataTable4BindingSource;
            this.ReportPlacaEntrada.LocalReport.DataSources.Add(reportDataSource4);
            this.ReportPlacaEntrada.LocalReport.ReportEmbeddedResource = "BlockAndPass.PPMWinform.Tickets.CopiaFactura.rdlc";
            this.ReportPlacaEntrada.Location = new System.Drawing.Point(5, 116);
            this.ReportPlacaEntrada.Name = "ReportPlacaEntrada";
            this.ReportPlacaEntrada.Size = new System.Drawing.Size(396, 107);
            this.ReportPlacaEntrada.TabIndex = 59;
            // 
            // tbPlacaBuscar
            // 
            this.tbPlacaBuscar.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPlacaBuscar.Location = new System.Drawing.Point(378, 558);
            this.tbPlacaBuscar.Name = "tbPlacaBuscar";
            this.tbPlacaBuscar.Size = new System.Drawing.Size(134, 23);
            this.tbPlacaBuscar.TabIndex = 63;
            this.tbPlacaBuscar.TextChanged += new System.EventHandler(this.tbPlacaBuscar_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(382, 538);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 17);
            this.label5.TabIndex = 62;
            this.label5.Text = "Placa";
            // 
            // dataTable3TableAdapter
            // 
            this.dataTable3TableAdapter.ClearBeforeFill = true;
            // 
            // dataTable4TableAdapter
            // 
            this.dataTable4TableAdapter.ClearBeforeFill = true;
            // 
            // dataTable2TableAdapter
            // 
            this.dataTable2TableAdapter.ClearBeforeFill = true;
            // 
            // dataTable5TableAdapter
            // 
            this.dataTable5TableAdapter.ClearBeforeFill = true;
            // 
            // CopiaFactura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.ClientSize = new System.Drawing.Size(523, 645);
            this.ControlBox = false;
            this.Controls.Add(this.tbPlacaBuscar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pnlCopiaFactura);
            this.Controls.Add(this.chkMensualidad);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboIdModulo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.iconButton1);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbnumerofactura);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CopiaFactura";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.CopiaFactura_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataTable3BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCopia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable5BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable2BindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetCopiaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable4BindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlCopiaFactura.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbnumerofactura;
        private System.Windows.Forms.Label label1;
        //private Tickets.DataSetCopiaTableAdapters.DataTable1TableAdapter dataTable1TableAdapter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private FontAwesome.Sharp.IconButton iconButton1;
        private FontAwesome.Sharp.IconButton btn_Ok;
        private System.Windows.Forms.ComboBox cboIdModulo;
        private System.Windows.Forms.Label label2;
        private Microsoft.Reporting.WinForms.ReportViewer ReportNumeroFactura;
        private System.Windows.Forms.BindingSource dataTable3BindingSource;
        private Tickets.DataSetCopia dataSetCopia;
        private Tickets.DataSetCopiaTableAdapters.DataTable3TableAdapter dataTable3TableAdapter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkMensualidad;
        private System.Windows.Forms.Panel pnlCopiaFactura;
        private System.Windows.Forms.TextBox tbPlacaBuscar;
        private System.Windows.Forms.Label label5;
        private Microsoft.Reporting.WinForms.ReportViewer ReportPlacaEntrada;
        private System.Windows.Forms.BindingSource dataTable4BindingSource;
        private System.Windows.Forms.BindingSource dataSetCopiaBindingSource;
        private Tickets.DataSetCopiaTableAdapters.DataTable4TableAdapter dataTable4TableAdapter;
        private Microsoft.Reporting.WinForms.ReportViewer ReportMensualidad;
        private System.Windows.Forms.BindingSource dataTable2BindingSource;
        private Tickets.DataSetCopiaTableAdapters.DataTable2TableAdapter dataTable2TableAdapter;
        private Microsoft.Reporting.WinForms.ReportViewer ReportMensualidadPorPlaca;
        private System.Windows.Forms.BindingSource dataTable5BindingSource;
        private Tickets.DataSetCopiaTableAdapters.DataTable5TableAdapter dataTable5TableAdapter;
    }
}