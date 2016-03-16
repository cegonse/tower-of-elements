namespace IceGameEditor
{
    partial class ResourcesLocationForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.groupBoxScp = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxScpAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxScpPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxScpPass = new System.Windows.Forms.TextBox();
            this.textBoxScpUser = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxScp.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Introduce la ruta a la carpeta Resources:";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(15, 25);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(450, 20);
            this.textBoxPath.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(15, 51);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Examinar";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(390, 174);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 3;
            this.buttonAccept.Text = "Aceptar";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // groupBoxScp
            // 
            this.groupBoxScp.Controls.Add(this.textBoxScpUser);
            this.groupBoxScp.Controls.Add(this.textBoxScpPass);
            this.groupBoxScp.Controls.Add(this.label5);
            this.groupBoxScp.Controls.Add(this.label4);
            this.groupBoxScp.Controls.Add(this.textBoxScpPort);
            this.groupBoxScp.Controls.Add(this.label3);
            this.groupBoxScp.Controls.Add(this.textBoxScpAddress);
            this.groupBoxScp.Controls.Add(this.label2);
            this.groupBoxScp.Location = new System.Drawing.Point(15, 80);
            this.groupBoxScp.Name = "groupBoxScp";
            this.groupBoxScp.Size = new System.Drawing.Size(450, 88);
            this.groupBoxScp.TabIndex = 4;
            this.groupBoxScp.TabStop = false;
            this.groupBoxScp.Text = "Parámetros SSH";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Dirección:";
            // 
            // textBoxScpAddress
            // 
            this.textBoxScpAddress.Location = new System.Drawing.Point(67, 23);
            this.textBoxScpAddress.Name = "textBoxScpAddress";
            this.textBoxScpAddress.Size = new System.Drawing.Size(143, 20);
            this.textBoxScpAddress.TabIndex = 1;
            this.textBoxScpAddress.Text = "cesar.jumbledevs.net";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Puerto:";
            // 
            // textBoxScpPort
            // 
            this.textBoxScpPort.Location = new System.Drawing.Point(67, 54);
            this.textBoxScpPort.Name = "textBoxScpPort";
            this.textBoxScpPort.Size = new System.Drawing.Size(100, 20);
            this.textBoxScpPort.TabIndex = 3;
            this.textBoxScpPort.Text = "22";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(225, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Usuario:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(225, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Contraseña:";
            // 
            // textBoxScpPass
            // 
            this.textBoxScpPass.Location = new System.Drawing.Point(295, 54);
            this.textBoxScpPass.Name = "textBoxScpPass";
            this.textBoxScpPass.Size = new System.Drawing.Size(140, 20);
            this.textBoxScpPass.TabIndex = 6;
            // 
            // textBoxScpUser
            // 
            this.textBoxScpUser.Location = new System.Drawing.Point(295, 23);
            this.textBoxScpUser.Name = "textBoxScpUser";
            this.textBoxScpUser.Size = new System.Drawing.Size(140, 20);
            this.textBoxScpUser.TabIndex = 7;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Selecciona la carpeta de recursos";
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // ResourcesLocationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 206);
            this.Controls.Add(this.groupBoxScp);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResourcesLocationForm";
            this.ShowIcon = false;
            this.Text = "Configuración Inicial";
            this.TopMost = true;
            this.groupBoxScp.ResumeLayout(false);
            this.groupBoxScp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.GroupBox groupBoxScp;
        private System.Windows.Forms.TextBox textBoxScpUser;
        private System.Windows.Forms.TextBox textBoxScpPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxScpPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxScpAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}