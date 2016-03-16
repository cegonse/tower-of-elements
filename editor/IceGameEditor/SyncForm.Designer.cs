namespace IceGameEditor
{
    partial class SyncForm
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
            this.labelTotal = new System.Windows.Forms.Label();
            this.progressBarFile = new System.Windows.Forms.ProgressBar();
            this.labelFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sincronizando archivos...";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(9, 106);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(171, 17);
            this.labelTotal.TabIndex = 1;
            this.labelTotal.Text = "Progreso total: 0 ficheros.";
            // 
            // progressBarFile
            // 
            this.progressBarFile.Location = new System.Drawing.Point(12, 68);
            this.progressBarFile.Name = "progressBarFile";
            this.progressBarFile.Size = new System.Drawing.Size(587, 23);
            this.progressBarFile.Step = 1;
            this.progressBarFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarFile.TabIndex = 3;
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(12, 38);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(96, 17);
            this.labelFile.TabIndex = 4;
            this.labelFile.Text = "Conectando...";
            // 
            // SyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 140);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.progressBarFile);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SyncForm";
            this.ShowIcon = false;
            this.Text = "Sincronizando repositorio";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SyncForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.ProgressBar progressBarFile;
        private System.Windows.Forms.Label labelFile;
    }
}