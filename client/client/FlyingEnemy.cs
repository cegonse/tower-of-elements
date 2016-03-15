using UnityEngine;
using System.Collections;

public class FlyingEnemyData : BaseEnemyData
{
	public Vector2 p0;
	public Vector2 pf;
};

public class FlyingEnemy : EnemyBase
{
	protected BaseEnemyData _data;
	
	public void SetData(BaseEnemyData data)
	{
		_data = data;
	}
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
