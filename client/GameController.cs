using UnityEngine;
using System.Collections.Generic;

public class GameController
{
	public const bool IS_EDITOR_RUNTIME = false;
    public const bool IS_DEBUG_MODE = true;

    //Singleton
    public static GameController instance;

	public enum GameState
	{
		Init,
		ShowingTestMenu,
		CreatingLevel,
		Playing
	}

    public enum SongType
    {
        Tutorial,
        Ice1,
        Ice2,
        Ice3,
        Fire1,
        Fire2,
        Fire3,
        Wind1,
        Wind2,
        Wind3,
        Earth1,
        Earth2,
        Earth3
    }

    private TextureController _textureController;
    private LevelController _levelController;
    private GuiController _guiController;
	private DebugMenuController _debugMenuController;
    private AudioController _audioController;

	private GameState _state = GameState.Init;

    private Camera _cam;
	private string _targetLevel;
    private bool _isGamePaused = false;

    private AudioClip _tutorialMusic;

    private AudioClip _wind1Music;
    private AudioClip _wind2Music;
    private AudioClip _wind3Music;

    private AudioClip _ice1Music;
    private AudioClip _ice2Music;
    private AudioClip _ice3Music;

    private AudioClip _rock1Music;
    private AudioClip _rock2Music;
    private AudioClip _rock3Music;

    private AudioClip _fire1Music;
    private AudioClip _fire2Music;
    private AudioClip _fire3Music;

    private AudioClip _winMusic;

    private AudioClip _torchSfx;
    private AudioClip _iceDragSfx;
    private AudioClip _iceDropSfx;
    private AudioClip _iceBurnSfx;
    private AudioClip _iceCreateSfx;

    public GameController()
    {

        //Singleton
        if (instance == null)
        {
            instance = this;
        }

        _textureController = new TextureController(this);

        _audioController = new AudioController(this, 12);

        // Load music and SFX
        _tutorialMusic = (AudioClip)Resources.Load("Music/IG_1");

        _ice1Music = (AudioClip)Resources.Load("Music/Ice_1");
        _ice2Music = (AudioClip)Resources.Load("Music/Ice_2");
        _ice3Music = (AudioClip)Resources.Load("Music/Ice_3");

        _fire1Music = (AudioClip)Resources.Load("Music/Fire_1");
        _fire2Music = (AudioClip)Resources.Load("Music/Fire_2");
        _fire3Music = (AudioClip)Resources.Load("Music/Fire_3");

        _wind1Music = (AudioClip)Resources.Load("Music/Wind_1");
        _wind2Music = (AudioClip)Resources.Load("Music/Wind_2");
        _wind3Music = (AudioClip)Resources.Load("Music/Wind_3");

        _rock1Music = (AudioClip)Resources.Load("Music/Rock_1");
        _rock2Music = (AudioClip)Resources.Load("Music/Rock_2");
        _rock3Music = (AudioClip)Resources.Load("Music/Rock_3");

        _torchSfx = (AudioClip)Resources.Load("SFX/Torch");
        _iceBurnSfx = (AudioClip)Resources.Load("SFX/IceBurn");
        _iceCreateSfx = (AudioClip)Resources.Load("SFX/IceCreate");
        _iceDragSfx = (AudioClip)Resources.Load("SFX/IceDrag");
        _iceDropSfx = (AudioClip)Resources.Load("SFX/IceDrop");

        // Set the audio channels
        _audioController.SetClipToChannel(0, _tutorialMusic);
        _audioController.SetLoopChannel(0, true);

        _audioController.SetClipToChannel(1, _iceBurnSfx);
        _audioController.SetClipToChannel(2, _iceCreateSfx);
        _audioController.SetClipToChannel(3, _iceDragSfx);
        _audioController.SetClipToChannel(4, _iceDropSfx);
        _audioController.SetClipToChannel(5, _torchSfx);

        _levelController = new LevelController(this);
        _guiController = new GuiController(this);
		_debugMenuController = new DebugMenuController(this);
    }

