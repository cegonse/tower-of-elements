using UnityEngine;
using System.Collections.Generic;

public class DebugMenuController
{
	private GameController _game;
	private List<string> _levels;
	private int _currentIndex = 0;

	public DebugMenuController(GameController game)
	{
		_game = game;
		_levels = new List<string>();
	}
	
	public void SetLevelList(Dictionary<string, Level> lv)
	{
		foreach (KeyValuePair<string, Level> lev in lv)
		{
			_levels.Add(lev.Key);
		}
		
		// UI
		GameObject.Find("DebugMenuUI/LevelsLabel").GetComponent<UnityEngine.UI.Text>().text = "Nivel Actual: " + _levels[0];
	}
	
	public string GetSelectedLevel()
	{
		return _levels[_currentIndex];
	}
	
	public void SetNextLevel()
	{
		if (_currentIndex == _levels.Count - 1)
		{
			_currentIndex = 0;
		}
		else
		{
			_currentIndex++;
		}
		
		// UI
		GameObject.Find("DebugMenuUI/LevelsLabel").GetComponent<UnityEngine.UI.Text>().text = "Nivel Actual: " + _levels[_currentIndex];
	}
	
	public void SetPreviousLevel()
	{
		if (_currentIndex == 0)
		{
			_currentIndex = _levels.Count - 1;
		}
		else
		{
			_currentIndex--;
		}
		
		// UI
		GameObject.Find("DebugMenuUI/LevelsLabel").GetComponent<UnityEngine.UI.Text>().text = "Nivel Actual: " + _levels[_currentIndex];
	}
}
