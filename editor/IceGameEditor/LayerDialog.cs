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
    public partial class LayerDialog : Form
    {
        private MainForm _main;
        private Dictionary<int, bool> _layers;

        public LayerDialog(MainForm m)
        {
            InitializeComponent();
            _main = m;
        }

        private void LayerDialog_Load(object sender, EventArgs e)
        {
            _layers = _main.GetLayerList();

            foreach (KeyValuePair<int,bool> l in _layers)
            {
                if (!checkedListBoxLayers.Items.Contains(l.Key.ToString()))
                {
                    int i = checkedListBoxLayers.Items.Add(l.Key.ToString());
                    checkedListBoxLayers.SetItemChecked(i, l.Value);
                }
            }

            numericUpDownActiveLayer.Value = _main.GetActiveLayer();
        }

        private void LayerDialog_Leave(object sender, EventArgs e)
        {
            Hide();
        }

        private void checkedListBoxLayers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkedListBoxLayers.SelectedIndex != -1)
            {
                int layer = int.Parse((string)checkedListBoxLayers.Items[checkedListBoxLayers.SelectedIndex]);
                bool value = checkedListBoxLayers.GetItemChecked(checkedListBoxLayers.Items.IndexOf((layer.ToString())));

                _layers[layer] = !value;
                _main.GetDesigner().GetPanel().Refresh();
            }
        }

        private void numericUpDownActiveLayer_ValueChanged(object sender, EventArgs e)
        {
            int l = (int)numericUpDownActiveLayer.Value;

            if (l >= 0 || l != 99 || l != 100)
            {
                _main.SetActiveLayer(l);
            }
            else
            {
                numericUpDownActiveLayer.BackColor = Color.Red;
                toolTipLayer.Show("La capa activa debe de ser una de las\nexistentes (lista inferior).", numericUpDownActiveLayer);
            }
        }
    }
}
