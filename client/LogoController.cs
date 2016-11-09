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
        else if (GameController.IS_DEMO_MODE)
        {
            if (GameController.IS_MOBILE_RUNTIME)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("MobileDemoScene");
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("DemoScene");
            }
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
