using UnityEngine;
using System.Collections;

public class LeverDoorData : BaseEnemyData
{
    public Vector2 p0;
    public Vector2 p1;
}

public class Lever : EnemyBase {

    private GameObject _door;
    private Sprite _left, _right;

    private Sprite _leftPos, _rightPos;
    

    //Door attributes
    private Vector2 _target;
    private int _targetIndex = 0;
    private LeverDoorData _leverDoorData;
    private bool _moving = false;

	// Use this for initialization
	void Start () {

        if (_door)
        {
            _door.transform.position = new Vector3(_target.x, _target.y, 0f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        MoveDoor();
	}

    public void MoveDoor()
    {
        if (_moving)
        {
            if (Vector2.Distance(new Vector2(_door.transform.position.x, _door.transform.position.y), _target) > 0.1)
            {
                Vector3 p = _door.transform.position;
                p = Vector2.SmoothDamp(_door.transform.position, _target, ref _velocity, _speed / 4, _speed, Time.deltaTime);
                _door.transform.position = new Vector3(p.x, p.y, 0f);
            }
            else
            {
                _door.transform.position = new Vector3(_target.x, _target.y, 0);
                _moving = false;
            }
        }
    }

    public void SetLevel (Level lv)
    {
        _level = lv;
    }

    public void SetDoor(GameObject door)
    {
        _door = door;
    }

    public void ChangeLeverDirection()
    {
        if (_targetIndex == 0)
        {
            _target = _leverDoorData.p1;
            _targetIndex = 1;
            _moving = true;
            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sprite = _rightPos;
        }
        else if (_targetIndex == 1)
        {
            _target = _leverDoorData.p0;
            _targetIndex = 0;
            _moving = true;
            SpriteRenderer rend = gameObject.GetComponent<SpriteRenderer>();
            rend.sprite = _leftPos;
        }
    }

    public override void SetEnemyData(BaseEnemyData data)
    {
        _leverDoorData = (LeverDoorData)data;
        _target = _leverDoorData.p0;
        //transform.localScale = new Vector3(transform.localScale.x * -1, 1f, 1f);

    }

    public void SetSprites (string tex1, string tex2)
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
