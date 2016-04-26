using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GuiTransformTweener : MonoBehaviour
{
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
	
	// Public members, settable from editor and code
	public RectTransform _srcTransform;
	public RectTransform _dstTransform;
	
	public float _tweenTime = 1f;
	public TweenType Type = TweenType.Linear;
	
	// Private members
	private State _state;
	private float _timer = 0f;
	private float _startTime = 0f;
	
	private RectTransform _currentTransform;
	private GameObject _caller = null;
	
	public void DoTween(GameObject caller = null)
	{
		if (_state == State.Idle)
		{
			_currentTransform = _srcTransform;
			GetComponent<RectTransform>() = _currentTransform;
			
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
			_timer = 0f;
			_startTime = 0f;
			_state = State.Idle;
		}
	}
	
	void Start()
	{
		DoTween(null);
	}
	
	void Update()
	{
		if (_state == State.Tweening)
		{
			// Perform tweening
			_timer = (Time.time - _startTime) / TweenTime;
			
			if (Type == TweenType.Smooth)
			{
				_timer = Mathf.SmoothStep(0f, 1f, _timer);
			}
			
			float x, y, w, h;
			
			x = Mathf.Lerp(_srcTransform.rect.x, _dstTransform.rect.x, _timer);
			y = Mathf.Lerp(_srcTransform.rect.y, _dstTransform.rect.x, _timer);
			xMax = Mathf.Lerp(_srcTransform.rect.xMax, _dstTransform.rect.xMax, _timer);
			yMax = Mathf.Lerp(_srcTransform.rect.yMax, _dstTransform.rect.yMax, _timer);
			
			_currentTransform.rect = new Rect(x, y, xMax, yMax);
			
			GetComponent<RectTransform> = _currentTransform;
			
			// Check bounds
			if (_timer >= TweenTime)
			{
				if (_caller != null)
				{
					_caller.SendMessage("OnGuiTweenFinished");
					_caller = null;
				}
			
				Stop();
			}
		}
	}
}