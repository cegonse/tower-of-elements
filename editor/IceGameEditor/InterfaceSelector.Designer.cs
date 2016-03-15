namespace IceGameEditor
{
    partial class InterfaceSelector
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
            this.listBoxAddresses = new System.Windows.Forms.ListBox();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBoxAddresses
            // 
            this.listBoxAddresses.FormattingEnabled = true;
            this.listBoxAddresses.ItemHeight = 16;
            this.listBoxAddresses.Location = new System.Drawing.Point(12, 12);
            this.listBoxAddresses.Name = "listBoxAddresses";
            this.listBoxAddresses.Size = new System.Drawing.Size(479, 164);
            this.listBoxAddresses.TabIndex = 0;
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(405, 190);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(86, 27);
            this.buttonAccept.TabIndex = 1;
            this.buttonAccept.Text = "Aceptar";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // InterfaceSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 229);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.listBoxAddresses);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InterfaceSelector";
            this.Text = "Selecciona la interfaz de red";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InterfaceSelector_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxAddresses;
        private System.Windows.Forms.Button buttonAccept;

    }
}