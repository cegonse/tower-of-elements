using UnityEngine;
using System.Collections.Generic;

public class JumbleProfiler : MonoBehaviour
{
	public static JumbleProfiler instance;

	public struct ProfField
	{
		public float StartTime;
		public float EndTime;
		public float ElapsedTime;
		public int Times;
	}

	private Dictionary<string, ProfField> _profData;
	
	void Start()
	{
		_profData = new Dictionary<string, ProfField>();
		if (instance == null) instance = this;
	}

	public void StartTiming(string pf)
	{
		if (_profData != null)
		{
			if (_profData.ContainsKey(pf))
			{
				ProfField field = _profData[pf];
				field.StartTime = Time.realtimeSinceStartup;
				_profData[pf] = field;
			}
			else
			{
				ProfField field = new ProfField();
				field.Times = 0;
				field.StartTime = Time.realtimeSinceStartup;
				
				_profData.Add(pf, field);
			}
		}
	}
	
	public void EndTiming(string pf)
	{
		if (_profData != null)
		{
			ProfField field = _profData[pf];
			field.EndTime = Time.realtimeSinceStartup;
			field.ElapsedTime += _profData[pf].EndTime - _profData[pf].StartTime;
			field.Times++;
			_profData[pf] = field;
		}
	}
	
	public float GetElapsedTime(string pf)
	{
		return _profData[pf].ElapsedTime / (float) _profData[pf].Times;
	}
}