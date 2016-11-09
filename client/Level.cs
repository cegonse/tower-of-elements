using UnityEngine;
using System.Collections.Generic;
using System.IO;

public enum EntityType
{
	Block,
	Door,
	FireBall,
	Enemy
}

public class Level
{
    private enum State
    {
        WaitingGuiCallbacks,
        WaitingEye,
        WaitingContinue,
        Idle
    }

	private string _name, _publicName;
	private int _difficulty;
	private Dictionary<string, GameObject> _entities;
	private LevelController _levelController;
	
	private AudioClip _song;
	private bool _illuminated = false;
    private int _minX, _minY, _maxX, _maxY;
    private int[] _camBounds = new int[4];
	private string _targetLevel;
    private bool _foundTorch = false;
    private Player _player;

    private float _startWaitTimer = 0f;
    private float _startWaitTime = 0.3f;
    private State _state = State.WaitingGuiCallbacks;

    private GuiCallbacks _guiCallbacks;
    private bool _hasLoaded = false;

    private int _fireUses;
    private int _windUses;
    private int _iceUses;
    private int _earthUses;

    private GameObject _backgroundParent;

	public Level(LevelController levelController, string name)
	{
		_levelController = levelController;
		_name = name;
		_entities = new Dictionary<string, GameObject>();
    }
	
	public void AddEntity(GameObject go, string name)
	{
		_entities.Add(name, go);
	}

