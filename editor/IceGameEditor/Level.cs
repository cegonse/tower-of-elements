using System.Collections.Generic;
using System.Windows.Forms;

public enum BlockType : int
{
    Rock = 0,
    Ice,
    Crate
}

public struct Vector2
{
    public int x;
    public int y;
}

public struct Vector3
{
    public int x;
    public int y;
    public int z;
}

public struct Light
{
    public float radius;
    public float color_r;
    public float color_g;
    public float color_b;
}

public struct PlayerData
{
    public int x;
    public int y;

    public int ice;
    public int fire;
    public int wind;
    public int earth;
    public int eye;

    public Light light;
}

public enum EnemyType
{
    Flyer = 0,
    Lever,
    Walker,
    Roamer
}

public class EnemyData
{
    public float speed;
    public int hp;
    public EnemyType type;
    public Vector2 spawn;
    public string texture;
    public Vector2 p0;
    public Vector2 pf;
    public int direction;
}

public struct BlockData
{
    public BlockType type;
    public Light light;
    public string texture;
    public string walk_sound;
    public string move_sound;
    public string destruction_sound;
    public int length;
}

public struct BackgroundData
{
    public string texture;
    public Light light;
    public int layer;
}

public struct Door
{
    public int x, y;
    public string targetLevel;
}

public class Level
{
    private string _name;
    private string _publicName;
	private int _difficulty;
    private bool _illuminated;
    private string _song;

    private PlayerData _player;
    private Door _door;
    private Dictionary<Vector2, BlockData> _blocks;
    private Dictionary<Vector3, BackgroundData> _backgrounds;
    private Dictionary<Vector2, EnemyData> _enemies;
	
	public Level(string name = "untitled", string pn = "Sin título", int d = 1, bool il = false, string sn = "")
	{
        _name = name;
        _publicName = pn;
        _difficulty = d;
        _illuminated = il;
        _song = sn;

        _blocks = new Dictionary<Vector2, BlockData>();
        _backgrounds = new Dictionary<Vector3, BackgroundData>();
        _enemies = new Dictionary<Vector2, EnemyData>();

        _door = new Door();
        _player = new PlayerData();
	}
	
    public string SerializeLevel()
    {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);

        json.AddField("public_name", _publicName);
        json.AddField("difficulty", _difficulty);
        json.AddField("illuminated", _illuminated);
        json.AddField("song", _song);

        JSONObject jsonDoor = new JSONObject(JSONObject.Type.OBJECT);

        jsonDoor.AddField("x", _door.x);
        jsonDoor.AddField("y", _door.y);
        jsonDoor.AddField("targetLevel", _door.targetLevel);

        JSONObject jsonPlayer = new JSONObject(JSONObject.Type.OBJECT);

        jsonPlayer.AddField("x", _player.x);
        jsonPlayer.AddField("y", _player.y);
        jsonPlayer.AddField("ice", _player.ice);
        jsonPlayer.AddField("fire", _player.fire);
        jsonPlayer.AddField("earth", _player.earth);
        jsonPlayer.AddField("wind", _player.wind);
        jsonPlayer.AddField("eye", _player.eye);

        JSONObject jsonPlayerLight = new JSONObject(JSONObject.Type.OBJECT);

        jsonPlayerLight.AddField("color_r", _player.light.color_r);
        jsonPlayerLight.AddField("color_g", _player.light.color_g);
        jsonPlayerLight.AddField("color_b", _player.light.color_b);
        jsonPlayerLight.AddField("radius", _player.light.radius);

        jsonPlayer.AddField("light", jsonPlayerLight);

        json.AddField("door", jsonDoor);
        json.AddField("player", jsonPlayer);

        JSONObject jsonBlocks = new JSONObject(JSONObject.Type.ARRAY);

        foreach (KeyValuePair<Vector2, BlockData> bd in _blocks)
        {
            JSONObject jsonBlock = new JSONObject(JSONObject.Type.OBJECT);

            jsonBlock.AddField("x", bd.Key.x);
            jsonBlock.AddField("y", bd.Key.y);
            jsonBlock.AddField("type", (int)bd.Value.type);
            jsonBlock.AddField("texture", bd.Value.texture);
            jsonBlock.AddField("walk_sound", bd.Value.walk_sound);
            jsonBlock.AddField("move_sound", bd.Value.move_sound);
            jsonBlock.AddField("destruction_sound", bd.Value.destruction_sound);
            jsonBlock.AddField("length", bd.Value.length);

            JSONObject jsonBlockLight = new JSONObject(JSONObject.Type.OBJECT);

            jsonBlockLight.AddField("color_r", bd.Value.light.color_r);
            jsonBlockLight.AddField("color_g", bd.Value.light.color_g);
            jsonBlockLight.AddField("color_b", bd.Value.light.color_b);
            jsonBlockLight.AddField("radius", bd.Value.light.radius);

            jsonBlock.AddField("light", jsonBlockLight);
            jsonBlocks.Add(jsonBlock);
        }

