using UnityEngine;
using System.Collections.Generic;

public class GuiController {

    private GameController _gameController;
	private Dictionary<string, GameObject> _dialogs;
    private int[] _camBounds = new int[4];
	
    public GuiController(GameController gc)
    {
        _gameController = gc;
		_dialogs = new Dictionary<string, GameObject>();
    }

    public void OnUpdate()
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                this.MovePlayerRight();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                this.MovePlayerLeft();
            }

            if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
            {
                this.StopPlayer();
            }
        }
    }
	
	public void RegisterDialog(string name, GameObject go)
	{
		if (!_dialogs.ContainsKey(name))
		{
			_dialogs.Add(name, go);
		}
	}

	public void ShowDialog(string name)
	{
		if (_dialogs.ContainsKey(name))
		{
			_dialogs[name].SetActive(true);
		}
	}
	
	public void HideDialog(string name)
	{
		if (_dialogs.ContainsKey(name))
		{
			_dialogs[name].SetActive(false);
		}
	}

    public GameObject GetDialog(string name)
    {
        if (_dialogs.ContainsKey(name))
        {
            return _dialogs[name];
        }
        else
        {
            Debug.LogError("No existe el dialogo con nombre: " + name);
            return null;
        }
    }
	
    //Metodos de gui
    public void MovePlayerRight()
    {
        GameObject go = _gameController.GetLevelController().GetActiveLevel().GetEntity("player");
        if (go)
        {
            Player pl = go.GetComponent<Player>();
            if (pl)
            {
                pl.SetTargetDirection(Direction.Right);
            }
        }
    }

    public void MovePlayerLeft()
    {
        GameObject go = _gameController.GetLevelController().GetActiveLevel().GetEntity("player");
        if (go)
        {
            Player pl = go.GetComponent<Player>();
            if (pl)
            {
                pl.SetTargetDirection(Direction.Left);
            }
        }
    }

    public void StopPlayer()
    {
        GameObject go = _gameController.GetLevelController().GetActiveLevel().GetEntity("player");
        if (go)
        {
            Player pl = go.GetComponent<Player>();
            if (pl)
            {
                pl.SetTargetDirection(Direction.None);
            }
        }
    }
    
    public void DoAction(PlayerActions type)
    {
        GameObject go = _gameController.GetLevelController().GetActiveLevel().GetEntity("player");
        if (go)
        {
            Player pl = go.GetComponent<Player>();
            
            if (pl)
            {
                pl.DoAction(type);
            }
        }
    }
    
    public void ResetLevel()
    {
       string activeLevel = _gameController.GetLevelController().GetActiveLevel().GetName(); 
	   _gameController.StartLevel(activeLevel);
    }

    public void MoveCamera()
    {

        _camBounds = _gameController.GetLevelController().GetActiveLevel().GetBounds();
        float x = (Mathf.Abs(_camBounds[2]) - Mathf.Abs(_camBounds[0]));
        float y = (Mathf.Abs(_camBounds[3]) - Mathf.Abs(_camBounds[1]));
        float width = (Mathf.Abs(_camBounds[2]) + Mathf.Abs(_camBounds[0]));
        float height = (Mathf.Abs(_camBounds[3]) + Mathf.Abs(_camBounds[1]));
        if (width > height)
        {
            _gameController.GetCamera().GetComponent<Camera>().orthographicSize = (((float)Screen.height / (float)Screen.width) * width) / 2;
        }
        else
        {
            _gameController.GetCamera().GetComponent<Camera>().orthographicSize = height / 2;
        }
        //_gameController.GetCamera().GetComponent<Camera>().orthographicSize = (Mathf.Abs(_camBounds[2]) + Mathf.Abs(_camBounds[0])) * 0.25f;
        //_gameController.GetCamera().GetComponent<Camera>().orthographicSize = Mathf.Abs(_camBounds[2]) + Mathf.Abs(_camBounds[0]);
        _gameController.GetCamera().transform.position = new Vector3(x, y+1, -10f);

    }
	
	public void OnDebugPreviousLevel()
	{
		_gameController.GetDebugMenuController().SetPreviousLevel();
	}
	
	public void OnDebugNextLevel()
	{
		_gameController.GetDebugMenuController().SetNextLevel();
	}
	
	public void OnDebugStartLevel()
	{
		_gameController.StartLevel(_gameController.GetDebugMenuController().GetSelectedLevel());
	}
}
