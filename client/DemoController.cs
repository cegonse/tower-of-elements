using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DemoController : MonoBehaviour
{
    public Image _blackImage;
    public GameObject _intro;
    public GameObject _instructions;
    public GameObject _instructions2;

    private AudioSource _audio;

    public enum State
    {
        FadingInIntro,
        WaitingIntro,
        FadingOutIntro,
        FadingInInstructions,
        WaitingInstructions,
        FadingOutInstructions,
        FadingInInstructions2,
        WaitingInstructions2,
        FadingOutInstructions2
    }

    private State _state = State.FadingInIntro;

    void Start()
    {
        if (!Application.isEditor && !Application.isMobilePlatform)
        {
            Cursor.visible = false;
        }

        _audio = GetComponent<AudioSource>();
    }

    void Update ()
    {
	    if (_state == State.FadingInIntro)
        {
            Color c = _blackImage.color;
            c.a -= Time.deltaTime * 2f;
            _blackImage.color = c;

            if (c.a < 0f)
            {
                c.a = 0f;
                _blackImage.color = c;
                _state = State.WaitingIntro;
            }
        }
        else if (_state == State.WaitingIntro)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                _state = State.FadingOutIntro;
            }
        }
        else if (_state == State.FadingOutIntro)
        {
            Vector3 s = _intro.transform.localScale;
            s.x -= Time.deltaTime * 4f;
            s.y -= Time.deltaTime * 4f;
            _intro.transform.localScale = s;

            if (s.x < 0f)
            {
                _intro.SetActive(false);
                _state = State.FadingInInstructions;
            }
        }
        else if (_state == State.FadingInInstructions)
        {
            Vector3 s = _instructions.transform.localScale;
            s.x += Time.deltaTime * 4f;
            s.y += Time.deltaTime * 4f;
            _instructions.transform.localScale = s;

            if (s.x > 0.9f)
            {
                _state = State.WaitingInstructions;
            }
        }
        else if (_state == State.WaitingInstructions)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                _state = State.FadingOutInstructions;
            }
        }
        else if (_state == State.FadingOutInstructions)
        {
            Vector3 s = _instructions.transform.localScale;
            s.x -= Time.deltaTime * 4f;
            s.y -= Time.deltaTime * 4f;
            _instructions.transform.localScale = s;

            if (s.x < 0f)
            {
                _instructions.SetActive(false);
                _state = State.FadingInInstructions2;
            }
        }
        else if (_state == State.FadingInInstructions2)
        {
            Vector3 s = _instructions2.transform.localScale;
            s.x += Time.deltaTime * 4f;
            s.y += Time.deltaTime * 4f;
            _instructions2.transform.localScale = s;

            if (s.x > 0.9f)
            {
                _state = State.WaitingInstructions2;
            }
        }
        else if (_state == State.WaitingInstructions2)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                _state = State.FadingOutInstructions2;
            }
        }
        if (_state == State.FadingOutInstructions2)
        {
            Color c = _blackImage.color;
            c.a += Time.deltaTime * 2f;

            _blackImage.color = c;
            _audio.volume -= Time.deltaTime * 2f;

            if (c.a > 1f)
            {
                c.a = 1f;
                _blackImage.color = c;
                SaveGameController.instance.SetTargetLevel("0_02");
                UnityEngine.SceneManagement.SceneManager.LoadScene("game");
            }
        }
    }
}
