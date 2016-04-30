using UnityEngine;
using System.Collections.Generic;


public enum BlockType : int
{
	Rock,
    Ice,
    Crate
}

public class Block : MonoBehaviour
{
    //Level

    private Level _activeLevel;
    private string _textureRoute;

    //Speed
	private float _speed = 3f;

    private const float _minAccSpeed = 1f;
    private float _accSpeed = _minAccSpeed;
    private const float _incrSpeed = 0.1f;

	private bool _isMovable = false;
	private BlockType _type;
    private State _state = State.Normal;
    //velocity
    private Vector2 _velocity;

    //Length
    private float _length = 1;
    private bool _vertical = false;
    private bool _isPlatform = false;
    //Rays

    private Ray2D _downRay;
    private Ray2D _rightRay;
    private Ray2D _leftRay;
	
	void Start()
	{
        //Down Ray
        _downRay = new Ray2D();
        _downRay.direction = Vector2.right;
        //Right Ray
        _rightRay = new Ray2D();
        _rightRay.direction = Vector2.right;
        //Left Ray
        _leftRay = new Ray2D();
        _leftRay.direction = Vector2.left;
        //Velocity
        _velocity = new Vector2(0, -_speed);
    }
	
	void Update()
	{
        if (!_activeLevel.GetLevelController().GetGameController().IsGamePaused())
        {
            if (_isMovable)
            {

                CheckMovingCollisions();

                AdjustVerticalSpeed();

                MovingBlock();

            }
        }
	}
    
    private void CheckMovingCollisions()
    {
        //*************************************************
        //Check if there is a block under the current block
        //*************************************************

        float ymax = -Mathf.Infinity;
        GameObject goHitDown = null;
        GameObject blockDown = null;
        RaycastHit2D[] hit_down;

        _downRay.origin = transform.position + new Vector3(0, -0.51f, 0);
        hit_down = Physics2D.RaycastAll(_downRay.origin, _downRay.direction, _length - 1f);
        //Go through all the colliders of the raycast
        for (int i_down = 0; i_down < hit_down.Length; i_down++)
        {
            goHitDown = hit_down[i_down].collider.gameObject;

            Block blockComponent = goHitDown.GetComponent<Block>();
            //Check if it is a block and if it is over the other blocks.
            //If it is over the other blocks means that it is the block
            // that we must save to adjust our position.y
            if (blockComponent != null && ymax <= goHitDown.transform.position.y)
            {
                //Save the maximum Y
                ymax = goHitDown.transform.position.y;
                //Save the block is down
                blockDown = goHitDown;
            }

        }

        //*************************************************
        //  Go ahead if there is a block under this block
        //*************************************************
        if (blockDown != null)
        {
            _state = State.Grounded;
                    
            //Ajustar posicion del bloque
            Vector3 pDown = transform.position;
            pDown.y = blockDown.transform.position.y + 1;
            transform.position = pDown;
                   
            //Left-Right
            if (_velocity.x > 0)
            {
                _rightRay.origin = transform.position + new Vector3(_length-0.49f, 0f, 0f);
                        
                RaycastHit2D hit_right = Physics2D.Raycast(_rightRay.origin, _rightRay.direction, 0.01f);

                if (hit_right.collider != null)
                {
                    GameObject goHitRight = hit_right.collider.gameObject;
                    if (goHitRight.GetComponent<Block>() != null)
                    {
                        _velocity.x = 0;
                        //Ajustar posicion del bloque
                        Vector3 pRight = transform.position;
                        pRight.x = goHitRight.transform.position.x - _length;
                        transform.position = pRight;
                    }
                }
                else
                {
                    _velocity.x = _speed;
                }
            }
            else if(_velocity.x < 0)
            {
                _leftRay.origin = transform.position + new Vector3(-0.51f, 0f, 0f);
                RaycastHit2D hit_left = Physics2D.Raycast(_leftRay.origin, _leftRay.direction, 0.01f);

                if (hit_left.collider != null)
                {
                    GameObject goHitLeft = hit_left.collider.gameObject;
                    if (goHitLeft.GetComponent<Block>() != null)
                    {
                        _velocity.x = 0;
                        //Ajustar posicion del bloque
                        Vector3 pLeft = transform.position;
                        pLeft.x = goHitLeft.transform.position.x + goHitLeft.GetComponent<Block>().GetLength();
                        transform.position = pLeft;
                    }
                }
                else
                {
                    _velocity.x = -_speed;
                }
            }
        }//Down if 1
        else
        {
            _state = State.Falling;
        }
        
    }

    private void AdjustVerticalSpeed()
    {
        
        switch(_state)
        {
            case State.Grounded:
                
                _velocity.y = 0;
                _accSpeed = _minAccSpeed;
                break;
            
            case State.Falling:
                
                _velocity.y = -_speed;
                _velocity.x = 0;
                _accSpeed += _incrSpeed;
                break;
        }
        
    }
    
