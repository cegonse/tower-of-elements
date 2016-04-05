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
            this.progressBarOverall = new System.Windows.Forms.ProgressBar();
            this.labelOverall = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sincronizando archivos...";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(7, 86);
            this.labelTotal.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(127, 13);
            this.labelTotal.TabIndex = 1;
            this.labelTotal.Text = "Progreso total: 0 ficheros.";
            // 
            // progressBarFile
            // 
            this.progressBarFile.Location = new System.Drawing.Point(9, 55);
            this.progressBarFile.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBarFile.Name = "progressBarFile";
            this.progressBarFile.Size = new System.Drawing.Size(440, 19);
            this.progressBarFile.Step = 1;
            this.progressBarFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarFile.TabIndex = 3;
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(9, 31);
            this.labelFile.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(74, 13);
            this.labelFile.TabIndex = 4;
            this.labelFile.Text = "Conectando...";
            // 
            // progressBarOverall
            // 
            this.progressBarOverall.Location = new System.Drawing.Point(9, 113);
            this.progressBarOverall.Name = "progressBarOverall";
            this.progressBarOverall.Size = new System.Drawing.Size(376, 23);
            this.progressBarOverall.Step = 1;
            this.progressBarOverall.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarOverall.TabIndex = 5;
            // 
            // labelOverall
            // 
            this.labelOverall.AutoSize = true;
            this.labelOverall.Location = new System.Drawing.Point(405, 118);
            this.labelOverall.Name = "labelOverall";
            this.labelOverall.Size = new System.Drawing.Size(21, 13);
            this.labelOverall.TabIndex = 6;
            this.labelOverall.Text = "0%";
            // 
            // SyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 151);
            this.Controls.Add(this.labelOverall);
            this.Controls.Add(this.progressBarOverall);
            this.Controls.Add(this.labelFile);
            this.Controls.Add(this.progressBarFile);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.ProgressBar progressBarOverall;
        private System.Windows.Forms.Label labelOverall;
    }
}