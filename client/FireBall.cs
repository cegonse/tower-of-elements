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

    private Texture2D[] _fireParticle;

    private GameController _gameController;


    void Start()
	{
        //Right Ray
        _rightRay = new Ray2D();
        
        //Left Ray
        _leftRay = new Ray2D();

        

    }
	
    public void SetGameController(GameController game)
    {
        _gameController = game;
        _fireParticle = new Texture2D[2];
        _fireParticle[0] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_1");
        _fireParticle[1] = (Texture2D)_gameController.GetTextureController().GetTexture("Particles/ParticleFire/ParticleFire_2");
    }
    
	void Update()
	{
        if (!_activeLevel.GetLevelController().GetGameController().IsGamePaused())
        {
            CheckMovingCollisions();
            MovingFireBall();

            if (_gameController != null)
            {
                if (_actionDirection == Direction.Left)
                {
                    CreateFireParticles(3, Vector3.left);
                }
                else
                {
                    CreateFireParticles(3, Vector3.right);
                }
            }
        }
	}

    private void CreateFireParticles(int count, Vector3 dir)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject();
            go.name = "Spark Particle";

            if (dir == Vector3.left)
            {
                Vector3 pos = transform.position;
                go.transform.position = pos;
            }
            else if (dir == Vector3.right)
            {
                Vector3 pos = transform.position;
                go.transform.position = pos;
            }

            int index = Random.Range(0, _fireParticle.Length);
            SpriteRenderer rend = go.AddComponent<SpriteRenderer>();
            Sprite spr = Sprite.Create(_fireParticle[index], new Rect(0, 0, _fireParticle[index].width, _fireParticle[index].height),
                        new Vector2(0.5f, 0.5f), 512f);
            rend.sprite = spr;
            rend.sortingOrder = 109 - i;

            SparkParticle sp = go.AddComponent<SparkParticle>();
            sp.StartParticle(dir, null, true);
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
		else if (_actionDirection == Direction.Left)
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
					if (goHitBlock.GetBlockType() == BlockType.Ice)
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
	
	public void SetActiveLevel(Level lv)
    {
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