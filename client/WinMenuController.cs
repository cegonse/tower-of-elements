using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinMenuController : MonoBehaviour
{
    public Image _backgroundTexture;
    public Text _timeText;
    public Image _hourGlass;
    public Image _star1;
    public Image _star2;
    public Image _star3;
    public Image _star4;
    public Image _star5;
    public Button _menuButton;
    public Button _resetButton;
    public Button _nextButton;

    private GameObject _caller;

    private enum WinState
    {
        Idle,
        Init,
        ShowingBackground,
        ShowingTimer,
        WaitingTimer,
        WaitingStar1,
        WaitingStar2,
        WaitingStar3,
        WaitingStar4,
        WaitingStar5,
        ShowingButtons
    }

    private WinState _state = WinState.Idle;
    private float _timer = 0f;

    private int _numStars = 0;
    private float _playTime = 0f;

    public void OnPlayerWin(GameObject caller, int numStars = 3, float playTime = 73f)
    {
        _state = WinState.Init;
        _caller = caller;
        _numStars = numStars;
        _playTime = playTime;
    }

    public void HideMenu()
    {
        Color cl = new Color(1f, 1f, 1f, 0f);

        _backgroundTexture.color = cl;
        _timeText.transform.localScale = Vector3.zero;
        _hourGlass.transform.localScale = Vector3.zero;
        _star1.transform.localScale = Vector3.zero;
        _star2.transform.localScale = Vector3.zero;
        _star3.transform.localScale = Vector3.zero;
        _star4.transform.localScale = Vector3.zero;
        _star5.transform.localScale = Vector3.zero;
        _menuButton.transform.localScale = Vector3.zero;
        _resetButton.transform.localScale = Vector3.zero;
        _nextButton.transform.localScale = Vector3.zero;

        _state = WinState.Idle;
    }

    void Start()
    {
        Color cl = new Color(1f, 1f, 1f, 0f);

        _backgroundTexture.color = cl;
        _timeText.transform.localScale = Vector3.zero;
        _hourGlass.transform.localScale = Vector3.zero;
        _star1.transform.localScale = Vector3.zero;
        _star2.transform.localScale = Vector3.zero;
        _star3.transform.localScale = Vector3.zero;
        _star4.transform.localScale = Vector3.zero;
        _star5.transform.localScale = Vector3.zero;
        _menuButton.transform.localScale = Vector3.zero;
        _resetButton.transform.localScale = Vector3.zero;
        _nextButton.transform.localScale = Vector3.zero;
    }

	void Update ()
    {
        if (_state == WinState.Init)
        {
            Color cl = new Color(1f,1f,1f,0f);

            _backgroundTexture.color = cl;
            _timeText.transform.localScale = Vector3.zero;
            _hourGlass.transform.localScale = Vector3.zero;
            _star1.transform.localScale = Vector3.zero;
            _star2.transform.localScale = Vector3.zero;
            _star3.transform.localScale = Vector3.zero;
            _star4.transform.localScale = Vector3.zero;
            _star5.transform.localScale = Vector3.zero;
            _menuButton.transform.localScale = Vector3.zero;
            _resetButton.transform.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;

            _timer = 0f;

            _state = WinState.ShowingBackground;
        }
        else if (_state == WinState.ShowingBackground)
        {
            Color cl = _backgroundTexture.color;
            cl.a += Time.deltaTime * 2f;
            _backgroundTexture.color = cl;

            if (cl.a > 0.5f)
            {
                _hourGlass.transform.localScale = Vector3.one;
                _timeText.transform.localScale = Vector3.one;
                _state = WinState.ShowingTimer;
            }
        }
        else if (_state == WinState.ShowingTimer)
        {
            Vector3 sc = _timeText.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 8f;
            _timeText.transform.localScale = sc;
            _hourGlass.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _timeText.GetComponent<WinTimerRollController>().StartRoll();
                _state = WinState.WaitingTimer;
            }
        }
        else if (_state == WinState.WaitingTimer)
        {
            _timer += Time.deltaTime;

            if (_timer >= 0.7f)
            {
                _timeText.GetComponent<WinTimerRollController>().ShowTime(_playTime);

                if (_numStars == 1)
                {
                    _state = WinState.WaitingStar2;
                }
                else if (_numStars == 2)
                {
                    _state = WinState.WaitingStar4;
                }
                else if (_numStars == 3)
                {
                    _state = WinState.WaitingStar1;
                }
            }
        }
        else if (_state == WinState.WaitingStar1)
        {
            Vector3 sc = _star1.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star1.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _state = WinState.WaitingStar2;
            }
        }
        else if (_state == WinState.WaitingStar2)
        {
            Vector3 sc = _star2.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star2.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                if (_numStars == 1)
                {
                    _state = WinState.ShowingButtons;
                }
                else if (_numStars == 3)
                {
                    _state = WinState.WaitingStar3;
                }
            }
        }
        else if (_state == WinState.WaitingStar3)
        {
            Vector3 sc = _star3.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star3.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _state = WinState.ShowingButtons;
            }
        }
        else if (_state == WinState.WaitingStar4)
        {
            Vector3 sc = _star4.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star4.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _state = WinState.WaitingStar5;
            }
        }
        else if (_state == WinState.WaitingStar5)
        {
            Vector3 sc = _star5.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;
            _star5.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _state = WinState.ShowingButtons;
            }
        }
        else if (_state == WinState.ShowingButtons)
        {
            Vector3 sc = _menuButton.transform.localScale;
            sc += Vector3.one * Time.deltaTime * 4f;

            _menuButton.transform.localScale = sc;
            _nextButton.transform.localScale = sc;
            _resetButton.transform.localScale = sc;

            if (sc.x >= 1f)
            {
                _state = WinState.Idle;
            }
        }
	}
}