    private void MovingBlock()
    {
        Vector3 p = transform.position;
        
        switch(_state)
        {
            case State.Grounded:
            
                p.x += Time.deltaTime * _velocity.x;
                //p.y += Time.deltaTime * _velocity.y * _accSpeed;
                break;
            
            case State.Falling:
                p.x = Mathf.Round(p.x);
                p.y += Time.deltaTime * _velocity.y * _accSpeed;
                break;
        }
        
		transform.position = p;
    }
	
	public bool IsMovable()
	{
		return _isMovable;
	}

    public void SetActiveLevel(Level lvl)
    {
        _activeLevel = lvl;
    }
	
	public void SetType(BlockType type)
	{
		_type = type;
		
		if (type == BlockType.Ice)
		{
			SetMovable(true);
		}
	}
	
	public BlockType GetBlockType()
	{
		return _type;
	}
	
	public void SetMovable(bool value)
	{
		_isMovable = value;
	}

    public void SetLength(float l)
    {
        _length = l;
        
    }

    public float GetLength()
    {
        return _length;
    }
	
	public void Kick(Direction direction)
	{
        if (_isMovable)
        {
            switch (direction)
            {
                case Direction.Left: _velocity.x = -_speed;
                    break;
                case Direction.Right: _velocity.x = _speed;
                    break;
            }
        }
	}
	
	public void Break()
	{
		
	}

    public void SetTextureRoute(string tex_route)
    {
        _textureRoute = tex_route;
    }

    public string GetTextureRoute()
    {
        return _textureRoute;
    }

    public void Destroy(Direction dir)
    {
        if (_length > 1)
        {

            Transform[] child_transforms = gameObject.GetComponentsInChildren<Transform>();
            float x_max = child_transforms[child_transforms.Length - 1].position.x;
            GameObject go_to_delete = child_transforms[child_transforms.Length - 1].gameObject;
            GameObject go_to_change = child_transforms[child_transforms.Length - 1].gameObject;

            if (go_to_delete == gameObject)
            {
                go_to_delete = child_transforms[child_transforms.Length - 2].gameObject;
            }
            if (go_to_change == gameObject)
            {
                go_to_change = child_transforms[child_transforms.Length - 2].gameObject;
            }

            foreach (Transform trans in child_transforms)
            {
                if (trans.gameObject != gameObject && trans.position.x > x_max)
                {
                    x_max = trans.position.x;
                    go_to_delete = trans.gameObject;
                }
            }

            foreach (Transform trans in child_transforms)
            {
                if (trans.gameObject != gameObject && trans.position.x == x_max - 1)
                {
                    go_to_change = trans.gameObject;
                    break;
                }
            }
            GameObject.Destroy(go_to_delete);
            //_activeLevel.RemoveEntity(go_to_delete2.name);
            SetLength(_length - 1);

            SpriteRenderer rend = go_to_change.GetComponent<SpriteRenderer>();
            Sprite spr = go_to_change.GetComponent<Sprite>();

            //Get the main identificator on the string --> i.e. "Ice","RBasaltA", "Crate"
            string tex_route = _textureRoute.Split('_')[0];
            //Create right corner

            //Get the texture
            Texture2D tex = null;
            if (_length != 1)
            {
                tex_route = tex_route + "_15";
            }
            else
            {
                tex_route = _textureRoute;
            }

            tex = (Texture2D)_activeLevel.GetLevelController().GetGameController().
                        GetTextureController().GetTexture(tex_route);

            spr = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                new Vector2(0.5f, 0.5f), 256f);
            rend.sprite = spr;
            //Adding value to sorting layer
            rend.sortingOrder = 100;
            //Adding the SpriteAnimator component
            if (_activeLevel.GetLevelController().GetGameController().GetTextureController().GetAnimation(tex_route + "_Anim") != null)
            {
                Destroy(go_to_change.GetComponent<SpriteAnimator>());
                SpriteAnimator sa = go_to_change.AddComponent<SpriteAnimator>();

                if (sa == null)
                {
                    sa = go_to_change.AddComponent<SpriteAnimator>();
                }

                sa.AddAnimation("STANDING",_activeLevel.GetLevelController().GetGameController().GetTextureController().GetAnimation(tex_route + "_Anim"));
            }

            //Change the parent collider
            if (dir == Direction.Right)
            {
                transform.position = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
            }

            BoxCollider2D boxColl2 = gameObject.GetComponent<BoxCollider2D>();
            boxColl2.size = new Vector2(_length, 1f);
            boxColl2.offset = new Vector2((_length / 2) - 0.5f, 0f);

        }
        else
        {
            _activeLevel.RemoveEntity(gameObject.name);
        }
    }

    public void SetVertical(bool v)
    {
        _vertical = v;
    }

    public bool IsVertical()
    {
        return _vertical;
    }

    public void SetPlatform(bool v)
    {
        _isPlatform = v;
    }

    public bool IsPlatform()
    {
        return _isPlatform;
    }
}