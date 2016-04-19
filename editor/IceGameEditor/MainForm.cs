using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using WeifenLuo.WinFormsUI.Docking;
using System.Security.Principal;
using System.Runtime.InteropServices;
using WinSCP;

public enum CurrentTool
{
    Select,
    Delete,
    Block,
    Background,
    BackgroundSelect,
    BackgroundDelete,
    EnemySelect,
    EnemyDelete,
    FlyerEnemy,
    WalkerEnemy,
    RoamerEnemy,
    LeverEnemy
}

namespace IceGameEditor
{
    public struct EditorSettings
    {
        public string ResourcesPath;
        public string SSHAddress;
        public int SSHPort;
        public string SSHUsername;
        public string SSHPassword;
    };

    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        Toolbox _toolbox;
        Designer _designer;
        List<Designer> _designers;

        Dictionary<string, Bitmap> _blocks;
        Level _activeLevel;
        List<Level> _levels;
        string _selectedBlock = "";

        Bitmap _spawnTexture, _doorTexture, _missingTexture;

        LayerDialog _layers;
        Dictionary<int,bool> _layerList;
        int _activeLayer = 0;

        Bitmap _walkerTex = null;
        Bitmap _flyerTex = null;
        Bitmap _roamerTex = null;
        Bitmap _leverTex = null;

        EditorSettings _settings;

        public MainForm()
        {
            InitializeComponent();

            dockPanel.Theme = new VS2013BlueTheme();

            _levels = new List<Level>();
            _designers = new List<Designer>();

            _blocks = new Dictionary<string, Bitmap>();
            _layerList = new Dictionary<int,bool>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string setPath = "data" + Path.DirectorySeparatorChar + "settings.json";

            if (File.Exists(setPath))
            {
                string strSet = File.ReadAllText(setPath);
                JSONObject jsonSet = new JSONObject(strSet);

                _settings.ResourcesPath = jsonSet["path"].str;
                _settings.SSHAddress = jsonSet["addr"].str;
                _settings.SSHPassword = jsonSet["pass"].str;
                _settings.SSHPort = (int)jsonSet["port"].n;
                _settings.SSHUsername = jsonSet["user"].str;

                _toolbox = new Toolbox(this);
                _toolbox.Show(dockPanel, DockState.DockLeft);

                BeginResourceLoad();
            }
            else
            {
                ResourcesLocationForm rcForm = new ResourcesLocationForm(this);
                rcForm.Show();
            }
        }

        public void SetSettings(string path, string addr, int port, string user, string pass)
        {
            string setPath = "data" + Path.DirectorySeparatorChar + "settings.json";

            _settings.ResourcesPath = path;
            _settings.SSHAddress = addr;
            _settings.SSHPassword = pass;
            _settings.SSHPort = port;
            _settings.SSHUsername = user;

            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            json.AddField("path", path);
            json.AddField("addr", addr);
            json.AddField("pass", pass);
            json.AddField("user", user);
            json.AddField("port", port);

            File.WriteAllText(setPath, json.Print(true));

            _toolbox = new Toolbox(this);
            _toolbox.Show(dockPanel, DockState.DockLeft);

            BeginResourceLoad();
        }