	public void LoadLevel()
    {
		string data = "";
        GameObject goPlayer = null;
        _foundTorch = false;
	
		if (GameController.IS_EDITOR_RUNTIME)
		{
			data = File.ReadAllText(Path.GetFullPath(".") + Path.DirectorySeparatorChar + 
				"data" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "debuglevel.txt");
		}
		else
		{
			data = (Resources.Load("Levels/"+_name) as TextAsset).text;
		}
		
        JSONObject json = new JSONObject(data);

        if (json)
        {
            _publicName = json["public_name"].str;
            _difficulty = (int)json["difficulty"].n;
            _illuminated = json["illuminated"].n == 1;
            string songName = json["song"].str;
            JSONObject door = json["door"];

            if (door)
            {
                // Create door entity and assign data
                int doorx = (int)door["x"].n;
                int doory = (int)door["y"].n;
                _targetLevel = door["targetLevel"].str;

                if (string.IsNullOrEmpty(_targetLevel))
                {
                    Debug.LogError("Level " + _name + " doesn't have a target level!");
                    Debug.LogError("Setting target level to self.");
                    _targetLevel = _name;
                }

                GameObject go_door = CreateDoor(doorx, doory, _targetLevel);
                AddEntity(go_door, go_door.name);
            }

            JSONObject player = json["player"];

            if (player)
            {
                // Load player data from JSON
                int x = (int)player["x"].n;
                int y = (int)player["y"].n;

                _iceUses = (int)player["ice"].n;
                _fireUses = (int)player["fire"].n;
                _earthUses = (int)player["earth"].n;
                _windUses = (int)player["wind"].n;

                JSONObject light = player["light"];

                // If the player has a light, assign light data
                if (light)
                {
                    float color_r = light["color_r"].n;
                    float color_g = light["color_g"].n;
                    float color_b = light["color_b"].n;
                    float radius = light["radius"].n;
                }

                goPlayer = CreatePlayer(x, y, _iceUses, _fireUses, _earthUses, _windUses);
                AddEntity(goPlayer, goPlayer.name);
            }

            List<JSONObject> jsonBlocks = json["blocks"].list;

            for (int i = 0; i < jsonBlocks.Count; i++)
            {
                int x = (int)jsonBlocks[i]["x"].n;
                int y = (int)jsonBlocks[i]["y"].n;
                BlockType t = (BlockType)jsonBlocks[i]["type"].n;
                string texture = jsonBlocks[i]["texture"].str;
                string destruction_sound = jsonBlocks[i]["destruction_sound"].str;
                string walk_sound = jsonBlocks[i]["walk_sound"].str;
                string move_sound = jsonBlocks[i]["move_sound"].str;

                float length = 1f;

                if (jsonBlocks[i]["length"] != null)
                {
                    length = jsonBlocks[i]["length"].n;
                }

                LoadBlock(t, x, y, texture, length);
            }

            // Background loading
            _backgroundParent = new GameObject();
            _backgroundParent.name = "Backgrounds";
            _backgroundParent.transform.position = Vector3.zero;
            AddEntity(_backgroundParent, "BackgroundParent");

            if (json["backgrounds"] != null)
            {
                List<JSONObject> jsonBgs = json["backgrounds"].list;

                for (int i = 0; i < jsonBgs.Count; i++)
                {
                    int x = (int)jsonBgs[i]["x"].n;
                    int y = (int)jsonBgs[i]["y"].n;
                    string texture = jsonBgs[i]["texture"].str;
                    int layer = (int)jsonBgs[i]["layer"].n;

                    GameObject go = CreateBackground(x, y, texture, layer);
                    AddEntity(go, go.name);
                }
            }

            if (json["enemies"] != null)
            {
                List<JSONObject> jsonEnemies = json["enemies"].list;

                for (int i = 0; i < jsonEnemies.Count; i++)
                {
                    EnemyType type = (EnemyType)jsonEnemies[i]["type"].n;
                    float speed = jsonEnemies[i]["speed"].n;
                    int hp = (int)jsonEnemies[i]["hp"].n;
                    string texture = jsonEnemies[i]["texture"].str;
                    int spawnx = (int)jsonEnemies[i]["spawnx"].n;
                    int spawny = (int)jsonEnemies[i]["spawny"].n;

                    if (type == EnemyType.Flyer)
                    {
                        FlyerEnemyData dd = new FlyerEnemyData();

                        int p0x = (int)jsonEnemies[i]["x0"].n;
                        int p0y = (int)jsonEnemies[i]["y0"].n;
                        int pfx = (int)jsonEnemies[i]["xf"].n;
                        int pfy = (int)jsonEnemies[i]["yf"].n;

                        dd.p0 = new Vector2(p0x, p0y);
                        dd.pf = new Vector2(pfx, pfy);
                        dd.speed = speed;
                        dd.hp = hp;

                        GameObject go_en = CreateEnemy(EnemyType.Flyer, spawnx, spawny, texture, (BaseEnemyData)dd);
                        AddEntity(go_en, "flyer_enemy" + "_" + spawnx.ToString() + "_" + spawny.ToString());
                    }
                    else if (type == EnemyType.Walker)
                    {
                        WalkerEnemyData dd = new WalkerEnemyData();

                        int p0x = (int)jsonEnemies[i]["x0"].n;
                        int p0y = (int)jsonEnemies[i]["y0"].n;
                        int pfx = (int)jsonEnemies[i]["xf"].n;
                        int pfy = (int)jsonEnemies[i]["yf"].n;

                        dd.p0 = new Vector2(p0x, p0y);
                        dd.p1 = new Vector2(pfx, pfy);
                        dd.speed = speed;
                        dd.hp = hp;
                        BaseEnemyData bb = (BaseEnemyData)dd;
                        bb.speed = dd.speed;
                        bb.hp = dd.hp;
                        GameObject go_en = CreateEnemy(EnemyType.Walker, spawnx, spawny, texture, bb);
                        AddEntity(go_en, "walker_enemy" + "_" + spawnx.ToString() + "_" + spawny.ToString());
                    }
                    else if (type == EnemyType.Roamer)
                    {
                        RoamerEnemyData dd = new RoamerEnemyData();

                        int direction = (int)jsonEnemies[i]["direction"].n;
                        dd.direction = (Direction)direction;
                        dd.speed = speed;
                        dd.hp = hp;

                        GameObject go_en = CreateEnemy(EnemyType.Roamer, spawnx, spawny, texture, (BaseEnemyData)dd);
                        AddEntity(go_en, "roamer_enemy" + "_" + spawnx.ToString() + "_" + spawny.ToString());
                    }
                    else if (type == EnemyType.Lever)
                    {
                        LeverDoorData dd = new LeverDoorData();

                        int p0x = (int)jsonEnemies[i]["x0"].n;
                        int p0y = (int)jsonEnemies[i]["y0"].n;
                        int pfx = (int)jsonEnemies[i]["xf"].n;
                        int pfy = (int)jsonEnemies[i]["yf"].n;


                        dd.p0 = new Vector2(p0x, p0y);
                        dd.p1 = new Vector2(pfx, pfy);
                        dd.speed = speed;
                        dd.hp = hp;

                        GameObject go_en = CreateEnemy(EnemyType.Lever, spawnx, spawny, texture, (BaseEnemyData)dd);
                        AddEntity(go_en, "lever_door" + "_" + spawnx.ToString() + "_" + spawny.ToString());
                    }
                }
            }

            if (json["enemies"] != null)
            {
                JSONObject jsonBounds = json["bounds"];

                if (jsonBounds)
                {
                    int minx = (int)jsonBounds["minx"].n;
                    int miny = (int)jsonBounds["miny"].n;
                    int maxx = (int)jsonBounds["maxx"].n;
                    int maxy = (int)jsonBounds["maxy"].n;

                    SetBounds(minx, miny, maxx, maxy);
                    goPlayer.GetComponent<Player>().SetLevelBounds(minx, miny, maxx, maxy);
                }
                else
                {
                    Debug.LogError("Level " + _name + " doesn't have bounds. Defaulting to zero.");
                }
            }

            _hasLoaded = true;
        }
    }
	
	public void LoadBlock(BlockType type, int x, int y, string name, float length = 1)
	{
		GameObject go = CreateBlock(type, x, y, name, length);
        AddEntity(go, go.name);
	}

    public GameObject CreateBlock(BlockType type, int x, int y, string texture, float length = 1, bool vertical = false, int sortingOrder = 125)
    {
        GameObject go = new GameObject("_" + x.ToString() + "_" + y.ToString() + "_" + Random.Range(0, 5000).ToString() + "_blockEntity");
        go.transform.position = new Vector3(x, y, 0);

        if (type != BlockType.Ice || !texture.Contains("Platform"))
        {
            go.isStatic = true;
        }

        BoxCollider2D boxColl = go.AddComponent<BoxCollider2D>();

        if (vertical)
        {
            boxColl.size = new Vector2(1f, length);
            boxColl.offset = new Vector2(0f, (length / 2) - 0.5f);
        }
        else
        {
            boxColl.size = new Vector2(length, 1f);
            boxColl.offset = new Vector2((length / 2) - 0.5f, 0f);
        }

        Block b = go.AddComponent<Block>();
        b.SetType(type);
        b.SetLength(length);
        b.SetActiveLevel(this);
        b.SetTextureRoute(texture);
        b.SetVertical(vertical);

        //****************
        //  Add textures
        //****************
                if (length == 1) //Create only one texture for the 'go' object
                {
                    Texture2D tex = null;

                    tex = (Texture2D)_levelController.GetGameController().
                                GetTextureController().GetTexture(texture);

                    if (tex == null)
                    {
                        Debug.Log("Falta textura: " + texture);
                    }

                    float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize(texture);

                    SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
                    Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f), texSize);

