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
    private float _speedAtteniuation = 1f;
    
    private int _ice = 0;
    private int _wind = 0;
    private int _earth = 0;
    private int _fire = 0;

    // Accel
    private const float _minAccSpeed = 1f;
    private float _accSpeed = _minAccSpeed;
    private const float _incrSpeed = 0.1f;

    private Vector2 _velocity;
    
    // Wish direction to walk
    private Direction _targetDirection = Direction.None;

    // Real direction to walk
    private Direction _playerDirection = Direction.None;

    // Direction on where to make actions
    private Direction _actionDirection = Direction.None;
    private Direction _actionDirectionSaved = Direction.None;
    
    private State _state = State.Normal;
    private PlayerActions _action = PlayerActions.None;

    // Rays
    private Ray2D _downRay;

    private Ray2D _rightRay1;
    private Ray2D _rightRay2;

    private Ray2D _leftRay1;
    private Ray2D _leftRay2;

    private Ray2D _upRay;

    // Offset values
    private const float _rayDownCollisionOffset = 0.25f;
    private const float _raySidesCollisionOffset = 0.31f;

    // Offset Vectors
    private Vector3 _vectorDownCollisionOffset;
    private Vector3 _vectorRight1CollisionOffset;
    private Vector3 _vectorRight2CollisionOffset;
    private Vector3 _vectorLeft1CollisionOffset;
    private Vector3 _vectorLeft2CollisionOffset;

    private Ray2D _actionRay;

    // Jumping
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

    // Wind and lever button switch
    private UnityEngine.UI.Button _windButton;
    private Texture2D _windTexture;
    private Texture2D _leverTexture;
    private Sprite _windSprite;
    private Sprite _leverSprite;
    private bool _hadWindUsesBefore = false;
    private float _leverDetectionDistance = 0.7f;

    // AnimState
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

    // Score related
    private float _startTime = 0f;
    private float _endTime = 0f;
    private int _stars = 3;

    // Scale tweening
    private float _targetScaleX = 1f;
    private float _targetScaleY = 1f;
    private float _scaleXvelocity = 0f;
    private float _scaleYvelocity = 0f;
    private float _targetXfalling = 0f;
    private float _targetXvelocity = 0f;

    // Dust particles
    private Texture2D[] _dustParticle;
    private Texture2D[] _iceParticle;
    private Texture2D[] _fireParticle;

    // Camera fixed to map bounds
    private float _boundX = 0f;
    private float _boundY = 0f;
    private float _boundH = 0f;
    private float _boundW = 0f;

    // Camera screenshake
    private float _cameraScreenShakeTimer = 0f;
    private float _cameraScreenShakeMaxTime = 0f;
    private float _screenShakeIntensity = 0.1f;
    private bool _doCameraScreenShake;

    // Jumping wait timers
    private float _jumpWaitTimer = 0f;
    private float _jumpWaitTime = 0.18f;

    private GameObject _animationGo;



    public void SetSpeedAttenuation(float att)
    {
        _speedAtteniuation = att;
    }

    public void SetActiveLevel(Level lv)
    {
        _activeLevel = lv;
    }
    
    public void SetGameController(GameController game)
    {
        _gameController = game;
    }

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

        // Vector Offsets
        _vectorDownCollisionOffset = new Vector3(-_rayDownCollisionOffset, -0.51f, 0f);
        _vectorRight1CollisionOffset = new Vector3(_raySidesCollisionOffset, 0f, 0f);
        _vectorRight2CollisionOffset = new Vector3(_raySidesCollisionOffset, 1f, 0f);
        _vectorLeft1CollisionOffset = new Vector3(-_raySidesCollisionOffset, 0f, 0f);
        _vectorLeft2CollisionOffset = new Vector3(-_raySidesCollisionOffset, 1f, 0f);

		_cameraVelocity = new Vector3();

        _animState = PlayerAnimState.IdleFront;
        _startTime = Time.time;

        // Adjust collider size while jumping to prevent colliding
        // against enemies hitboxes
        _collider = GetComponent<BoxCollider2D>();
        _jumpColliderSize = new Vector2(0.6f, 0.8f);
        _jumpColliderOffset = new Vector2(0.0f, -0.1f);
        _originalJumpColliderSize = _collider.size;

        // Instantiate particles
        _dustParticle = new Texture2D[3];

        _dustParticle[0] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleDust_1");
        _dustParticle[1] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleDust_2");
        _dustParticle[2] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleDust_3");

        _iceParticle = new Texture2D[3];

        _iceParticle[0] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleIcedDust_1");
        _iceParticle[1] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleIcedDust_2");
        _iceParticle[2] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleDust/ParticleIcedDust_3");

        _fireParticle = new Texture2D[2];

        _fireParticle[0] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_1");
        _fireParticle[1] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_2");

        _windTexture = (Texture2D)_gameController.GetTextureController().GetTexture("GUI/Wind");
        _leverTexture = (Texture2D)_gameController.GetTextureController().GetTexture("Blocks/Lever/Lever_1_Frame_2");

        _windSprite = Sprite.Create(_windTexture, new Rect(0, 0, _windTexture.width, _windTexture.height),
                        new Vector2(0.5f, 0.5f), 128f);

        _leverSprite = Sprite.Create(_leverTexture, new Rect(0, 0, _leverTexture.width, _leverTexture.height),
                        new Vector2(0.5f, 0.5f), 128f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (_windButton == null)
        {
            if (_gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().IsReady())
            {
                _windButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetWindButton().GetComponent<UnityEngine.UI.Button>();
            }
        }

        if (!_gameController.IsGamePaused())
        {
            if (!_isDying && !_isOnDoor)
            {
                SetPreviousPlayerDirection();
                CheckMovingCollisions();
                AdjustVelocityByParams();
                MovingPlayer();
            }
            
            AnimatingPlayer();
            CheckLevelFinished();
            AdjustCamera();
        }
    }

    private void CheckLevelFinished()
    {
        if (_isOnDoor && _fadeOutMusic)
        {
            float vol = _gameController.GetAudioController().GetChannelVolume(0);

            if (vol <= 0.1f)
            {
                // Pause in-game music
                _gameController.GetAudioController().PauseChannel(0);
                _gameController.GetAudioController().SetChannelVolume(0, 1f);

                // Play winning music
                _gameController.GetAudioController().SetChannelVolume(15, 1f);
                _gameController.GetAudioController().PlayChannel(15);

                _fadeOutMusic = false;
            }
            else
            {
                // Fade in 0.5 sec
                vol -= Time.deltaTime * 2f;
                _gameController.GetAudioController().SetChannelVolume(0, vol);
            }
        }
    }

    // Set the player's real direction (_playerDirection) in function of desired direction (_targetDirection)
    // The real direction could be changed by other methods
    private void SetPreviousPlayerDirection()
    {
        _playerDirection = _targetDirection;
    }

    // Check all collisions on the player's way to find the real player's direction
    private void CheckMovingCollisions()
    {
        //*************************************************************************************************
        //Check if the player collides with a death block
        bool playerDies = false;

        if (_state != State.Jumping)
        {
            //UP
            RaycastHit2D[] checkingDeathBlocksUP = Physics2D.RaycastAll(transform.position + Vector3.up * 0.3f + Vector3.left * 0.5f, Vector2.right, 1f);
            
            for (int i = 0; i < checkingDeathBlocksUP.Length; i++)
            {
                if (checkingDeathBlocksUP[i].collider != null)
                {
                    Block deathBlock = checkingDeathBlocksUP[i].collider.gameObject.GetComponent<Block>();

                    if (deathBlock != null && deathBlock.GetBlockType() == BlockType.Death)
                    {
                        this.DestroyPlayer();
                        playerDies = true;
                    }
                }
            }

            //DOWN
            RaycastHit2D[] checkingDeathBlocksDOWN = Physics2D.RaycastAll(transform.position + Vector3.down * 0.3f + Vector3.left * 0.5f, Vector2.right, 1f);

            for (int i = 0; i < checkingDeathBlocksDOWN.Length; i++)
            {
                if (checkingDeathBlocksDOWN[i].collider != null)
                {
                    Block deathBlock = checkingDeathBlocksDOWN[i].collider.gameObject.GetComponent<Block>();
                    if (deathBlock != null && deathBlock.GetBlockType() == BlockType.Death)
                    {
                        this.DestroyPlayer();
                        playerDies = true;
                    }
                }
            }

            //RIGHT
            RaycastHit2D[] checkingDeathBlocksRIGHT = Physics2D.RaycastAll(transform.position + Vector3.right * 0.5f, Vector2.right, 0.01f);

            for (int i = 0; i < checkingDeathBlocksRIGHT.Length; i++)
            {
                if (checkingDeathBlocksRIGHT[i].collider != null)
                {
                    Block deathBlock = checkingDeathBlocksRIGHT[i].collider.gameObject.GetComponent<Block>();
                    if (deathBlock != null && deathBlock.GetBlockType() == BlockType.Death)
                    {
                        this.DestroyPlayer();
                        playerDies = true;
                    }
                }
            }

            //LEFT
            RaycastHit2D[] checkingDeathBlocksLEFT = Physics2D.RaycastAll(transform.position + Vector3.left * 0.5f, Vector2.left, 0.01f);

            for (int i = 0; i < checkingDeathBlocksLEFT.Length; i++)
            {
                if (checkingDeathBlocksLEFT[i].collider != null)
                {
                    Block deathBlock = checkingDeathBlocksLEFT[i].collider.gameObject.GetComponent<Block>();
                    if (deathBlock != null && deathBlock.GetBlockType() == BlockType.Death)
                    {
                        this.DestroyPlayer();
                        playerDies = true;
                    }
                }
            }
        }
        // END checking death block
        //****************************************************************************************************

        bool hasHitDown = false;
        transform.parent = null;

        if (!playerDies && _state != State.Jumping)
        {
            _downRay.origin = transform.position + _vectorDownCollisionOffset;
            RaycastHit2D[] hit_down = Physics2D.RaycastAll(_downRay.origin, _downRay.direction, _rayDownCollisionOffset * 2);

            // Check for something on the player's Down
            // We cycle through all the possible colliders
            for (int i = 0; i < hit_down.Length; i++)
            {
                if (hit_down[i].collider != null)
                {
                    GameObject goHitDown;
                    goHitDown = hit_down[i].collider.gameObject;

                    // Check if there is a Block on player's Down
                    Block goHitDownBlock = goHitDown.GetComponent<Block>();

                    if (goHitDownBlock != null)
                    {
                        // Adjust player position to the height of the block
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

                        // Check if it is on a platform
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
                
                // Check left and right to unparent the player if
                // it is on a platform and it hits a block
                if (transform.parent != null &&
                                transform.parent.GetComponent<Block>() != null &&
                                transform.parent.GetComponent<Block>().IsPlatform())
                {

                    _rightRay1.origin = transform.position + _vectorRight1CollisionOffset;
                    RaycastHit2D hit_right = Physics2D.Raycast(_rightRay1.origin, _rightRay1.direction, 0.01f);

                    if (hit_right.collider != null)
                    {
                        GameObject goHitRight = hit_right.collider.gameObject;
                        Block goHitRightBlock = goHitRight.GetComponent<Block>();

                        // Check if it is a Block
                        if (goHitRightBlock != null)
                        {
                            transform.parent = null;
                        }
                    }

                    _leftRay1.origin = transform.position + _vectorLeft1CollisionOffset;
                    RaycastHit2D hit_left = Physics2D.Raycast(_leftRay1.origin, _leftRay1.direction, 0.01f);

                    // Check if there is something on the player's Left
                    if (hit_left.collider != null)
                    {
                        GameObject goHitLeft = hit_left.collider.gameObject;

                        // Check if it is a Block
                        if (goHitLeft.GetComponent<Block>() != null)
                        {
                            transform.parent = null;
                        }
                    }

                    // Check if there is a block over the player. If the
                    // player is on a platform and there's a block over it,
                    // unparent it to prevent the player from clipping through
                    // the walls
                    RaycastHit2D[] checkingUpCollision = Physics2D.RaycastAll(transform.position + Vector3.up * 0.5f + 
                        Vector3.left * 0.4f, Vector2.right, 0.8f);
                    
                    for (int i = 0; i < checkingUpCollision.Length; ++i)
                    {
                        if (checkingUpCollision[i].collider.gameObject.GetComponent<Block>())
                        {
                            transform.parent = null;
                            break;
                        }
                    }
                }

                // Check the player's Right
                if (_playerDirection == Direction.Right)
                {
                    _rightRay1.origin = transform.position + _vectorRight1CollisionOffset;
                    RaycastHit2D hit_right = Physics2D.Raycast(_rightRay1.origin, _rightRay1.direction, 0.01f);

                    // Check if there is something on the player's Right
                    if (hit_right.collider != null)
                    {
                        GameObject goHitRight = hit_right.collider.gameObject;
                        Block goHitRightBlock = goHitRight.GetComponent<Block>();

                        // Check if it is a Block
                        if (goHitRightBlock != null)
                        {
                            // Adjust player's real direction
                            _playerDirection = Direction.None;

                            // Check if there is a block over the block which player collided with
                            _rightRay2.origin = transform.position + _vectorRight2CollisionOffset;
                            RaycastHit2D hit_right2 = Physics2D.Raycast(_rightRay2.origin, _rightRay2.direction, 0.01f);

                            if (hit_right2.collider == null || (hit_right2.collider != null && hit_right2.collider.gameObject.GetComponent<Block>() == null))
                            {
                                // Check if there is a block over the player
                                _upRay.origin = transform.position + Vector3.up;
                                RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.01f);

                                if (hit_up.collider == null || (hit_up.collider != null && hit_up.collider.gameObject.GetComponent<Block>() == null))
                                {
                                    _jumpWaitTimer += Time.deltaTime;

                                    if (_jumpWaitTimer > _jumpWaitTime)
                                    {
                                        _playerDirection = Direction.Up;
                                        _jumpWaitTimer = 0f;
                                        SetJumpingValues();

                                        _state = State.Jumping;
                                    }
                                }
                            }

                        }

                    }

                    // Check lever collision
                    bool leverCollision = false;
                    RaycastHit2D[] leverHits = Physics2D.RaycastAll(transform.position + Vector3.left * 1.1f, Vector2.right, 2.2f);
                    Lever lev = null;

                    foreach (RaycastHit2D rayHit in leverHits)
                    {
                        if (rayHit.collider != null && rayHit.collider.gameObject.GetComponent<Lever>() != null)
                        {
                            leverCollision = true;
                            lev = rayHit.collider.gameObject.GetComponent<Lever>();
                        }
                    }
                    // Check if it is a lever
                    if (leverCollision)
                    {
                        if (_windButton == null)
                        {
                            _windButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetWindButton().GetComponent<UnityEngine.UI.Button>();
                        }

                        //_windButton.image.sprite = _leverSprite;
                        //_windButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                    }
                    else
                    {
                        if (_windButton == null)
                        {
                            _windButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetWindButton().GetComponent<UnityEngine.UI.Button>();
                        }

                        //_windButton.image.sprite = _windSprite;

                        //float color = (_wind <= 0 && _hadWindUsesBefore ? 0.5f : 1f);
                        //_windButton.GetComponent<Image>().color = new Color(color, color, color, (_wind > 0 || _hadWindUsesBefore ? 1f : 0f));
                    }
                }
                // Check the player's Left
                else if (_playerDirection == Direction.Left)
                {
                    _leftRay1.origin = transform.position + _vectorLeft1CollisionOffset;
                    RaycastHit2D hit_left = Physics2D.Raycast(_leftRay1.origin, _leftRay1.direction, 0.01f);

                    // Check if there is something on the player's Left
                    if (hit_left.collider != null)
                    {
                        GameObject goHitLeft = hit_left.collider.gameObject;

                        // Check if it is a Block
                        if (goHitLeft.GetComponent<Block>() != null)
                        {
                            // Adjust player's real direction
                            _playerDirection = Direction.None;

                            // Check if there is a block over the block which player collided with
                            _leftRay2.origin = transform.position + _vectorLeft2CollisionOffset;
                            RaycastHit2D hit_left2 = Physics2D.Raycast(_leftRay2.origin, _leftRay2.direction, 0.01f);

                            if (hit_left2.collider == null || (hit_left2.collider != null && hit_left2.collider.gameObject.GetComponent<Block>() == null))
                            {
                                // Check if there is a block over the player
                                _upRay.origin = transform.position + Vector3.up;
                                RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.01f);

                                if (hit_up.collider == null || (hit_up.collider != null && hit_up.collider.gameObject.GetComponent<Block>() == null))
                                {
                                    _jumpWaitTimer += Time.deltaTime;

                                    if (_jumpWaitTimer > _jumpWaitTime)
                                    {
                                        _playerDirection = Direction.Up;
                                        SetJumpingValues();
                                        _jumpWaitTimer = 0f;

                                        _state = State.Jumping;
                                    }
                                }
                            }
                            
                        }
                    }

                    // Check lever collision
                    bool leverCollision = false;
                    RaycastHit2D[] leverHits = Physics2D.RaycastAll(transform.position + Vector3.right * 0.5f, Vector2.left, _leverDetectionDistance);
                    foreach(RaycastHit2D rayHit in leverHits)
                    {
                        if (rayHit.collider != null && rayHit.collider.gameObject.GetComponent<Lever>() != null)
                        {
                            leverCollision = true;
                        }
                    }
                    // Check if it is a lever
                    if (leverCollision)
                    {
                        if (_windButton == null)
                        {
                            _windButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetWindButton().GetComponent<UnityEngine.UI.Button>();
                        }

                        //_windButton.image.sprite = _leverSprite;
                        //_windButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

                    }
                    else
                    {
                        if (_windButton == null)
                        {
                            _windButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetWindButton().GetComponent<UnityEngine.UI.Button>();
                        }

                        //_windButton.image.sprite = _windSprite;
                        //float color = (_wind <= 0 && _hadWindUsesBefore ? 0.5f : 1f);
                        //_windButton.GetComponent<Image>().color = new Color(color, color, color, (_wind > 0 || _hadWindUsesBefore ? 1f : 0f));
                    }
                }
                else
                {
                    _jumpWaitTimer = 0f;
                }

            }
            else
            {
                _state = State.Falling;

                // Adjust falling horizontal position to
                // the middle of the falling block
                if (_targetXvelocity == -1)
                { 
                    _targetXfalling = Mathf.Floor(transform.position.x);
                    _targetXvelocity = 0f;

                    if (_playerDirection == Direction.Right)
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

    private void CreateRockParticles(Vector3 position, int count = 1)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject();
            go.transform.localScale = Vector3.one * 0.8f;
            go.name = "Rock Particle";
            
            go.transform.position = position;

            int rp = Random.Range(0, 3);
            SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
            Sprite spr = Sprite.Create(_dustParticle[rp], new Rect(0, 0, _dustParticle[rp].width, _dustParticle[rp].height),
                        new Vector2(0.5f, 0.5f), 128f);
            rend.sprite = spr;
            rend.sortingOrder = 135 + i;

            DustParticle dp = go.AddComponent<DustParticle>();
            dp.StartParticle(Random.insideUnitCircle, true);
        }
    }

    private void CreateWindParticles(bool front = true)
    {
        if (front)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject go = new GameObject();
                go.transform.localScale = Vector3.one * 0.6f;
                go.name = "Wind Particle";

                Vector3 pos = transform.position;

                pos.y += 0.1f;

                if (_actionDirection == Direction.Right)
                {
                    pos.x += GetComponent<BoxCollider2D>().size.y * 0.5f - 0.2f;
                }
                else if (_actionDirection == Direction.Left)
                {
                    pos.x -= GetComponent<BoxCollider2D>().size.y * 0.5f - 0.2f;
                }

                go.transform.position = pos;

                int rp = Random.Range(0, 3);
                SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
                Sprite spr = Sprite.Create(_iceParticle[rp], new Rect(0, 0, _iceParticle[rp].width, _iceParticle[rp].height),
                            new Vector2(0.5f, 0.5f), 128f);
                rend.sprite = spr;
                rend.sortingOrder = 105 + i;

                DustParticle dp = go.AddComponent<DustParticle>();

                if (_actionDirection == Direction.Right)
                {
                    dp.StartParticle(Vector3.right);
                }
                else if (_actionDirection == Direction.Left)
                {
                    dp.StartParticle(Vector3.left);
                }
            }
        }
    }

    private void CreateFireParticles(bool front = true)
    {
        if (front)
        {
            for (int i = 0; i < 25; i++)
            {
                GameObject go = new GameObject();
                go.transform.localScale = Vector3.one * 0.15f;
                go.name = "Fire Particle";

                Vector3 pos = transform.position;

                pos.y += 0.1f;

                if (_actionDirection == Direction.Right)
                {
                    pos.x += GetComponent<BoxCollider2D>().size.y * 0.5f - 0.2f;
                }
                else if (_actionDirection == Direction.Left)
                {
                    pos.x -= GetComponent<BoxCollider2D>().size.y * 0.5f - 0.2f;
                }

                go.transform.position = pos;

                int rp = Random.Range(0, 2);
                SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
                Sprite spr = Sprite.Create(_fireParticle[rp], new Rect(0, 0, _fireParticle[rp].width, _fireParticle[rp].height),
                            new Vector2(0.5f, 0.5f), 128f);
                rend.sprite = spr;
                rend.sortingOrder = 105 + i;

                DustParticle dp = go.AddComponent<DustParticle>();

                if (_actionDirection == Direction.Right)
                {
                    dp.StartParticle(Vector3.right);
                }
                else if (_actionDirection == Direction.Left)
                {
                    dp.StartParticle(Vector3.left);
                }
            }
        }
    }

    // Adjust the velocity parameter (_velocity) in function of the real player's direction (_playerDirection), state
    // and other parameters.
    private void AdjustVelocityByParams()
    {
        switch (_state)
        {
            case State.Grounded:
            {
                _velocity.y = 0;
                _accSpeed = _minAccSpeed;

                switch (_playerDirection)
                {
                    case Direction.Right:
                        _velocity.x = _speed * _speedAtteniuation;
                        break;

                    case Direction.Left:
                        _velocity.x = -_speed * _speedAtteniuation;
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

                    // Animation
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
                    // Animation
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
                    // Adjust player position
                    p = _jumpPoint2;

                    // Animation
                    _animState = _animStateAfterJump;
                    _changeAnimation = true;

                    _state = State.Normal;
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

                _targetScaleX = Mathf.SmoothDamp(_targetScaleX, 0.8f, ref _scaleXvelocity, 0.1f);
                _targetScaleY = Mathf.SmoothDamp(_targetScaleY, 1.2f, ref _scaleYvelocity, 0.1f);

                if (_beginFalling)
                {
                    // Animation
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

        if (offset.x < minW)
        {
            offset.x = minW;
        }
        else if (offset.x > maxW)
        {
            offset.x = maxW;
        }

        camPos = Vector3.SmoothDamp(camPos, offset, ref _cameraVelocity, _cameraDampingTime);

        if (_doCameraScreenShake)
        {
            camPos += Random.onUnitSphere * _screenShakeIntensity;
            _cameraScreenShakeTimer += Time.deltaTime;

            if (_cameraScreenShakeTimer > _cameraScreenShakeMaxTime)
            {
                _doCameraScreenShake = false;
            }
        }

        _gameController.GetCamera().transform.position = camPos;
    }

    public void DoScreenShake(float duration, float intensity)
    {
        _cameraScreenShakeTimer = 0f;
        _cameraScreenShakeMaxTime = duration;
        _screenShakeIntensity = intensity;
        _doCameraScreenShake = true;
    }

    // Adjust the jumping values in funtion of the desired player's direction (_targetDirection)
    private void SetJumpingValues()
    {
        _jumpTimeActive = 0f;
        float ent_pointX = Mathf.Round(transform.position.x);
        
        if (_targetDirection == Direction.Right)
        {
            _jumpPoint0 = new Vector2(transform.position.x, transform.position.y);
            _jumpPoint1 = new Vector2(ent_pointX + 0.5f, transform.position.y + 1.5f);
            _jumpPoint2 = new Vector2(ent_pointX + 1f, transform.position.y + 1f);
        }
        else if (_targetDirection == Direction.Left)
        {
            _jumpPoint0 = new Vector2(transform.position.x, transform.position.y);
            _jumpPoint1 = new Vector2(ent_pointX - 0.5f, transform.position.y + 1.5f);
            _jumpPoint2 = new Vector2(ent_pointX - 1f, transform.position.y + 1f);
        }
    }
    
    public void SetActions(int ice, int fire, int wind, int earth)
    {
        Text earthLabel = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementLabel(PlayerActions.Earth);
        Text windLabel = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementLabel(PlayerActions.Wind);
        Text fireLabel = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementLabel(PlayerActions.Fire);
        Text iceLabel = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementLabel(PlayerActions.Ice);

        Image earthButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementButtonImage(PlayerActions.Earth);
        Image windButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementButtonImage(PlayerActions.Wind);
        Image fireButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementButtonImage(PlayerActions.Fire);
        Image iceButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementButtonImage(PlayerActions.Ice);

        _ice = ice;
        _fire = fire;
        _wind = wind;
        _earth = earth;

        if (earthLabel != null)
            earthLabel.text = _earth.ToString();

        if (fireLabel != null)
            fireLabel.text = _fire.ToString();

        if (iceLabel != null)
            iceLabel.text = _ice.ToString();

        if (windLabel != null)
            windLabel.text = _wind.ToString();

        if (_earth == 0 && !GameController.IS_DEBUG_MODE)
        {
            if (earthButton != null)
                earthButton.color = new Color(1f, 1f, 1f, 0f);

            if (earthLabel != null)
            {
                Color c = earthLabel.color;
                c.a = 0f;
                earthLabel.color = c;
            }
        }
        else
        {
            if (earthButton != null)
                earthButton.color = Color.white;

            if (earthLabel != null)
            {
                Color c = earthLabel.color;
                c.a = 1f;
                earthLabel.color = c;
            }
        }

        if (_fire == 0 && !GameController.IS_DEBUG_MODE)
        {
            if (fireButton != null)
                fireButton.color = new Color(1f, 1f, 1f, 0f);

            if (fireLabel != null)
            {
                Color c = fireLabel.color;
                c.a = 0f;
                fireLabel.color = c;
            }
        }
        else
        {
            if (fireButton != null)
                fireButton.color = Color.white;

            if (fireLabel != null)
            {
                Color c = fireLabel.color;
                c.a = 1f;
                fireLabel.color = c;
            }
        }

        if (_ice == 0 && !GameController.IS_DEBUG_MODE)
        {
            if (iceButton != null)
                iceButton.color = new Color(1f, 1f, 1f, 0f);

            if (iceLabel != null)
            {
                Color c = iceLabel.color;
                c.a = 0f;
                iceLabel.color = c;
            }
        }
        else
        {
            if (iceButton != null)
                iceButton.color = Color.white;

            if (iceLabel != null)
            {
                Color c = iceLabel.color;
                c.a = 1f;
                iceLabel.color = c;
            }
        }

        if (_wind == 0 && !GameController.IS_DEBUG_MODE)
        {
            if (windButton != null)
                windButton.color = new Color(1f, 1f, 1f, 0f);

            if (windLabel != null)
            {
                Color c = windLabel.color;
                c.a = 0f;
                windLabel.color = c;
            }
        }
        else
        {
            if (windButton != null)
            {
                windButton.color = Color.white;
                _hadWindUsesBefore = true;
            }

            if (windLabel != null)
            {
                Color c = windLabel.color;
                c.a = 1f;
                windLabel.color = c;
            }
        }
    }

    public void SetUsesOfElem(PlayerActions elem, int uses)
    {
        switch (elem)
        {
            case PlayerActions.Earth:
                {
                    Text earthLabel = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementLabel(PlayerActions.Earth);
                    Image earthButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementButtonImage(PlayerActions.Earth);

                    _earth = uses;

                    if (earthLabel != null)
                        earthLabel.text = _earth.ToString();

                    if (_earth == 0 && !GameController.IS_DEBUG_MODE)
                    {
                        if (earthButton != null)
                            earthButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);

                        if (earthLabel != null)
                        {
                            Color c = earthLabel.color;
                            c.a = 0f;
                            earthLabel.color = c;
                        }
                    }
                    else
                    {
                        if (earthButton != null)
                            earthButton.color = Color.white;

                        if (earthLabel != null)
                        {
                            Color c = earthLabel.color;
                            c.a = 1f;
                            earthLabel.color = c;
                        }
                    }
                }
                break;

            case PlayerActions.Fire:
                {
                    Text fireLabel = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementLabel(PlayerActions.Fire);
                    Image fireButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementButtonImage(PlayerActions.Fire);

                    _fire = uses;

                    if (fireLabel != null)
                        fireLabel.text = _fire.ToString();

                    if (_fire == 0 && !GameController.IS_DEBUG_MODE)
                    {
                        if (fireButton != null)
                            fireButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);

                        if (fireLabel != null)
                        {
                            Color c = fireLabel.color;
                            c.a = 0f;
                            fireLabel.color = c;
                        }
                    }
                    else
                    {
                        if (fireButton != null)
                            fireButton.color = Color.white;

                        if (fireLabel != null)
                        {
                            Color c = fireLabel.color;
                            c.a = 1f;
                            fireLabel.color = c;
                        }
                    }
                }
                break;

            case PlayerActions.Ice:
                {
                    Text iceLabel = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementLabel(PlayerActions.Ice);
                    Image iceButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementButtonImage(PlayerActions.Ice);

                    _ice = uses;

                    if (iceLabel != null)
                        iceLabel.text = _ice.ToString();

                    if (_ice == 0 && !GameController.IS_DEBUG_MODE)
                    { 
                        if (iceButton != null)
                            iceButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);

                        if (iceLabel != null)
                        {
                            Color c = iceLabel.color;
                            c.a = 0f;
                            iceLabel.color = c;
                        }
                    }
                    else
                    {
                        if (iceButton != null)
                            iceButton.color = Color.white;

                        if (iceLabel != null)
                        {
                            Color c = iceLabel.color;
                            c.a = 1f;
                            iceLabel.color = c;
                        }
                    }
                }
                break;

            case PlayerActions.Wind:
                {
                    Text windLabel = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementLabel(PlayerActions.Wind);
                    Image windButton = _gameController.GetLevelController().GetActiveLevel().GetGuiCallbacks().GetElementButtonImage(PlayerActions.Wind);

                    _wind = uses;

                    if (windLabel != null)
                        windLabel.text = _wind.ToString();

                    if (_wind == 0 && !GameController.IS_DEBUG_MODE)
                    {
                        if (windButton != null)
                            windButton.color = new Color(0.5f, 0.5f, 0.5f, 1f);

                        if (windLabel != null)
                        {
                            Color c = windLabel.color;
                            c.a = 0f;
                            windLabel.color = c;
                        }
                    }
                    else
                    {
                        if (windButton != null)
                            windButton.color = Color.white;

                        if (windLabel != null)
                        {
                            Color c = windLabel.color;
                            c.a = 1f;
                            windLabel.color = c;
                        }
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

            if ((_state == State.Grounded && _velocity.x == 0) || _state == State.Falling)
            {
                _actionRay = new Ray2D();
                
                if (_actionDirectionSaved == Direction.Left)
                {
                    if (_action == PlayerActions.Wind)
                    {
                        _actionRay.origin = transform.position;
                    }
                    else
                    {
                        _actionRay.origin = new Vector3(Mathf.Round(transform.position.x - 1), Mathf.Round(transform.position.y), 0f);
                    }

                    _actionRay.direction = Vector2.left;
                }
                else if (_actionDirectionSaved == Direction.Right)
                {
                    if (_action == PlayerActions.Wind)
                    {
                        _actionRay.origin = transform.position;
                    }
                    else
                    {
                        _actionRay.origin = new Vector3(Mathf.Round(transform.position.x + 1), Mathf.Round(transform.position.y), 0f);
                    }
                    
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

                                // Check if there is something on the player's Left or Right
                                GameObject aux = null;
                                GameObject auxLever = null;

                                for (int i_hit = 0; i_hit < hit.Length; i_hit++)
                                {
                                    aux = hit[i_hit].collider.gameObject;
                                    Block goHitBlock = aux.GetComponent<Block>();

                                    if (goHitBlock != null && goHitBlock.IsMovable())
                                    {
                                        auxLever = null;
                                        break;
                                    }

                                    if (aux.GetComponent<Lever>() != null)
                                    {
                                        auxLever = aux;
                                    }
                                }

                                _actionObjectAux = aux;

                                if (auxLever != null)
                                {
                                    _actionObjectAux = auxLever;
                                }

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
                if (_action == PlayerActions.Ice)
                {
                    _gameController.GetAudioController().PlayChannel(2);
                }

                // Animation
                _animState = PlayerAnimState.Action;
                _changeAnimation = true;

                // AnimationObject
                _animationGo = new GameObject("AnimationObject_" + type.ToString());
                _animationGo.transform.position = _actionRay.origin;

                if (_action != PlayerActions.Earth)
                {
                    SpriteRenderer rend = _animationGo.AddComponent<SpriteRenderer>();
                    rend.sortingOrder = 107;

                    AnimationObject animObj = _animationGo.AddComponent<AnimationObject>();
                    animObj.SetParams(_activeLevel, "Actions/" + type.ToString() + "Action/" + type.ToString() + "Action_1_Anim");
                }
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
                                            (int)_animationGo.transform.position.y, "Blocks/Ice/Ice_1");

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

                        // Play the wind animation
                        CreateWindParticles();
                        _gameController.GetAudioController().PlayChannel(8);
                    }

                    // Check if it is a Lever
                    Lever goHitLever = _actionObjectAux.GetComponent<Lever>();

                    if (goHitLever != null)
                    {
                        goHitLever.ChangeLeverDirection();

                        // Play the wind animation
                        if (goHitLever.GetLeverDirection() == 0)
                        {
                            _gameController.GetAudioController().PlayChannel(9);
                        }
                        else
                        {
                            _gameController.GetAudioController().PlayChannel(10);
                        }
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
                CreateFireParticles();
                SetUsesOfElem(_action, GetUsesOfElem(_action) - 1);
                _gameController.GetAudioController().PlayChannel(6);

                break;

            case PlayerActions.Earth:

                int rockIndex = Random.Range(1, 4);

                goToPut = _activeLevel.CreateBlock(BlockType.Rock, (int)_actionRay.origin.x,
                                            (int)_animationGo.transform.position.y, "Blocks/Stone/Stone_" + rockIndex.ToString());

                _activeLevel.AddEntity(goToPut, goToPut.name);

                SetUsesOfElem(_action, GetUsesOfElem(_action) - 1);
                CreateRockParticles(goToPut.transform.position, 10);
                _gameController.GetAudioController().PlayChannel(7);

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
                    _canMove = true;
                }

                if (_action == PlayerActions.Ice)
                {
                    if (sprite_animator.GetAnimationIndex() == 3)
                    {
                        MakeTheActionHappen();
                    }
                }
                else
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
            _gameController.GetAudioController().PlayChannel(11);

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
