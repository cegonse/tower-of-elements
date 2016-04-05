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
            this.textBoxScpUser = new System.Windows.Forms.TextBox();
            this.textBoxScpPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxScpPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxScpAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBoxScp.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Introduce la ruta a la carpeta Resources:";
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(20, 31);
            this.textBoxPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(599, 22);
            this.textBoxPath.TabIndex = 1;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Location = new System.Drawing.Point(20, 63);
            this.buttonBrowse.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(100, 28);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Examinar";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(520, 214);
            this.buttonAccept.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(100, 28);
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
            this.groupBoxScp.Location = new System.Drawing.Point(20, 98);
            this.groupBoxScp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxScp.Name = "groupBoxScp";
            this.groupBoxScp.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxScp.Size = new System.Drawing.Size(600, 108);
            this.groupBoxScp.TabIndex = 4;
            this.groupBoxScp.TabStop = false;
            this.groupBoxScp.Text = "Parámetros SSH";
            // 
            // textBoxScpUser
            // 
            this.textBoxScpUser.Location = new System.Drawing.Point(393, 28);
            this.textBoxScpUser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxScpUser.Name = "textBoxScpUser";
            this.textBoxScpUser.Size = new System.Drawing.Size(185, 22);
            this.textBoxScpUser.TabIndex = 7;
            this.textBoxScpUser.Text = "jumble";
            // 
            // textBoxScpPass
            // 
            this.textBoxScpPass.Location = new System.Drawing.Point(393, 66);
            this.textBoxScpPass.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxScpPass.Name = "textBoxScpPass";
            this.textBoxScpPass.Size = new System.Drawing.Size(185, 22);
            this.textBoxScpPass.TabIndex = 6;
            this.textBoxScpPass.Text = "fortcraftrules";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(300, 70);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Contraseña:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(300, 32);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "Usuario:";
            // 
            // textBoxScpPort
            // 
            this.textBoxScpPort.Location = new System.Drawing.Point(89, 66);
            this.textBoxScpPort.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxScpPort.Name = "textBoxScpPort";
            this.textBoxScpPort.Size = new System.Drawing.Size(132, 22);
            this.textBoxScpPort.TabIndex = 3;
            this.textBoxScpPort.Text = "22";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Puerto:";
            // 
            // textBoxScpAddress
            // 
            this.textBoxScpAddress.Location = new System.Drawing.Point(89, 28);
            this.textBoxScpAddress.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxScpAddress.Name = "textBoxScpAddress";
            this.textBoxScpAddress.Size = new System.Drawing.Size(189, 22);
            this.textBoxScpAddress.TabIndex = 1;
            this.textBoxScpAddress.Text = "cesar.jumbledevs.net";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Dirección:";
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Selecciona la carpeta de recursos";
            this.folderBrowserDialog.ShowNewFolderButton = false;
            // 
            // ResourcesLocationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 254);
            this.Controls.Add(this.groupBoxScp);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.textBoxPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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