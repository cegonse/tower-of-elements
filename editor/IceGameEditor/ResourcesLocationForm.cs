using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IceGameEditor
{
    public partial class ResourcesLocationForm : Form
    {
        private MainForm _main;

        public ResourcesLocationForm(MainForm mf)
        {
            InitializeComponent();
            _main = mf;
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowDialog();
            textBoxPath.Text = folderBrowserDialog.SelectedPath;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            _main.SetSettings(textBoxPath.Text, textBoxScpAddress.Text,
                int.Parse(textBoxScpPort.Text), textBoxScpUser.Text,
                textBoxScpPass.Text);

            Close();
        }
    }
}
