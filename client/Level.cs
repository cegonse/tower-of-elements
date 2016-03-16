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
	private string _name, _publicName;
	private int _difficulty;
	private Dictionary<string, GameObject> _entities;
	private LevelController _levelController;
	
	private AudioClip _song;
	private bool _illuminated = false;
	
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
			
			if (!string.IsNullOrEmpty(songName))
			{
				_song = Resources.Load("Music/" + songName) as AudioClip;
			}
			
            JSONObject door = json["door"];

            if (door)
            {
				// Create door entity and assign data
                int doorx = (int)door["x"].n;
                int doory = (int)door["y"].n;
                string targetLevel = door["targetLevel"].str;
				
				GameObject go_door = CreateDoor(doorx, doory, targetLevel);
				AddEntity(go_door, go_door.name);
            }

            JSONObject player = json["player"];

            if (player)
            {
				// Load player data from JSON
                int x = (int)player["x"].n;
                int y = (int)player["y"].n;

                int ice = (int)player["ice"].n;
                int fire = (int)player["fire"].n;
                int earth = (int)player["earth"].n;
                int wind = (int)player["wind"].n;
				
				JSONObject light = player["light"];
				
				// If the player has a light, assign light data
                if (light)
                {
                    //_player.light = new Light();

                    float color_r = light["color_r"].n;
                    float color_g = light["color_g"].n;
                    float color_b = light["color_b"].n;
                    float radius = light["radius"].n;

                    /*_player.light.color_r = color_r;
                    _player.light.color_g = color_g;
                    _player.light.color_b = color_b;
                    _player.light.radius = radius;*/
                }
				
				GameObject go_player = CreatePlayer(x, y, ice, fire, earth, wind);
				AddEntity(go_player, go_player.name);
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
				
                /*JSONObject light = jsonBlocks[i]["light"];

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
                }*/
				
				LoadBlock(t,x,y,texture,length);
            }
			
			// Background loading
			if (json["backgrounds"] != null)
			{
				List<JSONObject> jsonBgs = json["backgrounds"].list;
				
				for (int i = 0; i < jsonBgs.Count; i++)
				{
					int x = (int)jsonBgs[i]["x"].n;
					int y = (int)jsonBgs[i]["y"].n;
					string texture = jsonBgs[i]["texture"].str;
                    int layer = (int)jsonBgs[i]["layer"].n;
					/*JSONObject light = jsonBlocks[i]["light"];

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
					}*/
					
					GameObject go = CreateBackground(x,y,texture,layer);
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
						
						GameObject go_en = CreateEnemy(EnemyType.Walker, spawnx, spawny, texture, (BaseEnemyData)dd);
						AddEntity(go_en, "walker_enemy" + "_" + spawnx.ToString() + "_" + spawny.ToString());
					}
					else if (type == EnemyType.Roamer)
					{
						RoamerEnemyData dd = new RoamerEnemyData();
					
						int direction = (int)jsonEnemies[i]["direction"].n;
						dd.direction = (Direction)direction;
						
						GameObject go_en = CreateEnemy(EnemyType.Roamer, spawnx, spawny, texture, (BaseEnemyData)dd);
						AddEntity(go_en, "roamer_enemy" + "_" + spawnx.ToString() + "_" + spawny.ToString());
					}
				}
			}
        }

        /*
        RoamerEnemyData prueba = new RoamerEnemyData();

        int dir = (int)Direction.Right;
        prueba.direction = (Direction)dir;
        Vector2 spawn = new Vector2(3, 4);
        GameObject go_prueba = CreateEnemy(EnemyType.Roamer, 5, 4, "Blocks/Fireball/Fireball_1", (BaseEnemyData)prueba);
        AddEntity(go_prueba, "roamer_enemy" + "_" + spawn.x.ToString() + "_" + spawn.y.ToString());
        
        FlyerEnemyData prueba2 = new FlyerEnemyData();
        prueba2.p0 = new Vector2(-1, 4);
        prueba2.pf = new Vector2(5, 4);

        GameObject go_prueba2 = CreateEnemy(EnemyType.Flyer, 5, 4, "Blocks/EnemyFlyer_1/EnemyFlyer_1", (BaseEnemyData)prueba2);
        AddEntity(go_prueba2, "flyer_enemy" + "_" + prueba2.p0.x.ToString() + "_" + prueba2.p0.y.ToString());
        

        WalkerEnemyData prueba3 = new WalkerEnemyData();
        prueba3.p0 = new Vector2(-3, 0);
        prueba3.p1 = new Vector2(4, 4);

        GameObject go_prueba3 = CreateEnemy(EnemyType.Walker, 0, 0, "Blocks/EnemyFlyer_1/EnemyFlyer_1", (BaseEnemyData)prueba3);
        AddEntity(go_prueba3, "walker_enemy" + "_" + prueba3.p0.x.ToString() + "_" + prueba3.p0.y.ToString());
        */

        LeverDoorData prueba4 = new LeverDoorData();
        prueba4.p0 = new Vector2(0, 0);
        prueba4.p1 = new Vector2(-4, 0);

        GameObject go_prueba4 = CreateEnemy(EnemyType.Door, -1, 1, "Blocks/Ice/Ice_1", (BaseEnemyData)prueba4);
        AddEntity(go_prueba4, "lever_door" + "_" + prueba4.p0.x.ToString() + "_" + prueba4.p0.y.ToString());

    }
	
	public void LoadBlock(BlockType type, int x, int y, string name, float length = 1)
	{
		GameObject go = CreateBlock(type, x, y, name, length);
        AddEntity(go, go.name);
	}

    public GameObject CreateBlock(BlockType type, int x, int y, string texture, float length = 1)
    {
        GameObject go = new GameObject("_" + x.ToString() + "_" + y.ToString() + "_" + Random.Range(0, 5000).ToString() + "_blockEntity");
        go.transform.position = new Vector3(x, y, 0);

        BoxCollider2D boxColl = go.AddComponent<BoxCollider2D>();
        boxColl.size = new Vector2(length, 1f);
        boxColl.offset = new Vector2((length / 2) - 0.5f, 0f);
        Rigidbody2D r = go.AddComponent<Rigidbody2D>();
        r.isKinematic = true;

        Block b = go.AddComponent<Block>();
        b.SetType(type);
        b.SetLength(length);
        b.SetActiveLevel(this);
        b.SetTextureRoute(texture);

        //****************
        //  Add textures
        //****************
        switch (type)
        {
            //------------------------------
            //Blocks with more than one unit
            case BlockType.Ice:


                if (length == 1) //Create only one texture for the 'go' object
                {
                    Texture2D tex = null;

                    tex = (Texture2D)_levelController.GetGameController().
                                GetTextureController().GetTexture(texture);
                    float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize(texture);

                    SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
                    Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f), texSize);
                    rend.sprite = spr;
                    //Adding value to sorting layer
                    rend.sortingOrder = 100;

                    //Adding the SpriteAnimator component
                    SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();
                    if (_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim") != null)
                    {
                        sprite_animator.AddAnimation("STANDING",_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
                    }
                }
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
                                GetTextureController().GetTexture(main_tex_id + "_13");
                    float texSize = _levelController.GetGameController().
                                GetTextureController().GetTextureSize(main_tex_id + "_13");
                    //Create child object and add it to 'go' object
                    GameObject go_child = new GameObject("child_0_" + go.name);
                    go_child.transform.parent = go.transform;
                    //Make the child stay at the right position
                    go_child.transform.position = new Vector3(x + leftPos, y, 0f);

                    //Create the SpriteRenderer and attach an Sprite to it
                    SpriteRenderer rend = go_child.AddComponent<SpriteRenderer>();
                    Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f), texSize);
                    rend.sprite = spr;

                    //Adding value to sorting layer
                    rend.sortingOrder = 100;

                    //SpriteAnimator
                    SpriteAnimator sprite_animator = go_child.AddComponent<SpriteAnimator>();
                    if (_levelController.GetGameController().GetTextureController().GetAnimation(main_tex_id + "_13" + "_Anim") != null)
                    {
                        sprite_animator.AddAnimation("STANDING",_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
                    }

                    //Create middle blocks
                    for (int i = 1; i < length - 1; i++)
                    {
                        //Get the texture
                        tex = null;
                        tex = (Texture2D)_levelController.GetGameController().
                                    GetTextureController().GetTexture(main_tex_id + "_14");
                        texSize = _levelController.GetGameController().
                                GetTextureController().GetTextureSize(main_tex_id + "_14");
                        //Create child object and add it to 'go' object
                        go_child = new GameObject("child_0_" + go.name);
                        go_child.transform.parent = go.transform;
                        //Make the child stay at the right position
                        go_child.transform.position = new Vector3(x + leftPos + i, y, 0f);

                        //Create the SpriteRenderer and attach an Sprite to it
                        rend = go_child.AddComponent<SpriteRenderer>();
                        spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                            new Vector2(0.5f, 0.5f), texSize);
                        rend.sprite = spr;

                        //Adding value to sorting layer
                        rend.sortingOrder = 100;
                        //Adding the SpriteAnimator component
                        sprite_animator = go_child.AddComponent<SpriteAnimator>();
                        if (_levelController.GetGameController().GetTextureController().GetAnimation(main_tex_id + "_14" + "_Anim") != null)
                        {
                            sprite_animator.AddAnimation("STANDING",_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
                        }

                    }

                    //Create right corner

                    //Get the texture
                    tex = null;
                    tex = (Texture2D)_levelController.GetGameController().
                                GetTextureController().GetTexture(main_tex_id + "_15");
                    texSize = _levelController.GetGameController().
                                GetTextureController().GetTextureSize(main_tex_id + "_15");
                    //Create child object and add it to 'go' object
                    go_child = new GameObject("child_0_" + go.name);
                    go_child.transform.parent = go.transform;
                    //Make the child stay at the right position
                    go_child.transform.position = new Vector3(x + leftPos + length - 1, y, 0f);
                    
                    //Create the SpriteRenderer and attach an Sprite to it
                    rend = go_child.AddComponent<SpriteRenderer>();
                    spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f), texSize);
                    rend.sprite = spr;

                    //Adding value to sorting layer
                    rend.sortingOrder = 100;
                    //Adding the SpriteAnimator component
                    sprite_animator = go_child.AddComponent<SpriteAnimator>();
                    if (_levelController.GetGameController().GetTextureController().GetAnimation(main_tex_id + "_15" + "_Anim") != null)
                    {
                        sprite_animator.AddAnimation("STANDING", _levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
                    }

                }
                break;

            //---------------
            //One unit blocks
            case BlockType.Rock:
            case BlockType.Crate:

                Texture2D texOne = null;
                texOne = (Texture2D)_levelController.GetGameController().
                            GetTextureController().GetTexture(texture);
                float texSizeOne = _levelController.GetGameController().
                    GetTextureController().GetTextureSize(texture);

                SpriteRenderer rendOne = go.AddComponent<SpriteRenderer>();
                Sprite sprOne = Sprite.Create(texOne, new Rect(0, 0, texOne.width, texOne.height),
                    new Vector2(0.5f, 0.5f), texSizeOne);
                rendOne.sprite = sprOne;

                break;
        }

        return go;
    }
	
	public GameObject CreateBackground(int x, int y, string texture, int layer)
	{
		GameObject go = new GameObject("_" + x.ToString() + "_" + y.ToString() + "_" + Random.Range(0, 5000).ToString()+ "_backgroundEntity");
		go.transform.position = new Vector3(x, y, 0);
		
		Texture2D tex = null;
		
		tex = (Texture2D) _levelController.GetGameController().
					GetTextureController().GetTexture(texture);
        float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize(texture);
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
			new Vector2(0.5f, 0.5f), texSize);
		rend.sprite = spr;
		rend.sortingOrder = layer;

        SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();
		if (_levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim") != null)
		{
            sprite_animator.AddAnimation("STANDING", _levelController.GetGameController().GetTextureController().GetAnimation(texture + "_Anim"));
		}
		
		return go;
	}
	
	public GameObject CreateFireBall(float x, float y, Direction dir)
	{
		GameObject go = new GameObject("FireBall_"+ Random.Range(0, 5000).ToString());
		go.transform.position = new Vector3(x, y, 0);
		
		go.AddComponent<BoxCollider2D>();
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic = true;
		
		FireBall fb = go.AddComponent<FireBall>();
		fb.SetActiveLevel(this);
		fb.SetDirection(dir);
		
		Texture2D tex = null;
		
		tex = (Texture2D) _levelController.GetGameController().
					GetTextureController().GetTexture("Blocks/Fireball/Fireball_1_Frame_0");
        float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize("Blocks/Fireball/Fireball_1_Frame_0");
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
			new Vector2(0.5f, 0.5f), texSize);
		rend.sprite = spr;
		rend.sortingOrder = 100;

        SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();
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
		col.size = new Vector2(0.6f, 1f);
		
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic = true;
		
		Door d = go.AddComponent<Door>();
		d.SetActiveLevel(this);
		
		if (next != "")
		{
			d.SetDestinationLevel(next);
		}
		
		Texture2D tex = null;
		
		tex = (Texture2D) _levelController.GetGameController().
					GetTextureController().GetTexture("Blocks/Door_1");
        float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize("Blocks/Door_1");
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f), texSize);
		rend.sprite = spr;
		rend.sortingOrder = 100;
		
		return go;
	}

    public GameObject CreatePlayer(int x, int y, int ice, int fire, int earth, int wind )
    {
        GameObject go = new GameObject("player");
        go.transform.position = new Vector3(x, y, 0);

        BoxCollider2D col = go.AddComponent<BoxCollider2D>();
		col.size = new Vector2(0.6f , 1.0f);
        Rigidbody2D r = go.AddComponent<Rigidbody2D>();
        r.isKinematic = true;

        Player p = go.AddComponent<Player>();
		p.SetActiveLevel(this);
		p.SetGameController(_levelController.GetGameController());
		p.SetActions(ice, fire, wind, earth);

        Texture2D tex = null;

        tex = (Texture2D)_levelController.GetGameController().
                    GetTextureController().GetTexture("Blocks/Personaje_1");
        float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize("Blocks/Personaje_1");
        SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
        Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f), texSize);
        rend.sprite = spr;
		rend.sortingOrder = 100;

        SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();

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
                Debug.Log("WARNING!! Enemy without an assigned animation: " + anim[i]);
            }
        }
            

        return go;
    }
	
	public GameObject CreateEnemy(EnemyType type, int x, int y, string name, BaseEnemyData data)
	{
		GameObject go = new GameObject(x.ToString() + "_" + y.ToString() + "_" + name + "_enemy");
		go.transform.position = new Vector3(x, y, 0);
		
		BoxCollider2D box = go.AddComponent<BoxCollider2D>();
        box.size = new Vector2(0.9f, 0.9f);
		Rigidbody2D r = go.AddComponent<Rigidbody2D>();
		r.isKinematic = true;
		
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
                break;
			
			case EnemyType.Roamer:
                EnemyRoamer er = go.AddComponent<EnemyRoamer>();
                er.SetType(type);
                er.SetLevel(this);
                er.SetEnemyData(data);
				break;

            case EnemyType.Door:
                Lever lv = go.AddComponent<Lever>();
                GameObject bl = CreateBlock(BlockType.Crate, x, y, "Blocks/Wood/WoodBox_1", 2);
                AddEntity(bl, bl.name);
                lv.SetDoor(bl);
                lv.SetEnemyData(data);
                break;
        }
		
		tex = (Texture2D) _levelController.GetGameController().
				GetTextureController().GetTexture(name);
        float texSize = _levelController.GetGameController().
                    GetTextureController().GetTextureSize(name);
		SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
		Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
            new Vector2(0.5f, 0.5f), texSize);
			
		rend.sprite = spr;
		rend.sortingOrder = 100;

        SpriteAnimator sprite_animator = go.AddComponent<SpriteAnimator>();
        if (_levelController.GetGameController().GetTextureController().GetAnimation(name + "_Anim") != null)
        {
            sprite_animator.AddAnimation("WALKING", _levelController.GetGameController().GetTextureController().GetAnimation(name + "_Anim"));
        }
        else
        {
            Debug.Log("WARNING!! Enemy without an assigned animation: " + name);
        }
        if (_levelController.GetGameController().GetTextureController().GetAnimation(name + "_2_Anim") != null)
        {
            sprite_animator.AddAnimation("TURNING", _levelController.GetGameController().GetTextureController().GetAnimation(name + "_2_Anim"));
        }
        else
        {
            Debug.Log("WARNING!! Enemy without an assigned animation: " + name);
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
		
	}
	
	//gets and Sets
	
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
}