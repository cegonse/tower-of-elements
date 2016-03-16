using UnityEngine;
using System.Collections;

public class LeverDoorData : BaseEnemyData
{
    public Vector2 p0;
    public Vector2 p1;
}

public class LeverDoor : EnemyBase {

    private Lever _lever;
    private Vector2 _target;
    private int _targetIndex;
    private LeverDoorData _leverDoorData;

	// Use this for initialization
	void Start () {

        _targetIndex = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
        Move();

	}

    public void Move()
    {
        Vector3 p = transform.position;
        p = Vector2.SmoothDamp(transform.position, _target, ref _velocity, _speed / 4, _speed, Time.deltaTime);
        transform.position = new Vector3(p.x, p.y, 0f);
    }

    public void ChangeTargetDirection()
    {
        if(_targetIndex == 0)
        {
            _target = _leverDoorData.p1;
            _targetIndex = 1;
        }
        else if (_targetIndex == 1)
        {
            _target = _leverDoorData.p0;
            _targetIndex = 0;
        }
    }
}
