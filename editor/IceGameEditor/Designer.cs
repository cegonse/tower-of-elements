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
    public partial class Designer : DockContent
    {
        MainForm _main;

        const int _xSize = 200;
        const int _ySize = 200;

        Dictionary<Vector2, BlockData> _blocks;
        Dictionary<Vector3, string> _backgrounds;
        Dictionary<string, Bitmap> _blockTextures;

        Bitmap _spawnTexture;
        Bitmap _doorTexture;
        Pen _redPen, _bluePen, _grayPen, _pinkPen, _greenPen, _yellowPen;
        CurrentTool _tool = CurrentTool.Select;

        int _lastX = 0;
        int _lastY = 0;

        int _index = 0;
        IntPtr _hwnd = IntPtr.Zero;

        public Designer(MainForm main, int index)
        {
            InitializeComponent();
            _main = main;
            _blocks = new Dictionary<Vector2, BlockData>();
            _backgrounds = new Dictionary<Vector3, string>();

            _redPen = new Pen(Color.Red, 1);
            _bluePen = new Pen(Color.Blue, 1);
            _grayPen = new Pen(Color.Gray, 0.1f);
            _pinkPen = new Pen(Color.LightPink, 2f);
            _greenPen = new Pen(Color.Green, 2f);
            _yellowPen = new Pen(Color.Yellow, 2f);

            _index = index;
        }

        public void SetTextures(Dictionary<string, Bitmap> tex)
        {
            _blockTextures = tex;
        }

        public void SetSpawnTexture(Bitmap bmp)
        {
            _spawnTexture = bmp;
        }

        public void SetDoorTexture(Bitmap bmp)
        {
            _doorTexture = bmp;
        }

        public void SetBlockTexture(int x, int y, string tex)
        {
            Vector2 pt = new Vector2();
            pt.x = x;
            pt.y = y;

            BlockData bd = _blocks[pt];
            bd.texture = tex;
            _blocks[pt] = bd;
        }

        public void SetBackgroundTexture(int x, int y, string tex)
        {
            Vector3 pt = new Vector3();
            pt.x = x;
            pt.y = y;
            pt.z = _main.GetActiveLayer();

            _backgrounds[pt] = tex;
        }

        public void SetBlockTextures(Dictionary<Vector2, BlockData> blocks)
        {
            _blocks = _main.GetActiveLevel().GetBlockList();
            panel.Refresh();
        }

        public void SetBackgroundTextures(Dictionary<Vector3, BackgroundData> bgs)
        {
            _backgrounds.Clear();

            foreach (KeyValuePair<Vector3, BackgroundData> bg in bgs)
            {
                _backgrounds.Add(bg.Key, bg.Value.texture);
            }

            panel.Refresh();
        }

        public Panel GetPanel()
        {
            return panel;
        }

        private void Designer_Load(object sender, EventArgs e)
        {
            PictureBox p = new PictureBox();
            p.Size = new Size(1, 1);
            p.Location = new Point(_xSize * 64, _ySize * 64);
            panel.Controls.Add(p);
            panel.AutoScrollPosition = new Point((_xSize - 9) * 32, (_ySize - 9) * 32);
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            if (_main.GetActiveLevel() != null && _blockTextures != null)
            {
                Graphics g = e.Graphics;

                // Grid lines
                for (int i = 0; i < _xSize; i++)
                {
                    // Horizontal lines
                    g.DrawLine(_grayPen,
                    0, i * 64 + panel.AutoScrollPosition.Y,
                    _xSize * 64, i * 64 + panel.AutoScrollPosition.Y);

                    // Vertical lins
                    g.DrawLine(_grayPen,
                    i * 64 + panel.AutoScrollPosition.X, 0,
                    i * 64 + panel.AutoScrollPosition.X, _ySize * 64);
                }

                // Horizontal axis center
                g.DrawLine(_redPen,
                    0, (1+_ySize) * 32 + panel.AutoScrollPosition.Y,
                    _xSize * 64, (1+_ySize) * 32 + panel.AutoScrollPosition.Y);

                // Vertical Axis center
                g.DrawLine(_bluePen,
                    (1+_xSize) * 32 + panel.AutoScrollPosition.X, 0,
                    (1+_xSize) * 32 + panel.AutoScrollPosition.X, _ySize * 64);

                // Draw all background layers in order (painter's algorithm)
                List<KeyValuePair<int, bool>> layers = _main.GetLayerList().ToList();
                layers.Sort((first, next) =>
                {
                    return first.Key.CompareTo(next.Key);
                });

                foreach (KeyValuePair<int, bool> l in layers)
                {
                    // Only draw if the layer is visible
                    if (l.Value)
                    {
                        foreach (KeyValuePair<Vector3, BackgroundData> tex in _main.GetActiveLevel().GetBackgroundList())
                        {
                            // Only draw tile if it is in the layer being drawn
                            if (tex.Key.z == l.Key)
                            {
                                Rectangle rect = new Rectangle((_xSize / 2 + tex.Key.x) * 64 + panel.AutoScrollPosition.X, (_ySize / 2 - tex.Key.y) * 64 + panel.AutoScrollPosition.Y, 64, 64);

                                // Draw the missing texture block if missing
                                if (_blockTextures.Keys.Contains(tex.Value.texture))
                                {
                                    g.DrawImage(_blockTextures[tex.Value.texture], rect);
                                }
                                else
                                {
                                    g.DrawImage(_main.GetMissingTextureBitmap(), rect);
                                }

                                if (_tool == CurrentTool.BackgroundSelect)
                                {
                                    if (rect.Contains(_lastX, _lastY))
                                    {
                                        g.DrawRectangle(_greenPen, rect);
                                    }
                                }
                            }
                        }
                    }
                }

                // Draw blocks
                foreach (KeyValuePair<Vector2, BlockData> tex in _main.GetActiveLevel().GetBlockList())
                {
                    Rectangle rect;

                    if (tex.Value.length > 1)
                    {
                        for (int i = 0; i < tex.Value.length; i++)
                        {
                            rect = new Rectangle((_xSize / 2 + (tex.Key.x + i)) * 64 + panel.AutoScrollPosition.X, (_ySize / 2 - tex.Key.y) * 64 + panel.AutoScrollPosition.Y, 64, 64);
                            string texn = tex.Value.texture;
                            string[] tk = texn.Split('_');

                            if (i == 0)
                            {
                                texn = tk[0] + "_13";
                            }
                            else if (i == tex.Value.length - 1)
                            {
                                texn = tk[0] + "_15";
                            }
                            else
                            {
                                texn = tk[0] + "_14";
                            }

                            if (_blockTextures.Keys.Contains(texn))
                            {
                                g.DrawImage(_blockTextures[texn], rect);
                            }
                            else
                            {
                                g.DrawImage(_main.GetMissingTextureBitmap(), rect);
                            }
                        }

                        rect = new Rectangle((_xSize / 2 + tex.Key.x) * 64 + panel.AutoScrollPosition.X, (_ySize / 2 - tex.Key.y) * 64 + panel.AutoScrollPosition.Y, 64 + 64 * (tex.Value.length-1), 64);
                    }
                    else
                    {
                        rect = new Rectangle((_xSize / 2 + tex.Key.x) * 64 + panel.AutoScrollPosition.X, (_ySize / 2 - tex.Key.y) * 64 + panel.AutoScrollPosition.Y, 64, 64);

                        if (_blockTextures.Keys.Contains(tex.Value.texture))
                        {
                            g.DrawImage(_blockTextures[tex.Value.texture], rect);
                        }
                        else
                        {
                            g.DrawImage(_main.GetMissingTextureBitmap(), rect);
                        }
                    }

                    if (_tool == CurrentTool.Select)
                    {
                        if (rect.Contains(_lastX, _lastY))
                        {
                            g.DrawRectangle(_pinkPen, rect);
                        }
                    }
                }

                // Draw enemies
                foreach (KeyValuePair<Vector2, EnemyData> tex in _main.GetActiveLevel().GetEnemyList())
                {
                    Rectangle rect;

                    if (!string.IsNullOrEmpty(tex.Value.texture))
                    {
                        if (tex.Value.type == EnemyType.Flyer)
                        {
                            // Draw the path line  
                            Point p0 = new Point((_xSize / 2 + tex.Value.p0.x) * 64 + panel.AutoScrollPosition.X, (_ySize / 2 - tex.Value.p0.y) * 64 + panel.AutoScrollPosition.Y + 32);
                            Point pf = new Point((_xSize / 2 + tex.Value.pf.x) * 64 + panel.AutoScrollPosition.X, (_ySize / 2 - tex.Value.pf.y) * 64 + panel.AutoScrollPosition.Y + 32);

                            g.DrawLine(_yellowPen, p0, pf);
                        }

                        rect = new Rectangle((_xSize / 2 + tex.Key.x) * 64 + panel.AutoScrollPosition.X, (_ySize / 2 - tex.Key.y) * 64 + panel.AutoScrollPosition.Y, 64, 64);
                        g.DrawImage(_blockTextures[tex.Value.texture], rect);

                        if (_tool == CurrentTool.EnemySelect)
                        {
                            if (rect.Contains(_lastX, _lastY))
                            {
                                g.DrawRectangle(_yellowPen, rect);
                            }
                        }
                    }
                }

                Rectangle spawnRect = new Rectangle((_xSize / 2 + _main.GetActiveLevel().GetPlayer().x) * 64 + panel.AutoScrollPosition.X, ((_ySize / 2 - _main.GetActiveLevel().GetPlayer().y) * 64 + panel.AutoScrollPosition.Y), 64, 64);
                g.DrawImage(_spawnTexture, spawnRect);

                Rectangle doorRect = new Rectangle((_xSize / 2 + _main.GetActiveLevel().GetDoor().x) * 64 + panel.AutoScrollPosition.X, ((_ySize / 2 - _main.GetActiveLevel().GetDoor().y) * 64 + panel.AutoScrollPosition.Y) - 23, 64, 87);
                g.DrawImage(_doorTexture, doorRect);

                g.Dispose();
            }
        }

        public void OnSelectToolPressed()
        {
            _tool = CurrentTool.Select;
        }

        public void OnBackgroundSelectToolPressed()
        {
            _tool = CurrentTool.BackgroundSelect;
        }

        public void OnEnemySelectToolPressed()
        {
            _tool = CurrentTool.EnemySelect;
        }

        public void OnDeleteToolPressed()
        {
            _tool = CurrentTool.Delete;
        }

        public void OnBackgroundDeleteToolPressed()
        {
            _tool = CurrentTool.BackgroundDelete;
        }

        public void OnEnemyDeleteToolPressed()
        {
            _tool = CurrentTool.EnemyDelete;
        }

        public void OnBlockToolPressed()
        {
            _tool = CurrentTool.Block;
        }

        public void OnBackgroundToolPressed()
        {
            _tool = CurrentTool.Background;
        }

        public void OnFlyerEnemySelected()
        {
            _tool = CurrentTool.FlyerEnemy;
        }

        public void OnWalkerEnemySelected()
        {
            _tool = CurrentTool.WalkerEnemy;
        }

        public void OnRoamerEnemySelected()
        {
            _tool = CurrentTool.RoamerEnemy;
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void panel_Scroll(object sender, ScrollEventArgs e)
        {
        }

        public Vector2 GetSelectedBlock()
        {
            Vector2 v = new Vector2();

            v.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
            v.y = (((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));

            return v;
        }

        public Vector2 GetSelectedEnemy()
        {
            Vector2 v = new Vector2();

            v.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
            v.y = (((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));

            return v;
        }

        public Vector3 GetSelectedBackground()
        {
            Vector3 v = new Vector3();

            v.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
            v.y = (((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));
            v.z = _main.GetActiveLayer();

            return v;
        }

        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            _lastX = e.Location.X;
            _lastY = e.Location.Y;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_tool == CurrentTool.Delete)
                {
                    Vector2 delKey = new Vector2();

                    foreach (KeyValuePair<Vector2, BlockData> tex in _blocks)
                    {
                        Rectangle rect = new Rectangle((_xSize / 2 + tex.Key.x) * 64 + panel.AutoScrollPosition.X, ((_ySize / 2 - tex.Key.y) * 64 + panel.AutoScrollPosition.Y), 64, 64);

                        if (rect.Contains(_lastX, _lastY))
                        {
                            delKey = tex.Key;
                        }
                    }

                    _main.GetActiveLevel().RemoveBlock(delKey);
                    SetBlockTextures(_main.GetActiveLevel().GetBlockList());
                }
                else if (_tool == CurrentTool.Block)
                {
                    Vector2 p = new Vector2();

                    p.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
                    p.y = -(((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));

                    _main.PlaceBlock(p);
                    SetBlockTextures(_main.GetActiveLevel().GetBlockList());
                }
                else if (_tool == CurrentTool.Background)
                {
                    Vector3 p = new Vector3();

                    p.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
                    p.y = -(((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));
                    p.z = _main.GetActiveLayer();

                    _main.PlaceBackground(p);
                    SetBackgroundTextures(_main.GetActiveLevel().GetBackgroundList());
                }
                else if (_tool == CurrentTool.FlyerEnemy)
                {
                    Vector2 p = new Vector2();

                    p.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
                    p.y = -(((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));

                    _main.PlaceFlyerEnemy(p);
                }
                else if (_tool == CurrentTool.WalkerEnemy)
                {
                    Vector2 p = new Vector2();

                    p.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
                    p.y = -(((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));

                    _main.PlaceWalkerEnemy(p);
                }
                else if (_tool == CurrentTool.RoamerEnemy)
                {
                    Vector2 p = new Vector2();

                    p.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
                    p.y = -(((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));

                    _main.PlaceRoamerEnemy(p);
                }
                else if (_tool == CurrentTool.EnemyDelete)
                {
                    Vector2 p = new Vector2();

                    p.x = ((_lastX - panel.AutoScrollPosition.X) / 64) - (_xSize / 2);
                    p.y = -(((_lastY - panel.AutoScrollPosition.Y) / 64) - (_ySize / 2));

                    _main.GetActiveLevel().RemoveEnemy(p);
                }
                else if (_tool == CurrentTool.EnemySelect)
                {
                    _main.GetToolbox().OnEnemySelected();
                }
                else if (_tool == CurrentTool.Select)
                {
                    _main.GetToolbox().OnBlockSelected();
                }
                else if (_tool == CurrentTool.BackgroundSelect)
                {
                    _main.GetToolbox().OnBackgroundBlockSelected();
                }
                else if (_tool == CurrentTool.BackgroundDelete)
                {
                    Vector3 delKey = new Vector3();

                    foreach (KeyValuePair<Vector3, string> tex in _backgrounds)
                    {
                        Rectangle rect = new Rectangle((_xSize / 2 + tex.Key.x) * 64 + panel.AutoScrollPosition.X, ((_ySize / 2 - tex.Key.y) * 64 + panel.AutoScrollPosition.Y), 64, 64);

                        if (rect.Contains(_lastX, _lastY) && tex.Key.z == _main.GetActiveLayer())
                        {
                            delKey = tex.Key;
                        }
                    }

                    _main.GetActiveLevel().RemoveBackground(delKey);
                    SetBackgroundTextures(_main.GetActiveLevel().GetBackgroundList());
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Vector2 delKey = new Vector2();

                foreach (KeyValuePair<Vector2, BlockData> tex in _blocks)
                {
                    Rectangle rect = new Rectangle((_xSize / 2 + tex.Key.x) * 64 + panel.AutoScrollPosition.X, ((_ySize / 2 - tex.Key.y) * 64 + panel.AutoScrollPosition.Y), 64, 64);

                    if (rect.Contains(_lastX, _lastY))
                    {
                        delKey = tex.Key;
                    }
                }

                _main.GetActiveLevel().RemoveBlock(delKey);
                SetBlockTextures(_main.GetActiveLevel().GetBlockList());
            }

            panel.Refresh();
        }

        private void Designer_MdiChildActivate(object sender, EventArgs e)
        {
            _main.SetActiveDesigner(_index);
        }

        private void Designer_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void Designer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_hwnd == IntPtr.Zero)
            {
                DialogResult res = MessageBox.Show("¿Guardar los cambios?", "Atención", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    _main.SaveActiveLevel();
                }
            }
        }

        private void Designer_Resize(object sender, EventArgs e)
        {
            if (_hwnd != IntPtr.Zero)
            {
                MainForm.MoveWindow(_hwnd, -8, -30, panel.Width+16, panel.Height+30, true);
            }
        }

        public void SetRuntimeHandle(IntPtr hwnd)
        {
            _hwnd = hwnd;
        }

        private void panel_Resize(object sender, EventArgs e)
        {
        }
    }
}
