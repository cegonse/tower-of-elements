using UnityEngine;
using System.Collections.Generic;

public enum EnemyType : int
{
	Flyer,
    Walker,
	Roamer,
    Lever
}
public enum EnemyState : int
{
    WALKING,
    TURNING
}

public class BaseEnemyData
{
    public float speed = 3f;
    public int hp = 1;    
}

public class EnemyBase : MonoBehaviour
{
	private EnemyType _type;
	protected Level _level;
    private BaseEnemyData _data;

    //State
    protected EnemyState _state = EnemyState.WALKING;

    //Speed
    protected float _speed = 3f;

    //velocity
    protected Vector2 _velocity = new Vector2(0f, 0f);
	
	private Ray2D _downRay;
    private Ray2D _upRay;
    private Ray2D _rightRay;
    private Ray2D _leftRay;
	
	protected void Start()
	{
        _downRay = new Ray2D();
        _upRay = new Ray2D();
        _rightRay = new Ray2D();
        _leftRay = new Ray2D();

        _downRay.direction = Vector2.down;
        _leftRay.direction = Vector2.left;
        _rightRay.direction = Vector2.right;
        _upRay.direction = Vector2.up;
	}

    protected void EnemyStart()
    {
        _downRay = new Ray2D();
        _upRay = new Ray2D();
        _rightRay = new Ray2D();
        _leftRay = new Ray2D();

        _downRay.direction = Vector2.left;
        _leftRay.direction = Vector2.up;
        _rightRay.direction = Vector2.down;
        _upRay.direction = Vector2.right;
    }
	
	void Update ()
	{
        CheckPlayerCollisions();
        MovingEnemy();
	}


    public void CheckPlayerCollisions()
    {
        _downRay.origin = transform.position + new Vector3(0.46f, -0.46f, 0f);
        _upRay.origin = transform.position + new Vector3(-0.46f, 0.46f, 0f);
        _leftRay.origin = transform.position + new Vector3(-0.46f, -0.46f, 0f);
        _rightRay.origin = transform.position + new Vector3(0.46f, 0.46f, 0f);

        RaycastHit2D hit_right = Physics2D.Raycast(_rightRay.origin, _rightRay.direction, 0.9f);
        RaycastHit2D hit_left = Physics2D.Raycast(_leftRay.origin, _leftRay.direction, 0.9f);
        RaycastHit2D hit_down = Physics2D.Raycast(_downRay.origin, _downRay.direction, 0.9f);
        RaycastHit2D hit_up = Physics2D.Raycast(_upRay.origin, _upRay.direction, 0.9f);

        bool killThaPlayer = false;

        if (hit_right.collider != null)
        {
            GameObject goHitRight = hit_right.collider.gameObject;
            if (goHitRight.GetComponent<Player>() != null)
            {
                if (goHitRight.transform.position.x - transform.position.x < 0.5)
                {
                    killThaPlayer = true;
                }   
            }
        }

        if (hit_left.collider != null)
        {
            GameObject goHitLeft = hit_left.collider.gameObject;
            if (goHitLeft.GetComponent<Player>() != null)
            {
                if (transform.position.x - goHitLeft.transform.position.x < 0.5)
                {
                    killThaPlayer = true;
                } 
            }
        }

        if (hit_up.collider != null)
        {
            GameObject goHitUp = hit_up.collider.gameObject;
            if (goHitUp.GetComponent<Player>() != null)
            {
                if (goHitUp.transform.position.y - goHitUp.transform.position.y < 0.5)
                {
                    killThaPlayer = true;
                } 
            }
        }

        if (hit_down.collider != null)
        {
            GameObject goHitDown = hit_down.collider.gameObject;
            if (goHitDown.GetComponent<Player>() != null)
            {
                if (transform.position.y - goHitDown.transform.position.y < 0.5)
                {
                    killThaPlayer = true;
                } 
            }
        }

        if (killThaPlayer)
        {
            GameObject go = _level.GetEntity("player");
            Player player = go.GetComponent<Player>();
            if (player)
            {
                player.DestroyPlayer();
                Debug.Log("Se llama a DestroyPlayer");
            }
        }
    }

    public virtual void MovingEnemy()
    {
    }
	
	public EnemyType GetEnemyType()
	{
		return _type;
	}
	
	public void SetType(EnemyType type)
	{
		_type = type;		
	}
	
	public void SetLevel(Level lv)
	{
		_level = lv;
	}
    
    public virtual void SetEnemyData (BaseEnemyData data)
	{
	}
}