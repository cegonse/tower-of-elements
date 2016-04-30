using UnityEngine;
using System.Collections;

public class SpriteAlphaTweener : MonoBehaviour
{
    public float StartAlpha = 1f;
    public float EndAlpha = 0f;
    public float TweenTime = 1f;

    public bool StartOnLoad = false;
    public bool Loop = false;
    public TweenType Type = TweenType.Linear;

    public enum TweenType : int
    {
        Linear,
        Smooth
    }

    private enum State : int
    {
        Idle,
        Tweening
    }

    private float _alpha = 0f;
    private Color _color = Color.white;
    private float _srcAlpha;
    private float _dstAlpha;

    private GameObject _caller = null;
    private State _state = State.Idle;

    private float _startTime = 0f;
    private float _timer = 0f;
    private SpriteRenderer _rend;

	void Start ()
    {
        _rend = GetComponent<SpriteRenderer>();

        if (_rend == null)
        {
#if UNITY_EDITOR
            Debug.LogError("GameObject doesn't have a SpriteRenderer attached!");
#endif
        }

        if (StartOnLoad)
        {
            DoTween();
        }
	}

    public void DoTween(GameObject caller = null)
    {
        if (_state == State.Idle)
        {
            _srcAlpha = StartAlpha;
            _dstAlpha = EndAlpha;

            _startTime = Time.time;
            _timer = 0f;

            _caller = caller;
            _state = State.Tweening;
        }
    }

    public void Stop()
    {
        if (_state == State.Tweening)
        {
            _state = State.Idle;
        }
    }

	void Update ()
    {
	    if (_state == State.Tweening)
		{
			// Perform tweening
			_timer = (Time.time - _startTime) / TweenTime;
			
			if (Type == TweenType.Smooth)
			{
				_timer = Mathf.SmoothStep(0f, 1f, _timer);
			}
			
            _alpha = Mathf.Lerp(_srcAlpha, _dstAlpha, _timer);
			_color.a = _alpha;
            _rend.color = _color;
			
			// Check bounds
			if (_timer >= 1f)
			{
				if (Loop)
				{
					if (_srcAlpha == StartAlpha)
					{
                        _srcAlpha = EndAlpha;
                        _dstAlpha = StartAlpha;
					}
					else
					{
						_srcAlpha = StartAlpha;
                        _dstAlpha = EndAlpha;
					}
					
					_startTime = Time.time;
					_timer = 0f;
				}
				else
				{
					if (_caller != null)
					{
						_caller.SendMessage("OnAlphaTweenFinished", gameObject.name);
						_caller = null;
					}
					
					Stop();
				}
			}
		}
	}
}
