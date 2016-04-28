﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Player enums
public enum PlayerActions : int
{
    Ice,
    Fire, 
    Earth, 
    Wind,
    None
}

public enum PlayerAnimState : int
{
    IdleFront,
    Turning,
    IdleTurned,
    BeginMove,
    Jump,
    Action,
    Death,
    Move,
    EndMove
}

public class Player : MonoBehaviour {

    private float _speed = 3f;
    
    private int _ice = 0;
    private int _wind = 0;
    private int _earth = 0;
    private int _fire = 0;

    //Accel
    private const float _minAccSpeed = 1f;
    private float _accSpeed = _minAccSpeed;
    private const float _incrSpeed = 0.1f;

    private Vector2 _velocity;
    
    //Wish direction to walk
    private Direction _targetDirection = Direction.None;
    //Real direction to walk
    private Direction _playerDirection = Direction.None;
    //Direction on where to make actions
    private Direction _actionDirection = Direction.None;
    private Direction _actionDirectionSaved = Direction.None;
    
    private State _state = State.Normal;
    private PlayerActions _action = PlayerActions.None;

    //Rays
    private Ray2D _downRay;

    private Ray2D _rightRay1;
    private Ray2D _rightRay2;

    private Ray2D _leftRay1;
    private Ray2D _leftRay2;

    private Ray2D _upRay;
    
    private const float _rayDownCollisionOffset = 0.25f;
    private const float _raySidesCollisionOffset = 0.31f;

    private Ray2D _actionRay;

    //Jumping

    private Vector2 _jumpPoint0, _jumpPoint1, _jumpPoint2;
    private const float _jumpTime = 0.5f;
    private float _jumpTimeActive = 0f;
	
	// Camera position
	private Vector3 _cameraOffset;
	private float _cameraDampingTime = 0.1f;
	private Vector3 _cameraVelocity;
    
    private Level _activeLevel;
    private GameController _gameController;


    //AnimState
    private PlayerAnimState _animState = PlayerAnimState.IdleFront;
    private bool _changeAnimation = true;
    private bool _canMove = false;
    private bool _isDying = false;
    private bool _beginFalling = true;
    private bool _falling = false;
    private bool _actionHappen = false;
    private GameObject _actionObjectAux;
    private Direction _animationDirection = Direction.None;
    private PlayerAnimState _animStateAfterJump = PlayerAnimState.BeginMove;

    private bool _beginning = true;
    
    public void SetActiveLevel(Level lv)
    {
        _activeLevel = lv;
    }
    
    public void SetGameController(GameController game)
    {
        _gameController = game;
    }


    // Use this for initialization
    void Start () {

        _downRay = new Ray2D();
        _downRay.direction = Vector2.right;

        _rightRay1 = new Ray2D();
        _rightRay2 = new Ray2D();
        _rightRay1.direction = Vector2.right;
        _rightRay2.direction = Vector2.right;

        _leftRay1 = new Ray2D();
        _leftRay2 = new Ray2D();
        _leftRay1.direction = Vector2.left;
        _leftRay2.direction = Vector2.left;

        _upRay = new Ray2D();
        _upRay.direction = Vector2.up;

		_cameraOffset = new Vector3(0,0,-1); 
		_cameraVelocity = new Vector3();

        _animState = PlayerAnimState.IdleFront;

	}
	
	// Update is called once per frame
	void Update () {

        if (_gameController.IsGamePaused() == false)
        {
            if (!_activeLevel.GetLevelController().GetGameController().IsGamePaused())
            {
                if (!_isDying)
                {
                    SetPreviousPlayerDirection();

                    CheckMovingCollisions();

                    AdjustVelocityByParams();

                    MovingPlayer();
                }

                AdjustCamera();

                AnimatingPlayer();
            }
        }

    }

    //Set the player's real direction (_playerDirection) in function of desired direction (_targetDirection)
    //The real direction could be changed by other methods
    private void SetPreviousPlayerDirection()
    {
        _playerDirection = _targetDirection;
    }

