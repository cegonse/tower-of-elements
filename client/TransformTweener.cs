using UnityEngine;
using System.Collections.Generic;

public class TransformTweener : MonoBehaviour
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
	public Vector3 Position0;
	public Vector3 Scale0;
	public float Rotation0 = 0f;
	
	public Vector3 PositionF;
	public Vector3 ScaleF;
	public float RotationF = 0f;
	
	public Vector3 RotationAxis = Vector3.up;
	public float TweenTime = 1f;
	public bool Loop = true;
	public TweenType Type = TweenType.Linear;
	
	// Private members
	private State _state;
	private float _timer = 0f;
	private float _startTime = 0f;
	
	private Vector3 _srcPosition;
	private Vector3 _srcScale;
	private float _srcRotation = 0f;
	
	private Vector3 _dstPosition;
	private Vector3 _dstScale;
	private float _dstRotation = 0f;
	
	public void DoTween()
	{
		if (_state == State.Idle)
		{
			_srcPosition = Position0;
			_srcScale = Scale0;
			_srcRotation = Rotation0;
			
			_dstPosition = PositionF;
			_dstScale = ScaleF;
			_dstRotation = RotationF;
			
			_startTime = Time.time;
			_timer = 0f;
			
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
	
	void Start()
	{
		DoTween();
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
			
			transform.position = Vector3.Lerp(_srcPosition, _dstPosition, _timer);
			transform.localScale = Vector3.Lerp(_srcScale, _dstScale, _timer);
				
			float ang = Mathf.Lerp(_srcRotation, _dstRotation, _timer);
			transform.localRotation = Quaternion.AngleAxis(ang, RotationAxis);
			
			// Check bounds
			if (_timer >= 1f)
			{
				if (Loop)
				{
					if (_srcPosition == Position0 && _srcScale == Scale0 && _srcRotation == Rotation0)
					{
						_srcPosition = PositionF;
						_srcScale = ScaleF;
						_srcRotation = RotationF;
						
						_dstPosition = Position0;
						_dstScale = Scale0;
						_dstRotation = Rotation0;
					}
					else
					{
						_srcPosition = Position0;
						_srcScale = Scale0;
						_srcRotation = Rotation0;
						
						_dstPosition = PositionF;
						_dstScale = ScaleF;
						_dstRotation = RotationF;
					}
					
					_startTime = Time.time;
					_timer = 0f;
				}
				else
				{
					Stop();
				}
			}
		}
	}
}