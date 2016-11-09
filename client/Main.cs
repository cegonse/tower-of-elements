using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//ENUMS

public enum Direction : int
{
    Left,
    Right,
    Down,
    Up,
    None
}

public enum State : int
{
    Normal,
    Jumping,
    Grounded,
    Falling

}

public class Main : MonoBehaviour
{
    private GameController _gameController;
    private GuiCallbacks _guiCallbacks;
    private GameObject _canvas;

    /*
	  Este método se llama cuando
	  se inicia el juego.
	*/
    void Start()
    {
        _gameController = new GameController(this);

        GameObject canvas_prefab = null;

        if (GameController.IS_MOBILE_RUNTIME)
        {
            if (GameController.IS_DEMO_MODE)
            {
                canvas_prefab = Resources.Load<GameObject>("GUI/MobileDemoCanvas");
            }
            else
            {
                canvas_prefab = Resources.Load<GameObject>("GUI/MobileCanvas");
            }
        }
        else
        {
            if (GameController.IS_DEMO_MODE)
            {
                canvas_prefab = Resources.Load<GameObject>("GUI/DemoCanvas");
            }
            else
            {
                canvas_prefab = Resources.Load<GameObject>("GUI/Canvas");
            }
        }

        _canvas = GameObject.Instantiate(canvas_prefab);

        if (!GameController.IS_MOBILE_RUNTIME)
        {
            if (_gameController.HasJoystick())
            {
                GameObject.Find("Fire/KeyS").SetActive(false);
                GameObject.Find("Water/KeyD").SetActive(false);
                GameObject.Find("Wind/KeyF").SetActive(false);
                GameObject.Find("Earth/KeyA").SetActive(false);
                GameObject.Find("Eye/KeyE").SetActive(false);
                GameObject.Find("WinResetButton/KeyR").SetActive(false);

                GameObject.Find("MenuStartMenuUI/KeyA").SetActive(false);
                GameObject.Find("MenuEyeMenuUI/KeyE").SetActive(false);

                GameObject.Find("DeathText/KeyA").SetActive(false);
                GameObject.Find("DeathResetButton/KeyA").SetActive(false);

                GameObject.Find("WinResetButton/KeyS").SetActive(false);
                GameObject.Find("WinNextButton/KeyA").SetActive(false);
            }
            else
            {
                GameObject.Find("Fire/ButtonB").SetActive(false);
                GameObject.Find("Water/ButtonX").SetActive(false);
                GameObject.Find("Wind/ButtonA").SetActive(false);
                GameObject.Find("Earth/ButtonY").SetActive(false);
                GameObject.Find("Eye/Back").SetActive(false);
                GameObject.Find("WinResetButton/Start").SetActive(false);

                GameObject.Find("MenuStartMenuUI/ButtonA").SetActive(false);
                GameObject.Find("MenuEyeMenuUI/Back").SetActive(false);

                GameObject.Find("DeathText/ButtonA").SetActive(false);
                GameObject.Find("DeathResetButton/ButtonA").SetActive(false);

                GameObject.Find("WinResetButton/ButtonB").SetActive(false);
                GameObject.Find("WinNextButton/ButtonA").SetActive(false);
            }
        }

        _guiCallbacks = _canvas.GetComponentInChildren<GuiCallbacks>();
        _guiCallbacks.SetGameController(_gameController);

        Image[] imagesInCanvas = _canvas.GetComponentsInChildren<Image>();

        for (int i = 0; i < imagesInCanvas.Length; i++)
        {
            string path = "Textures/GUI/" + imagesInCanvas[i].gameObject.name;
			Texture2D tex = Resources.Load(path) as Texture2D;

            if (tex == null)
            {
                Debug.LogError("Missing texture on GUI!!! -> " + path);
            }

			Sprite spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
			new Vector2(0.5f, 0.5f), 256f);
			imagesInCanvas[i].sprite = spr;
        }
		
		// Register UI dialogs
		_gameController.GetGuiController().RegisterDialog("InGameUI", GameObject.Find("InGameUI"));
		_gameController.GetGuiController().RegisterDialog("DebugUI", GameObject.Find("DebugUI"));
		_gameController.GetGuiController().RegisterDialog("DebugMenuUI", GameObject.Find("DebugMenuUI"));
		
        GameObject pauseMenuUi = GameObject.Find("PauseMenuUI");
        _gameController.GetGuiController().RegisterDialog("PauseMenuUI", pauseMenuUi);
        pauseMenuUi.SetActive(false);
		
		GameObject deathMenuUi = GameObject.Find("DeathMenuUI");
        _gameController.GetGuiController().RegisterDialog("DeathMenuUI", deathMenuUi);
        deathMenuUi.SetActive(false);

        GameObject winMenuUi = GameObject.Find("WinMenuUI");
        _gameController.GetGuiController().RegisterDialog("WinMenuUI", winMenuUi);
        winMenuUi.SetActive(false);

        GameObject startLevelUi = GameObject.Find("MenuStartMenuUI");
        _gameController.GetGuiController().RegisterDialog("MenuStartMenuUI", startLevelUi);
        startLevelUi.SetActive(false);

        GameObject eyeMenuUi = GameObject.Find("MenuEyeMenuUI");
        _gameController.GetGuiController().RegisterDialog("MenuEyeMenuUI", eyeMenuUi);
        eyeMenuUi.SetActive(false);
    }

    /*
	  Este método se llama antes
	  de renderizarse el cuadro.
	*/
    void Update()
    {
        _gameController.OnUpdate();
    }

    /*
	  Este método se llama cuando
	  se ha acabado de renderizar
	  el cuadro.
	*/
    void LateUpdate()
    {
    }
    
    /*
	  Este método se llama cuando
	  la aplicación se pausa, por
	  ejemplo cuando pasa a segundo
	  plano.
	*/
    void OnApplicationPause()
    {
        _gameController.OnApplicationPause();
    }

    /*
	  Este método se llama cuando
	  se va a cerrar la aplicación.
	*/
    void OnApplicationQuit()
    {
        _gameController.OnApplicationQuit();
    }

    public GameObject GetCanvas()
    {
        return _canvas;
    }
}

