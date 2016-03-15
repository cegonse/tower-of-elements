namespace IceGameEditor
{
    partial class LayerDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownActiveLayer = new System.Windows.Forms.NumericUpDown();
            this.checkedListBoxLayers = new System.Windows.Forms.CheckedListBox();
            this.toolTipLayer = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownActiveLayer)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Capa activa:";
            // 
            // numericUpDownActiveLayer
            // 
            this.numericUpDownActiveLayer.Location = new System.Drawing.Point(15, 29);
            this.numericUpDownActiveLayer.Name = "numericUpDownActiveLayer";
            this.numericUpDownActiveLayer.Size = new System.Drawing.Size(120, 22);
            this.numericUpDownActiveLayer.TabIndex = 1;
            this.numericUpDownActiveLayer.ValueChanged += new System.EventHandler(this.numericUpDownActiveLayer_ValueChanged);
            // 
            // checkedListBoxLayers
            // 
            this.checkedListBoxLayers.CheckOnClick = true;
            this.checkedListBoxLayers.FormattingEnabled = true;
            this.checkedListBoxLayers.Location = new System.Drawing.Point(15, 57);
            this.checkedListBoxLayers.Name = "checkedListBoxLayers";
            this.checkedListBoxLayers.Size = new System.Drawing.Size(186, 174);
            this.checkedListBoxLayers.Sorted = true;
            this.checkedListBoxLayers.TabIndex = 2;
            this.checkedListBoxLayers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxLayers_ItemCheck);
            // 
            // toolTipLayer
            // 
            this.toolTipLayer.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Error;
            this.toolTipLayer.ToolTipTitle = "La capa debe existir";
            // 
            // LayerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(211, 253);
            this.Controls.Add(this.checkedListBoxLayers);
            this.Controls.Add(this.numericUpDownActiveLayer);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LayerDialog";
            this.Text = "Capas";
            this.Load += new System.EventHandler(this.LayerDialog_Load);
            this.Leave += new System.EventHandler(this.LayerDialog_Leave);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownActiveLayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownActiveLayer;
        private System.Windows.Forms.CheckedListBox checkedListBoxLayers;
        private System.Windows.Forms.ToolTip toolTipLayer;
    }
}