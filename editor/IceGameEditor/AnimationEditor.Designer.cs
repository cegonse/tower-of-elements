﻿namespace IceGameEditor
{
    partial class AnimationEditor
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
            this.listBoxFrames = new System.Windows.Forms.ListBox();
            this.buttonAddFrame = new System.Windows.Forms.Button();
            this.buttonRemoveFrame = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxTextureName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFrameTime = new System.Windows.Forms.TextBox();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.buttonTextureList = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxRandomInterval = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Lista de frames:";
            // 
            // listBoxFrames
            // 
            this.listBoxFrames.FormattingEnabled = true;
            this.listBoxFrames.Location = new System.Drawing.Point(9, 32);
            this.listBoxFrames.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxFrames.Name = "listBoxFrames";
            this.listBoxFrames.Size = new System.Drawing.Size(132, 238);
            this.listBoxFrames.TabIndex = 1;
            this.listBoxFrames.SelectedIndexChanged += new System.EventHandler(this.listBoxFrames_SelectedIndexChanged);
            // 
            // buttonAddFrame
            // 
            this.buttonAddFrame.Location = new System.Drawing.Point(9, 274);
            this.buttonAddFrame.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAddFrame.Name = "buttonAddFrame";
            this.buttonAddFrame.Size = new System.Drawing.Size(27, 28);
            this.buttonAddFrame.TabIndex = 2;
            this.buttonAddFrame.Text = "+";
            this.buttonAddFrame.UseVisualStyleBackColor = true;
            this.buttonAddFrame.Click += new System.EventHandler(this.buttonAddFrame_Click);
            // 
            // buttonRemoveFrame
            // 
            this.buttonRemoveFrame.Location = new System.Drawing.Point(40, 275);
            this.buttonRemoveFrame.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRemoveFrame.Name = "buttonRemoveFrame";
            this.buttonRemoveFrame.Size = new System.Drawing.Size(27, 27);
            this.buttonRemoveFrame.TabIndex = 3;
            this.buttonRemoveFrame.Text = "-";
            this.buttonRemoveFrame.UseVisualStyleBackColor = true;
            this.buttonRemoveFrame.Click += new System.EventHandler(this.buttonRemoveFrame_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(154, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Textura:";
            // 
            // textBoxTextureName
            // 
            this.textBoxTextureName.Location = new System.Drawing.Point(157, 48);
            this.textBoxTextureName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxTextureName.Name = "textBoxTextureName";
            this.textBoxTextureName.Size = new System.Drawing.Size(178, 20);
            this.textBoxTextureName.TabIndex = 5;
            this.textBoxTextureName.TextChanged += new System.EventHandler(this.textBoxTextureName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(154, 76);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Tiempo siguiente frame:";
            // 
            // textBoxFrameTime
            // 
            this.textBoxFrameTime.Location = new System.Drawing.Point(157, 92);
            this.textBoxFrameTime.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxFrameTime.Name = "textBoxFrameTime";
            this.textBoxFrameTime.Size = new System.Drawing.Size(178, 20);
            this.textBoxFrameTime.TabIndex = 7;
            this.textBoxFrameTime.TextChanged += new System.EventHandler(this.textBoxFrameTime_TextChanged);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPlay.Location = new System.Drawing.Point(305, 236);
            this.buttonPlay.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(28, 32);
            this.buttonPlay.TabIndex = 9;
            this.buttonPlay.Text = "►";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(278, 275);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(56, 27);
            this.buttonSave.TabIndex = 10;
            this.buttonSave.Text = "Guardar";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(157, 169);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(99, 100);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            // 
            // buttonTextureList
            // 
            this.buttonTextureList.Location = new System.Drawing.Point(91, 275);
            this.buttonTextureList.Margin = new System.Windows.Forms.Padding(2);
            this.buttonTextureList.Name = "buttonTextureList";
            this.buttonTextureList.Size = new System.Drawing.Size(110, 27);
            this.buttonTextureList.TabIndex = 11;
            this.buttonTextureList.Text = "Lista de texturas";
            this.buttonTextureList.UseVisualStyleBackColor = true;
            this.buttonTextureList.Click += new System.EventHandler(this.button1_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "txt";
            this.saveFileDialog.Title = "Guardar animación";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(206, 275);
            this.buttonLoad.Margin = new System.Windows.Forms.Padding(2);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(68, 27);
            this.buttonLoad.TabIndex = 12;
            this.buttonLoad.Text = "Cargar";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Animación|*.txt";
            this.openFileDialog.Title = "Cargar animación";
            // 
            // checkBoxRandomInterval
            // 
            this.checkBoxRandomInterval.AutoSize = true;
            this.checkBoxRandomInterval.Location = new System.Drawing.Point(157, 118);
            this.checkBoxRandomInterval.Name = "checkBoxRandomInterval";
            this.checkBoxRandomInterval.Size = new System.Drawing.Size(111, 17);
            this.checkBoxRandomInterval.TabIndex = 13;
            this.checkBoxRandomInterval.Text = "Intervalo Aleatorio";
            this.checkBoxRandomInterval.UseVisualStyleBackColor = true;
            // 
            // AnimationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 311);
            this.Controls.Add(this.checkBoxRandomInterval);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonTextureList);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.textBoxFrameTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTextureName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonRemoveFrame);
            this.Controls.Add(this.buttonAddFrame);
            this.Controls.Add(this.listBoxFrames);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AnimationEditor";
            this.Text = "Editor de Animaciones";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxFrames;
        private System.Windows.Forms.Button buttonAddFrame;
        private System.Windows.Forms.Button buttonRemoveFrame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxTextureName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxFrameTime;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonTextureList;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.CheckBox checkBoxRandomInterval;
    }
}