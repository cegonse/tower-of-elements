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
    public partial class TextureListForm : Form
    {
        MainForm _main;
        Dictionary<string, Bitmap> _textures;

        public TextureListForm(MainForm m)
        {
            InitializeComponent();
            _main = m;
            _textures = _main.GetTextureList();
        }

        private void TextureListForm_Load(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, Bitmap> bmp in _textures)
            {
                listBoxTextures.Items.Add(bmp.Key);
            }
        }

        private void listBoxTextures_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox.Image = _textures[(string)listBoxTextures.Items[listBoxTextures.SelectedIndex]];
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (listBoxTextures.SelectedIndex >= 0)
            {
                Clipboard.SetText((string)listBoxTextures.Items[listBoxTextures.SelectedIndex]);
            }
        }
    }
}
