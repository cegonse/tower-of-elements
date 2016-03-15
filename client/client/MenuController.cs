using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController
{
	private Dictionary<string, GameObject> _guiElements;
	private GameController _gameController;
	
	public MenuController(GameController game)
	{
		_gameController = game;
		_guiElements = new Dictionary<string, GameObject>();
	}
	
	public bool AddGuiElement(string name, GameObject element)
	{
		if (_guiElements.ContainsKey(name))
		{
			return false;
		}
		else
		{
			_guiElements.Add(name, element);
			return true;
		}
	}
	
	//Gets and Sets
	
	public GameController GetGameController()
	{
		return _gameController;
	}
	
	public void SetElementActive(string name, bool active)
	{
		if (_guiElements.ContainsKey(name))
		{
			_guiElements[name].SetActive(active);
		}
	}
	
	public string SerializeGuiElement(string name)
	{
		string data = "";
		
		return data;
	}
	
	public void MarshallGuiElement(string name, string guiData)
	{
		
	}
	
	public void OnUpdate()
	{
	}
}