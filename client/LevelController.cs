using UnityEngine;
using System.Collections.Generic;

public class LevelController
{
    public const string LAST_TUTORIAL_LEVEL = "0_04";

	private GameController _gameController;
	private Dictionary<string, Level> _levels;
	private Level _activeLevel;
    private string _lastLevel = "";
    
	public LevelController(GameController gameController)
	{
		_gameController = gameController;
		_levels = new Dictionary<string, Level>();
		
		// Level loading
        string levelList = (Resources.Load("level_list") as TextAsset).text;
        JSONObject json = new JSONObject(levelList);

        if (json)
        {
            List<JSONObject> jsonLev = json.list;

            for (int i = 0; i < jsonLev.Count; i++)
            {
                CreateLevel(jsonLev[i].str);
            }
        }

        _gameController.GetAudioController().PlayChannel(0);
        _gameController.GetAudioController().PauseChannel(0);
	}
	
    
	public bool CreateLevel(string name)
    {
        Level lv = new Level(this, name);

        if (lv == null)
        {
            return false;
        }
        else
        {
            _levels.Add(name, lv);
            return true;
        }
    }
	
	public void MarshallLevel(string levelData)
	{
		
	}
	
	public void CreateTestLevel()
	{
		Level lv = new Level(this, "test");
		_levels.Add("test", lv);
		_activeLevel = lv;
		
		for (int i = 0; i < 50; i++)
		{
			int posx = Random.Range(-10, 11);
			int posy = Random.Range(-5, 6);
			GameObject go = lv.CreateBlock((BlockType) Random.Range(0, 2),
				posx, posy, "block" + i.ToString());
				
			lv.AddEntity(go, go.name);
		}
	}
	
	public string SerializeLevel()
	{
		return "";
	}
	
	public void OnUpdate()
	{
		if(_activeLevel != null)
		{
			_activeLevel.OnUpdate();
		}
	}
	
	//Gets and Sets
	
	public GameController GetGameController()
	{
		return _gameController;
	}
	
	public Level GetActiveLevel()
	{
		return _activeLevel;
	}
	
	public Dictionary<string, Level> GetLevelList()
	{
		return _levels;
	}
	
	public void SetActiveLevel(string level)
	{
        if(_activeLevel != null)
		    _activeLevel.ClearLevel();
			
		if (GameController.IS_DEBUG_MODE)
		{
			GameObject.Find("LevelName").GetComponent<UnityEngine.UI.Text>().text = level;
		}

        if (GameController.IS_DEMO_MODE)
        {
            GameRecorder.GetInstance().SetLevel(level);
        }
        
		_activeLevel = _levels[level];

        _gameController.GetAudioController().PauseChannel(0);

        // Set appropriate music
        if (level.Contains("0_"))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Tutorial));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.Win));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("1_") && (level.Contains("_01") || level.Contains("_02") || level.Contains("_03")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Wind1));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinWind));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("1_") && (level.Contains("_04") || level.Contains("_05") || level.Contains("_06")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Wind2));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinWind));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("1_") && (level.Contains("_07") || level.Contains("_08") || level.Contains("_09") || level.Contains("_10")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Wind3));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinWind));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("2_") && (level.Contains("_01") || level.Contains("_02") || level.Contains("_03")))
        {
            // Unload unused textures from previous levels
            if (level.Contains("2_01"))
            {
                List<string> texs = _gameController.GetTextureController().GetTextureList();

                for (int i = 0; i < texs.Count; ++i)
                {
                    if (texs[i].Contains("/WoodRock/"))
                    {
                        _gameController.GetTextureController().UnloadTexture(texs[i]);
                    }
                }
            }

            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Ice1));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinIce));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("2_") && (level.Contains("_04") || level.Contains("_05") || level.Contains("_06")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Ice2));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinIce));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("2_") && (level.Contains("_07") || level.Contains("_08") || level.Contains("_09") || level.Contains("_10")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Ice3));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinIce));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("3_") && (level.Contains("_01") || level.Contains("_02") || level.Contains("_03")))
        {
            // Unload unused textures from previous levels
            if (level.Contains("3_01"))
            {
                List<string> texs = _gameController.GetTextureController().GetTextureList();

                for (int i = 0; i < texs.Count; ++i)
                {
                    if (texs[i].Contains("WR_Ice"))
                    {
                        _gameController.GetTextureController().UnloadTexture(texs[i]);
                    }
                }
            }

            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Fire1));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinFire));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("3_") && (level.Contains("_04") || level.Contains("_05") || level.Contains("_06")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Fire2));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinFire));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("3_") && (level.Contains("_07") || level.Contains("_08") || level.Contains("_09") || level.Contains("_10")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Fire3));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinFire));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("3_"))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Fire1));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("4_") && (level.Contains("_01") || level.Contains("_02") || level.Contains("_03")))
        {
            // Unload unused textures from previous levels
            if (level.Contains("4_01"))
            {
                List<string> texs = _gameController.GetTextureController().GetTextureList();

                for (int i = 0; i < texs.Count; ++i)
                {
                    if (texs[i].Contains("WR_Fire") || texs[i].Contains("BlueRockHeated"))
                    {
                        _gameController.GetTextureController().UnloadTexture(texs[i]);
                    }
                }
            }

            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Earth1));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinRock));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("4_") && (level.Contains("_04") || level.Contains("_05") || level.Contains("_06") || level.Contains("_07")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Earth2));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinRock));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("4_") && (level.Contains("_08") || level.Contains("_09") || level.Contains("_10")))
        {
            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Earth3));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.WinRock));
            _gameController.GetAudioController().PlayChannel(0);
        }
        else if (level.Contains("5_"))
        {
            // Unload unused textures from previous levels
            if (level.Contains("5_01"))
            {
                List<string> texs = _gameController.GetTextureController().GetTextureList();

                for (int i = 0; i < texs.Count; ++i)
                {
                    if (texs[i].Contains("WR_Stone") || texs[i].Contains("FondoE"))
                    {
                        _gameController.GetTextureController().UnloadTexture(texs[i]);
                    }
                }
            }

            _gameController.GetAudioController().SetClipToChannel(0, _gameController.GetCachedSong(GameController.SongType.Tutorial));
            _gameController.GetAudioController().SetClipToChannel(15, _gameController.GetCachedSong(GameController.SongType.Win));
            _gameController.GetAudioController().PlayChannel(0);
        }

        _gameController.GetAudioController().StopChannel(1, true);

        _lastLevel = level;
		_activeLevel.LoadLevel();
	}
	
}