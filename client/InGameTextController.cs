using UnityEngine;
using System.Collections;
using System.Text;

public class InGameTextController : MonoBehaviour
{
    private enum TextState
    {
        Idle,
        Showing,
        Waiting,
        Clearing
    }

    public float _wordWait = 0.6f;
    public float _letterWait = 0.1f;
    public float _fadeWait = 3.0f;
    public float _fadeTime = 0.5f;

    private float _timer = 0f;

    private string _textShow = "";
    private StringBuilder _text = null;

    private char _lastLetter;
    private int _letterIndex = 0;

    private TextState _state = TextState.Idle;
    private UnityEngine.UI.Text _label;
    private Color _textColor = Color.white;

    public void Clear()
    {
        _textShow = "";
        _timer = 0f;
        _state = TextState.Idle;
    }

    public void ShowText(string t, UnityEngine.UI.Text lb)
    {
        Clear();

        _text = new StringBuilder(t);
        _letterIndex = 0;
        _lastLetter = t[_letterIndex];
        _label = lb;
        _label.text = "";
        _label.color = Color.white;

        _state = TextState.Showing;
    }
	
	void Update ()
    {
	    if (_state == TextState.Showing)
        {
            _timer += Time.deltaTime;

            if (_lastLetter == ' ')
            {
                if (_timer > _wordWait)
                {
                    _timer = 0f;

                    if (_letterIndex < _text.Length)
                    {
                        _textShow += _text[_letterIndex];
                        _label.text = _textShow;
                        _letterIndex++;
                    }
                    else
                    {
                        _state = TextState.Waiting;
                    }
                }
            }
            else
            {
                if (_timer > _letterWait)
                {
                    _timer = 0f;

                    if (_letterIndex < _text.Length)
                    {
                        _textShow += _text[_letterIndex];
                        _label.text = _textShow;
                        _letterIndex++;
                    }
                    else
                    {
                        _state = TextState.Waiting;
                    }
                }
            }
        }
        else if (_state == TextState.Waiting)
        {
            _timer += Time.deltaTime;

            if (_timer > _fadeWait)
            {
                _timer = 0f;
                _state = TextState.Clearing;
            }
        }
        else if (_state == TextState.Clearing)
        {
            _timer += Time.deltaTime;
            _textColor.a = 1.0f - (_timer / _fadeTime);
            _label.color = _textColor;

            if (_timer > _fadeTime)
            {
                _timer = 0f;
                _state = TextState.Idle;
            }
        }
	}
}
