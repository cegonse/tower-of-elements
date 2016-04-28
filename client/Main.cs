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
    /*
	  Este método se llama cuando
	  se inicia el juego.
	*/
    void Start()
    {

        _gameController = new GameController();
        
        GameObject canvas_prefab = Resources.Load<GameObject>("GUI/Canvas");
        GameObject canvas = GameObject.Instantiate(canvas_prefab);

        _guiCallbacks = canvas.GetComponentInChildren<GuiCallbacks>();
        _guiCallbacks.SetGameController(_gameController);

        Image[] imagesInCanvas = canvas.GetComponentsInChildren<Image>();

        for (int i = 0; i < imagesInCanvas.Length; i++)
        {
			Texture2D tex = Resources.Load("Textures/" + imagesInCanvas[i].gameObject.name) as Texture2D;;
		
			/*if (imagesInCanvas[i].gameObject.name.Contains("Button"))
			{
				tex = Resources.Load("Textures/ice_block") as Texture2D;
			}
			else
			{
				tex = Resources.Load("Textures/"+imagesInCanvas[i].gameObject.name) as Texture2D;
			}*/
			
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

    }

    /*
	  Este método se llama cuando
	  se va a cerrar la aplicación.
	*/
    void OnApplicationQuit()
    {

    }

    /*
      Estos métodos se llaman cuando un botón acciona un callback
    */
    public void PointerDown(UnityEngine.UI.Image img)
    {
        GameObject go = img.gameObject;
        Debug.Log("PointerDown!");
        switch (go.name)
        {
            case "Left" :
                _gameController.GetGuiController().MovePlayerLeft();
                Debug.Log("Left done!");
                break;
            case "Right" :
                _gameController.GetGuiController().MovePlayerRight();
                Debug.Log("Right done!");
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

}

