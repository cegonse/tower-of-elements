using UnityEngine;
using System.Collections.Generic;

public class GuiController {

    private GameController _gameController;
	private Dictionary<string, GameObject> _dialogs;
    private int[] _camBounds = new int[4];
    private bool _isCameraMoving;
    private Vector3 _cameraToGo;
    private Vector3 _cameraVelocity = Vector3.zero;
    private float _cameraLerp;
    private float _lerpTime;

	
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

        if (_isCameraMoving)
        {
            _lerpTime += 0.01f;
            _gameController.GetCamera().GetComponent<Camera>().orthographicSize = Mathf.Lerp(_gameController.GetCamera().GetComponent<Camera>().orthographicSize, _cameraLerp, _lerpTime);

            Vector3 camPos = Vector3.SmoothDamp(_gameController.GetCamera().transform.position, _cameraToGo, ref _cameraVelocity, 0.2f);
            _gameController.GetCamera().transform.position = camPos;

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

    public void MoveCamera(bool camera)
    {
        _isCameraMoving = camera;

        if (camera)
        {
            _camBounds = _gameController.GetLevelController().GetActiveLevel().GetBounds();

            float x = (_camBounds[2] + _camBounds[0]) / 2f;
            float y = (_camBounds[3] + _camBounds[1]) / 2f;
            float width = (Mathf.Abs(_camBounds[2]) + Mathf.Abs(_camBounds[0]));
            float height = (Mathf.Abs(_camBounds[3]) + Mathf.Abs(_camBounds[1]));

            float levelAspect = height / width;
            float cameraAspect = (float)Screen.height / (float)Screen.width;

            if (levelAspect < cameraAspect)
            {
                _cameraLerp = (cameraAspect * width) / 2f;
            }
            else
            {
                _cameraLerp = height / 2;
            }

            _cameraToGo = new Vector3(x, y, -10f);
        }
        else
        {
            _cameraLerp = 3.5f;
        }

        _lerpTime = 0f;
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