        public void BeginResourceLoad()
        {
            try
            {
                _spawnTexture = new Bitmap("data" + Path.DirectorySeparatorChar + "spawn.png");
                _doorTexture = new Bitmap("data" + Path.DirectorySeparatorChar + "door.png");
                _missingTexture = new Bitmap("data" + Path.DirectorySeparatorChar + "missing.png");
            }
            catch
            {
                MessageBox.Show("Faltan texturas internas. Reinstalar el editor puede que arregle el problema.", "Error abriendo editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            // Rebuild the texture database and load the textures
            try
            {
                RebuildTextureList();
                RebuildAnimationList();
                RebuildLevelList();

                File.WriteAllText(Application.StartupPath + Path.DirectorySeparatorChar + "data" +
                 Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "rcpath.txt",
                 _settings.ResourcesPath);

                LoadBlockTextures();
                LoadAdditionalTextures();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message);
            }
        }

        public void RebuildLevelList()
        {
            string path = _settings.ResourcesPath + Path.DirectorySeparatorChar + "Levels";
            List<string> fileList = Directory.EnumerateFiles(path, "*.txt", SearchOption.AllDirectories).ToList();

            if (fileList != null)
            {
                JSONObject jsonTexList = new JSONObject(JSONObject.Type.OBJECT);

                for (int i = 0; i < fileList.Count; i++)
                {
                    // Remove the path left from the Blocks folder
                    // and the extension. This is done because in
                    // Unity the resource loader appends them automatically.
                    string fn = "";

                    if (!string.IsNullOrEmpty(fileList[i]))
                    {
                        fileList[i] = fileList[i].Replace(path.Replace("Levels", ""), "");
                        string[] ffn = fileList[i].Split(Path.DirectorySeparatorChar);

                        for (int j = 1; j < ffn.Length; j++)
                        {
                            fn += ffn[j];
                            fn += Path.DirectorySeparatorChar;
                        }

                        // Paths are always separated by / inside the game files
                        fn = fn.Replace(Path.DirectorySeparatorChar, '/');
                        fn = fn.Split('.')[0];

                        jsonTexList.Add(fn);
                    }
                }

                File.WriteAllText(_settings.ResourcesPath + Path.DirectorySeparatorChar + "level_list.txt", jsonTexList.Print(true));
            }
        }

        public void RebuildTextureList()
        {
            string path = _settings.ResourcesPath + Path.DirectorySeparatorChar + "Textures";
            List<string> fileList = Directory.EnumerateFiles(path, "*.png", SearchOption.AllDirectories).ToList();
            
            if (fileList != null)
            {
                JSONObject jsonTexList = new JSONObject(JSONObject.Type.OBJECT);
                JSONObject jsonOverwrite = null;
                string overwritePath = _settings.ResourcesPath + Path.DirectorySeparatorChar + "overwrite_texture_list.txt";

                if (File.Exists(overwritePath))
                {
                    jsonOverwrite = new JSONObject(File.ReadAllText(overwritePath));
                }

                for (int i = 0; i < fileList.Count; i++)
                {
                    // Get the image size which will be used
                    // by the client.
                    Image tmp = Image.FromFile(fileList[i]);
                    int width = tmp.Width;
                    tmp.Dispose();
                    tmp = null;

                    // Remove the path left from the Blocks folder
                    // and the extension. This is done because in
                    // Unity the resource loader appends them automatically.
                    string fn = "";

                    if (!string.IsNullOrEmpty(fileList[i]))
                    {
                        fileList[i] = fileList[i].Replace(path.Replace("Textures" + Path.DirectorySeparatorChar,""), "");

                        string[] ffn = fileList[i].Split(Path.DirectorySeparatorChar);

                        for (int j = 0; j < ffn.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(ffn[j]))
                            {
                                fn += ffn[j];
                                fn += Path.DirectorySeparatorChar;
                            }
                        }

                        // Paths are always separated by / inside the game files
                        fn = fn.Replace(Path.DirectorySeparatorChar, '/');
                        fn = fn.Split('.')[0];

                        JSONObject jstex = new JSONObject(JSONObject.Type.OBJECT);
                        jstex.AddField("id", fn);

                        // Check the overwrite list to see if
                        // the sprite uses a different size.
                        if (jsonOverwrite != null)
                        {
                            for (int j = 0; j < jsonOverwrite.list.Count; j++)
                            {
                                if (jsonOverwrite.list[j]["id"].str.Contains(fn))
                                {
                                    width = (int)jsonOverwrite.list[j]["size"].n;
                                }
                            }
                        }

                        jstex.AddField("size", width);

                        jsonTexList.Add(jstex);
                    }
                }

                File.WriteAllText(_settings.ResourcesPath + Path.DirectorySeparatorChar + "texture_list.txt", jsonTexList.Print(true));
            }
        }

        public void RebuildAnimationList()
        {
            string path = _settings.ResourcesPath + Path.DirectorySeparatorChar + "Textures";
            List<string> fileList = Directory.EnumerateFiles(path, "*Anim.txt", SearchOption.AllDirectories).ToList();

            if (fileList != null)
            {
                JSONObject jsonTexList = new JSONObject(JSONObject.Type.OBJECT);

                for (int i = 0; i < fileList.Count; i++)
                {
                    // Remove the path left from the Blocks folder
                    // and the extension. This is done because in
                    // Unity the resource loader appends them automatically.
                    string fn = "";

                    if (!string.IsNullOrEmpty(fileList[i]))
                    {
                        fileList[i] = fileList[i].Replace(path.Replace("Textures", ""), "");
                        string[] ffn = fileList[i].Split(Path.DirectorySeparatorChar);

                        for (int j = 1; j < ffn.Length; j++)
                        {
                            fn += ffn[j];
                            fn += Path.DirectorySeparatorChar;
                        }

                        // Paths are always separated by / inside the game files
                        fn = fn.Replace(Path.DirectorySeparatorChar, '/');
                        fn = fn.Split('.')[0];
                        
                        jsonTexList.Add(fn);
                    }
                }

                File.WriteAllText(_settings.ResourcesPath + Path.DirectorySeparatorChar + "animation_list.txt", jsonTexList.Print(true));
            }
        }