        json.AddField("blocks", jsonBlocks);

        JSONObject jsonBgs = new JSONObject(JSONObject.Type.ARRAY);

        foreach (KeyValuePair<Vector3, BackgroundData> bg in _backgrounds)
        {
            JSONObject jsonBg = new JSONObject(JSONObject.Type.OBJECT);

            jsonBg.AddField("x", bg.Key.x);
            jsonBg.AddField("y", bg.Key.y);
            jsonBg.AddField("texture", bg.Value.texture);
            jsonBg.AddField("layer", bg.Value.layer);

            JSONObject jsonBgLight = new JSONObject(JSONObject.Type.OBJECT);

            jsonBgLight.AddField("color_r", bg.Value.light.color_r);
            jsonBgLight.AddField("color_g", bg.Value.light.color_g);
            jsonBgLight.AddField("color_b", bg.Value.light.color_b);
            jsonBgLight.AddField("radius", bg.Value.light.radius);

            jsonBg.AddField("light", jsonBgLight);
            jsonBgs.Add(jsonBg);
        }

        json.AddField("backgrounds", jsonBgs);

        JSONObject jsonEnemies = new JSONObject(JSONObject.Type.ARRAY);

        foreach (KeyValuePair<Vector2, EnemyData> ene in _enemies)
        {
            JSONObject jsonEnemy = new JSONObject(JSONObject.Type.OBJECT);

            jsonEnemy.AddField("speed", ene.Value.speed);
            jsonEnemy.AddField("hp", ene.Value.hp);
            jsonEnemy.AddField("type", (int)ene.Value.type);
            jsonEnemy.AddField("spawnx", ene.Value.spawn.x);
            jsonEnemy.AddField("spawny", ene.Value.spawn.y);
            jsonEnemy.AddField("texture", ene.Value.texture);

            if (ene.Value.type == EnemyType.Flyer || ene.Value.type == EnemyType.Walker ||
                ene.Value.type == EnemyType.Lever)
            {
                EnemyData fe = ene.Value;

                jsonEnemy.AddField("x0", fe.p0.x);
                jsonEnemy.AddField("y0", fe.p0.y);

                jsonEnemy.AddField("xf", fe.pf.x);
                jsonEnemy.AddField("yf", fe.pf.y);
            }
            else if (ene.Value.type == EnemyType.Roamer)
            {
                EnemyData we = ene.Value;
                jsonEnemy.AddField("direction", we.direction);
            }

            jsonEnemies.Add(jsonEnemy);
        }

        json.AddField("enemies", jsonEnemies);

        // Bounds for the camera zooming
        int minX = 0, minY = 0, maxX = 0, maxY = 0;
        int tx, ty;

        foreach (KeyValuePair<Vector2,BlockData> bl in _blocks)
        {
            tx = bl.Key.x;
            ty = bl.Key.y;

            if (tx < minX && ty < minY)
            {
                minX = tx;
                minY = ty;
            }

            if (tx >= maxX && ty >= maxY)
            {
                maxX = tx;
                maxY = ty;
            }
        }

        JSONObject jsonBounds = new JSONObject(JSONObject.Type.OBJECT);

        jsonBounds.AddField("minx", minX);
        jsonBounds.AddField("miny", minY);
        jsonBounds.AddField("maxx", maxX);
        jsonBounds.AddField("maxy", maxY);

        json.AddField("bounds", jsonBounds);

