using UnityEngine;
using System.Collections.Generic;

public class Door : MonoBehaviour
{
	
    private Level _activeLevel;
	private string _destinationLevel;
	
	private Ray2D _downRay;
    private Ray2D _upRay;
    private Ray2D _rightRay;
    private Ray2D _leftRay;
	
	void Start()
	{
        
        _downRay = new Ray2D();
        _upRay = new Ray2D();
        _rightRay = new Ray2D();
        _leftRay = new Ray2D();
	}
	
	void Update ()
	{
		_downRay.direction = Vector2.down;
		_leftRay.direction = Vector2.left;
		_rightRay.direction = Vector2.right;
		_upRay.direction = Vector2.up;
		
		_downRay.origin = transform.position + new Vector3(0f , -0.51f, 0f);
		_upRay.origin = transform.position + new Vector3(0f , 0.51f, 0f);
		_leftRay.origin = transform.position + new Vector3(-0.51f , 0f, 0f);
		_rightRay.origin = transform.position + new Vector3(0.49f , 0f, 0f);
		
		RaycastHit2D hit_right = Physics2D.Raycast(_rightRay.origin, _rightRay.direction, 0.01f);
		RaycastHit2D hit_left = Physics2D.Raycast(_leftRay.origin, _leftRay.direction, 0.01f);
		RaycastHit2D hit_down = Physics2D.Raycast(_downRay.origin, _downRay.direction, 0.01f);
		RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.01f);
		
		if(hit_right.collider != null)
		{
			GameObject goHitRight = hit_right.collider.gameObject;
			if(goHitRight.GetComponent<Player>() != null)
			{
				Debug.Log("Loading next level");

                _activeLevel.ClearLevel();
                _activeLevel.GetLevelController().SetActiveLevel(_destinationLevel);
			} 
		}
		
		if(hit_left.collider != null)
		{
			GameObject goHitRight = hit_left.collider.gameObject;
			if(goHitRight.GetComponent<Player>() != null)
			{
				Debug.Log("Loading next level");

                _activeLevel.ClearLevel();
                _activeLevel.GetLevelController().SetActiveLevel(_destinationLevel);
			} 
		}
		
		if(hit_up.collider != null)
		{
			GameObject goHitRight = hit_up.collider.gameObject;
			if(goHitRight.GetComponent<Player>() != null)
			{
				Debug.Log("Loading next level");

                _activeLevel.ClearLevel();
                _activeLevel.GetLevelController().SetActiveLevel(_destinationLevel);
			} 
		}
		
		if(hit_down.collider != null)
		{
			GameObject goHitRight = hit_down.collider.gameObject;
			if(goHitRight.GetComponent<Player>() != null)
			{
				Debug.Log("Loading next level");

                _activeLevel.ClearLevel();
                _activeLevel.GetLevelController().SetActiveLevel(_destinationLevel);
			} 
		}
	}
	
	public void SetDestinationLevel(string lv)
	{
		_destinationLevel = lv;
	}
	
	public void SetActiveLevel(Level lv)
	{
		_activeLevel = lv;
	}
	
	public string GetDestinationLevel()
	{
		return _destinationLevel;
	}
}