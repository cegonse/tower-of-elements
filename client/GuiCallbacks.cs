using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GuiCallbacks : MonoBehaviour
{
    private GameController _gameController;
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
                _gameController.GetGuiController().MovePlayerLeft();
                break;
				
            case "Right":
                _gameController.GetGuiController().MovePlayerRight();
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
                _gameController.GetGuiController().DoAction(PlayerActions.Earth);
                break;
				
            case "Fire":
                _gameController.GetGuiController().DoAction(PlayerActions.Fire);
                break;
				
            case "Wind":
                _gameController.GetGuiController().DoAction(PlayerActions.Wind);
                break;
				
            case "Water":
                _gameController.GetGuiController().DoAction(PlayerActions.Ice);
                break;
				
            case "Reset":
                _gameController.GetGuiController().ResetLevel();
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