        return json.Print(true);
    }

	public void LoadLevel(string data)
    {
        JSONObject json = new JSONObject(data);

        if (json)
        {
			_publicName = json["public_name"].str;
			_difficulty = (int)json["difficulty"].n;
            _illuminated = json["illuminated"].n == 1;
            _song = json["song"].str;

            JSONObject door = json["door"];

            if (door)
            {
                _door.x = (int)door["x"].n;
                _door.y = (int)door["y"].n;
                _door.targetLevel = door["targetLevel"].str;
            }

            JSONObject player = json["player"];

            if (player)
            {
                int x = (int)player["x"].n;
                int y = (int)player["y"].n;

                int ice = (int)player["ice"].n;
                int fire = (int)player["fire"].n;
                int earth = (int)player["earth"].n;
                int wind = (int)player["wind"].n;
                int eye = 0;

                if (player["eye"])
                {
                    eye = (int)player["eye"].n;
                }

                _player.x = x;
                _player.y = y;
                _player.ice = ice;
                _player.fire = fire;
                _player.earth = earth;
                _player.wind = wind;
                _player.eye = eye;

                JSONObject light = player["light"];

                if (light)
                {
                    _player.light = new Light();

                    float color_r = light["color_r"].n;
                    float color_g = light["color_g"].n;
                    float color_b = light["color_b"].n;
                    float radius = light["radius"].n;

                    _player.light.color_r = color_r;
                    _player.light.color_g = color_g;
                    _player.light.color_b = color_b;
                    _player.light.radius = radius;
                }
            }

			List<JSONObject> jsonBlocks = json["blocks"].list;

            for (int i = 0; i < jsonBlocks.Count; i++)
            {
                BlockData d = new BlockData();

                int x = (int)jsonBlocks[i]["x"].n;
                int y = (int)jsonBlocks[i]["y"].n;
                BlockType t = (BlockType)jsonBlocks[i]["type"].n;
                string texture = jsonBlocks[i]["texture"].str;
                string destruction_sound = jsonBlocks[i]["destruction_sound"].str;
                string walk_sound = jsonBlocks[i]["walk_sound"].str;
                string move_sound = jsonBlocks[i]["move_sound"].str;

                JSONObject light = jsonBlocks[i]["light"];

                if (light)
                {
                    d.light = new Light();

                    float color_r = light["color_r"].n;
                    float color_g = light["color_g"].n;
                    float color_b = light["color_b"].n;
                    float radius = light["radius"].n;

                    d.light.color_r = color_r;
                    d.light.color_g = color_g;
                    d.light.color_b = color_b;
                    d.light.radius = radius;
                }

                if (jsonBlocks[i]["length"])
                {
                    d.length = (int)jsonBlocks[i]["length"].n;
                }
                else
                {
                    d.length = 1;
                }

                Vector2 pt = new Vector2();
                pt.x = x;
                pt.y = y;

                d.type = t;
                d.destruction_sound = destruction_sound;
                d.move_sound = move_sound;
                d.walk_sound = walk_sound;
                d.texture = texture;

                _blocks.Add(pt, d);
            }

            if (json["backgrounds"] != null)
            {
                List<JSONObject> jsonBgs = json["backgrounds"].list;

                for (int i = 0; i < jsonBgs.Count; i++)
                {
                    BackgroundData d = new BackgroundData();

                    int x = (int)jsonBgs[i]["x"].n;
                    int y = (int)jsonBgs[i]["y"].n;
                    string texture = jsonBgs[i]["texture"].str;
                    int layer = 0;

                    if (jsonBgs[i]["layer"])
                    {
                        layer = (int)jsonBgs[i]["layer"].n;
                    }

                    JSONObject light = jsonBgs[i]["light"];

                    if (light)
                    {
                        d.light = new Light();

                        float color_r = light["color_r"].n;
                        float color_g = light["color_g"].n;
                        float color_b = light["color_b"].n;
                        float radius = light["radius"].n;

                        d.light.color_r = color_r;
                        d.light.color_g = color_g;
                        d.light.color_b = color_b;
                        d.light.radius = radius;
                    }

                    Vector3 pt = new Vector3();
                    pt.x = x;
                    pt.y = y;
                    pt.z = layer;

                    d.texture = texture;
                    d.layer = layer;

                    _backgrounds.Add(pt, d);
                }
            }

            if (json["enemies"] != null)
            {
                List<JSONObject> jsonEnemies = json["enemies"].list;

                for (int i = 0; i < jsonEnemies.Count; i++)
                {
                    EnemyType t = (EnemyType)jsonEnemies[i]["type"].n;

                    if (t == EnemyType.Flyer)
                    {
                        EnemyData fd = new EnemyData();

                        try
                        {
                            fd.speed = jsonEnemies[i]["speed"].n;
                            fd.hp = (int)jsonEnemies[i]["hp"].n;
                            fd.texture = jsonEnemies[i]["texture"].str;
                            fd.type = t;

                            fd.spawn.x = (int)jsonEnemies[i]["spawnx"].n;
                            fd.spawn.y = (int)jsonEnemies[i]["spawny"].n;

                            fd.p0.x = (int)jsonEnemies[i]["x0"].n;
                            fd.p0.y = (int)jsonEnemies[i]["y0"].n;
                            fd.pf.x = (int)jsonEnemies[i]["xf"].n;
                            fd.pf.y = (int)jsonEnemies[i]["yf"].n;
                        }
                        catch {}

                        _enemies.Add(fd.spawn, fd);
                    }
                    else if (t == EnemyType.Walker)
                    {
                        EnemyData wd = new EnemyData();

                        try
                        {
                            wd.speed = jsonEnemies[i]["speed"].n;
                            wd.hp = (int)jsonEnemies[i]["hp"].n;
                            wd.texture = jsonEnemies[i]["texture"].str;
                            wd.type = t;

                            wd.spawn.x = (int)jsonEnemies[i]["spawnx"].n;
                            wd.spawn.y = (int)jsonEnemies[i]["spawny"].n;

                            wd.p0.x = (int)jsonEnemies[i]["x0"].n;
                            wd.p0.y = (int)jsonEnemies[i]["y0"].n;
                            wd.pf.x = (int)jsonEnemies[i]["xf"].n;
                            wd.pf.y = (int)jsonEnemies[i]["yf"].n;
                        }
                        catch { }

                        _enemies.Add(wd.spawn, wd);
                    }
                    else if (t == EnemyType.Roamer)
                    {
                        EnemyData wd = new EnemyData();

                        try
                        {
                            wd.speed = jsonEnemies[i]["speed"].n;
                            wd.hp = (int)jsonEnemies[i]["hp"].n;
                            wd.texture = jsonEnemies[i]["texture"].str;
                            wd.type = t;

                            wd.spawn.x = (int)jsonEnemies[i]["spawnx"].n;
                            wd.spawn.y = (int)jsonEnemies[i]["spawny"].n;

                            wd.direction = (int)jsonEnemies[i]["direction"].n;
                        }
                        catch { }

                        _enemies.Add(wd.spawn, wd);
                    }
                    else if (t == EnemyType.Lever)
                    {
                        EnemyData wd = new EnemyData();

                        try
                        {
                            wd.speed = jsonEnemies[i]["speed"].n;
                            wd.hp = (int)jsonEnemies[i]["hp"].n;
                            //wd.texture = jsonEnemies[i]["texture"].str;

                            if (wd.speed == 0)
                            {
                                // horizontal
                                wd.texture = "Resources/Textures/Blocks/Plataform";
                            }
                            else
                            {
                                // vertical
                                wd.texture = "Resources/Textures/Blocks/Grille";
                            }

                            wd.type = t;

                            wd.spawn.x = (int)jsonEnemies[i]["spawnx"].n;
                            wd.spawn.y = (int)jsonEnemies[i]["spawny"].n;

                            wd.p0.x = (int)jsonEnemies[i]["x0"].n;
                            wd.p0.y = (int)jsonEnemies[i]["y0"].n;
                            wd.pf.x = (int)jsonEnemies[i]["xf"].n;
                            wd.pf.y = (int)jsonEnemies[i]["yf"].n;
                        }
                        catch { }

                        _enemies.Add(wd.spawn, wd);
                    }
                }
            }
        }
    }

    public Door GetDoor()
    {
        return _door;
    }

    public void SetDoor(Door d)
    {
        _door = d;
    }

    public BlockData GetBlock(Vector2 p)
    {
        return _blocks[p];
    }

    public Dictionary<Vector2, BlockData> GetBlockList()
    {
        return _blocks;
    }

    public Dictionary<Vector3, BackgroundData> GetBackgroundList()
    {
        return _backgrounds;
    }

    public Dictionary<Vector2, EnemyData> GetEnemyList()
    {
        return _enemies;
    }

    public void SetBlock(Vector2 p, BlockData d)
    {
        if (_blocks.ContainsKey(p))
        {
            _blocks[p] = d;
        }
        else
        {
            _blocks.Add(p, d);
        }
    }

    public void SetBackground(Vector3 p, BackgroundData d)
    {
        if (_backgrounds.ContainsKey(p))
        {
            _backgrounds[p] = d;
        }
        else
        {
            _backgrounds.Add(p, d);
        }
    }

    public void SetEnemy(Vector2 p, EnemyData d)
    {
        if (_enemies.ContainsKey(p))
        {
            _enemies[p] = d;
        }
        else
        {
            _enemies.Add(p, d);
        }
    }

    public void RemoveBlock(Vector2 pos)
    {
        _blocks.Remove(pos);
    }

    public void RemoveBackground(Vector3 pos)
    {
        _backgrounds.Remove(pos);
    }

    public void RemoveEnemy(Vector2 pos)
    {
        _enemies.Remove(pos);
    }

    public string GetPublicName()
    {
        return _publicName;
    }

    public void SetPublicName(string pn)
    {
        _publicName = pn;
    }

    public int GetDifficulty()
    {
        return _difficulty;
    }

    public void SetDifficulty(int d)
    {
        _difficulty = d;
    }
	
	public string GetName()
	{
		return _publicName;
	}
	
	public void SetName(string name)
	{
		_publicName = name;
	}

    public bool GetIlluminated()
    {
        return _illuminated;
    }

    public void SetIlluminated(bool i)
    {
        _illuminated = i;
    }

    public void SetSong(string s)
    {
        _song = s;
    }

    public string GetSong()
    {
        return _song;
    }

    public PlayerData GetPlayer()
    {
        return _player;
    }

    public void SetPlayer(PlayerData p)
    {
        _player = p;
    }
}