    public AudioClip GetCachedSong(SongType type)
    {
        AudioClip result = null;

        if (type == SongType.Tutorial)
        {
            result = _tutorialMusic;
        }
        else if (type == SongType.Ice1)
        {
            result = _ice1Music;
        }
        else if (type == SongType.Ice2)
        {
            result = _ice2Music;
        }
        else if (type == SongType.Ice3)
        {
            result = _ice3Music;
        }
        else if (type == SongType.Fire1)
        {
            result = _fire1Music;
        }
        else if (type == SongType.Fire2)
        {
            result = _fire2Music;
        }
        else if (type == SongType.Fire3)
        {
            result = _fire3Music;
        }
        else if (type == SongType.Wind1)
        {
            result = _wind1Music;
        }
        else if (type == SongType.Wind2)
        {
            result = _wind2Music;
        }
        else if (type == SongType.Wind3)
        {
            result = _wind3Music;
        }
        else if (type == SongType.Earth1)
        {
            result = _rock1Music;
        }
        else if (type == SongType.Earth2)
        {
            result = _rock2Music;
        }
        else if (type == SongType.Earth3)
        {
            result = _rock3Music;
        }

        return result;
    }

    public void OnUpdate()
    {
		switch (_state)
		{
			case GameState.Init:
				GameObject go = new GameObject();
                go.name = "Player Camera";
				go.transform.position = new Vector3(0,0,-1);
				
				_cam = go.AddComponent<Camera>();
				_cam.orthographic = true;
				_cam.orthographicSize = 3.5f;
				_cam.backgroundColor = Color.black;

                go.AddComponent<AudioListener>();
                _audioController.SetFollowGameObject(go);
			
				_guiController.HideDialog("InGameUI");

                if (IS_DEBUG_MODE)
                {
                    _guiController.HideDialog("DebugUI");
                    _guiController.ShowDialog("DebugMenuUI");

                    _debugMenuController.SetLevelList(_levelController.GetLevelList());
                    _state = GameState.ShowingTestMenu;
                }
                else
                {
                    _guiController.HideDialog("DebugUI");
                    _guiController.HideDialog("DebugMenuUI");
                    _targetLevel = "0_01";

                    if (SaveGameController.instance != null)
                    {
                        _targetLevel = SaveGameController.instance.GetTargetLevel();
                    }

                    _state = GameState.CreatingLevel;
                }
				
				if (IS_EDITOR_RUNTIME)
				{
					_targetLevel = "test";
					_levelController.CreateLevel("test");
					_state = GameState.CreatingLevel;
				}
				
				break;
			
			case GameState.ShowingTestMenu:
				break;
			
			case GameState.CreatingLevel:
                _guiController.ShowDialog("InGameUI");

                if (IS_DEBUG_MODE)
                {
                    _guiController.HideDialog("DebugMenuUI");
                    _guiController.ShowDialog("DebugUI");
                }
				
				_levelController.SetActiveLevel(_targetLevel);
				
				_state = GameState.Playing;
				break;
				
			case GameState.Playing: 
				break;
		}
	
        _levelController.OnUpdate();
        _guiController.OnUpdate();
    }
	
	public void StartLevel(string lv)
	{
		if (_state == GameState.Playing)
		{
			_levelController.SetActiveLevel(lv);
		}
		else
		{
			_targetLevel = lv;
			_state = GameState.CreatingLevel;
		}
	}
    
    public void OnApplicationPause()
    {
        
    }
    
    public void OnApplicationQuit()
    {
        
    }
    
    //Gets and Sets
    public LevelController GetLevelController()
    {
        return _levelController;
    }
    
    public TextureController GetTextureController()
    {
        return _textureController;
    }

    public GuiController GetGuiController()
    {
        return _guiController;
    }
    
	public DebugMenuController GetDebugMenuController()
	{
		return _debugMenuController;
	}

    public AudioController GetAudioController()
    {
        return _audioController;
    }

    public Camera GetCamera()
    {
        return _cam;
    }
    
    public static float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }

    public bool IsGamePaused()
    {
        return _isGamePaused;
    }

    public void SetGamePaused(bool pause)
    {
        _isGamePaused = pause;
    }
}