    //Check all collisions on the player's way to find the real player's direction
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
                Block goHitDownBlock = goHitDown.GetComponent<Block>();
                if (goHitDownBlock != null)
                {
                    _state = State.Grounded;
                    //Adjust player position to the height of the block
                    Vector3 pDown = transform.position;
                    if (goHitDownBlock.IsVertical())
                    {
                        pDown.y = goHitDown.transform.position.y + goHitDownBlock.GetLength();
                    }
                    else
                    {
                        pDown.y = goHitDown.transform.position.y + 1;
                    }
                    
                    transform.position = pDown;

                    //Check the player's Right
                    if (_playerDirection == Direction.Right)
                    {
                        _rightRay1.origin = transform.position + new Vector3(_raySidesCollisionOffset, 0f, 0f);

                        RaycastHit2D hit_right = Physics2D.Raycast(_rightRay1.origin, _rightRay1.direction, 0.01f);
                        //Check if there is something on the player's Right
                        if (hit_right.collider != null)
                        {
                            GameObject goHitRight = hit_right.collider.gameObject;
                            
                            //Check if it is a Block
                            if (goHitRight.GetComponent<Block>() != null)
                            {
                                
                                //Adjust player's real direction
                                _playerDirection = Direction.None;

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
                                        _playerDirection = Direction.Up;
                                        SetJumpingValues();
                                    }
                                }

                            }
                        }

                    }

                    //Check the player's Left
                    if (_playerDirection == Direction.Left)
                    {
                        
                        _leftRay1.origin = transform.position + new Vector3(-_raySidesCollisionOffset, 0f, 0f);

                        RaycastHit2D hit_left = Physics2D.Raycast(_leftRay1.origin, _leftRay1.direction, 0.01f);
                        
                        //Check if there is something on the player's Left
                        if (hit_left.collider != null)
                        {
                            GameObject goHitLeft = hit_left.collider.gameObject;
                            
                            //Check if it is a Block
                            if (goHitLeft.GetComponent<Block>() != null)
                            {
                                
                                //Adjust player's real direction
                                _playerDirection = Direction.None;

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
                                        _playerDirection = Direction.Up;
                                        SetJumpingValues();
                                    }
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

    //Adjust the velocity parameter (_velocity) in function of the real player's direction (_playerDirection), state
    // and other parameters.
    private void AdjustVelocityByParams()
    {
        switch(_state)
        {
            case State.Grounded:
                //if (_canMove)
                //{
                    _velocity.y = 0;
                    _accSpeed = _minAccSpeed;
                    switch (_playerDirection)
                    {
                        case Direction.Right:

                            _velocity.x = _speed;

                            break;
                        case Direction.Left:

                            _velocity.x = -_speed;

                            break;

                        default:
                            _velocity.x = 0;
                            break;
                    }
                //}

                break;
            
            case State.Falling:
            
                _velocity.y = -_speed;
                _accSpeed += _incrSpeed;
                _velocity.x = 0;
                
                break;
        }
        
    }

    private void MovingPlayer()
    {
        Vector3 p = transform.position;
        
        switch(_state)
        {
            case State.Grounded:
                if (_canMove)
                {
                    p.x += Time.deltaTime * _velocity.x;
                }
                
                if (_falling)
                {
                    _falling = false;
                    //Animation
                    _animState = _animStateAfterJump;
                    _changeAnimation = true;
                }

                break;
            
            case State.Jumping:

                if (_jumpTimeActive == 0f)
                {
                    //Animation
                    _animState = PlayerAnimState.Jump;
                    _changeAnimation = true;
                }
            
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

                    //Animation
                    _animState = _animStateAfterJump;
                    _changeAnimation = true;
                }
                
                break;
            
            case State.Falling:
                p.y += Time.deltaTime * _velocity.y * _accSpeed;

                if (_beginFalling)
                {
                    //Animation
                    _animState = PlayerAnimState.Jump;
                    _changeAnimation = true;
                    _beginFalling = false;
                    _falling = true;
                }
                break;
        }
		
        transform.position = p;
		
		
    }

    public void AdjustCamera()
    {
        Vector3 camPos = _gameController.GetCamera().transform.position;
        camPos = Vector3.SmoothDamp(camPos, transform.position, ref _cameraVelocity, _cameraDampingTime);

        _gameController.GetCamera().transform.position = camPos;
        _gameController.GetCamera().transform.position += _cameraOffset;
    }
    
    //Adjust the jumping values in funtion of the desired player's direction (_targetDirection)
    private void SetJumpingValues()
    {
        _jumpTimeActive = 0f;
        float ent_pointX = Mathf.Round(transform.position.x);
        
        if (_targetDirection == Direction.Right)
        {
            _jumpPoint0 = new Vector2(transform.position.x, transform.position.y);
            _jumpPoint1 = new Vector2(ent_pointX+0.5f, transform.position.y+1.5f);
            _jumpPoint2 = new Vector2(ent_pointX+1f, transform.position.y+1f);
        }
        else if (_targetDirection == Direction.Left)
        {
            _jumpPoint0 = new Vector2(transform.position.x, transform.position.y);
            _jumpPoint1 = new Vector2(ent_pointX-0.5f, transform.position.y+1.5f);
            _jumpPoint2 = new Vector2(ent_pointX-1f, transform.position.y+1f);
        }
    }
    
    public void SetActions(int ice, int fire, int wind, int earth)
    {
        _ice = ice;
        _fire = fire;
        _wind = wind;
        _earth = earth;
        
         GameObject.Find("WaterUsesLabel").GetComponent<Text>().text = _ice.ToString();
         GameObject.Find("FireUsesLabel").GetComponent<Text>().text = _fire.ToString();
         GameObject.Find("EarthUsesLabel").GetComponent<Text>().text = _earth.ToString();
         GameObject.Find("WindUsesLabel").GetComponent<Text>().text = _wind.ToString();
    }

    public void SetUsesOfElem(PlayerActions elem, int uses)
    {
        switch (elem)
        {
            case PlayerActions.Earth:
                _earth = uses;
                GameObject.Find("EarthUsesLabel").GetComponent<Text>().text = _earth.ToString();
                break;
            case PlayerActions.Fire:
                _fire = uses;
                GameObject.Find("FireUsesLabel").GetComponent<Text>().text = _fire.ToString();
                break;
            case PlayerActions.Ice:
                _ice = uses;
                GameObject.Find("WaterUsesLabel").GetComponent<Text>().text = _ice.ToString();
                break;
            case PlayerActions.Wind:
                _wind = uses;
                GameObject.Find("WindUsesLabel").GetComponent<Text>().text = _wind.ToString();
                break;

        }
    }

    public int GetUsesOfElem(PlayerActions elem)
    {
        switch (elem)
        {
            case PlayerActions.Earth:
                return _earth;
                break;
            case PlayerActions.Fire:
                return _fire;
                break;
            case PlayerActions.Ice:
                return _ice;
                break;
            case PlayerActions.Wind:
                return _wind;
                break;
            default:
                return -1;

        }
    }

    
    public void DoAction(PlayerActions type)
    {
        if (!_isDying && _animState != PlayerAnimState.Action)
        {
            _action = type;

            _actionHappen = false;
            _actionDirectionSaved = _actionDirection;

            if (_state == State.Grounded && _velocity.magnitude == 0)
            {

                _actionRay = new Ray2D();
                if (_actionDirectionSaved == Direction.Left)
                {
                    _actionRay.origin = new Vector3(Mathf.Round(transform.position.x - 1), Mathf.Round(transform.position.y), 0f); //transform.position + new Vector3(-0.31f, 0f, 0f);
                    _actionRay.direction = Vector2.left;
                }
                else if (_actionDirectionSaved == Direction.Right)
                {
                    _actionRay.origin = new Vector3(Mathf.Round(transform.position.x + 1), Mathf.Round(transform.position.y), 0f);//transform.position + new Vector3(0.31f, 0f, 0f);
                    _actionRay.direction = Vector2.right;
                }


                if ((_actionDirectionSaved == Direction.Right || _actionDirectionSaved == Direction.Left))
                {
                    switch (_action)
                    {
                        case PlayerActions.Ice:
                            if (!Physics2D.Raycast(_actionRay.origin, _actionRay.direction, 0.1f))
                            {
                                if (true) //_ice > 0)
                                {
                                    _actionHappen = true;
                                }
                            }

                            break;
                        case PlayerActions.Wind:

                            if (true) //_wind > 0)
                            {
                                if (_actionDirectionSaved == Direction.Right || _actionDirectionSaved == Direction.Left)
                                {
                                    RaycastHit2D hit = Physics2D.Raycast(_actionRay.origin, _actionRay.direction, 0.1f);

                                    //Check if there is something on the player's Left or Right
                                    if (hit.collider != null)
                                    {
                                        _actionObjectAux = hit.collider.gameObject;

                                        //Check if it is a Block
                                        Block goHitBlock = _actionObjectAux.GetComponent<Block>();
                                        if (goHitBlock != null && goHitBlock.IsMovable())
                                        {
                                            _actionHappen = true;
                                        }
                                        //Check if it is a Lever
                                        Lever goHitLever = _actionObjectAux.GetComponent<Lever>();
                                        if (goHitLever != null)
                                        {
                                            _actionHappen = true;
                                        }
                                    }
                                    else
                                    {
                                        _actionObjectAux = null;
                                    }
                                }
                            }

                            break;

                        case PlayerActions.Fire:

                            if (true) //_fire > 0)
                            {
                                _actionHappen = true;
                            }

                            break;

                        case PlayerActions.Earth:

                            if (!Physics2D.Raycast(_actionRay.origin, _actionRay.direction, 0.1f))
                            {
                                if (true) //_earth > 0)
                                {
                                    _actionHappen = true;
                                }
                            }

                            break;
                    }

                }
            }

            if (_actionHappen)
            {
                //Animation
                _animState = PlayerAnimState.Action;
                _changeAnimation = true;

                //AnimationObject
                GameObject go = new GameObject("AnimationObject_" + type.ToString());
                go.transform.position = _actionRay.origin;

                AnimationObject animObj = go.AddComponent<AnimationObject>();
                animObj.SetParams(_activeLevel, "Actions/" + type.ToString() + "Action/" + type.ToString() + "Action_1_Anim");
            }
        }
    }

    private void MakeTheActionHappen()
    {
        GameObject goToPut = null;

        switch (_action)
        {
            case PlayerActions.Ice:
                 goToPut = _activeLevel.CreateBlock(BlockType.Ice, (int)_actionRay.origin.x,
                                            (int)transform.position.y, "Blocks/Ice/Ice_1");

                 _activeLevel.AddEntity(goToPut, goToPut.name);
                 SetUsesOfElem(_action, GetUsesOfElem(_action) - 1);
                break;

            case PlayerActions.Wind:

                if (_actionObjectAux)
                {
                    //Check if it is a Block
                    Block goHitBlock = _actionObjectAux.GetComponent<Block>();
                    if (goHitBlock != null && goHitBlock.IsMovable())
                    {
                        goHitBlock.Kick(_actionDirectionSaved);
                        SetUsesOfElem(_action, GetUsesOfElem(_action) - 1);

                    }
                    //Check if it is a Lever
                    Lever goHitLever = _actionObjectAux.GetComponent<Lever>();
                    if (goHitLever != null)
                    {
                        goHitLever.ChangeLeverDirection();
                    }

                }
                
                break;

            case PlayerActions.Fire:

                if (_actionDirectionSaved == Direction.Right)
                {
                    goToPut = _activeLevel.CreateFireBall(transform.position.x + 0.3f, transform.position.y, _actionDirectionSaved);
                }
                else
                {
                    goToPut = _activeLevel.CreateFireBall(transform.position.x - 0.3f, transform.position.y, _actionDirectionSaved);
                    goToPut.transform.localScale = new Vector3(-1f, 1f, 1f);
                }

                _activeLevel.AddEntity(goToPut, goToPut.name);

                SetUsesOfElem(_action, GetUsesOfElem(_action) - 1);

                break;

            case PlayerActions.Earth:

                int rockIndex = Random.Range(1, 4);

                goToPut = _activeLevel.CreateBlock(BlockType.Rock, (int)_actionRay.origin.x,
                                            (int)transform.position.y, "Blocks/Stone/Stone_" + rockIndex.ToString());

                _activeLevel.AddEntity(goToPut, goToPut.name);

                SetUsesOfElem(_action, GetUsesOfElem(_action) - 1);

                break;

        }
        _action = PlayerActions.None;
    }

    public void AnimatingPlayer()
    {
        SpriteAnimator sprite_animator = GetComponentInChildren<SpriteAnimator>();

        switch (_animationDirection)
        {
            case Direction.Right:

                transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);

                break;
            case Direction.Left:

                transform.GetChild(0).localScale = new Vector3(-1f, 1f, 1f);

                break;
        }

        
        switch (_animState)
        {
            //ACTION
            case PlayerAnimState.Action:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("ACTION");
                    _changeAnimation = false;
                    _canMove = false;
                }
                if (sprite_animator.GetAnimationIndex() == 3)
                {
                    MakeTheActionHappen();
                }
                if (sprite_animator.IsTheLastFrame())
                {
                    _animState = _animStateAfterJump;
                    _changeAnimation = true;
                    _animationDirection = _actionDirection;
                }
                break;
            //BEGIN_MOVE
            case PlayerAnimState.BeginMove:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("BEGIN_MOVE");
                    _changeAnimation = false;
                    _canMove = true;
                }
                if (sprite_animator.IsTheLastFrame())
                {
                    _animState = PlayerAnimState.Move;
                    _changeAnimation = true;
                }
                break;
            //DEATH
            case PlayerAnimState.Death:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("DEATH");
                    _changeAnimation = false;
                    _canMove = false;
                }
                if (sprite_animator.IsTheLastFrame())
                {
                    OnPlayerDestroyed();
                }
                break;
            //END_MOVE
            case PlayerAnimState.EndMove:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("END_MOVE");
                    _changeAnimation = false;
                    _canMove = false;
                }
                if (sprite_animator.IsTheLastFrame())
                {
                    _animState = PlayerAnimState.IdleTurned;
                    _changeAnimation = true;
                }
                break;
            //IDLE_FRONT
            case PlayerAnimState.IdleFront:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("IDLE_FRONT");
                    _changeAnimation = false;
                    _canMove = false;
                }
                break;
            //IDLE_TURNED
            case PlayerAnimState.IdleTurned:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("IDLE_TURNED");
                    _changeAnimation = false;
                    _canMove = false;
                }
                break;
            //JUMP
            case PlayerAnimState.Jump:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("JUMP");
                    _changeAnimation = false;
                    _canMove = true;
                }
                break;
            //MOVE
            case PlayerAnimState.Move:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("MOVE");
                    _changeAnimation = false;
                    _canMove = true;
                }
                break;
            //TURNING
            case PlayerAnimState.Turning:
                if (_changeAnimation)
                {
                    sprite_animator.SetActiveAnimation("TURNING");
                    _changeAnimation = false;
                    _canMove = true;
                }
                
                if (sprite_animator.IsTheLastFrame())
                {
                    _animState = PlayerAnimState.BeginMove;
                    _changeAnimation = true;
                }
                break;
        }
    }

    public void SetTargetDirection(Direction tg_dir)
    {
        if (!_isDying)
        {
            _targetDirection = tg_dir;

            if (_targetDirection != Direction.None)
            {
                _actionDirection = _targetDirection;
                if (_animState != PlayerAnimState.Action)
                {
                    //ANIMATION
                    if ((_animState == PlayerAnimState.IdleTurned || _animState == PlayerAnimState.BeginMove || _animState == PlayerAnimState.EndMove) && _animationDirection == _targetDirection)
                    {
                        _animState = PlayerAnimState.BeginMove;
                        _changeAnimation = true;

                    }
                    else if (_animState != PlayerAnimState.Jump)
                    {
                        _animState = PlayerAnimState.Turning;
                        _changeAnimation = true;
                    }

                    _animationDirection = _targetDirection;
                    _animStateAfterJump = PlayerAnimState.BeginMove;
                }
                else
                {
                    _animStateAfterJump = PlayerAnimState.BeginMove;
                }
                
                
            }
            else
            {
                if (!_beginning)
                {
                    if (_animState == PlayerAnimState.Action)
                    {
                        _animStateAfterJump = PlayerAnimState.IdleTurned;
                        //_animationDirection = _actionDirection;
                    }
                    else if (_animState == PlayerAnimState.Jump)
                    {
                        _animStateAfterJump = PlayerAnimState.IdleTurned;
                    }
                    else
                    {
                        _animState = PlayerAnimState.EndMove;
                        _animStateAfterJump = PlayerAnimState.IdleTurned;
                        _changeAnimation = true;
                    }
                }
                else
                {
                    _beginning = false;
                }
                
            }
        }

    }
    
    public Direction GetDirection ()
    {
        return _playerDirection;
    }

    public void DestroyPlayer()
    {
        if (!_isDying)
        {
            _animState = PlayerAnimState.Death;
            _changeAnimation = true;
            _isDying = true;
        }
    }

    public void OnPlayerDestroyed()
    {
        GameObject.Destroy(gameObject);
        //_level.RemoveEntity("player"); TO DO!
    }
    
}