                    rend.sprite = spr;

                    //Adding value to sorting layer
                    rend.sortingOrder = sortingOrder;

                    if (type == BlockType.Ice || texture.Contains("Stone"))
                    {
                        rend.sortingOrder = 116;
                    }

                    //Adding the SpriteAnimator component
                    SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();
                    sprite_animator.SetActiveLevel(this);
                    if (_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim") != null)
                    {
                        sprite_animator.AddAnimation("STANDING",_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
                    }
                }
            //------------------------------
            //Blocks with more than one unit
                else
                {
                    //Create go's childs with the right texture

                    //Get the main identificator on the string --> i.e. "Ice","RBasaltA", "Crate"
                    string main_tex_id = texture.Split('_')[0];
                    //Get the most left block relative position
                    float leftPos = 0; //-((length-1)*0.5f);

                    //Create left corner

                    //Get the texture
                    Texture2D tex = null;
                    tex = (Texture2D)_levelController.GetGameController().
                        GetTextureController().GetTexture(main_tex_id + "_13" + (vertical ? "_vert" : ""));
                    float texSize = _levelController.GetGameController().
                                GetTextureController().GetTextureSize(main_tex_id + "_13" + (vertical ? "_vert" : ""));
                    //Create child object and add it to 'go' object
                    GameObject go_child = new GameObject("child_0_" + go.name);
                    go_child.transform.parent = go.transform;

                    //Make the child stay at the right position
                    if (vertical)
                    {
                        go_child.transform.position = new Vector3(x, y + leftPos, 0f);
                    }
                    else
                    {
                        go_child.transform.position = new Vector3(x + leftPos, y, 0f);
                    }
                    

                    //Create the SpriteRenderer and attach an Sprite to it
                    SpriteRenderer rend = go_child.AddComponent<SpriteRenderer>();
                    Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f), texSize);
                    rend.sprite = spr;

                    //Adding value to sorting layer
                    rend.sortingOrder = sortingOrder;

                    if (type == BlockType.Ice || texture.Contains("Stone"))
                    {
                        rend.sortingOrder = 116;
                    }

