using UnityEngine;
using System.Collections;

public class DemoEndController : MonoBehaviour
{
    public UnityEngine.UI.Image _black;
    private int _state = 0;

	void Update ()
    {
	    if (_state == 0)
        {
            Color c = _black.color;
            c.a -= Time.deltaTime * 4f;
            _black.color = c;

            if (c.a < 0f)
            {
                c.a = 0f;
                _black.color = c;
                _state++;
            }
        }
        else if (_state == 1)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
            {
                _state++;
            }
        }
        else if (_state == 2)
        {
            Color c = _black.color;
            c.a += Time.deltaTime * 4f;
            _black.color = c;

            if (c.a > 1f)
            {
                c.a = 1f;
                _black.color = c;

                if (GameController.IS_MOBILE_RUNTIME)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("MobileDemoScene");
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("DemoScene");
                }
            }
        }
	}
}