        public void LoadAdditionalTextures()
        {
            Bitmap removeTex = null;
            Bitmap selectTex = null;

            try
            {
                removeTex = new Bitmap("data" + Path.DirectorySeparatorChar + "delete.png");
                selectTex = new Bitmap("data" + Path.DirectorySeparatorChar + "select.png");

                _walkerTex = new Bitmap("data" + Path.DirectorySeparatorChar + "enemywalker.png");
                _flyerTex = new Bitmap("data" + Path.DirectorySeparatorChar + "enemyflyer.png");
                _roamerTex = new Bitmap("data" + Path.DirectorySeparatorChar + "enemyroamer.png");
                _leverTex = new Bitmap("data" + Path.DirectorySeparatorChar + "lever.png");
            }
            catch
            {
                MessageBox.Show("Faltan texturas internas. Reinstalar el editor puede que arregle el problema.", "Error abriendo editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            _toolbox.AddBlockTool("Eliminar", removeTex);
            _toolbox.AddBlockTool("Seleccionar", selectTex);

            _toolbox.AddBackgroundTool("Eliminar", removeTex);
            _toolbox.AddBackgroundTool("Seleccionar", selectTex);

            _toolbox.AddEnemyTool("Volador", _flyerTex);
            _toolbox.AddEnemyTool("Caminador", _walkerTex);
            _toolbox.AddEnemyTool("Roamer", _roamerTex);
            _toolbox.AddEnemyTool("Palanca", _leverTex);
            _toolbox.AddEnemyTool("Eliminar", removeTex);
            _toolbox.AddEnemyTool("Seleccionar", selectTex);
        }

        public void LoadBlockTextures()
        {
            // Read texture types
            string tex = "";

            try
            {
                tex = File.ReadAllText(_settings.ResourcesPath + Path.DirectorySeparatorChar + "texture_list.txt");
            }
            catch
            {
                MessageBox.Show("No se ha podido abrir la lista de texturas. Reinstalar el editor puede que arregle el problema.", "Error abriendo editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            JSONObject json = new JSONObject(tex);

            if (json)
            {
                List<JSONObject> texList = json.list;

                for (int i = 0; i < texList.Count; i++)
                {
                    string path = _settings.ResourcesPath + Path.DirectorySeparatorChar + "Textures" + 
                        Path.DirectorySeparatorChar + texList[i]["id"].str + ".png";

                    Bitmap bmp = null;

                    if (path.Contains("Blocks") || path.Contains("Enemies"))
                    {
                        try
                        {
                            bmp = new Bitmap(path);
                        }
                        catch
                        {
                            MessageBox.Show("Falta la textura " + texList[i].str + ".", "Error abriendo editor", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                        }

                        string[] fileSplit = path.Split('/');
                        string fileName = fileSplit[fileSplit.Length - 1];

                        int texType = 0;

                        if (!texList[i]["id"].str.Contains("Background"))
                        {
                            string[] fileNameSplit = fileName.Split('_');
                            texType = int.Parse(fileNameSplit[1].Split('.')[0]);
                        }

                        _blocks.Add(texList[i]["id"].str, bmp);

                        if (texType == 1 && !texList[i]["id"].str.Contains("Frame"))
                        {
                            _toolbox.AddBlockTool(texList[i]["id"].str, bmp);
                        }

                        if (texList[i]["id"].str.Contains("Background"))
                        {
                            _toolbox.AddBackgroundTool(texList[i]["id"].str, bmp);
                        }
                    }
                }
            }
        }

        public void Syncronize()
        {
            try
            {
                RebuildTextureList();
                RebuildAnimationList();
                RebuildLevelList();
            }
            catch { }

            SyncForm sf = new SyncForm(this);
            sf.Show();
        }
        
        public EditorSettings GetSettings()
        {
            return _settings;
        }

        public int GetActiveLayer()
        {
            return _activeLayer;
        }

        public void SetActiveLayer(int l)
        {
            _activeLayer = l;
        }

        public void OnSelectToolPressed()
        {
            _designer.OnSelectToolPressed();
        }

        public Dictionary<int,bool> GetLayerList()
        {
            return _layerList;
        }

        public void UpdateLayerList()
        {
            if (_activeLevel != null)
            {
                Dictionary<Vector3, BackgroundData> bgs = _activeLevel.GetBackgroundList();

                foreach (KeyValuePair<Vector3, BackgroundData> bg in bgs)
                {
                    if (!_layerList.ContainsKey(bg.Value.layer))
                    {
                        _layerList.Add(bg.Value.layer, true);
                    }
                }
            }
        }

        public void OnBackgroundSelectToolPressed()
        {
            _designer.OnBackgroundSelectToolPressed();
        }

        public void OnEnemySelectToolPressed()
        {
            _designer.OnEnemySelectToolPressed();
        }

        public void SetTitle(string t)
        {
            Text = t;
            _designer.Text = t;
        }

        public void OnDeleteToolPressed()
        {
            _designer.OnDeleteToolPressed();
        }

        public void OnBackgroundDeleteToolPressed()
        {
            _designer.OnBackgroundDeleteToolPressed();
        }

        public void OnEnemyDeleteToolPressed()
        {
            _designer.OnEnemyDeleteToolPressed();
        }

        public void OnFlyerEnemySelected()
        {
            _designer.OnFlyerEnemySelected();
        }

        public void OnWalkerEnemySelected()
        {
            _designer.OnWalkerEnemySelected();
        }

        public void OnRoamerEnemySelected()
        {
            _designer.OnRoamerEnemySelected();
        }

        public void OnLeverEnemySelected()
        {
            _designer.OnLeverEnemySelected();
        }
        
        public void PlaceBlock(Vector2 p)
        {
            BlockData d = new BlockData();
            d.texture = _selectedBlock;
            d.length = 1;

            if (_selectedBlock.Contains("Ice"))
            {
                d.type = BlockType.Ice;
            }

            _activeLevel.SetBlock(p, d);
        }

        public void PlaceBackground(Vector3 p)
        {
            BackgroundData d = new BackgroundData();
            d.texture = _selectedBlock;
            d.layer = p.z;

            _activeLevel.SetBackground(p, d);
            UpdateLayerList();
        }

        public void PlaceFlyerEnemy(Vector2 p)
        {
            EnemyData d = new EnemyData();

            d.hp = 1;
            d.speed = 3f;
            d.spawn = p;
            d.p0 = p;
            d.pf = p;
            d.texture = "Enemies/EnemyFlyer_1/EnemyFlyer_1";
            d.type = EnemyType.Flyer;

            _activeLevel.SetEnemy(p, d);
        }

        public void PlaceWalkerEnemy(Vector2 p)
        {
            EnemyData d = new EnemyData();

            d.hp = 1;
            d.speed = 3f;
            d.spawn = p;
            d.p0 = p;
            d.pf = p;
            d.texture = "Enemies/EnemyWalker/EnemyWalker_1";
            d.type = EnemyType.Walker;

            _activeLevel.SetEnemy(p, d);
        }

        public void PlaceRoamerEnemy(Vector2 p)
        {
            EnemyData d = new EnemyData();

            d.hp = 1;
            d.speed = 3f;
            d.spawn = p;
            d.p0 = p;
            d.pf = p;
            d.texture = "Enemies/EnemyRoamer/Roamer_1_Frame_0";
            d.type = EnemyType.Roamer;

            _activeLevel.SetEnemy(p, d);
        }

        public void PlaceLeverEnemy(Vector2 p)
        {
            EnemyData d = new EnemyData();

            d.hp = 1;
            d.speed = 3f;
            d.spawn = p;
            d.p0 = p;
            d.pf = p;
            d.texture = "Blocks/Ice/Ice_1";
            d.type = EnemyType.Lever;

            _activeLevel.SetEnemy(p, d);
        }

        public Bitmap GetFlyerEnemyTexture()
        {
            return _flyerTex;
        }

        public Bitmap GetWalkerEnemyTexture()
        {
            return _walkerTex;
        }

        public Bitmap GetRoamerEnemyTexture()
        {
            return _roamerTex;
        }

        public Level GetActiveLevel()
        {
            return _activeLevel;
        }

        public Designer GetDesigner()
        {
            return _designer;
        }

        public void OnBlockSelected(string name)
        {
            _selectedBlock = name;
            _designer.OnBlockToolPressed();
        }

        public void OnBackgroundSelected(string name)
        {
            _selectedBlock = name;
            _designer.OnBackgroundToolPressed();
        }

        public Toolbox GetToolbox()
        {
            return _toolbox;
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cargarNivelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = openFileDialog.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    string lvPath = openFileDialog.FileName;
                    string lvData = File.ReadAllText(lvPath);

                    _activeLevel = new Level();
                    _activeLevel.LoadLevel(lvData);
                    _levels.Add(_activeLevel);
                    _layerList.Clear();
                    UpdateLayerList();

                    _designer = new Designer(this, _designers.Count);
                    _designers.Add(_designer);

                    _designer.Show(dockPanel, DockState.Document);
                    _designer.SetTextures(_blocks);
                    _designer.SetSpawnTexture(_spawnTexture);
                    _designer.SetDoorTexture(_doorTexture);

                    _designer.SetBlockTextures(_activeLevel.GetBlockList());
                    _designer.Text = _activeLevel.GetPublicName();

                    _toolbox.SetActiveLevel(_activeLevel);
                    _toolbox.UpdateLevelData();
                }
            }
            catch { }
        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public void SetActiveDesigner(int index)
        {
            _activeLevel = _levels[index];
            _designer = _designers[index];

            _designer.SetTextures(_blocks);
            _designer.SetSpawnTexture(_spawnTexture);
            _designer.SetDoorTexture(_doorTexture);

            _designer.SetBlockTextures(_activeLevel.GetBlockList());
            _designer.Text = _activeLevel.GetPublicName();

            _toolbox.SetActiveLevel(_activeLevel);
            _toolbox.UpdateLevelData();
        }

        public Dictionary<string, Bitmap> GetTextureList()
        {
            return _blocks;
        }

        private void nuevoNivelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _designer = new Designer(this, _designers.Count);
            _designers.Add(_designer);
            _designer.Show(dockPanel, DockState.Document);
            _designer.SetTextures(_blocks);
            _designer.SetSpawnTexture(_spawnTexture);
            _designer.SetDoorTexture(_doorTexture);

            _activeLevel = new Level();
            _levels.Add(_activeLevel);
            _designer.Text = _activeLevel.GetPublicName();

            _toolbox.SetActiveLevel(_activeLevel);
            _toolbox.UpdateLevelData();
        }

        private void guardarNivelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveActiveLevel();
        }

        public void SaveActiveLevel()
        {
            if (_activeLevel != null)
            {
                saveFileDialog.ShowDialog();

                try
                {
                    File.WriteAllText(saveFileDialog.FileName, _activeLevel.SerializeLevel());
                }
                catch
                {
                    MessageBox.Show("No se ha podido guardar el nivel en el camino especificado.", "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public Bitmap GetMissingTextureBitmap()
        {
            return _missingTexture;
        }

        private void ayudaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.Show();
        }

        private void probarEnLocalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_activeLevel != null)
            {
                // Rebuild the texture and animation lists
                RebuildTextureList();
                RebuildAnimationList();

                try
                {
                    File.WriteAllText("data\\bin\\debuglevel.txt", _activeLevel.SerializeLevel());
                    Process rt = System.Diagnostics.Process.Start(Application.StartupPath + "\\data\\bin\\rt.exe");
                    /*IntPtr hwndRt = IntPtr.Zero;

                    Designer dd = new Designer(this, _designers.Count);
                    dd.Show(dockPanel, DockState.Document);

                    dd.GetPanel().Hide();

                    while (hwndRt == IntPtr.Zero)
                    {
                        rt.WaitForInputIdle(1000); //wait for the window to be ready for input;
                        rt.Refresh();              //update process info

                        hwndRt = rt.MainWindowHandle;  //cache the window handle
                    }

                    SetParent(hwndRt, dd.Handle);
                    dd.Text = "Modo de Prueba";

                    MoveWindow(hwndRt, 0, -20, dd.Width, dd.Height+20, true);
                    SetWindowLong(hwndRt, -20, 0);

                    dd.SetRuntimeHandle(hwndRt);*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se ha podido lanzar el nivel.\n" + ex.Message, "Error al compilar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void probarEnRemotoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_activeLevel != null)
            {
                try
                {
                    File.WriteAllText("data/tmp/debuglevel.txt", _activeLevel.SerializeLevel());
                    RemoteDebug rd = new RemoteDebug();
                    rd.Show();
                    rd.Init();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se ha podido lanzar el nivel.\n" + ex.Message, "Error al compilar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void editorDeAnimacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnimationEditor anim = new AnimationEditor(this);
            anim.Show();
        }

        private void capasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_activeLevel != null)
            {
                if (_layers != null)
                {
                    _layers.Hide();
                }

                _layers = new LayerDialog(this);
                _layers.Show();
            }
        }

        private void sincronizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Syncronize();
        }
    }
}