                    //SpriteAnimator
                    SpriteAnimator sprite_animator = go_child.AddComponent<SpriteAnimator>();
                    sprite_animator.SetActiveLevel(this);
                    if (_levelController.GetGameController().GetTextureController().GetAnimation(main_tex_id + "_13" + (vertical ? "_vert" : "") + "_Anim") != null)
                    {
                        sprite_animator.AddAnimation("STANDING",_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
                    }

                    //Create middle blocks
                    for (int i = 1; i < length - 1; i++)
                    {
                        //Get the texture
                        tex = null;
                        tex = (Texture2D)_levelController.GetGameController().
                                    GetTextureController().GetTexture(main_tex_id + "_14" + (vertical ? "_vert" : ""));
                        texSize = _levelController.GetGameController().
                                GetTextureController().GetTextureSize(main_tex_id + "_14" + (vertical ? "_vert" : ""));
                        //Create child object and add it to 'go' object
                        go_child = new GameObject("child_0_" + go.name);
                        go_child.transform.parent = go.transform;

                        //Make the child stay at the right position
                        if (vertical)
                        {
                            go_child.transform.position = new Vector3(x, y + leftPos + i, 0f);
                        }
                        else
                        {
                            go_child.transform.position = new Vector3(x + leftPos + i, y, 0f);
                        }
                        

                        //Create the SpriteRenderer and attach an Sprite to it
                        rend = go_child.AddComponent<SpriteRenderer>();
                        spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                            new Vector2(0.5f, 0.5f), texSize);
                        rend.sprite = spr;

                        //Adding value to sorting layer
                        rend.sortingOrder = sortingOrder;
                        if (type == BlockType.Ice || type == BlockType.Rock)
                        {
                            rend.sortingOrder = 116;
                        }
                        //Adding the SpriteAnimator component
                        sprite_animator = go_child.AddComponent<SpriteAnimator>();
                        sprite_animator.SetActiveLevel(this);
                        if (_levelController.GetGameController().GetTextureController().GetAnimation(main_tex_id + "_14" + (vertical ? "_vert" : "") + "_Anim") != null)
                        {
                            sprite_animator.AddAnimation("STANDING",_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
                        }

                    }

                    //Create right corner

                    //Get the texture
                    tex = null;
                    tex = (Texture2D)_levelController.GetGameController().
                                GetTextureController().GetTexture(main_tex_id + "_15" + (vertical ? "_vert" : ""));
                    texSize = _levelController.GetGameController().
                                GetTextureController().GetTextureSize(main_tex_id + "_15" + (vertical ? "_vert" : ""));
                    //Create child object and add it to 'go' object
                    go_child = new GameObject("child_0_" + go.name);
                    go_child.transform.parent = go.transform;
                    //Make the child stay at the right position
                    if (vertical)
                    {
                        go_child.transform.position = new Vector3(x, y + leftPos + length -1, 0f);
                    }
                    else
                    {
                        go_child.transform.position = new Vector3(x + leftPos + length - 1, y, 0f);
                    }
                    
                    
                    //Create the SpriteRenderer and attach an Sprite to it
                    rend = go_child.AddComponent<SpriteRenderer>();
                    spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f), texSize);
                    rend.sprite = spr;

                    //Adding value to sorting layer
                    rend.sortingOrder = sortingOrder;
                    if (type == BlockType.Ice || texture.Contains("Stone"))
                    {
                        rend.sortingOrder = 116;
                    }
                    //Adding the SpriteAnimator component
                    sprite_animator = go_child.AddComponent<SpriteAnimator>();
                    sprite_animator.SetActiveLevel(this);
                    if (_levelController.GetGameController().GetTextureController().GetAnimation(main_tex_id + "_15" + (vertical ? "_vert" : "") + "_Anim") != null)
                    {
                        sprite_animator.AddAnimation("STANDING", _levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
                    }

                }

        return go;
    }
	
	public GameObject CreateBackground(int x, int y, string texture, int layer)
	{
        // Instantiate background tile GameObject and set it to its position
		GameObject go = new GameObject("_" + x.ToString() + "_" + y.ToString() + "_" + layer + "_backgroundEntity");
		go.transform.position = new Vector3(x, y, 0);
        go.isStatic = true;
        go.transform.parent = _backgroundParent.transform;
		
        // Fetch the texture from the controller and get its size
		Texture2D tex = (Texture2D) _levelController.GetGameController().GetTextureController().GetTexture(texture);
        float texSize = _levelController.GetGameController().GetTextureController().GetTextureSize(texture);

#if UNITY_EDITOR
        if (tex == null)
        {
            Debug.LogError("MISSING TEXTURE! \"" + texture + "\" doesn't exist and is used as a background.");
            UnityEditor.EditorApplication.isPaused = true;
        }
#endif

        // Create the sprite renderer and set the texture and layer data
        SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
        
		Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), texSize);
		rend.sprite = spr;
		rend.sortingOrder = layer;

        // If the background tile is animated, create the animation and set its animation data
		if (_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim") != null)
		{
            SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();
            sprite_animator.SetActiveLevel(this);
            sprite_animator.AddAnimation("STANDING", _levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
		}

        // Trigger block handling
        // This only is handled if the background is a trigger block
        if (texture.Contains("Trigger"))
        {
            GameObject ttl = GameObject.Find("TriggerTextLabel");
            GameObject tti = GameObject.Find("TriggerTutorialImage");
            TriggerBlock tgb = null;

            tgb = go.AddComponent<TriggerBlock>();

            if (texture.Contains("WindTrigger"))
            {
                ttl.GetComponent<InGameTextController>().SetGameController(_levelController.GetGameController());
                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.Wind, ttl);
            }
            else if (texture.Contains("IceTrigger"))
            {
                ttl.GetComponent<InGameTextController>().SetGameController(_levelController.GetGameController());
                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.Ice, ttl);
            }
            else if (texture.Contains("FireTrigger"))
            {
                ttl.GetComponent<InGameTextController>().SetGameController(_levelController.GetGameController());
                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.Fire, ttl);
            }
            else if (texture.Contains("EarthTrigger"))
            {
                ttl.GetComponent<InGameTextController>().SetGameController(_levelController.GetGameController());
                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.Earth, ttl);
            }
            else if (texture.Contains("EyeTotem"))
            {
                string tgTex = "GUI/TriggerTutorialImageEye";

                if (SaveGameController.instance.GetActiveLanguage() == SystemLanguage.English ||
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Spanish &&
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Catalan)
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageEye_English_Mobile";
                    }
                    else
                    {
                        tgTex = "GUI/TriggerTutorialImageEye_English";
                    }
                }
                else
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageEye_Mobile";
                    }
                }

                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.TotemEye, tti);

                Sprite totemSpr = Sprite.Create((Texture2D)_levelController.GetGameController().GetTextureController().GetTexture(tgTex),
                                           new Rect(0, 0, 1024, 512), new Vector2(0.5f, 0.5f), 100f);
                tti.GetComponent<UnityEngine.UI.Image>().sprite = totemSpr;
            }
            else if (texture.Contains("WindTotem"))
            {
                string tgTex = "GUI/TriggerTutorialImageWind";

                if (SaveGameController.instance.GetActiveLanguage() == SystemLanguage.English ||
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Spanish &&
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Catalan)
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageWind_English_Mobile";
                    }
                    else
                    {
                        tgTex = "GUI/TriggerTutorialImageWind_English";
                    }
                }
                else
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageWind_Mobile";
                    }
                }

                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.TotemWind, tti);

                Sprite totemSpr = Sprite.Create((Texture2D)_levelController.GetGameController().GetTextureController().GetTexture(tgTex),
                                           new Rect(0, 0, 1024, 512), new Vector2(0.5f, 0.5f), 100f);
                tti.GetComponent<UnityEngine.UI.Image>().sprite = totemSpr;
            }
            else if (texture.Contains("FireTotem"))
            {
                string tgTex = "GUI/TriggerTutorialImageFire";

                if (SaveGameController.instance.GetActiveLanguage() == SystemLanguage.English ||
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Spanish &&
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Catalan)
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageFire_English_Mobile";
                    }
                    else
                    {
                        tgTex = "GUI/TriggerTutorialImageFire_English";
                    }
                }
                else
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageFire_Mobile";
                    }
                }

                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.TotemFire, tti);

                Sprite totemSpr = Sprite.Create((Texture2D)_levelController.GetGameController().GetTextureController().GetTexture(tgTex),
                                           new Rect(0, 0, 1024, 512), new Vector2(0.5f, 0.5f), 100f);
                tti.GetComponent<UnityEngine.UI.Image>().sprite = totemSpr;
            }
            else if (texture.Contains("IceTotem"))
            {
                string tgTex = "GUI/TriggerTutorialImageIce";

                if (SaveGameController.instance.GetActiveLanguage() == SystemLanguage.English ||
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Spanish &&
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Catalan)
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageIce_English_Mobile";
                    }
                    else
                    {
                        tgTex = "GUI/TriggerTutorialImageIce_English";
                    }
                }
                else
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageIce_Mobile";
                    }
                }

                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.TotemIce, tti);

                Sprite totemSpr = Sprite.Create((Texture2D)_levelController.GetGameController().GetTextureController().GetTexture(tgTex),
                                           new Rect(0, 0, 1024, 512), new Vector2(0.5f, 0.5f), 100f);
                tti.GetComponent<UnityEngine.UI.Image>().sprite = totemSpr;
            }
            else if (texture.Contains("EarthTotem"))
            {
                string tgTex = "GUI/TriggerTutorialImageRock";

                if (SaveGameController.instance.GetActiveLanguage() == SystemLanguage.English ||
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Spanish &&
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Catalan)
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageRock_English_Mobile";
                    }
                    else
                    {
                        tgTex = "GUI/TriggerTutorialImageRock_English";
                    }
                }
                else
                {
                    if (GameController.IS_MOBILE_RUNTIME)
                    {
                        tgTex = "GUI/TriggerTutorialImageRock_Mobile";
                    }
                }

                tgb.SetTriggerData(TriggerBlock.TriggerBlockType.TotemEarth, tti);

                Sprite totemSpr = Sprite.Create((Texture2D)_levelController.GetGameController().GetTextureController().GetTexture(tgTex),
                                           new Rect(0, 0, 1024, 512), new Vector2(0.5f, 0.5f), 100f);
                tti.GetComponent<UnityEngine.UI.Image>().sprite = totemSpr;
            }
            else
            {
                Debug.LogError("[Level.cs:CreateBackground()] Trigger type " + texture + " unknown!");
            }

            if (tgb != null)
            {
                tgb.SetGameController(_levelController.GetGameController());
            }

            if (!GameController.IS_DEBUG_MODE)
            {
                rend.enabled = false;
            }

            BoxCollider2D boxColl = go.AddComponent<BoxCollider2D>();
            boxColl.size = Vector2.one;
            boxColl.offset = Vector2.zero;

            go.name += "_Trigger";
        }

        // Audio control, set the appropiate sound effect to the channels if
        // there is a background tile with sound
        if (texture.Contains("Torch") && !_foundTorch)
        {
            _levelController.GetGameController().GetAudioController().PlayChannel(5);
            _foundTorch = true;
        }
		
		return go;
	}
	
	public GameObject CreateFireBall(float x, float y, Direction dir)
	{
		GameObject go = new GameObject("FireBall_" + x + "_" + y + "_" + Random.Range(0, 5000).ToString());
		go.transform.position = new Vector3(x, y, 0);
		
		go.AddComponent<BoxCollider2D>();

        // ---------TEST-------- - //
        // - Disabled rigidbodies on blocks
        //   to check the effect
        //
        Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic = true;
        // ---------TEST-------- - //

        FireBall fb = go.AddComponent<FireBall>();
		fb.SetActiveLevel(this);
		fb.SetDirection(dir);
        fb.SetGameController(_levelController.GetGameController());
		
		Texture2D tex = null;
		
		tex = (Texture2D) _levelController.GetGameController().
					GetTextureController().GetTexture("Blocks/Fireball/Fireball_1_Frame_0");
        float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize("Blocks/Fireball/Fireball_1_Frame_0");
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
			new Vector2(0.5f, 0.5f), texSize);
		rend.sprite = spr;
		rend.sortingOrder = 125;

        SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();
        sprite_animator.SetActiveLevel(this);

		if (_levelController.GetGameController().GetTextureController().GetAnimation("Blocks/Fireball/Fireball_1_Anim") != null)
		{
            sprite_animator.AddAnimation("STANDING", _levelController.GetGameController().GetTextureController().GetAnimation("Blocks/Fireball/Fireball_1_Anim"));
		}
		
		return go;
	}
	
	public GameObject CreateDoor(int x, int y, string next)
	{
		GameObject go = new GameObject("door");
		// Offset por que el sprite no es cuadrado
		go.transform.position = new Vector3(x, y+0.15f, 0);
		
		BoxCollider2D col = go.AddComponent<BoxCollider2D>();
		col.size = new Vector2(0.1f, 1f);

        Door d = go.AddComponent<Door>();
		d.SetActiveLevel(this);
		
		if (next != "")
		{
			d.SetDestinationLevel(next);
		}
		
		Texture2D tex = null;
        float texSize = 256;
        string textureName = "Blocks/Door_1";

        if (_name.Contains("1_"))
        {
            //textureName = "Blocks/DoorWind_1";
        }
        else if (_name.Contains("2_"))
        {
            textureName = "Blocks/DoorWater_1";
        }
        else if (_name.Contains("3_"))
        {
            textureName = "Blocks/DoorFire_1";
        }
        else if (_name.Contains("4_"))
        {
            textureName = "Blocks/DoorEarth_1";
        }

        tex = (Texture2D)_levelController.GetGameController().
                        GetTextureController().GetTexture(textureName);
        texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize(textureName);

        SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f), texSize);
		rend.sprite = spr;
		rend.sortingOrder = 90;
		
		return go;
	}

    public GameObject CreatePlayer(int x, int y, int ice, int fire, int earth, int wind )
    {
        GameObject go = new GameObject("player");
        go.transform.position = new Vector3(x, y, 0);

        BoxCollider2D col = go.AddComponent<BoxCollider2D>();
		col.size = new Vector2(0.6f , 1.0f);

        Player p = go.AddComponent<Player>();
		p.SetActiveLevel(this);
		p.SetGameController(_levelController.GetGameController());

        GameObject child = new GameObject("player_child");
        child.transform.position = new Vector3(x, y, 0);
        child.transform.parent = go.transform;

        TransformSinTweener tst = child.AddComponent<TransformSinTweener>();
        tst.SetParams(0f, 0.005f, 0.05f);

        Texture2D tex = null;

        tex = (Texture2D)_levelController.GetGameController().
                    GetTextureController().GetTexture("Blocks/Personaje_1");

        float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize("Blocks/Personaje_1");

        SpriteRenderer rend = child.AddComponent<SpriteRenderer>();

        Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f), texSize);
        rend.sprite = spr;
		rend.sortingOrder = 117;

        SpriteAnimator sprite_animator = child.AddComponent<SpriteAnimator>();
        sprite_animator.SetActiveLevel(this);

        //Animations
        string[] anim = new string[9];
        anim[0] = "Player/IdleFront/IdleFront_1_Anim";      //IDLE_FRONT
        anim[1] = "Player/Action/Action_1_Anim";            //ACTION
        anim[2] = "Player/BeginMove/BeginMove_1_Anim";      //BEGIN_MOVE
        anim[3] = "Player/Death/Death_1_Anim";              //DEATH
        anim[4] = "Player/EndMove/EndMove_1_Anim";          //END_MOVE
        anim[5] = "Player/IdleTurned/IdleTurned_1_Anim";    //IDLE_TURNED
        anim[6] = "Player/Jump/Jump_1_Anim";                //JUMP
        anim[7] = "Player/Move/Move_1_Anim";                //MOVE
        anim[8] = "Player/Turning/Turning_1_Anim";          //TURNING

        string[] animName = new string[9];
        animName[0] = "IDLE_FRONT";     //IDLE_FRONT
        animName[1] = "ACTION";         //ACTION
        animName[2] = "BEGIN_MOVE";     //BEGIN_MOVE
        animName[3] = "DEATH";          //DEATH
        animName[4] = "END_MOVE";       //END_MOVE
        animName[5] = "IDLE_TURNED";    //IDLE_TURNED
        animName[6] = "JUMP";           //JUMP
        animName[7] = "MOVE";           //MOVE
        animName[8] = "TURNING";        //TURNING

        for (int i = 0; i < anim.Length; i++)
        {
            if (_levelController.GetGameController().GetTextureController().GetAnimation(anim[i]) != null)
            {
                sprite_animator.AddAnimation(animName[i], _levelController.GetGameController().GetTextureController().GetAnimation(anim[i]));
            }
            else
            {
                Debug.Log("WARNING!! Player without an assigned animation: " + anim[i]);
            }
        }

        // Add the alpha tweener for the finishing animation
        SpriteAlphaTweener alphaTweener = child.AddComponent<SpriteAlphaTweener>();

        alphaTweener.StartAlpha = 1f;
        alphaTweener.EndAlpha = 0f;
        alphaTweener.TweenTime = 1f;

        _player = p;

        return go;
    }
	
    public Player GetPlayer()
    {
        return _player;
    }

	public GameObject CreateEnemy(EnemyType type, int x, int y, string name, BaseEnemyData data)
	{
		GameObject go = new GameObject(x.ToString() + "_" + y.ToString() + "_" + name + "_enemy");
		go.transform.position = new Vector3(x, y, 0);
		
		BoxCollider2D box = go.AddComponent<BoxCollider2D>();
        box.size = new Vector2(0.9f, 0.9f);

        // ---------TEST-------- - //
        //Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		//r.isKinematic = true;
        // ---------TEST-------- - //

        Texture2D tex = null;
		
		switch (type)
		{
			case EnemyType.Flyer:
                EnemyFlyer e1 = go.AddComponent<EnemyFlyer>();
		        e1.SetType(type);
		        e1.SetLevel(this);
				e1.SetEnemyData(data);
				break;
                
            case EnemyType.Walker:
                EnemyWalker ew = go.AddComponent<EnemyWalker>();
                ew.SetType(type);
                ew.SetLevel(this);
				ew.SetEnemyData(data);
                ew.SetGameController(_levelController.GetGameController());
                break;
			
			case EnemyType.Roamer:
                EnemyRoamer er = go.AddComponent<EnemyRoamer>();
                er.SetType(type);
                er.SetLevel(this);
                er.SetEnemyData(data);
				break;

            case EnemyType.Lever:
                Lever lv = go.AddComponent<Lever>();
                lv.SetLevel(this);
                GameObject bl = CreateBlock(BlockType.Crate, x, y, name, (data.speed > 0 ? data.speed : 1), (data.hp > 0 ? true : false), 104);
                AddEntity(bl, "blockEntity_" + x + "_" + y + "_crate");
                lv.SetDoor(bl);
                bl.GetComponent<Block>().SetPlatform(true);
                lv.SetEnemyData(data);
                break;
        }

        if (type == EnemyType.Lever)
        {
            tex = (Texture2D)_levelController.GetGameController().
                GetTextureController().GetTexture("Blocks/Lever/Lever_1_Frame_1");
            float texSize = _levelController.GetGameController().
                        GetTextureController().GetTextureSize(name);
            SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
            Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f), texSize);

            rend.sprite = spr;
            rend.sortingOrder = 103;

            Lever lv = go.GetComponent<Lever>();
            lv.SetSprites("Blocks/Lever/Lever_1_Frame_1", "Blocks/Lever/Lever_1_Frame_6");

            // Add tooltip
            GameObject tp = new GameObject();
            tp.transform.position = go.transform.position + Vector3.up * 0.5f;

            SpriteRenderer tpRend = tp.AddComponent<SpriteRenderer>();
            tpRend.sortingOrder = 300;

            string tpTexString = "GUI/LeverPressA_Spanish";

            if (SaveGameController.instance.GetActiveLanguage() == SystemLanguage.English ||
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Spanish &&
                    SaveGameController.instance.GetActiveLanguage() != SystemLanguage.Catalan)
            {
                if (GameController.IS_MOBILE_RUNTIME)
                {
                    tpTexString = "GUI/LeverTapHere_English";
                }
                else
                {
                    if (!_levelController.GetGameController().HasJoystick())
                    {
                        tpTexString = "GUI/LeverPressF";
                    }
                    else
                    {
                        tpTexString = "GUI/LeverPressA";
                    }
                }
            }
            else
            {
                if (GameController.IS_MOBILE_RUNTIME)
                {
                    tpTexString = "GUI/LeverTapHere";
                }
                else
                {
                    if (!_levelController.GetGameController().HasJoystick())
                    {
                        tpTexString = "GUI/LeverPressF_Spanish";
                    }
                }
            }

            Texture2D tpTex = (Texture2D)_levelController.GetGameController().
                GetTextureController().GetTexture(tpTexString);
            float tpTexSize = _levelController.GetGameController().
                        GetTextureController().GetTextureSize(tpTexString);

            Sprite tpSpr = Sprite.Create(tpTex, new Rect(0, 0, tpTex.width, tpTex.height),
                new Vector2(0.5f, 0.5f), tpTexSize);

            tpRend.sprite = tpSpr;

            lv.SetTooltip(tp);
            AddEntity(tp, "LeverTooltip_" + x + "_" + y);
        }
        else
        {
            tex = (Texture2D)_levelController.GetGameController().
                GetTextureController().GetTexture(name);
            float texSize = _levelController.GetGameController().
                        GetTextureController().GetTextureSize(name);
            SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
            Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f), texSize);

            rend.sprite = spr;
            rend.sortingOrder = 119;


            SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();
            sprite_animator.SetActiveLevel(this);
            if (_levelController.GetGameController().GetTextureController().GetAnimation(name + "_Anim") != null)
            {
                sprite_animator.AddAnimation("WALKING", _levelController.GetGameController().GetTextureController().GetAnimation(name + "_Anim"));
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("WARNING!! Enemy without WALKING animation: " + name);
#endif
            }
            if (_levelController.GetGameController().GetTextureController().GetAnimation(name + "_2_Anim") != null)
            {
                sprite_animator.AddAnimation("TURNING", _levelController.GetGameController().GetTextureController().GetAnimation(name + "_2_Anim"));
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("WARNING!! Enemy without TURNING animation: " + name);
#endif
            }
            if (_levelController.GetGameController().GetTextureController().GetAnimation(name + "_3_Anim") != null)
            {
                sprite_animator.AddAnimation("JUMPING", _levelController.GetGameController().GetTextureController().GetAnimation(name + "_3_Anim"));
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("WARNING!! Enemy without JUMPING animation: " + name);
#endif
            }
        }

        return go;
	}


    public void ClearLevel()
	{
        foreach( KeyValuePair<string, GameObject> entry in _entities)
        {
            GameObject.Destroy(entry.Value);
        }

        _entities.Clear();
	}
	
	public bool RemoveEntity(string name)
	{
		GameObject go = _entities[name];
		GameObject.Destroy(go);
		return _entities.Remove(name);
	}

	
	public void OnUpdate()
	{
        if (_state == State.WaitingGuiCallbacks)
        {
            _levelController.GetGameController().GetGuiController().HideDialog("MenuStartMenuUI");
            _guiCallbacks = GameObject.Find("GuiCallbacks").GetComponent<GuiCallbacks>();

            if (_guiCallbacks != null && _guiCallbacks.IsReady())
            {
                if (_name.Contains("0_01") || _name.Contains("0_02"))
                {
                    _guiCallbacks.DisableEye();
                    _state = State.Idle;
                }
                else
                {
                    _guiCallbacks.OnEye(true);
                    _guiCallbacks.DisableEye();
                    _state = State.WaitingEye;
                }
            }
        }
        else if (_state == State.WaitingEye)
        {
            _startWaitTimer += Time.deltaTime;

            if (_startWaitTimer > _startWaitTime)
            {
                _levelController.GetGameController().GetGuiController().ShowDialog("MenuStartMenuUI");

                // Show level uses
                if (_windUses != 0)
                {
                    GameObject.Find("MenuStartMenuUI/WindIcon").GetComponent<UnityEngine.UI.Image>().enabled = true;
                    GameObject.Find("MenuStartMenuUI/StartWindUses").GetComponent<UnityEngine.UI.Text>().enabled = true;
                    GameObject.Find("MenuStartMenuUI/StartWindUses").GetComponent<UnityEngine.UI.Text>().text = _windUses.ToString();
                }
                else
                {
                    GameObject.Find("MenuStartMenuUI/WindIcon").GetComponent<UnityEngine.UI.Image>().enabled = false;
                    GameObject.Find("MenuStartMenuUI/StartWindUses").GetComponent<UnityEngine.UI.Text>().enabled = false;
                }

                if (_fireUses != 0)
                {
                    GameObject.Find("MenuStartMenuUI/FireIcon").GetComponent<UnityEngine.UI.Image>().enabled = true;
                    GameObject.Find("MenuStartMenuUI/StartFireUses").GetComponent<UnityEngine.UI.Text>().enabled = true;
                    GameObject.Find("MenuStartMenuUI/StartFireUses").GetComponent<UnityEngine.UI.Text>().text = _fireUses.ToString();
                }
                else
                {
                    GameObject.Find("MenuStartMenuUI/FireIcon").GetComponent<UnityEngine.UI.Image>().enabled = false;
                    GameObject.Find("MenuStartMenuUI/StartFireUses").GetComponent<UnityEngine.UI.Text>().enabled = false;
                }

                if (_iceUses != 0)
                {
                    GameObject.Find("MenuStartMenuUI/WaterIcon").GetComponent<UnityEngine.UI.Image>().enabled = true;
                    GameObject.Find("MenuStartMenuUI/StartWaterUses").GetComponent<UnityEngine.UI.Text>().enabled = true;
                    GameObject.Find("MenuStartMenuUI/StartWaterUses").GetComponent<UnityEngine.UI.Text>().text = _iceUses.ToString();
                }
                else
                {
                    GameObject.Find("MenuStartMenuUI/WaterIcon").GetComponent<UnityEngine.UI.Image>().enabled = false;
                    GameObject.Find("MenuStartMenuUI/StartWaterUses").GetComponent<UnityEngine.UI.Text>().enabled = false;
                }

                if (_earthUses != 0)
                {
                    GameObject.Find("MenuStartMenuUI/EarthIcon").GetComponent<UnityEngine.UI.Image>().enabled = true;
                    GameObject.Find("MenuStartMenuUI/StartEarthUses").GetComponent<UnityEngine.UI.Text>().enabled = true;
                    GameObject.Find("MenuStartMenuUI/StartEarthUses").GetComponent<UnityEngine.UI.Text>().text = _earthUses.ToString();
                }
                else
                {
                    GameObject.Find("MenuStartMenuUI/EarthIcon").GetComponent<UnityEngine.UI.Image>().enabled = false;
                    GameObject.Find("MenuStartMenuUI/StartEarthUses").GetComponent<UnityEngine.UI.Text>().enabled = false;
                }

                _state = State.WaitingContinue;
            }
        }
        else if (_state == State.WaitingContinue)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                _levelController.GetGameController().GetGuiController().HideDialog("MenuStartMenuUI");
                _guiCallbacks.OnEye(true);
                _guiCallbacks.EnableEye();
                _levelController.GetGameController().GetCamera().transform.localPosition = Vector3.zero + Vector3.back;

                // Set player uses
                GetEntity("player").GetComponent<Player>().SetActions(_iceUses, _fireUses, _windUses, _earthUses);

                _state = State.Idle;
            }
        }
	}
	
	//gets and Sets
    public GuiCallbacks GetGuiCallbacks()
    {
        return _guiCallbacks;
    }
	
	public Dictionary<string, GameObject> GetEntities()
	{
		return _entities;
	}
	
	public GameObject GetEntity(string name)
	{
		
		if (_entities.ContainsKey(name))
		{
			return _entities[name];
		}
		else
		{
			return null;
		}
		
	}
	
	public string GetName()
	{
		return _name;
	}
	
	public void SetName(string name)
	{
		_name = name;
	}

    public LevelController GetLevelController()
    {
        return _levelController;
    }

    public void SetBounds(int minx, int miny, int maxx, int maxy)
    {
        _camBounds[0] = minx;
        _camBounds[1] = miny;
        _camBounds[2] = maxx;
        _camBounds[3] = maxy;
    }
    
    public int[] GetBounds()
    {
        return _camBounds;
    }
	
	public string GetTargetLevel()
	{
		return _targetLevel;
	}
}