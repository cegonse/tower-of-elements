using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GuiCallbacks : MonoBehaviour
{
    private GameController _gameController;
    private bool _pause = false;
    

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
				
            case "Reset":
                if (_pause == false)
                {
                    _gameController.GetGuiController().ResetLevel();
                }
                break;

            case "Eye":
                 if (_pause == false)
                 {
                     //Time.timeScale = 0;
                     _pause = true;
                 }
                 else
                 {
                     //Time.timeScale = 1;
                     _pause = false;
                 }

                _gameController.SetGamePaused(_pause);
                _gameController.GetGuiController().MoveCamera();
                break;
        }
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