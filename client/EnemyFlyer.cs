using UnityEngine;
using System.Collections;


public class FlyerEnemyData : BaseEnemyData
{
	public Vector2 p0;
	public Vector2 pf;
}


public class EnemyFlyer : EnemyBase
{
	private FlyerEnemyData _flyerData;
	
	private Vector2 _position;
	private Vector2 _target;
	private int _targetIndex = 0;

    new void Start()
    {
        //Let the base.Start() method for a properly initialization
        base.Start();
    }

    void Update()
    {
        CheckPlayerCollisions();
        AdjustVelocity();
        MovingEnemy();
    }

    private void AdjustVelocity()
    {
        float dist = Vector2.Distance(_target, _position);
		// Check if we are close enough to the target to change animation
        if (dist < 0.7f)
        {
            // Check if we are close enough to the target to change target
            if (dist < 0.5f)
            {
                // Change the target to the other point
                if (_targetIndex == 0)
                {
                    _target = _flyerData.pf;
                    _targetIndex = 1;


                }
                else
                {
                    _target = _flyerData.p0;
                    _targetIndex = 0;
                }

                
            }

            if (_state == EnemyState.WALKING)
            {
                SpriteAnimator sa = GetComponent<SpriteAnimator>();
                sa.SetActiveAnimation("TURNING");
                _state = EnemyState.TURNING;
            }
            
        }
		
    }

    private void MovingEnemy()
    {

        _position = Vector2.SmoothDamp(_position, _target, ref _velocity, _speed / 3f, _speed, Time.deltaTime);
        transform.position = new Vector3(_position.x, _position.y, 0f);

        switch (_state)
        {
            case EnemyState.WALKING:
                
                SpriteAnimator saW = GetComponent<SpriteAnimator>();
                saW.SetAnimationTimerScaler(Mathf.Abs(_velocity.magnitude));
                break;

            case EnemyState.TURNING:
                SpriteAnimator sa = GetComponent<SpriteAnimator>();
                if (sa.GetAnimationIndex() == 7)
                {
                    _state = EnemyState.WALKING;
                    sa.SetActiveAnimation("WALKING");
                    transform.localScale = new Vector3(transform.localScale.x*-1, 1f, 1f);
                }
                break;
        }
		
    }
	
	public override void SetEnemyData(BaseEnemyData data)
	{
		// Start moving towards P0
		_flyerData = (FlyerEnemyData) data;
        _position = transform.position;
		_target = _flyerData.p0;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1f, 1f);
        
	}
}
