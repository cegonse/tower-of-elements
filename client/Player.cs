using UnityEngine;
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

    //Offset values
    private const float _rayDownCollisionOffset = 0.25f;
    private const float _raySidesCollisionOffset = 0.31f;

    //Offset Vectors
    private Vector3 _vectorDownCollisionOffset;
    private Vector3 _vectorRight1CollisionOffset;
    private Vector3 _vectorRight2CollisionOffset;
    private Vector3 _vectorLeft1CollisionOffset;
    private Vector3 _vectorLeft2CollisionOffset;

    private Ray2D _actionRay;

    //Jumping
    private BoxCollider2D _collider;
    private Vector2 _jumpColliderSize;
    private Vector2 _originalJumpColliderSize;
    private Vector2 _jumpColliderOffset;

    private Vector2 _jumpPoint0, _jumpPoint1, _jumpPoint2;
    private const float _jumpTime = 0.5f;
    private float _jumpTimeActive = 0f;
	
	// Camera position
	private float _cameraDampingTime = 0.1f;
	private Vector3 _cameraVelocity;
    
    private Level _activeLevel;
    private GameController _gameController;


    //AnimState
    private PlayerAnimState _animState = PlayerAnimState.IdleFront;
    private bool _begining = true;
    private bool _changeAnimation = true;
    private bool _canMove = false;
    private bool _isDying = false;
    private bool _beginFalling = true;
    private bool _falling = false;
    private bool _actionHappen = false;
    private GameObject _actionObjectAux;
    private Direction _animationDirection = Direction.None;
    private PlayerAnimState _animStateAfterJump = PlayerAnimState.BeginMove;
    private bool _isOnDoor = false;
    private bool _fadeOutMusic = true;

    private float _startTime = 0f;
    private float _endTime = 0f;
    private int _stars = 3;

    private float _targetScaleX = 1f;
    private float _targetScaleY = 1f;
    private float _scaleXvelocity = 0f;
    private float _scaleYvelocity = 0f;
    private float _targetXfalling = 0f;
    private float _targetXvelocity = 0f;

    private Texture2D[] _dustParticle;

    private float _boundX = 0f;
    private float _boundY = 0f;
    private float _boundH = 0f;
    private float _boundW = 0f;

    public void SetActiveLevel(Level lv)
    {
        _activeLevel = lv;
    }
    
    public void SetGameController(GameController game)
    {
        _gameController = game;
    }


    // Use this for initialization
    void Start ()
    {
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

        //Vector Offsets
        _vectorDownCollisionOffset = new Vector3(-_rayDownCollisionOffset, -0.51f, 0f);
        _vectorRight1CollisionOffset = new Vector3(_raySidesCollisionOffset, 0f, 0f);
        _vectorRight2CollisionOffset = new Vector3(_raySidesCollisionOffset, 1f, 0f);
        _vectorLeft1CollisionOffset = new Vector3(-_raySidesCollisionOffset, 0f, 0f);
        _vectorLeft2CollisionOffset = new Vector3(-_raySidesCollisionOffset, 1f, 0f);

		_cameraVelocity = new Vector3();

        _animState = PlayerAnimState.IdleFront;
        _startTime = Time.time;

        _collider = GetComponent<BoxCollider2D>();
        _jumpColliderSize = new Vector2(0.6f, 0.8f);
        _jumpColliderOffset = new Vector2(0.0f, -0.1f);
        _originalJumpColliderSize = _collider.size;

        _dustParticle = new Texture2D[3];

        _dustParticle[0] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleDust_1");
        _dustParticle[1] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleDust_2");
        _dustParticle[2] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleDust_3");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_gameController.IsGamePaused() == false)
        {
            if (!_activeLevel.GetLevelController().GetGameController().IsGamePaused())
            {
                if (!_isDying && !_isOnDoor)
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

        if (_isOnDoor && _fadeOutMusic)
        {
            float vol = _gameController.GetAudioController().GetChannelVolume(0);

            if (vol <= 0.1f)
            {
                // Pause in-game music
                _gameController.GetAudioController().PauseChannel(0);
                _gameController.GetAudioController().SetChannelVolume(0, 1f);

                // Play winning music
                //...

                _fadeOutMusic = false;
            }
            else
            {
                // Fade in 1 sec
                vol -= Time.deltaTime;
                _gameController.GetAudioController().SetChannelVolume(0, vol);
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
        bool hasHitDown = false;
        transform.parent = null;

        if (_state != State.Jumping)
        {
            _downRay.origin = transform.position + _vectorDownCollisionOffset;
            RaycastHit2D[] hit_down = Physics2D.RaycastAll(_downRay.origin, _downRay.direction, _rayDownCollisionOffset * 2);

            //Check for something on the player's Down
            // We cycle through all the possible colliders
            for (int i = 0; i < hit_down.Length; i++)
            {
                if (hit_down[i].collider != null)
                {
                    GameObject goHitDown;
                    goHitDown = hit_down[i].collider.gameObject;

                    //Check if there is a Block on player's Down
                    Block goHitDownBlock = goHitDown.GetComponent<Block>();

                    if (goHitDownBlock != null)
                    {
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

                        //Check if it is on a platform
                        if (goHitDownBlock.IsPlatform())
                        {
                            transform.parent = goHitDownBlock.transform;
                        }

                        hasHitDown = true;
                    }
                }
            }

            if (hasHitDown)
            {
                if (_state == State.Falling)
                {
                    // Create a falling particle
                    CreateFallingParticles(Random.Range(6, 8));
                }

                _state = State.Grounded;
                _targetXvelocity = -1f;

                //Check the player's Right
                if (_playerDirection == Direction.Right)
                {
                    _rightRay1.origin = transform.position + _vectorRight1CollisionOffset;
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
                            _rightRay2.origin = transform.position + _vectorRight2CollisionOffset;
                            RaycastHit2D hit_right2 = Physics2D.Raycast(_rightRay2.origin, _rightRay2.direction, 0.01f);

                            if (hit_right2.collider == null || (hit_right2.collider != null && hit_right2.collider.gameObject.GetComponent<Block>() == null))
                            {
                                //Check if there is a block over the player
                                _upRay.origin = transform.position + Vector3.up;
                                RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.01f);

                                if (hit_up.collider == null || (hit_up.collider != null && hit_up.collider.gameObject.GetComponent<Block>() == null))
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
                    _leftRay1.origin = transform.position + _vectorLeft1CollisionOffset;
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
                            _leftRay2.origin = transform.position + _vectorLeft2CollisionOffset;
                            RaycastHit2D hit_left2 = Physics2D.Raycast(_leftRay2.origin, _leftRay2.direction, 0.01f);

                            if (hit_left2.collider == null || (hit_left2.collider != null && hit_left2.collider.gameObject.GetComponent<Block>() == null))
                            {
                                //Check if there is a block over the player
                                _upRay.origin = transform.position + Vector3.up;
                                RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.01f);

                                if (hit_up.collider == null || (hit_up.collider != null && hit_up.collider.gameObject.GetComponent<Block>() == null))
                                {
                                    _state = State.Jumping;
                                    _playerDirection = Direction.Up;
                                    SetJumpingValues();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                _state = State.Falling;

                if (_targetXvelocity == -1)
                { 
                    _targetXfalling = Mathf.Floor(transform.position.x);
                    _targetXvelocity = 0f;

                    if (_playerDirection == Direction.Left)
                    {
                        //_targetXfalling -= 0.5f;
                    }
                    else if (_playerDirection == Direction.Right)
                    {
                        _targetXfalling += 1f;
                    }
                }

                transform.parent = null;
            }
        }
    }

    private void CreateFallingParticles(int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject();
            go.transform.localScale = Vector3.one * 0.3f;
            go.name = "Dust Particle";

            Vector3 pos = transform.position;
            pos.y -= GetComponent<BoxCollider2D>().size.y / 2f;
            go.transform.position = pos;

            int rp = Random.Range(0, 3);
            SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
            Sprite spr = Sprite.Create(_dustParticle[rp], new Rect(0, 0, _dustParticle[rp].width, _dustParticle[rp].height),
                        new Vector2(0.5f, 0.5f), 128f);
            rend.sprite = spr;
            rend.sortingOrder = 105 + i;

            DustParticle dp = go.AddComponent<DustParticle>();
            dp.StartParticle(Vector3.up);
        }
    }

    //Adjust the velocity parameter (_velocity) in function of the real player's direction (_playerDirection), state
    // and other parameters.
    private void AdjustVelocityByParams()
    {
        switch(_state)
        {
            case State.Grounded:
            {
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

                break;
            }
            
            case State.Falling:
            {
                _velocity.y = -_speed;
                _accSpeed += _incrSpeed;
                _velocity.x = 0;

                break;
            }
        }
        
    }

    private void MovingPlayer()
    {
        Vector3 p = transform.localPosition;

        _collider.size = _originalJumpColliderSize;
        _collider.offset = Vector3.zero;

        switch (_state)
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

                _targetScaleX = Mathf.SmoothDamp(_targetScaleX, 1f, ref _scaleXvelocity, 0.1f);
                _targetScaleY = Mathf.SmoothDamp(_targetScaleY, 1f, ref _scaleYvelocity, 0.1f);

                transform.localPosition = p;
                break;
            
            case State.Jumping:
                if (_jumpTimeActive == 0f)
                {
                    //Animation
                    _animState = PlayerAnimState.Jump;
                    _changeAnimation = true;
                }

                if (_jumpTimeActive < 0.2f)
                {
                    _targetScaleX = Mathf.SmoothDamp(_targetScaleX, 0.8f, ref _scaleXvelocity, 0.1f);
                    _targetScaleY = Mathf.SmoothDamp(_targetScaleY, 1.2f, ref _scaleYvelocity, 0.1f);
                }
                else
                {
                    _targetScaleX = Mathf.SmoothDamp(_targetScaleX, 1f, ref _scaleXvelocity, 0.1f);
                    _targetScaleY = Mathf.SmoothDamp(_targetScaleY, 1f, ref _scaleYvelocity, 0.1f);
                }
            
                if (_jumpTimeActive < 1f)
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

                // Change collider size to fix the bug where ice blocks
                // get pushed while jumping
                _collider.size = _jumpColliderSize;
                _collider.offset = _jumpColliderOffset;

                transform.position = p;
                break;
            
            case State.Falling:
                p.y += Time.deltaTime * _velocity.y * _accSpeed;
                p.x = Mathf.SmoothDamp(p.x, _targetXfalling, ref _targetXvelocity, 0.1f);

                _targetScaleX = Mathf.SmoothDamp(_targetScaleX, 1.1f, ref _scaleXvelocity, 0.1f);
                _targetScaleY = Mathf.SmoothDamp(_targetScaleY, 0.8f, ref _scaleYvelocity, 0.1f);

                if (_beginFalling)
                {
                    //Animation
                    _animState = PlayerAnimState.Jump;
                    _changeAnimation = true;
                    _beginFalling = false;
                    _falling = true;
                }

                transform.localPosition = p;
                break;
        }
    }

    public void SetLevelBounds(float x, float y, float w, float h)
    {
        _boundX = x;
        _boundY = y;
        _boundW = w;
        _boundH = h;
    }

    public void AdjustCamera()
    {
        Vector3 camPos = _gameController.GetCamera().transform.position;
        Vector3 offset = transform.position - Vector3.forward;

        float height = 3f;
        float width = height * ((float)Screen.width / (float)Screen.height);
        float maxH = _boundH - height;
        float minH = _boundY + height;
        float maxW = _boundW - width;
        float minW = _boundX + width;
        
        if (offset.y > maxH)
        {
            offset.y = maxH;
        }
        else if (offset.y < minH)
        {
            offset.y = minH;
        }
        else if (offset.x < minW)
        {
            offset.x = minW;
        }
        else if (offset.x > maxW)
        {
            offset.x = maxW;
        }

        camPos = Vector3.SmoothDamp(camPos, offset, ref _cameraVelocity, _cameraDampingTime);
        _gameController.GetCamera().transform.position = camPos;
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
        Text earthLabel = GameObject.Find("EarthUsesLabel").GetComponent<Text>();
        Text windLabel = GameObject.Find("WindUsesLabel").GetComponent<Text>();
        Text fireLabel = GameObject.Find("FireUsesLabel").GetComponent<Text>();
        Text iceLabel = GameObject.Find("WaterUsesLabel").GetComponent<Text>();

        Image earthButton = GameObject.Find("Earth").GetComponent<Image>();
        Image windButton = GameObject.Find("Wind").GetComponent<Image>();
        Image fireButton = GameObject.Find("Fire").GetComponent<Image>();
        Image iceButton = GameObject.Find("Water").GetComponent<Image>();

        _ice = ice;
        _fire = fire;
        _wind = wind;
        _earth = earth;

        earthLabel.text = _earth.ToString();
        fireLabel.text = _fire.ToString();
        iceLabel.text = _ice.ToString();
        windLabel.text = _wind.ToString();

        if (_earth == 0 && !GameController.IS_DEBUG_MODE)
        {
            earthButton.color = new Color(1f, 1f, 1f, 0f);

            Color c = earthLabel.color;
            c.a = 0f;
            earthLabel.color = c;
        }
        else
        {
            earthButton.color = Color.white;

            Color c = earthLabel.color;
            c.a = 1f;
            earthLabel.color = c;
        }

        if (_fire == 0 && !GameController.IS_DEBUG_MODE)
        {
            fireButton.color = new Color(1f, 1f, 1f, 0f);

            Color c = fireLabel.color;
            c.a = 0f;
            fireLabel.color = c;
        }
        else
        {
            fireButton.color = Color.white;

            Color c = fireLabel.color;
            c.a = 1f;
            fireLabel.color = c;
        }

        if (_ice == 0 && !GameController.IS_DEBUG_MODE)
        {
            iceButton.color = new Color(1f, 1f, 1f, 0f);

            Color c = iceLabel.color;
            c.a = 0f;
            iceLabel.color = c;
        }
        else
        {
            iceButton.color = Color.white;

            Color c = iceLabel.color;
            c.a = 1f;
            iceLabel.color = c;
        }

        if (_wind == 0 && !GameController.IS_DEBUG_MODE)
        {
            windButton.color = new Color(1f, 1f, 1f, 0f);

            Color c = windLabel.color;
            c.a = 0f;
            windLabel.color = c;
        }
        else
        {
            windButton.color = Color.white;

            Color c = windLabel.color;
            c.a = 1f;
            windLabel.color = c;
        }
    }

    public void SetUsesOfElem(PlayerActions elem, int uses)
    {
        switch (elem)
        {
            case PlayerActions.Earth:
                {
                    Text earthLabel = GameObject.Find("EarthUsesLabel").GetComponent<Text>();
                    Image earthButton = GameObject.Find("Earth").GetComponent<Image>();

                    _earth = uses;
                    earthLabel.text = _earth.ToString();

                    if (_earth == 0 && !GameController.IS_DEBUG_MODE)
                    {
                        earthButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);

                        Color c = earthLabel.color;
                        c.a = 0f;
                        earthLabel.color = c;
                    }
                    else
                    {
                        earthButton.color = Color.white;

                        Color c = earthLabel.color;
                        c.a = 1f;
                        earthLabel.color = c;
                    }
                }
                break;

            case PlayerActions.Fire:
                {
                    Text fireLabel = GameObject.Find("FireUsesLabel").GetComponent<Text>();
                    Image fireButton = GameObject.Find("Fire").GetComponent<Image>();

                    _fire = uses;
                    fireLabel.text = _fire.ToString();

                    if (_fire == 0 && !GameController.IS_DEBUG_MODE)
                    {
                        fireButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);

                        Color c = fireLabel.color;
                        c.a = 0f;
                        fireLabel.color = c;
                    }
                    else
                    {
                        fireButton.color = Color.white;

                        Color c = fireLabel.color;
                        c.a = 1f;
                        fireLabel.color = c;
                    }
                }
                break;

            case PlayerActions.Ice:
                {
                    Text iceLabel = GameObject.Find("WaterUsesLabel").GetComponent<Text>();
                    Image iceButton = GameObject.Find("Water").GetComponent<Image>();

                    _ice = uses;
                    iceLabel.text = _ice.ToString();

                    if (_ice == 0 && !GameController.IS_DEBUG_MODE)
                    {
                        iceButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);

                        Color c = iceLabel.color;
                        c.a = 0f;
                        iceLabel.color = c;
                    }
                    else
                    {
                        iceButton.color = Color.white;

                        Color c = iceLabel.color;
                        c.a = 1f;
                        iceLabel.color = c;
                    }
                }
                break;

            case PlayerActions.Wind:
                {
                    Text windLabel = GameObject.Find("WindUsesLabel").GetComponent<Text>();
                    Image windButton = GameObject.Find("Wind").GetComponent<Image>();

                    _wind = uses;
                    windLabel.text = _wind.ToString();

                    if (_wind == 0 && !GameController.IS_DEBUG_MODE)
                    {
                        windButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);

                        Color c = windLabel.color;
                        c.a = 0f;
                        windLabel.color = c;
                    }
                    else
                    {
                        windButton.color = Color.white;

                        Color c = windLabel.color;
                        c.a = 1f;
                        windLabel.color = c;
                    }
                }
                break;
        }
    }

    public int GetUsesOfElem(PlayerActions elem)
    {
        switch (elem)
        {
            case PlayerActions.Earth:
                return _earth;
            case PlayerActions.Fire:
                return _fire;
            case PlayerActions.Ice:
                return _ice;
            case PlayerActions.Wind:
                return _wind;
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

            if ((_state == State.Grounded && _velocity.magnitude == 0) || _state == State.Falling)
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
                                if (_ice > 0 || GameController.IS_DEBUG_MODE)
                                {
                                    _actionHappen = true;
                                }
                            }

                            break;
                        case PlayerActions.Wind:
                            {
                                RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, _actionRay.direction, 1f);

                                //Check if there is something on the player's Left or Right
                                GameObject aux = null;

                                for (int i_hit = 0; i_hit < hit.Length; i_hit++)
                                {
                                    aux = hit[i_hit].collider.gameObject;
                                    Block goHitBlock = aux.GetComponent<Block>();

                                    if (goHitBlock != null && goHitBlock.IsMovable())
                                    {
                                        break;
                                    }
                                }

                                _actionObjectAux = aux;

                                // Check if the wind in front of the player is a lever
                                // If it's not, reduce the wind use count
                                // If it is, force the action to happen and don't change
                                // the wind use count
                                if (_actionObjectAux != null)
                                {
                                    Lever goHitLever = _actionObjectAux.GetComponent<Lever>();

                                    if (goHitLever != null)
                                    {
                                        _actionHappen = true;
                                    }
                                    else
                                    {
                                        if (_wind > 0 || GameController.IS_DEBUG_MODE)
                                        {
                                            _actionHappen = true;
                                        }
                                    }
                                }
                            }

                            break;

                        case PlayerActions.Fire:

                            if (_fire > 0 || GameController.IS_DEBUG_MODE)
                            {
                                _actionHappen = true;
                            }

                            break;

                        case PlayerActions.Earth:

                            if (!Physics2D.Raycast(_actionRay.origin, _actionRay.direction, 0.1f))
                            {
                                if (_earth > 0 || GameController.IS_DEBUG_MODE)
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

                SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
                rend.sortingOrder = 107;

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

        transform.GetChild(0).transform.localScale = transform.GetChild(0).localScale.x * _targetScaleX * Vector3.right + _targetScaleY * Vector3.up;

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

                _begining = false;
            }
            else
            {
                if (!_begining)
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
                    
                
            }
        }

    }

    public void OnAlphaTweenFinished()
    {
        _endTime = Time.time - _startTime;
        _stars = SaveGameController.instance.GetStarCount(_endTime, _activeLevel.GetName());

        GameObject.Find("GuiCallbacks").GetComponent<GuiCallbacks>().OnPlayerHitDoor(_endTime, _stars, _activeLevel.GetName());
    }

    public void OnLevelFinished()
    {
        _isOnDoor = true;
        transform.GetChild(0).GetComponent<SpriteAlphaTweener>().DoTween(gameObject);
    }
    
    public Direction GetDirection ()
    {
        return _playerDirection;
    }

    public void DestroyPlayer()
    {
        if (!_isDying && !_isOnDoor)
        {
            _animState = PlayerAnimState.Death;
            _changeAnimation = true;
            _isDying = true;
        }
    }

    public void OnPlayerDestroyed()
    {
        GameObject.Destroy(gameObject);
        GameObject.Find("GuiCallbacks").GetComponent<GuiCallbacks>().OnPlayerDied();
    }
    
}
