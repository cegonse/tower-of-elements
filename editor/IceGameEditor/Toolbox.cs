using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace IceGameEditor
{
    public partial class Toolbox : DockContent
    {
        MainForm _main;

        List<Button> _tools;
        List<Button> _backgroundTools;
        List<Button> _enemyTools;

        Level _activeLevel;
        Vector2 _selectedBlockPosition;
        Vector3 _selectedBackgroundPosition;
        Vector2 _selectedEnemyPosition;

        BlockData _selectedBlock;
        BackgroundData _selectedBackground;
        EnemyData _selectedEnemy;

        public Toolbox(MainForm main)
        {
            InitializeComponent();
            CloseButtonVisible = false;

            _main = main;
            _tools = new List<Button>();
            _backgroundTools = new List<Button>();
            _enemyTools = new List<Button>();
        }

        public void AddBlockTool(string nombre, Bitmap tex)
        {
            Button b = new Button();
            _tools.Add(b);

            Size sz = new Size(250, 50);
            b.Size = sz;
            Point pt = new Point(0, (_tools.Count-1) * 50 + 5);
            b.Location = pt;
            b.Dock = DockStyle.Top;

            b.Text = nombre;
            b.Image = tex;
            b.TextImageRelation = TextImageRelation.Overlay;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Click += OnToolClick;

            panelBlocks.Controls.Add(b);
        }

        public void AddBackgroundTool(string nombre, Bitmap tex)
        {
            Button b = new Button();
            _backgroundTools.Add(b);

            Size sz = new Size(250, 50);
            b.Size = sz;
            Point pt = new Point(0, (_backgroundTools.Count - 1) * 50 + 5);
            b.Location = pt;
            b.Dock = DockStyle.Top;

            b.Text = nombre;
            b.Image = tex;
            b.TextImageRelation = TextImageRelation.Overlay;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Click += OnBackgroundToolClick;

            panelBackgrounds.Controls.Add(b);
        }

        public void AddEnemyTool(string nombre, Bitmap tex)
        {
            Button b = new Button();
            _enemyTools.Add(b);

            Size sz = new Size(250, 50);
            b.Size = sz;
            Point pt = new Point(0, (_tools.Count - 1) * 50 + 5);
            b.Location = pt;
            b.Dock = DockStyle.Top;

            b.Text = nombre;
            b.Image = tex;
            b.TextImageRelation = TextImageRelation.Overlay;
            b.TextAlign = ContentAlignment.MiddleLeft;
            b.Click += OnEnemyToolClick;

            panelEnemies.Controls.Add(b);
        }

        private void OnToolClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            
            if (b.Text.Contains("Eliminar"))
            {
                _main.OnDeleteToolPressed();
            }
            else if (b.Text.Contains("Seleccionar"))
            {
                _main.OnSelectToolPressed();
            }
            else
            {
                _main.OnBlockSelected(b.Text);
            }
        }

        private void OnBackgroundToolClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b.Text.Contains("Eliminar"))
            {
                _main.OnBackgroundDeleteToolPressed();
            }
            else if (b.Text.Contains("Seleccionar"))
            {
                _main.OnBackgroundSelectToolPressed();
            }
            else
            {
                _main.OnBackgroundSelected(b.Text);
            }
        }

        private void OnEnemyToolClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;

            if (b.Text.Contains("Eliminar"))
            {
                _main.OnEnemyDeleteToolPressed();
            }
            else if (b.Text.Contains("Seleccionar"))
            {
                _main.OnEnemySelectToolPressed();
            }
            else if (b.Text.Contains("Volador"))
            {
                _main.OnFlyerEnemySelected();
            }
            else if (b.Text.Contains("Caminador"))
            {
                _main.OnWalkerEnemySelected();
            }
            else if (b.Text.Contains("Roamer"))
            {
                _main.OnRoamerEnemySelected();
            }
            else if (b.Text.Contains("Palanca"))
            {
                _main.OnLeverEnemySelected();
            }
        }

        private void buttonBlockLightColor_Click(object sender, EventArgs e)
        {
            colorDialogBlockLight.ShowDialog();
            Color sc = colorDialogBlockLight.Color;
            buttonBlockLightColor.BackColor = sc;
            buttonBlockLightColor.ForeColor = Color.FromArgb(sc.ToArgb() ^ 0xffffff);

            _selectedBlock.light.color_r = (float)sc.R / 255f;
            _selectedBlock.light.color_g = (float)sc.G / 255f;
            _selectedBlock.light.color_b = (float)sc.B / 255f;
        }

        public void SetActiveLevel(Level l)
        {
            _activeLevel = l;
        }

        public void UpdateLevelData()
        {
            Level l = _activeLevel;

            textBoxLevelName.Text = l.GetName();
            numericUpDownDifficulty.Value = l.GetDifficulty();
            checkBoxBlockIlluminated.Checked = l.GetIlluminated();
            textBoxSong.Text = l.GetSong();

            PlayerData pl = l.GetPlayer();

            textBoxPlayerX.Text = pl.x.ToString();
            textBoxPlayerY.Text = pl.y.ToString();

            numericUpDownFireUses.Value = pl.fire;
            numericUpDownWindUses.Value = pl.wind;
            numericUpDownIceUses.Value = pl.ice;
            numericUpDownEarthUses.Value = pl.earth;

            Door d = l.GetDoor();

            textBoxDoorX.Text = d.x.ToString();
            textBoxDoorY.Text = d.y.ToString();
            textBoxTargetLevel.Text = d.targetLevel;
        }

        private void textBoxLevelName_TextChanged(object sender, EventArgs e)
        {
            _activeLevel.SetPublicName(textBoxLevelName.Text);
            _main.SetTitle(_activeLevel.GetPublicName());
        }

        private void numericUpDownDifficulty_ValueChanged(object sender, EventArgs e)
        {
            _activeLevel.SetDifficulty((int)numericUpDownDifficulty.Value);
        }

        private void checkBoxIlluminated_CheckedChanged(object sender, EventArgs e)
        {
            _activeLevel.SetIlluminated(checkBoxIlluminated.Checked);
        }

        private void textBoxSong_TextChanged(object sender, EventArgs e)
        {
            _activeLevel.SetSong(textBoxSong.Text);
        }

        private void textBoxPlayerX_TextChanged(object sender, EventArgs e)
        {
            PlayerData p = _activeLevel.GetPlayer();

            try
            {
                p.x = int.Parse(textBoxPlayerX.Text);
                textBoxPlayerX.BackColor = Color.White;
            }
            catch
            {
                textBoxPlayerX.BackColor = Color.Red;
            }

            _activeLevel.SetPlayer(p);
            _main.GetDesigner().Refresh();
        }

        private void textBoxPlayerY_TextChanged(object sender, EventArgs e)
        {
            PlayerData p = _activeLevel.GetPlayer();

            try
            {
                p.y = int.Parse(textBoxPlayerY.Text);
                textBoxPlayerY.BackColor = Color.White;
            }
            catch
            {
                textBoxPlayerY.BackColor = Color.Red;
            }

            _activeLevel.SetPlayer(p);
            _main.GetDesigner().Refresh();
        }

        private void numericUpDownFireUses_ValueChanged(object sender, EventArgs e)
        {
            PlayerData p = _activeLevel.GetPlayer();
            p.fire = (int)numericUpDownFireUses.Value;
            _activeLevel.SetPlayer(p);
        }

        private void numericUpDownIceUses_ValueChanged(object sender, EventArgs e)
        {
            PlayerData p = _activeLevel.GetPlayer();
            p.ice = (int)numericUpDownIceUses.Value;
            _activeLevel.SetPlayer(p);
        }

        private void numericUpDownEarthUses_ValueChanged(object sender, EventArgs e)
        {
            PlayerData p = _activeLevel.GetPlayer();
            p.earth = (int)numericUpDownEarthUses.Value;
            _activeLevel.SetPlayer(p);
        }

        private void numericUpDownWindUses_ValueChanged(object sender, EventArgs e)
        {
            PlayerData p = _activeLevel.GetPlayer();
            p.wind = (int)numericUpDownWindUses.Value;
            _activeLevel.SetPlayer(p);
        }

        public void OnBlockSelected()
        {
            Vector2 sb = _main.GetDesigner().GetSelectedBlock();
            sb.y *= -1;

            if (_main.GetActiveLevel().GetBlockList().ContainsKey(sb))
            {
                _selectedBlock = _main.GetActiveLevel().GetBlockList()[sb];
                _selectedBlockPosition = sb;

                textBoxBlockPosX.Text = sb.x.ToString();
                textBoxBlockPosY.Text = sb.y.ToString();
                comboBoxBlockType.SelectedIndex = (int)_selectedBlock.type;
                textBoxBlockWalkSound.Text = _selectedBlock.walk_sound;
                textBoxBlockDestructionSound.Text = _selectedBlock.destruction_sound;
                textBoxBlockMoveSound.Text = _selectedBlock.move_sound;
                textBoxBlockLightRadius.Text = _selectedBlock.light.radius.ToString();
                textBoxIceLength.Text = _selectedBlock.length.ToString();

                Color cl = Color.FromArgb((int)(255f * _selectedBlock.light.color_r),
                    (int)(255f * _selectedBlock.light.color_g),
                    (int)(255f * _selectedBlock.light.color_b));

                buttonBlockLightColor.BackColor = cl;
                buttonBlockLightColor.ForeColor = Color.FromArgb(cl.ToArgb() ^ 0xffffff);
            }
        }

        public void OnBackgroundBlockSelected()
        {
            Vector3 sb = _main.GetDesigner().GetSelectedBackground();
            sb.y *= -1;

            if (_main.GetActiveLevel().GetBackgroundList().ContainsKey(sb))
            {
                _selectedBackground = _main.GetActiveLevel().GetBackgroundList()[sb];
                _selectedBackgroundPosition = sb;

                textBoxBackgroundX.Text = sb.x.ToString();
                textBoxBackgroundY.Text = sb.y.ToString();
                textBoxBackgroundLayer.Text = _selectedBackground.layer.ToString();
            }
        }

        public void OnEnemySelected()
        {
            Vector2 sb = _main.GetDesigner().GetSelectedBlock();
            sb.y *= -1;

            if (_main.GetActiveLevel().GetEnemyList().ContainsKey(sb))
            {
                _selectedEnemy = _main.GetActiveLevel().GetEnemyList()[sb];
                _selectedEnemyPosition = sb;

                textBoxEnemyPositionX.Text = sb.x.ToString();
                textBoxEnemyPositionY.Text = sb.y.ToString();
                textBoxEnemyStartX.Text = _selectedEnemy.p0.x.ToString();
                textBoxEnemyStartY.Text = _selectedEnemy.p0.y.ToString();
                textBoxEnemyEndX.Text = _selectedEnemy.pf.x.ToString();
                textBoxEnemyEndY.Text = _selectedEnemy.p0.y.ToString();
                comboBoxEnemyDirection.SelectedIndex = _selectedEnemy.direction;
                textBoxEnemySpeed.Text = _selectedEnemy.speed.ToString();
            }
        }

        private void ChangeBlockTexture(int n)
        {
            Vector2 sb = _main.GetDesigner().GetSelectedBlock();
            sb.y *= -1;
            BlockData bd = _main.GetActiveLevel().GetBlockList()[sb];
            string path = bd.texture;

            string[] fileSplit = path.Split('/');
            string fileName = fileSplit[fileSplit.Length - 1];

            string[] fileNameSplit = fileName.Split('_');
            string blockName = fileNameSplit[0];
            int texType = int.Parse(fileNameSplit[1].Split('.')[0]);

            string lpath = "";

            for (int i = 0; i < fileSplit.Length - 1; i++)
            {
                lpath += fileSplit[i];
                lpath += "/";
            }

            lpath += blockName;
            lpath += "_";
            lpath += n.ToString();

            bd.texture = lpath;
            _main.GetActiveLevel().GetBlockList()[sb] = bd;
            _main.GetDesigner().Refresh();
        }

        private void buttonBlockTexture1_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(1);
        }

        private void buttonBlockTexture2_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(2);
        }

        private void buttonBlockTexture3_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(3);
        }

        private void buttonBlockTexture4_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(4);
        }

        private void buttonBlockTexture5_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(5);
        }

        private void buttonBlockTexture6_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(6);
        }

        private void buttonBlockTexture7_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(7);
        }

        private void buttonBlockTexture8_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(8);
        }

        private void buttonBlockTexture9_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(9);
        }

        private void buttonBlockTexture10_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(10);
        }

        private void buttonBlockTexture11_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(11);
        }

        private void buttonBlockTexture12_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(12);
        }

        private void buttonBlockTexture13_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(13);
        }

        private void buttonBlockTexture14_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(14);
        }

        private void buttonBlockTexture15_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(15);
        }

        private void buttonBlockTexture16_Click(object sender, EventArgs e)
        {
            ChangeBlockTexture(16);
        }

        private void textBoxDoorX_TextChanged(object sender, EventArgs e)
        {
            Door d = _activeLevel.GetDoor();

            try
            {
                d.x = int.Parse(textBoxDoorX.Text);
                textBoxDoorX.BackColor = Color.White;
            }
            catch
            {
                textBoxDoorX.BackColor = Color.Red;
            }

            _activeLevel.SetDoor(d);
            _main.GetDesigner().Refresh();
        }

        private void textBoxDoorY_TextChanged(object sender, EventArgs e)
        {
            Door d = _activeLevel.GetDoor();

            try
            {
                d.y = int.Parse(textBoxDoorY.Text);
                textBoxDoorY.BackColor = Color.White;
            }
            catch
            {
                textBoxDoorY.BackColor = Color.Red;
            }
            
            _activeLevel.SetDoor(d);
            _main.GetDesigner().Refresh();
        }

        private void textBoxTargetLevel_TextChanged(object sender, EventArgs e)
        {
            Door d = _activeLevel.GetDoor();
            d.targetLevel = textBoxTargetLevel.Text;
            _activeLevel.SetDoor(d);
        }

        private void comboBoxBlockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _selectedBlock.type = (BlockType)comboBoxBlockType.SelectedIndex;
            _main.GetActiveLevel().GetBlockList()[_selectedBlockPosition] = _selectedBlock;
        }

        private void textBoxBlockWalkSound_TextChanged(object sender, EventArgs e)
        {
            _selectedBlock.walk_sound = textBoxBlockWalkSound.Text;
            _main.GetActiveLevel().GetBlockList()[_selectedBlockPosition] = _selectedBlock;
        }

        private void textBoxBlockMoveSound_TextChanged(object sender, EventArgs e)
        {
            _selectedBlock.move_sound = textBoxBlockMoveSound.Text;
            _main.GetActiveLevel().GetBlockList()[_selectedBlockPosition] = _selectedBlock;
        }

        private void textBoxBlockDestructionSound_TextChanged(object sender, EventArgs e)
        {
            _selectedBlock.destruction_sound = textBoxBlockDestructionSound.Text;
            _main.GetActiveLevel().GetBlockList()[_selectedBlockPosition] = _selectedBlock;
        }

        private void textBoxIceLength_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxIceLength.BackColor = Color.White;
                _selectedBlock.length = int.Parse(textBoxIceLength.Text);
                _main.GetActiveLevel().GetBlockList()[_selectedBlockPosition] = _selectedBlock;
                _main.GetDesigner().GetPanel().Refresh();
            }
            catch
            {
                textBoxIceLength.BackColor = Color.Red;
            }
        }

        private void textBoxBlockLightRadius_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxBlockLightRadius.BackColor = Color.White;
                _selectedBlock.light.radius = int.Parse(textBoxBlockLightRadius.Text);
                _main.GetActiveLevel().GetBlockList()[_selectedBlockPosition] = _selectedBlock;
            }
            catch
            {
                textBoxBlockLightRadius.BackColor = Color.Red;
            }
        }

        private void textBoxBackgroundLayer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxBackgroundLayer.BackColor = Color.White;
                int ly = int.Parse(textBoxBackgroundLayer.Text);

                if (ly < 0 || ly == 99 || ly == 100)
                {
                    throw new Exception();
                }
                else
                {
                    _selectedBackground.layer = ly;
                    Dictionary<Vector3,BackgroundData> bgs = _main.GetActiveLevel().GetBackgroundList();

                    bgs.Remove(_selectedBackgroundPosition);
                    _selectedBackgroundPosition.z = ly;
                    bgs.Add(_selectedBackgroundPosition, _selectedBackground);

                    _main.UpdateLayerList();
                    _main.GetDesigner().GetPanel().Refresh();
                }
            }
            catch
            {
                textBoxBackgroundLayer.BackColor = Color.Red;
            }
        }

        private void textBoxEnemyStartX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxEnemyStartX.BackColor = Color.White;
                _selectedEnemy.p0.x = int.Parse(textBoxEnemyStartX.Text);
            }
            catch
            {
                textBoxEnemyStartX.BackColor = Color.Red;
            }
        }

        private void textBoxEnemyStartY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxEnemyStartY.BackColor = Color.White;
                _selectedEnemy.p0.y = int.Parse(textBoxEnemyStartY.Text);
            }
            catch
            {
                textBoxEnemyStartY.BackColor = Color.Red;
            }
        }

        private void textBoxEnemyEndX_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxEnemyEndX.BackColor = Color.White;
                _selectedEnemy.pf.x = int.Parse(textBoxEnemyEndX.Text);
            }
            catch
            {
                textBoxEnemyEndX.BackColor = Color.Red;
            }
        }

        private void textBoxEnemyEndY_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxEnemyEndY.BackColor = Color.White;
                _selectedEnemy.pf.y = int.Parse(textBoxEnemyEndY.Text);
            }
            catch
            {
                textBoxEnemyEndY.BackColor = Color.Red;
            }
        }

        private void comboBoxEnemyDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxEnemyDirection.SelectedIndex != -1)
            {
                comboBoxEnemyDirection.BackColor = Color.White;
                _selectedEnemy.direction = comboBoxEnemyDirection.SelectedIndex;
            }
            else
            {
                comboBoxEnemyDirection.BackColor = Color.Red;
            }
        }

        private void textBoxEnemySpeed_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxEnemySpeed.BackColor = Color.White;
                _selectedEnemy.speed = int.Parse(textBoxEnemySpeed.Text);
            }
            catch
            {
                textBoxEnemySpeed.BackColor = Color.Red;
            }
        }

        private void textBoxHp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                textBoxHp.BackColor = Color.White;
                _selectedEnemy.hp = int.Parse(textBoxHp.Text);
            }
            catch
            {
                textBoxHp.BackColor = Color.Red;
            }
        }

        private void Toolbox_Load(object sender, EventArgs e)
        {

        }
    }
}
