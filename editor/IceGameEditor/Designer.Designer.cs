namespace IceGameEditor
{
    partial class Designer
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
            this.panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(989, 697);
            this.panel.TabIndex = 0;
            this.panel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panel_Scroll);
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            this.panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_MouseClick);
            this.panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_MouseMove);
            this.panel.Resize += new System.EventHandler(this.panel_Resize);
            // 
            // Designer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(989, 697);
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Designer";
            this.Text = "Designer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Designer_FormClosing);
            this.Load += new System.EventHandler(this.Designer_Load);
            this.MdiChildActivate += new System.EventHandler(this.Designer_MdiChildActivate);
            this.VisibleChanged += new System.EventHandler(this.Designer_VisibleChanged);
            this.Resize += new System.EventHandler(this.Designer_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel;
    }
}