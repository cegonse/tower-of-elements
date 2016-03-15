using UnityEngine;
using System.Collections;

public class WalkerEnemyData : BaseEnemyData
{
	public Vector2 p0;
	public Vector2 p1;
}


public class EnemyWalker: EnemyBase {

   private WalkerEnemyData _walkerData;
	
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
		// Check if we are close enough to the target
		if (Vector2.Distance(_target, _position) < 0.5f)
		{
			// Change the target to the other point
			if (_targetIndex == 0)
			{
				_target = _walkerData.p1;
				_targetIndex = 1;
			}
			else
			{
				_target = _walkerData.p0;
				_targetIndex = 0;
			}
		}
    }

    private void MovingEnemy()
    {
		_position = Vector2.SmoothDamp(_position, _target, ref _velocity, _speed / 4f, _speed, Time.deltaTime);
		transform.position = new Vector3(_position.x, _position.y, 0f);
    }

    /*private void MovingEnemy()
    {
        Vector3 p = transform.position;
        p.x += Time.deltaTime * _velocity.x;
        p.y += Time.deltaTime * _velocity.y;
        transform.position = p;
    }*/
	
	public override void SetEnemyData (BaseEnemyData data)
	{
		_walkerData = (WalkerEnemyData) data;
		_target = _walkerData.p0;
	}
	
}
