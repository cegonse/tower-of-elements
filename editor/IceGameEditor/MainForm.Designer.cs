namespace IceGameEditor
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nuevoNivelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cargarNivelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guardarNivelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compilarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.probarEnLocalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.probarEnRemotoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.herramientasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editorDeAnimacionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.capasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ayudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(57)))), ((int)(((byte)(85)))));
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.compilarToolStripMenuItem,
            this.herramientasToolStripMenuItem,
            this.ayudaToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(806, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            this.menuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip_ItemClicked);
            // 
            // archivoToolStripMenuItem
            // 
            this.archivoToolStripMenuItem.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nuevoNivelToolStripMenuItem,
            this.cargarNivelToolStripMenuItem,
            this.guardarNivelToolStripMenuItem,
            this.toolStripSeparator1,
            this.salirToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // nuevoNivelToolStripMenuItem
            // 
            this.nuevoNivelToolStripMenuItem.Name = "nuevoNivelToolStripMenuItem";
            this.nuevoNivelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.nuevoNivelToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.nuevoNivelToolStripMenuItem.Text = "Nuevo Nivel";
            this.nuevoNivelToolStripMenuItem.Click += new System.EventHandler(this.nuevoNivelToolStripMenuItem_Click);
            // 
            // cargarNivelToolStripMenuItem
            // 
            this.cargarNivelToolStripMenuItem.Name = "cargarNivelToolStripMenuItem";
            this.cargarNivelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.cargarNivelToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.cargarNivelToolStripMenuItem.Text = "Cargar Nivel";
            this.cargarNivelToolStripMenuItem.Click += new System.EventHandler(this.cargarNivelToolStripMenuItem_Click);
            // 
            // guardarNivelToolStripMenuItem
            // 
            this.guardarNivelToolStripMenuItem.Name = "guardarNivelToolStripMenuItem";
            this.guardarNivelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.guardarNivelToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.guardarNivelToolStripMenuItem.Text = "Guardar Nivel";
            this.guardarNivelToolStripMenuItem.Click += new System.EventHandler(this.guardarNivelToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // compilarToolStripMenuItem
            // 
            this.compilarToolStripMenuItem.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.compilarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.probarEnLocalToolStripMenuItem,
            this.probarEnRemotoToolStripMenuItem});
            this.compilarToolStripMenuItem.Name = "compilarToolStripMenuItem";
            this.compilarToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.compilarToolStripMenuItem.Text = "Compilar";
            // 
            // probarEnLocalToolStripMenuItem
            // 
            this.probarEnLocalToolStripMenuItem.Name = "probarEnLocalToolStripMenuItem";
            this.probarEnLocalToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.probarEnLocalToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.probarEnLocalToolStripMenuItem.Text = "Probar en Local";
            this.probarEnLocalToolStripMenuItem.Click += new System.EventHandler(this.probarEnLocalToolStripMenuItem_Click);
            // 
            // probarEnRemotoToolStripMenuItem
            // 
            this.probarEnRemotoToolStripMenuItem.Name = "probarEnRemotoToolStripMenuItem";
            this.probarEnRemotoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F5)));
            this.probarEnRemotoToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.probarEnRemotoToolStripMenuItem.Text = "Probar en Remoto";
            this.probarEnRemotoToolStripMenuItem.Click += new System.EventHandler(this.probarEnRemotoToolStripMenuItem_Click);
            // 
            // herramientasToolStripMenuItem
            // 
            this.herramientasToolStripMenuItem.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.herramientasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editorDeAnimacionesToolStripMenuItem,
            this.capasToolStripMenuItem});
            this.herramientasToolStripMenuItem.Name = "herramientasToolStripMenuItem";
            this.herramientasToolStripMenuItem.Size = new System.Drawing.Size(90, 20);
            this.herramientasToolStripMenuItem.Text = "Herramientas";
            // 
            // editorDeAnimacionesToolStripMenuItem
            // 
            this.editorDeAnimacionesToolStripMenuItem.Name = "editorDeAnimacionesToolStripMenuItem";
            this.editorDeAnimacionesToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.editorDeAnimacionesToolStripMenuItem.Text = "Editor de Animaciones";
            this.editorDeAnimacionesToolStripMenuItem.Click += new System.EventHandler(this.editorDeAnimacionesToolStripMenuItem_Click);
            // 
            // capasToolStripMenuItem
            // 
            this.capasToolStripMenuItem.Name = "capasToolStripMenuItem";
            this.capasToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.capasToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.capasToolStripMenuItem.Text = "Capas";
            this.capasToolStripMenuItem.Click += new System.EventHandler(this.capasToolStripMenuItem_Click);
            // 
            // ayudaToolStripMenuItem
            // 
            this.ayudaToolStripMenuItem.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.ayudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.acercaDeToolStripMenuItem});
            this.ayudaToolStripMenuItem.Name = "ayudaToolStripMenuItem";
            this.ayudaToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.ayudaToolStripMenuItem.Text = "Ayuda";
            // 
            // acercaDeToolStripMenuItem
            // 
            this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.acercaDeToolStripMenuItem.Text = "Acerca de";
            this.acercaDeToolStripMenuItem.Click += new System.EventHandler(this.acercaDeToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Niveles|*.txt";
            // 
            // dockPanel
            // 
            this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel.Location = new System.Drawing.Point(0, 24);
            this.dockPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dockPanel.Name = "dockPanel";
            this.dockPanel.Size = new System.Drawing.Size(806, 511);
            this.dockPanel.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(806, 535);
            this.Controls.Add(this.dockPanel);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "MainForm";
            this.Text = "Editor de Frozen Cubes";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cargarNivelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarNivelToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ToolStripMenuItem compilarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem probarEnLocalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem probarEnRemotoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nuevoNivelToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem ayudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem acercaDeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem herramientasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editorDeAnimacionesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem capasToolStripMenuItem;
    }
}

