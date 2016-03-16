using UnityEngine;
using System.Collections;

public class WalkerEnemyData : BaseEnemyData
{
	public Vector2 p0;
	public Vector2 p1;
}


public class EnemyWalker: EnemyBase {

   private WalkerEnemyData _walkerData;

	private Vector2 _target;
	private int _targetIndex = 0;
    private Direction _targetDirection = Direction.None;
    private Direction _directionToP0, _directionToPf;
    
    private Ray2D _downRay;
    private Ray2D _upRay;
    private Ray2D _rightRay;
    private Ray2D _leftRay;
    private Ray2D _rightRay1;
    private Ray2D _rightRay2;

    private Ray2D _leftRay1;
    private Ray2D _leftRay2;
    private const float _rayDownCollisionOffset = 0.25f;
    private const float _raySidesCollisionOffset = 0.5f;

    private Vector2 _jumpPoint0, _jumpPoint1, _jumpPoint2;
    private const float _jumpTime = 0.5f;
    private float _jumpTimeActive = 0f;
	
	private State _state = State.Normal;
	private Direction _enemyDirection = Direction.None;
    
    //Accel
    private const float _minAccSpeed = 1f;
    private float _accSpeed = _minAccSpeed;
    private const float _incrSpeed = 0.1f;

   new void Start()
    {
        //Let the base.Start() method for a properly initialization
        base.Start();
        
        _downRay = new Ray2D();
        _upRay = new Ray2D();
        _rightRay = new Ray2D();
        _leftRay = new Ray2D();

        _downRay.direction = Vector2.right;
        _leftRay.direction = Vector2.left;
        _rightRay.direction = Vector2.right;
        _upRay.direction = Vector2.up;
        
        _rightRay1 = new Ray2D();
        _rightRay2 = new Ray2D();
        _rightRay1.direction = Vector2.right;
        _rightRay2.direction = Vector2.right;

        _leftRay1 = new Ray2D();
        _leftRay2 = new Ray2D();
        _leftRay1.direction = Vector2.left;
        _leftRay2.direction = Vector2.left;
        
    }

    void Update()
    {
        CheckPlayerCollisions();
        MovingEnemy();
        CheckMovingCollisions();
        AdjustVelocity();
    }
	
