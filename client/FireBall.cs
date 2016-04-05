using UnityEngine;
using System.Collections.Generic;


public class FireBall : MonoBehaviour
{
	private Player _player;
	private Direction _actionDirection = Direction.None;
	private Level _activeLevel;
    //Speed
	private float _speed = 5f;

    //velocity
    private Vector2 _velocity = new Vector2(0f, 0f);

    //Rays
    private Ray2D _rightRay;
    private Ray2D _leftRay;
	
	void Start()
	{
        //Right Ray
        _rightRay = new Ray2D();
        
        //Left Ray
        _leftRay = new Ray2D();

    }
	
	void Update()
	{
        if (!_activeLevel.GetLevelController().GetGameController().IsGamePaused())
        {
            CheckMovingCollisions();
            MovingFireBall();
        }

	}
    
    private void CheckMovingCollisions()
    {
		if (_actionDirection == Direction.Right)
		{
			_rightRay.direction = Vector2.right;
			_rightRay.origin = transform.position + new Vector3(0.51f, 0f, 0f);
			RaycastHit2D hit_right = Physics2D.Raycast(_rightRay.origin, _rightRay.direction, 0.01f);

			if (hit_right.collider != null)
			{
				GameObject goHit = hit_right.collider.gameObject;
				//Check if it is a Block
				Block goHitBlock = goHit.GetComponent<Block>();
				
				if (goHitBlock != null)
				{
					if(goHitBlock.GetBlockType() == BlockType.Ice)
					{
						
                        goHitBlock.Destroy(_actionDirection);
					}
					_activeLevel.RemoveEntity(this.name);
				}
			}

		}
		else if(_actionDirection == Direction.Left)
		{
			_leftRay.direction = Vector2.left;
			_leftRay.origin = transform.position + new Vector3(-0.51f, 0f, 0f);
			RaycastHit2D hit_left = Physics2D.Raycast(_leftRay.origin, _leftRay.direction, 0.01f);

			if (hit_left.collider != null)
			{
				GameObject goHit = hit_left.collider.gameObject;
				//Check if it is a Block
				Block goHitBlock = goHit.GetComponent<Block>();
				
				if (goHitBlock != null)
				{
					if(goHitBlock.GetBlockType() == BlockType.Ice)
					{
                        goHitBlock.Destroy(_actionDirection);
					}
                    _activeLevel.RemoveEntity(this.name);
				}
			}
		}
	} 

    private void MovingFireBall()
    {
        Vector3 p = transform.position;
		p.x += Time.deltaTime * _velocity.x;
		transform.position = p;
    }
	
	public void SetActiveLevel(Level lv){
		_activeLevel = lv;
	}
	
	public void SetDirection(Direction dir)
	{
		_actionDirection = dir;

        if (_actionDirection == Direction.Left)
        {
            _velocity.x = -_speed;
        }
        else
        {
            _velocity.x = _speed;
        }
	}
}