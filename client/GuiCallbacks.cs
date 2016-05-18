﻿using UnityEngine;
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
                 if (_pause == false)
                 {
                    //Time.timeScale = 0;
                    _isCameraMoving = true;
                    _pause = true;
                 }
                 else
                 {
                    //Time.timeScale = 1;
                    _isCameraMoving = false;
                    _gameController.GetCamera().GetComponent<Camera>().orthographicSize = 3.5f;
                    _pause = false;
                 }

                _gameController.SetGamePaused(_pause);
                _gameController.GetGuiController().MoveCamera(_pause);
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
                    _wantsToReset = true;
                    HideWinMenu();
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

    public void OnPlayerHitDoor(float t, int s)
    {
        GuiController gc = _gameController.GetGuiController();
        GameObject winMenu = gc.GetDialog("WinMenuUI");

        if (winMenu != null && !_isOnWinMenu)
        {
            if (!winMenu.activeSelf)
            {
                _gameController.SetGamePaused(true);
                WinMenuController wmc = winMenu.GetComponent<WinMenuController>();
                winMenu.SetActive(true);
                wmc.OnPlayerWin(gameObject, s, t);
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
        string lv = _gameController.GetLevelController().GetActiveLevel().GetTargetLevel();

        if (SaveGameController.instance != null)
        {
            SaveGameController.instance.SetTargetLevel(lv);
        }

        _gameController.GetLevelController().GetActiveLevel().ClearLevel();
        _gameController.GetLevelController().SetActiveLevel(lv);
    }

    private void OnBackToMenu()
    {
        // To-Do: Save state and go back to the menu scene
        Application.Quit();
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