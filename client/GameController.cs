using UnityEngine;
using System.Collections.Generic;

public class GameController
{
	public const bool IS_EDITOR_RUNTIME = true;
    public const bool IS_DEBUG_MODE = true;

	public enum GameState
	{
		Init,
		ShowingTestMenu,
		CreatingLevel,
		Playing
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

    private AudioClip _inGameMusic;
    private AudioClip _winMusic;

    private AudioClip _torchSfx;
    private AudioClip _iceDragSfx;
    private AudioClip _iceDropSfx;
    private AudioClip _iceBurnSfx;
    private AudioClip _iceCreateSfx;

    public GameController()
    {
        _textureController = new TextureController(this);

        _audioController = new AudioController(this, 12);

        // Load music and SFX
        _inGameMusic = (AudioClip)Resources.Load("Music/IG_1");

        _torchSfx = (AudioClip)Resources.Load("SFX/Torch");
        _iceBurnSfx = (AudioClip)Resources.Load("SFX/IceBurn");
        _iceCreateSfx = (AudioClip)Resources.Load("SFX/IceCreate");
        _iceDragSfx = (AudioClip)Resources.Load("SFX/IceDrag");
        _iceDropSfx = (AudioClip)Resources.Load("SFX/IceDrop");

        // Set the audio channels
        _audioController.SetClipToChannel(0, _inGameMusic);
        _audioController.SetLoopChannel(0, true);

        _audioController.SetClipToChannel(1, _iceBurnSfx);
        _audioController.SetClipToChannel(2, _iceCreateSfx);

        _audioController.SetClipToChannel(3, _iceDragSfx);
        _audioController.SetLoopChannel(3, true);

        _audioController.SetClipToChannel(4, _iceDropSfx);
        _audioController.SetClipToChannel(5, _torchSfx);

        _levelController = new LevelController(this);
        _guiController = new GuiController(this);
		_debugMenuController = new DebugMenuController(this);
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
