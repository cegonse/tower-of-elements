namespace IceGameEditor
{
    partial class TextureListForm
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
            this.listBoxTextures = new System.Windows.Forms.ListBox();
            this.buttonCopy = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxTextures
            // 
            this.listBoxTextures.FormattingEnabled = true;
            this.listBoxTextures.ItemHeight = 16;
            this.listBoxTextures.Location = new System.Drawing.Point(12, 12);
            this.listBoxTextures.Name = "listBoxTextures";
            this.listBoxTextures.Size = new System.Drawing.Size(375, 260);
            this.listBoxTextures.TabIndex = 0;
            this.listBoxTextures.SelectedIndexChanged += new System.EventHandler(this.listBoxTextures_SelectedIndexChanged);
            // 
            // buttonCopy
            // 
            this.buttonCopy.Location = new System.Drawing.Point(461, 244);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new System.Drawing.Size(127, 29);
            this.buttonCopy.TabIndex = 2;
            this.buttonCopy.Text = "Copiar nombre";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(393, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(195, 183);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // TextureListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 285);
            this.Controls.Add(this.buttonCopy);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.listBoxTextures);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TextureListForm";
            this.Text = "Lista de Texturas";
            this.Load += new System.EventHandler(this.TextureListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxTextures;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button buttonCopy;
    }
}