	private void CheckMovingCollisions()
    {
        if (_state != State.Jumping)
        {
            _downRay.origin = transform.position + new Vector3(-_rayDownCollisionOffset, -0.51f, 0f);

            RaycastHit2D hit_down = Physics2D.Raycast(_downRay.origin, _downRay.direction, _rayDownCollisionOffset*2);

            //Check for something on the player's Down
            if (hit_down.collider != null)
            {
                GameObject goHitDown;
                goHitDown = hit_down.collider.gameObject;
                    
                //Check if there is a Block on player's Down
                if (goHitDown.GetComponent<Block>() != null)
                {
                    _state = State.Grounded;
                    //Adjust player position to the height of the block
                    Vector3 pDown = transform.position;
                    pDown.y = goHitDown.transform.position.y + 1;
                    transform.position = pDown;

                    //Check the player's Right
                    if (_enemyDirection == Direction.Right)
                    {
                        Debug.Log(_targetIndex + "holaaaa");

                        _rightRay1.origin = transform.position + new Vector3(_raySidesCollisionOffset, 0f, 0f);

                        RaycastHit2D hit_right = Physics2D.Raycast(_rightRay1.origin, _rightRay1.direction, 0.01f);
                        //Check if there is something on the player's Right
                        if (hit_right.collider != null)
                        {
                            GameObject goHitRight = hit_right.collider.gameObject;
                            
                            //Check if it is a Block
                            if (goHitRight.GetComponent<Block>() != null)
                            {
                                

                                //Check if there is a block over the block which player collided with
                                _rightRay2.origin = transform.position + new Vector3(_raySidesCollisionOffset, 1f, 0f);

                                RaycastHit2D hit_right2 = Physics2D.Raycast(_rightRay2.origin, _rightRay2.direction, 0.01f);

                                if(hit_right2.collider == null)
                                {
                                    //Check if ther is a block over the player
                                    _upRay.origin = transform.position + new Vector3(0f, 1f, 0f);

                                    RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.01f);

                                    if (hit_up.collider == null)
                                    {
                                        _state = State.Jumping;
                                        SetJumpingValues();
                                        Debug.Log("Salto right");
                                    }
                                    else
                                    {
                                        _state = State.Normal;
                                        Debug.Log("Colision arriba");
                                    }
                                }
                                 else if(hit_right2.collider != null)
                                {
                                    _state = State.Grounded;
                                    _targetIndex = 1;
                                    ChangeTargetDirection();
                                    Debug.Log("Colision bloque");
                                    
                                }

                            }
                        }

                    }

                    //Check the player's Left
                    if (_enemyDirection == Direction.Left)
                    {
                        Debug.Log(_targetIndex + "holaaaa");

                        Debug.Log(_enemyDirection);
                        _leftRay1.origin = transform.position + new Vector3(-_raySidesCollisionOffset, 0f, 0f);

                        RaycastHit2D hit_left = Physics2D.Raycast(_leftRay1.origin, _leftRay1.direction, 0.01f);
                        
                        //Check if there is something on the player's Left
                        if (hit_left.collider != null)
                        {
                            GameObject goHitLeft = hit_left.collider.gameObject;
                            
                            //Check if it is a Block
                            if (goHitLeft.GetComponent<Block>() != null)
                            {
                                Debug.Log("izquierda");

                                //Check if there is a block over the block which player collided with
                                _leftRay2.origin = transform.position + new Vector3(-_raySidesCollisionOffset, 1f, 0f);

                                RaycastHit2D hit_left2 = Physics2D.Raycast(_leftRay2.origin, _leftRay2.direction, 0.01f);

                                if(hit_left2.collider == null)
                                {

                                    //Check if ther is a block over the player
                                    _upRay.origin = transform.position + new Vector3(0f, 1f, 0f);

                                    RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.01f);

                                    if (hit_up.collider == null)
                                    {
                                        _state = State.Jumping;
                                        SetJumpingValues();
                                        Debug.Log("Salto left");
                                    }
                                    else
                                    {
                                        _state = State.Grounded;
                                        Debug.Log("Colision arriba");
                                        
                                    if(_enemyDirection == _directionToPf)
                                        _enemyDirection = _directionToP0;
                                    else
                                        _enemyDirection = _directionToPf;
                                    }
                                }
                                else if (hit_left2.collider != null)
                                {
                                    _state = State.Grounded;
                                    _targetIndex = 0;
                                    ChangeTargetDirection();
                                    Debug.Log("Colision bloque");

                                }


                            }
                        }

                    }

                }//Down if 2
                else
                {
                    _state = State.Falling;
                }
            }//Down if 1
            else
            {
                _state = State.Falling;
            }
        }
        
    }

    private void AdjustVelocity()
    {
		// Check if we are close enough to the target
		if (Vector2.Distance(_target, transform.position) < 0.5f)
		{
            ChangeTargetDirection();
        }
    }

    private void ChangeTargetDirection()
    {
        // Change the target to the other point
        if (_targetIndex == 0)
        {
            _target = _walkerData.p1;
            _targetIndex = 1;
            _enemyDirection = _directionToPf;
        }
        else if (_targetIndex == 1)
        {
            _target = _walkerData.p0;
            _targetIndex = 0;
            _enemyDirection = _directionToP0;
        }
    }
    
    private void SetJumpingValues()
    {
        _jumpTimeActive = 0f;
        float ent_pointX = Mathf.Round(transform.position.x);
        
        if (_enemyDirection == Direction.Right)
        {
            _jumpPoint0 = new Vector2(transform.position.x, transform.position.y);
            _jumpPoint1 = new Vector2(ent_pointX+0.5f, transform.position.y+1.5f);
            _jumpPoint2 = new Vector2(ent_pointX+1f, transform.position.y+1f);
        }
        else if (_enemyDirection == Direction.Left)
        {
            _jumpPoint0 = new Vector2(transform.position.x, transform.position.y);
            _jumpPoint1 = new Vector2(ent_pointX-0.5f, transform.position.y+1.5f);
            _jumpPoint2 = new Vector2(ent_pointX-1f, transform.position.y+1f);
        }
    }

    private void MovingEnemy()
    {
        
        Vector3 p = transform.position;
        
        switch(_state)
        {
            case State.Grounded:
                _accSpeed = _minAccSpeed;
                p = Vector2.SmoothDamp(transform.position, _target, ref _velocity, _speed / 4, _speed, Time.deltaTime);
                Debug.Log("ground");
                transform.position = new Vector3(p.x, transform.position.y, 0f);
                break;
            
            case State.Jumping:

                if(_jumpTimeActive < 1f)
                {
                    p = (1 - _jumpTimeActive) * (1 - _jumpTimeActive) * _jumpPoint0 +
                        2 * (1 - _jumpTimeActive) * _jumpTimeActive * _jumpPoint1 +
                        _jumpTimeActive * _jumpTimeActive * _jumpPoint2;
    
                    _jumpTimeActive += Time.deltaTime /_jumpTime;
                }
                else
                {
                    _state = State.Normal;
                    //Ajustar posicion del jugador
                    p = _jumpPoint2; //Por algun motivo no se ejecuta bien
                }
                Debug.Log("jump");
                transform.position = new Vector3(p.x, p.y, 0f);
                break;
            
            case State.Falling:
               
                _velocity.y = -_speed;
                _accSpeed += _incrSpeed;
                _velocity.x = 0;
                p.y += Time.deltaTime * _velocity.y *_accSpeed;
                Debug.Log("fall");
                transform.position = new Vector3(transform.position.x, p.y, 0f);
                break;
        }
		
        
    }
	
	public override void SetEnemyData (BaseEnemyData data)
	{
		_walkerData = (WalkerEnemyData) data;
		_target = _walkerData.p0;
        if(transform.position.x < _target.x)
        {
            _directionToP0 = Direction.Right;
            _directionToPf = Direction.Left;
        }else
        {
            _directionToP0 = Direction.Left;
            _directionToPf = Direction.Right;
        }
	}
	
}
