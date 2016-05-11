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
        //Only one Ray is enought to detect when the Player collides
        // whatever the player's direction.
        _downRay = new Ray2D();
        _downRay.origin = transform.position + new Vector3(0f, -0.51f, 0f);
    }
	
	void Update ()
	{
        if (!_playerHit)
        {
            RaycastHit2D hit = Physics2D.Raycast(_downRay.origin, Vector2.up, 0.71f);

            if (hit.collider != null)
            {
                GameObject goHit = hit.collider.gameObject;
                Player pl = goHit.GetComponent<Player>();

                if (pl != null)
                {
                    OnPlayerHit(pl);
                }
            }
        }
	}

    private void OnPlayerHit(Player p)
    {
        #if UNITY_EDITOR
        Debug.Log("Loading next level");
        #endif

        p.OnLevelFinished();
        _playerHit = true;
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