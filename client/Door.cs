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

    private Vector3 _offsetDown;
    private Vector3 _offsetUp;
    private Vector3 _offsetLeft;
    private Vector3 _offsetRight;

    private bool _playerHit = false;

    void Start()
    {
        _downRay = new Ray2D();
        _upRay = new Ray2D();
        _rightRay = new Ray2D();
        _leftRay = new Ray2D();

        _downRay.direction = Vector2.down;
        _leftRay.direction = Vector2.left;
        _rightRay.direction = Vector2.right;
        _upRay.direction = Vector2.up;

        _offsetDown = new Vector3(0f, -0.51f, 0f);
        _offsetUp = new Vector3(0f, 0.51f, 0f);
        _offsetLeft = new Vector3(-0.51f, 0f, 0f);
        _offsetRight = new Vector3(0.49f, 0f, 0f);
    }
	
	void Update ()
	{
        if (!_playerHit)
        {
            _downRay.origin = transform.position + _offsetDown;
            _upRay.origin = transform.position + _offsetUp;
            _leftRay.origin = transform.position + _offsetLeft;
            _rightRay.origin = transform.position + _offsetRight;

            RaycastHit2D hit_right = Physics2D.Raycast(_rightRay.origin, _rightRay.direction, 0.01f);
            RaycastHit2D hit_left = Physics2D.Raycast(_leftRay.origin, _leftRay.direction, 0.01f);
            RaycastHit2D hit_down = Physics2D.Raycast(_downRay.origin, _downRay.direction, 0.01f);
            RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.01f);

            OnPlayerHit(hit_right);
            OnPlayerHit(hit_left);
            OnPlayerHit(hit_down);
            OnPlayerHit(hit_up);
        }
	}

    private void OnPlayerHit(RaycastHit2D hit)
    {
        if (hit.collider != null)
        {
            GameObject goHit = hit.collider.gameObject;

            if (goHit.GetComponent<Player>() != null)
            {
                #if UNITY_EDITOR
                Debug.Log("Loading next level");
                #endif

                goHit.GetComponent<Player>().OnLevelFinished();
                _playerHit = true;
                //_activeLevel.ClearLevel();
                //_activeLevel.GetLevelController().SetActiveLevel(_destinationLevel);
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