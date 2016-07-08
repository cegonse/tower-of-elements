using UnityEngine;
using System.Collections;

public class LogoController : MonoBehaviour
{
	void Start ()
    {
        if (GameController.IS_DEBUG_MODE)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("game");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("IntroMenu");
        }
	}
	
	void Update ()
    {
	
	}
}
