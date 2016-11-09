using UnityEngine;
using System.Collections;

public class LeverDoorData : BaseEnemyData
{
    public Vector2 p0;
    public Vector2 p1;
}

public class Lever : EnemyBase
{
    private GameObject _door;
    private Sprite _leftPos;
    private Sprite _rightPos;
    private Sprite[] _animFrames;
    
    //Door attributes
    private Vector2 _target;
    private int _targetIndex = 0;
    private LeverDoorData _leverDoorData;
    private bool _moving = false;

    private int _animationIndex = 0;
    private float _frameTimer = 0f;
    private float _frameWait = 0.08f;
    private bool _playAnimation = false;

    private SpriteRenderer _rend;
    private SpriteRenderer _tooltip;

    private enum TooltipState
    {
        Hidden,
        FadingIn,
        Showing,
        FadingOut
    }

    private TooltipState _tooltipState = TooltipState.Hidden;

	// Use this for initialization
	void Start()
    {
        if (_door)
        {
            _door.transform.position = new Vector3(_target.x, _target.y, 0f);
        }

        _animFrames = new Sprite[4];
        _rend = GetComponent<SpriteRenderer>();
        SetAnimationSprites();
	}
	
	// Update is called once per frame
	void Update()
    {
        MoveDoor();
        PlayAnimation();
        PlayTooltipAnimation();
	}

    public void OnPlayerHit()
    {
        _tooltipState = TooltipState.FadingIn;
    }

    public void OnPlayerLeave()
    {
        _tooltipState = TooltipState.FadingOut;
    }

    private void PlayTooltipAnimation()
    {
        RaycastHit2D[] hit = Physics2D.LinecastAll(transform.position + Vector3.left * 1.1f,
            transform.position + Vector3.right * 1.1f);
        bool foundPlayer = false;

        foreach (RaycastHit2D h in hit)
        {
            if (h.collider.gameObject != null)
            {
                if (h.collider.gameObject.GetComponent<Player>() != null)
                {
                    foundPlayer = true;
                }
            }
        }

        if (foundPlayer)
        {
            if (GameController.IS_MOBILE_RUNTIME && Input.GetMouseButtonDown(0))
            {
                Vector3 mouseWd = _level.GetLevelController().GetGameController().GetCamera().ScreenToWorldPoint(Input.mousePosition);

                if (Vector3.Distance(transform.position, mouseWd) < 1.3f)
                {
                    ChangeLeverDirection();

                    if (_targetIndex == 0)
                    {
                        _level.GetLevelController().GetGameController().GetAudioController().PlayChannel(9);
                    }
                    else
                    {
                        _level.GetLevelController().GetGameController().GetAudioController().PlayChannel(10);
                    }
                }
            }

            if (_tooltipState == TooltipState.Hidden)
            {
                _tooltipState = TooltipState.FadingIn;
            }
        }
        else
        { 
            if (_tooltipState == TooltipState.Showing)
            {
                _tooltipState = TooltipState.FadingOut;
            }
        }

        if (_tooltipState == TooltipState.FadingIn)
        {
            Color c = _tooltip.color;
            c.a += Time.deltaTime * 4f;
            _tooltip.color = c;

            if (c.a > 1f)
            {
                _tooltip.color = Color.white;
                _tooltipState = TooltipState.Showing;
            }
        }
        else if (_tooltipState == TooltipState.FadingOut)
        {
            Color c = _tooltip.color;
            c.a -= Time.deltaTime * 4f;
            _tooltip.color = c;

            if (c.a < 0f)
            {
                _tooltip.color = new Color(1f,1f,1f,0f);
                _tooltipState = TooltipState.Hidden;
            }
        }
    }

    private void PlayAnimation()
    {
        if (_playAnimation)
        {
            if (_targetIndex == 1)
            {
                _frameTimer += Time.deltaTime;

                if (_frameTimer > _frameWait)
                {
                    _frameTimer = 0f;

                    if (_animationIndex < _animFrames.Length - 1)
                    {
                        ++_animationIndex;
                        _rend.sprite = _animFrames[_animationIndex];
                    }
                    else
                    {
                        _rend.sprite = _rightPos;
                        _playAnimation = false;
                    }
                }
            }
            else
            {
                _frameTimer += Time.deltaTime;

                if (_frameTimer > _frameWait)
                {
                    _frameTimer = 0f;

                    if (_animationIndex > 0)
                    {
                        --_animationIndex;
                        _rend.sprite = _animFrames[_animationIndex];
                    }
                    else
                    {
                        _rend.sprite = _leftPos;
                        _playAnimation = false;
                    }
                }
            }
        }
    }

    public void MoveDoor()
    {
        if (_moving && !GameController.instance.IsGamePaused())
        {
            if (Vector2.Distance(new Vector2(_door.transform.position.x, _door.transform.position.y), _target) > 0.01)
            {
                Vector3 p = _door.transform.position;
                p = Vector2.SmoothDamp(_door.transform.position, _target, ref _velocity, _speed / 4.0f, _speed, Time.deltaTime);
                _door.transform.position = new Vector3(p.x, p.y, 0f);
            }
            else
            {
                _door.transform.position = new Vector3(_target.x, _target.y, 0);
                _moving = false;
            }
        }
    }

    public void SetLevel(Level lv)
    {
        _level = lv;
    }

    public void SetDoor(GameObject door)
    {
        _door = door;
    }

    public int GetLeverDirection()
    {
        return _targetIndex;
    }

    public void ChangeLeverDirection()
    {
        if (_targetIndex == 0)
        {
            _target = _leverDoorData.p1;
            _targetIndex = 1;
            _moving = true;
            _animationIndex = 0;
            _frameTimer = 0f;
        }
        else if (_targetIndex == 1)
        {
            _target = _leverDoorData.p0;
            _targetIndex = 0;
            _moving = true;
            _animationIndex = _animFrames.Length - 1;
            _frameTimer = 0f;
        }

        _playAnimation = true;
    }

    public override void SetEnemyData(BaseEnemyData data)
    {
        _leverDoorData = (LeverDoorData)data;
        _target = _leverDoorData.p0;
    }

    public void SetAnimationSprites()
    {
        string texName = "Blocks/Lever/Lever_1_Frame_";

        for (int i = 0; i < _animFrames.Length; ++i)
        {
            Texture2D tex = (Texture2D)_level.GetLevelController().GetGameController().
            GetTextureController().GetTexture(texName + (i + 2).ToString());

            float texSize = _level.GetLevelController().GetGameController().
                        GetTextureController().GetTextureSize(texName + i);

            _animFrames[i] = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f), texSize);
        }
    }

    public void SetTooltip(GameObject tool)
    {
        _tooltip = tool.GetComponent<SpriteRenderer>();
        _tooltip.color = new Color(1f, 1f, 1f, 0f);
    }

    public void SetSprites(string tex1, string tex2)
    {
        Texture2D tex = (Texture2D)_level.GetLevelController().GetGameController().
            GetTextureController().GetTexture(tex1);
        float texSize = _level.GetLevelController().GetGameController().
                    GetTextureController().GetTextureSize(tex1);
        _leftPos = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f), texSize);

        tex = (Texture2D)_level.GetLevelController().GetGameController().
            GetTextureController().GetTexture(tex2);
        texSize = _level.GetLevelController().GetGameController().
                    GetTextureController().GetTextureSize(tex2);
        _rightPos = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f), texSize);
    }
}
