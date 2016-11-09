using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GuiCallbacks : MonoBehaviour
{
    private GameController _gameController;
    private bool _pause = false;
    private bool _isOnMenu = false;
    private bool _isOnDeathMenu = false;
	private bool _wantsToReset = false;
	private bool _wantsToMenu = false;
    private bool _isCameraMoving = false;
    private bool _isOnWinMenu = false;
    private bool _wantsToContinue = false;
    private bool _isOnEye = false;

    private GameObject _settings;
    private GameObject _fire;
    private GameObject _water;
    private GameObject _earth;
    private GameObject _wind;
    private GameObject _skills;
    private GameObject _right;
    private GameObject _left;
    private GameObject _eye;
    private GameObject _reset;

    private UnityEngine.UI.Text _iceLabel;
    private UnityEngine.UI.Text _windLabel;
    private UnityEngine.UI.Text _fireLabel;
    private UnityEngine.UI.Text _earthLabel;

    private UnityEngine.UI.Image _iceButtonImage;
    private UnityEngine.UI.Image _windButtonImage;
    private UnityEngine.UI.Image _fireButtonImage;
    private UnityEngine.UI.Image _earthButtonImage;

    private float _lastJoyX = 0f;
    private float _lastJoyStickX = 0f;

    private bool _foundAll = false;
    private bool _uiHidden = false;

    void Start()
    {
        _settings = GameObject.Find("Settings"); 
        _fire = GameObject.Find("Fire");
        _water = GameObject.Find("Water");
        _earth = GameObject.Find("Earth");
        _wind = GameObject.Find("Wind");
        _skills = GameObject.Find("Skills_Background");
        _right = GameObject.Find("Right");
        _left = GameObject.Find("Left");
        _eye = GameObject.Find("Eye");
        _reset = GameObject.Find("Reset");

        if (_water != null)
        {
            _iceButtonImage = _water.GetComponent<UnityEngine.UI.Image>();
            _iceLabel = GameObject.Find("WaterUsesLabel").GetComponent<Text>();
        }

        if (_wind != null)
        {
            _windButtonImage = _wind.GetComponent<UnityEngine.UI.Image>();
            _windLabel = GameObject.Find("WindUsesLabel").GetComponent<Text>();
        }

        if (_fire != null)
        {
            _fireButtonImage = _fire.GetComponent<UnityEngine.UI.Image>();
            _fireLabel = GameObject.Find("FireUsesLabel").GetComponent<Text>();
        }

        if (_earth != null)
        {
            _earthButtonImage = _earth.GetComponent<UnityEngine.UI.Image>();
            _earthLabel = GameObject.Find("EarthUsesLabel").GetComponent<Text>();
        }
    }

    public bool IsReady()
    {
        return _foundAll;
    }

    public void Update()
    {
        if (!_foundAll)
        {
            _settings = GameObject.Find("Settings");
            _fire = GameObject.Find("Fire");
            _water = GameObject.Find("Water");
            _earth = GameObject.Find("Earth");
            _wind = GameObject.Find("Wind");
            _skills = GameObject.Find("Skills_Background");
            _right = GameObject.Find("Right");
            _left = GameObject.Find("Left");
            _eye = GameObject.Find("Eye");
            _reset = GameObject.Find("WinResetButton");

            if (_water != null)
            {
                _iceButtonImage = _water.GetComponent<UnityEngine.UI.Image>();
                _iceLabel = GameObject.Find("WaterUsesLabel").GetComponent<Text>();
            }

            if (_wind != null)
            {
                _windButtonImage = _wind.GetComponent<UnityEngine.UI.Image>();
                _windLabel = GameObject.Find("WindUsesLabel").GetComponent<Text>();
            }

            if (_fire != null)
            {
                _fireButtonImage = _fire.GetComponent<UnityEngine.UI.Image>();
                _fireLabel = GameObject.Find("FireUsesLabel").GetComponent<Text>();
            }

            if (_earth != null)
            {
                _earthButtonImage = _earth.GetComponent<UnityEngine.UI.Image>();
                _earthLabel = GameObject.Find("EarthUsesLabel").GetComponent<Text>();
            }

            if (_settings != null &&
                _fire     != null &&
                _water    != null &&
                _earth    != null &&
                _wind     != null &&
                _skills   != null &&
                _right    != null &&
                _left     != null &&
                _eye      != null &&
                _reset    != null)
            {
                _foundAll = true;
            }
        }

        if (Application.platform != RuntimePlatform.Android ||
            Application.platform != RuntimePlatform.WSAPlayerARM)
        {
            if (GameController.IS_DEMO_MODE)
            {
                if (Input.GetKey(KeyCode.F10) && Input.GetKey(KeyCode.F11))
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("DemoScene");
                }
            }

            if (!_isOnDeathMenu && !_isOnWinMenu)
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _gameController.GetGuiController().MovePlayerRight();

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.MoveRight));
                    }
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _gameController.GetGuiController().MovePlayerLeft();

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.MoveLeft));
                    }
                }

                if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    _gameController.GetGuiController().StopPlayer();

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.StopMove));
                    }
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.F))
                {
                    _gameController.GetGuiController().DoAction(PlayerActions.Wind);

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Wind));
                    }
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.D))
                {
                    _gameController.GetGuiController().DoAction(PlayerActions.Ice);

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Ice));
                    }
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.S))
                {
                    _gameController.GetGuiController().DoAction(PlayerActions.Fire);

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Fire));
                    }
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton3) || Input.GetKeyDown(KeyCode.A))
                {
                    _gameController.GetGuiController().DoAction(PlayerActions.Earth);

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Earth));
                    }
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.E))
                {
                    if (_eye.activeSelf)
                    {
                        OnEye();

                        if (GameController.IS_DEMO_MODE)
                        {
                            GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Eye));
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.R))
                {
                    if (!_pause)
                    {
                        _gameController.GetGuiController().ResetLevel();

                        if (GameController.IS_DEMO_MODE)
                        {
                            GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Reset));
                        }
                    }
                }

                // D-pad handling
                float x = Input.GetAxis("Horizontal");

                if (x == 1f && _lastJoyX == 0f)
                {
                    _gameController.GetGuiController().MovePlayerRight();
                    _gameController.GetGuiController().SetSpeedAtteniuation(1f);

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.MoveRight));
                    }
                }
                else if (x == -1f && _lastJoyX == 0f)
                {
                    _gameController.GetGuiController().MovePlayerLeft();
                    _gameController.GetGuiController().SetSpeedAtteniuation(1f);

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.MoveLeft));
                    }
                }
                else if (x == 0f && _lastJoyX != 0f)
                {
                    _gameController.GetGuiController().StopPlayer();

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.StopMove));
                    }
                }

                _lastJoyX = x;

                // Joystick handling
                float jx = Input.GetAxis("HorizontalStick");

                if (jx > 0.11f && Mathf.Abs(_lastJoyStickX) < 0.1f)
                {
                    _gameController.GetGuiController().MovePlayerRight();

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.MoveRight));
                    }
                }
                else if (jx < -0.1f && Mathf.Abs(_lastJoyStickX) < 0.1f)
                {
                    _gameController.GetGuiController().MovePlayerLeft();

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.MoveLeft));
                    }
                }
                else if (Mathf.Abs(jx) < 0.1f && Mathf.Abs(_lastJoyStickX) > 0.1f)
                {
                    _gameController.GetGuiController().StopPlayer();

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.StopMove));
                    }
                }

                _lastJoyStickX = jx;
            }
            
            if (_isOnDeathMenu)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.A))
                {
                    _wantsToReset = true;
                    HideDeathMenu();
                }
            }

            if (_isOnWinMenu)
            {
                if (_gameController.GetGuiController().GetDialog("WinMenuUI").GetComponent<WinMenuController>().CanContinue())
                {
                    if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.A))
                    {
                        _wantsToContinue = true;
                        HideWinMenu();
                    }

                    if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.S))
                    {
                        _wantsToReset = true;
                        HideWinMenu();
                    }
                }
            }

            // Mobile eye handling
            if (GameController.IS_MOBILE_RUNTIME && _isOnEye)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    OnEye();

                    if (GameController.IS_DEMO_MODE)
                    {
                        GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Eye));
                    }
                }
            }

            // Debug UI hidding handling
            if (GameController.IS_DEBUG_MODE && !_isOnEye)
            {
                if (Input.GetKeyDown(KeyCode.F12))
                {
                    ToggleShowUI();
                }
            }
        }
    }

    public void ToggleShowUI()
    {
        if (_foundAll)
        {
            if (_uiHidden)
            {
                _gameController.GetGuiController().ShowDialog("InGameUI");
                _gameController.GetGuiController().ShowDialog("DebugUI");

                _uiHidden = false;
            }
            else
            {
                _gameController.GetGuiController().HideDialog("InGameUI");
                _gameController.GetGuiController().HideDialog("DebugUI");

                _uiHidden = true;
            }
        }
    }

    public UnityEngine.UI.Text GetElementLabel(PlayerActions action)
    {
        UnityEngine.UI.Text res = null;

        switch (action)
        {
            case PlayerActions.Ice:
                res = _iceLabel;
                break;
            case PlayerActions.Wind:
                res = _windLabel;
                break;
            case PlayerActions.Fire:
                res = _fireLabel;
                break;
            case PlayerActions.Earth:
                res = _earthLabel;
                break;
        }

        return res;
    }

    public UnityEngine.UI.Image GetElementButtonImage(PlayerActions action)
    {
        UnityEngine.UI.Image res = null;

        switch (action)
        {
            case PlayerActions.Ice:
                res = _iceButtonImage;
                break;
            case PlayerActions.Wind:
                res = _windButtonImage;
                break;
            case PlayerActions.Fire:
                res = _fireButtonImage;
                break;
            case PlayerActions.Earth:
                res = _earthButtonImage;
                break;
        }

        return res;
    }

    public GameObject GetWindButton()
    {
        return _wind;
    }

    public void SetGameController(GameController gc)
    {
        _gameController = gc;
    }

    /*
      Estos métodos se llaman cuando un botón acciona un callback
    */
    public void PointerDown(UnityEngine.UI.Image img)
    {
        GameObject go = img.gameObject;
		
        switch (go.name)
        {
            case "Left":
                if (_pause == false)
                {
                    _gameController.GetGuiController().MovePlayerLeft();
                }
                break;
				
            case "Right":
                if (_pause == false)
                {
                    _gameController.GetGuiController().MovePlayerRight();
                }
                break;

        }
    }

    public void PointerUp(UnityEngine.UI.Image img)
    {
        GameObject go = img.gameObject;

        switch (go.name)
        {
            case "Left":
			
            case "Right":
                _gameController.GetGuiController().StopPlayer();
                break;
        }
    }

    public void DisableEye()
    {
        _eye.SetActive(false);
    }

    public void EnableEye()
    {
        _eye.SetActive(true);
    }

    public void Click(UnityEngine.UI.Image img)
    {
        GameObject go = img.gameObject;
        switch (go.name)
        {
            case "Earth":
                if (_pause == false)
                {
                    _gameController.GetGuiController().DoAction(PlayerActions.Earth);
                }
                break;
				
            case "Fire":
                if (_pause == false)
                {
                    _gameController.GetGuiController().DoAction(PlayerActions.Fire);
                }
                break;
				
            case "Wind":
                if (_pause == false)
                {
                    _gameController.GetGuiController().DoAction(PlayerActions.Wind);
                }
                break;
				
            case "Water":
                if (_pause == false)
                {
                    _gameController.GetGuiController().DoAction(PlayerActions.Ice);
                }
                break;

            case "Eye":
                {
                    OnEye();
                }
                break;

            case "Settings":
                {
                    ShowMenu();
                }
                break;

            case "ResumeButton":
                {
                    HideMenu();
                }
                break;
				
			case "ResetButton":
                {
					_wantsToReset = true;
                    HideMenu();
                }
                break;
				
			case "MenuButton":
                {
                    _wantsToMenu = true;
					HideMenu();
                }
                break;
				
			case "SkipLevel":
				{
                    OnContinueNextLevel();
				}
				break;

            case "DeathMenuButton":
                {
                    _wantsToMenu = true;
                    HideDeathMenu();
                }
                break;

            case "DeathResetButton":
                {
                    _wantsToReset = true;
                    HideDeathMenu();
                }
                break;

            case "WinMenuButton":
                {
                    _wantsToMenu = true;
                    HideWinMenu();
                }
                break;

            case "WinResetButton":
                {
                    if (_isOnWinMenu)
                    {
                        _wantsToReset = true;
                        HideWinMenu();
                    }
                    else
                    {
                        if (!_pause)
                        {
                            _gameController.GetGuiController().ResetLevel();

                            if (GameController.IS_DEMO_MODE)
                            {
                                GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Reset));
                            }
                        }
                    }
                }
                break;

            case "WinNextButton":
                {
                    _wantsToContinue = true;
                    HideWinMenu();
                }
                break;
        }
    }

    public bool IsOnEye()
    {
        return _pause;
    }

    public void OnEye(bool forced = false)
    {
        if (_pause == false)
        {
            _isCameraMoving = true;
            _pause = true;
            _isOnEye = true;

            _settings.GetComponent<Button>().interactable = false;
            _fire.SetActive(false);
            _water.SetActive(false);
            _wind.SetActive(false);
            _earth.SetActive(false);
            _skills.SetActive(false);
            _right.SetActive(false);
            _reset.SetActive(false);
            _left.SetActive(false);
            _eye.GetComponent<Image>().enabled = false;

            if (forced)
            {
                _eye.SetActive(false);
                _gameController.GetLevelController().GetActiveLevel().GetPlayer().transform.GetChild(0).GetComponent<TransformSinTweener>().enabled = false;
            }
            else
            {
                _gameController.GetGuiController().ShowDialog("MenuEyeMenuUI");
            }
        }
        else
        {
            _isCameraMoving = false;
            _gameController.GetCamera().GetComponent<Camera>().orthographicSize = 3.5f;
            _pause = false;
            _isOnEye = false;

            _settings.GetComponent<Button>().interactable = true;
            _fire.SetActive(true);
            _water.SetActive(true);
            _wind.SetActive(true);
            _earth.SetActive(true);
            _skills.SetActive(true);
            _right.SetActive(true);
            _left.SetActive(true);
            _reset.SetActive(true);
            _eye.GetComponent<Image>().enabled = true;

            _gameController.GetLevelController().GetActiveLevel().GetPlayer().transform.GetChild(0).GetComponent<TransformSinTweener>().enabled = true;

            if (forced)
            {
                _eye.SetActive(true);
            }
            else
            {
                _gameController.GetGuiController().HideDialog("MenuEyeMenuUI");
            }
        }

        _gameController.SetGamePaused(_pause);
        _gameController.GetGuiController().MoveCamera(_pause);
    }

    private void HideDeathMenu()
    {
        GuiController gc = _gameController.GetGuiController();
        GameObject deathMenu = gc.GetDialog("DeathMenuUI");

        if (deathMenu != null && _isOnDeathMenu)
        {
            if (deathMenu.activeSelf)
            {
                _gameController.SetGamePaused(false);
                TransformTweener tt = deathMenu.GetComponent<TransformTweener>();

                tt.Position0 = new Vector3(0, 0, 0);
                tt.PositionF = new Vector3(0, 0, 0);

                tt.Rotation0 = 0f;
                tt.RotationF = 0f;

                tt.Scale0 = new Vector3(1, 1, 1);
                tt.ScaleF = new Vector3(0, 0, 0);

                tt.DoTween(this.gameObject);
            }
        }
        else
        {
            Debug.LogError("DeathMenuUI is null!");
        }
    }

	private void HideMenu()
	{
		GuiController gc = _gameController.GetGuiController();
		GameObject pauseMenu = gc.GetDialog("PauseMenuUI");
        _gameController.GetAudioController().SetChannelVolume(0, 1f);
		
		if (pauseMenu != null && _isOnMenu)
		{
			if (pauseMenu.activeSelf)
			{
				_gameController.SetGamePaused(false);
				TransformTweener tt = pauseMenu.GetComponent<TransformTweener>();
				
				tt.Position0 = new Vector3(0, 0, 0);
				tt.PositionF = new Vector3(0, 0, 0);

				tt.Rotation0 = 0f;
				tt.RotationF = 0f;

				tt.Scale0 = new Vector3(1, 1, 1);
				tt.ScaleF = new Vector3(0, 0, 0);
				
				tt.DoTween(this.gameObject);
			}
		}
		else
		{
			Debug.LogError("PauseMenuUI is null!");
		}
	}
	
	private void ShowMenu()
	{
		GuiController gc = _gameController.GetGuiController();
		GameObject pauseMenu = gc.GetDialog("PauseMenuUI");
        _gameController.GetAudioController().SetChannelVolume(0, 0.5f);

		if (pauseMenu != null && !_isOnMenu)
		{
			if (!pauseMenu.activeSelf)
			{
				_gameController.SetGamePaused(true);
				TransformTweener tt = pauseMenu.GetComponent<TransformTweener>();
				pauseMenu.SetActive(true);
				
				tt.Position0 = new Vector3(0, 0, 0);
				tt.PositionF = new Vector3(0, 0, 0);

				tt.Rotation0 = 0f;
				tt.RotationF = 0f;

				tt.Scale0 = new Vector3(0, 0, 0);
				tt.ScaleF = new Vector3(1, 1, 1);
				
				tt.DoTween(this.gameObject);
			}
		}
		else
		{
			Debug.LogError("PauseMenuUI is null!");
		}
	}

    public void OnPlayerHitDoor(float t, int s, string id)
    {
        if (GameController.IS_DEMO_MODE)
        {
            GameRecorder.GetInstance().Upload();
        }
        else
        {
            // Search the level in the level progress save file
            bool found = false;
            List<SaveGameController.LevelProgressData> llpd = SaveGameController.instance.GetLevelProgress();
            SaveGameController.LevelProgressData lpd;

            for (int i = 0; i < llpd.Count; i++)
            {
                lpd = llpd[i];

                // If the level has been played and the player took
                // a longer time than now, replace the lowest time
                if (lpd.Id == id)
                {
                    found = true;

                    if (lpd.Score > t)
                    {
                        lpd.Score = t;
                        SaveGameController.instance.SetLevelProgress(lpd);
                    }
                }
            }

            // If this is the first time the player goes through
            // the level, add the time
            if (!found)
            {
                lpd = new SaveGameController.LevelProgressData();

                lpd.Id = id;
                lpd.Score = t;

                SaveGameController.instance.SetLevelProgress(lpd);
            }
        }

        GuiController gc = _gameController.GetGuiController();
        GameObject winMenu = gc.GetDialog("WinMenuUI");

        if (winMenu != null && !_isOnWinMenu)
        {
            if (!winMenu.activeSelf)
            {
                _gameController.SetGamePaused(true);
                WinMenuController wmc = winMenu.GetComponent<WinMenuController>();

                winMenu.SetActive(true);
                wmc.OnPlayerWin(gameObject, _gameController, s, t);
                _isOnWinMenu = true;
            }
        }
        else
        {
            Debug.LogError("WinMenuUI is null!");
        }
    }

    private void HideWinMenu()
    {
        GuiController gc = _gameController.GetGuiController();
        GameObject winMenu = gc.GetDialog("WinMenuUI");

        if (winMenu != null && _isOnWinMenu)
        {
            if (winMenu.activeSelf)
            {
                _gameController.SetGamePaused(false);
                WinMenuController wmc = winMenu.GetComponent<WinMenuController>();
                wmc.HideMenu();
                OnTweenFinished("WinMenuUI");
            }
        }
        else
        {
            Debug.LogError("WinMenuUI is null!");
        }
    }

    public void OnPlayerDied()
    {
        GuiController gc = _gameController.GetGuiController();
        GameObject deathMenu = gc.GetDialog("DeathMenuUI");

        if (GameController.IS_DEMO_MODE)
        {
            GameRecorder.GetInstance().OnGameEvent(new GameRecorder.GameEvent(Time.time, GameRecorder.GameCommand.Death));
        }

        if (deathMenu != null && !_isOnDeathMenu)
        {
            if (!deathMenu.activeSelf)
            {
                _gameController.SetGamePaused(true);
                TransformTweener tt = deathMenu.GetComponent<TransformTweener>();
                deathMenu.SetActive(true);

                tt.Position0 = new Vector3(0, 0, 0);
                tt.PositionF = new Vector3(0, 0, 0);

                tt.Rotation0 = 0f;
                tt.RotationF = 0f;

                tt.Scale0 = new Vector3(0, 0, 0);
                tt.ScaleF = new Vector3(1, 1, 1);

                tt.DoTween(this.gameObject);
            }
        }
        else
        {
            Debug.LogError("DeathMenuUI is null!");
        }
    }
	
	public void OnTweenFinished(string control)
	{
		if (control.Contains("PauseMenuUI"))
		{
			if (_isOnMenu)
			{
				GuiController gc = _gameController.GetGuiController();
				GameObject pauseMenu = gc.GetDialog("PauseMenuUI");
				pauseMenu.SetActive(false);
			}
			
			_isOnMenu = !_isOnMenu;
			
			// Handle reset event
			if (_wantsToReset)
			{
                _gameController.GetAudioController().PauseChannel(0);
                _gameController.GetAudioController().StopChannel(1, true);
				_gameController.GetGuiController().ResetLevel();
				_wantsToReset = false;
			}
			
			// Handle go back to menu event
			if (_wantsToMenu)
			{
                OnBackToMenu();
			}
		}
        else if (control.Contains("DeathMenuUI"))
        {
            if (_isOnDeathMenu)
            {
                GuiController gc = _gameController.GetGuiController();
                GameObject deathMenu = gc.GetDialog("DeathMenuUI");
                deathMenu.SetActive(false);
            }

            _isOnDeathMenu = !_isOnDeathMenu;

            // Handle reset event
            if (_wantsToReset)
            {
                _gameController.GetAudioController().PauseChannel(0);
                _gameController.GetAudioController().StopChannel(1, true);
                _gameController.GetGuiController().ResetLevel();
                _wantsToReset = false;
            }

            // Handle go back to menu event
            if (_wantsToMenu)
            {
                OnBackToMenu();
            }
        }
        else if (control.Contains("WinMenuUI"))
        {
            if (_isOnWinMenu)
            {
                GuiController gc = _gameController.GetGuiController();
                GameObject winMenu = gc.GetDialog("WinMenuUI");
                winMenu.SetActive(false);
            }

            _isOnWinMenu = !_isOnWinMenu;

            // Handle reset event
            if (_wantsToReset)
            {
                _gameController.GetGuiController().ResetLevel();
                _wantsToReset = false;
            }

            // Handle go back to menu event
            if (_wantsToMenu)
            {
                OnBackToMenu();
            }

            // Handle continue event
            if (_wantsToContinue)
            {
                OnContinueNextLevel();
                _wantsToContinue = false;
            }
        }
	}

    private void OnContinueNextLevel()
    {
        // Check if the game is set in demo mode and if 
        // it is the last level of the demo
        if (GameController.IS_DEMO_MODE)
        {
            if (_gameController.GetLevelController().GetActiveLevel().GetTargetLevel().Contains("demoend"))
            {
                if (GameController.IS_MOBILE_RUNTIME)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MobileDemoEndScene");
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("DemoEndScene");
                }
            }
        }

        // Check if it is the last level of the tutorial or
        // if it is the last level of a dungeon
        if (_gameController.GetLevelController().GetActiveLevel().GetName().Contains("_10") ||
            _gameController.GetLevelController().GetActiveLevel().GetName().Contains(LevelController.LAST_TUTORIAL_LEVEL))
        {
            // If it is the last level of the game, load the
            // credits sequence
            if (_gameController.GetLevelController().GetActiveLevel().GetName().Contains("5_"))
            {
                // Load the credits scene
            }
            else
            {
                SaveGameController.instance.SetTargetMenu(UIState.SelectDungeonMenu);
                UnityEngine.SceneManagement.SceneManager.LoadScene("IntroMenu");
            }
        }

        // Set next target level
        string lv = _gameController.GetLevelController().GetActiveLevel().GetTargetLevel();
        SaveGameController.instance.SetTargetLevel(lv);

        _gameController.GetLevelController().GetActiveLevel().ClearLevel();
        _gameController.GetLevelController().SetActiveLevel(lv);
    }

    private void OnBackToMenu()
    {
        SaveGameController.instance.SetTargetMenu(UIState.Intro);
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroMenu");
    }
	
	public void DebugMenuPreviousLevel()
	{
		_gameController.GetGuiController().OnDebugPreviousLevel();
	}
	
	public void DebugMenuNextLevel()
	{
		_gameController.GetGuiController().OnDebugNextLevel();
	}
	
	public void DebugMenuStartLevel()
	{
		_gameController.GetGuiController().OnDebugStartLevel();
	}
}