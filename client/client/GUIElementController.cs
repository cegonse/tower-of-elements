using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIElementController : MonoBehaviour
{
	
	private MenuController _menuController;
	
	void Update()
	{
	}
	
	void Start()
	{
	}
	
	public virtual void OnEvent()
	{
	}
	
	//Gets and Sets
	public void SetMenuController(MenuController menuController)
	{
		_menuController = menuController;
	}
}