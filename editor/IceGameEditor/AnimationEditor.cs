using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace IceGameEditor
{
    public struct AnimationFrame
    {
        public string texture;
        public float time;
    }

    public partial class AnimationEditor : Form
    {
        MainForm _main;
        List<AnimationFrame> _frames;
        int _frame = 0;
        bool _playing = false;

        public AnimationEditor(MainForm m)
        {
            InitializeComponent();
            _main = m;
            _frames = new List<AnimationFrame>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TextureListForm txl = new TextureListForm(_main);
            txl.Show();
        }

        private void buttonAddFrame_Click(object sender, EventArgs e)
        {
            AnimationFrame frame = new AnimationFrame();
            _frames.Add(frame);
            listBoxFrames.Items.Add("Frame " + _frames.Count.ToString());
        }

        private void buttonRemoveFrame_Click(object sender, EventArgs e)
        {
            if (_playing)
            {
                timer.Stop();
                _playing = false;
            }

            if (listBoxFrames.SelectedIndex >= 0)
            {
                _frames.RemoveAt(listBoxFrames.SelectedIndex);
                listBoxFrames.Items.Clear();

                for (int i = 0; i < _frames.Count; i++)
                {
                    listBoxFrames.Items.Add("Frame " + (i + 1).ToString());
                }
            }
        }

        private void listBoxFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFrames.SelectedIndex >= 0)
            {
                textBoxFrameTime.Text = _frames[listBoxFrames.SelectedIndex].time.ToString();
                textBoxTextureName.Text = _frames[listBoxFrames.SelectedIndex].texture;

                if (_main.GetTextureList().ContainsKey(textBoxTextureName.Text))
                {
                    pictureBox.Image = _main.GetTextureList()[textBoxTextureName.Text];
                }
            }
        }

        private void textBoxTextureName_TextChanged(object sender, EventArgs e)
        {
            if (listBoxFrames.SelectedIndex >= 0)
            {
                textBoxTextureName.BackColor = Color.White;

                if (!_main.GetTextureList().ContainsKey(textBoxTextureName.Text))
                {
                    textBoxTextureName.BackColor = Color.Red;
                }
                else
                {
                    AnimationFrame ff = _frames[listBoxFrames.SelectedIndex];
                    ff.texture = textBoxTextureName.Text;
                    _frames[listBoxFrames.SelectedIndex] = ff;

                    pictureBox.Image = _main.GetTextureList()[textBoxTextureName.Text];
                }
            }
        }

        private void textBoxFrameTime_TextChanged(object sender, EventArgs e)
        {
            if (listBoxFrames.SelectedIndex >= 0)
            {
                try
                {
                    textBoxFrameTime.BackColor = Color.White;
                    float t = float.Parse(textBoxFrameTime.Text);

                    AnimationFrame ff = _frames[listBoxFrames.SelectedIndex];
                    ff.time = t;
                    _frames[listBoxFrames.SelectedIndex] = ff;
                }
                catch
                {
                    textBoxFrameTime.BackColor = Color.Red;
                }
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (_playing)
            {
                buttonPlay.Text = "►";
                timer.Stop();
                _playing = false;
            }
            else
            {
                if ((int)(_frames[_frame].time * 1000f) > 0 && !string.IsNullOrEmpty(_frames[_frame].texture))
                {
                    buttonPlay.Text = "■";
                    timer.Interval = (int)(_frames[_frame].time * 1000f);
                    pictureBox.Image = _main.GetTextureList()[_frames[_frame].texture];

                    timer.Start();
                    _playing = true;
                }
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_frame == _frames.Count - 1)
            {
                _frame = 0;
            }
            else
            {
                _frame++;
            }

            pictureBox.Image = _main.GetTextureList()[_frames[_frame].texture];
            timer.Interval = (int)(_frames[_frame].time * 1000f);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            JSONObject json = new JSONObject();
            JSONObject jsonAnim = new JSONObject(JSONObject.Type.ARRAY);

            json.AddField("interval", (int)(checkBoxRandomInterval.Checked == true ? 1 : 0));

            for (int i = 0; i < _frames.Count; i++)
            {
                JSONObject jsonFrame = new JSONObject(JSONObject.Type.OBJECT);
                jsonFrame.AddField("texture", _frames[i].texture);

                string tt = _frames[i].time.ToString();

                if (tt.Contains(','))
                {
                    tt.Replace(',', '.');
                }

                jsonFrame.AddField("time", tt);
                jsonAnim.Add(jsonFrame);
            }

            json.AddField("animation", jsonAnim);

            saveFileDialog.ShowDialog();

            try
            {
                File.WriteAllText(saveFileDialog.FileName, json.Print(true));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error guardando la animación");
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();

            try
            {
                string data = File.ReadAllText(openFileDialog.FileName);
                JSONObject json = new JSONObject(data);

                if (json["interval"] != null)
                {
                    checkBoxRandomInterval.Checked = ((int)json["interval"].n) == 1 ? true : false;
                }

                JSONObject jsonAnim = json["animation"];
                _frames.Clear();
                listBoxFrames.Items.Clear();

                for (int i = 0; i < jsonAnim.Count; i++)
                {
                    JSONObject jsonFrame = jsonAnim[i];
                    AnimationFrame frame = new AnimationFrame();

                    frame.texture = jsonFrame["texture"].str;
                    frame.time = float.Parse(jsonFrame["time"].str);

                    _frames.Add(frame);
                    listBoxFrames.Items.Add("Frame " + (i + 1).ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error cargando la animación");
            }
        }
    }
}
