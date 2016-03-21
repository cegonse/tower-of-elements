using UnityEngine;
using System.Collections;

public class TransformSinTweener : MonoBehaviour {

    private float _angle = 0f;
    private float _altura = 1f;
    private float _angleSpeed = 0.5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(_angle) * _altura, transform.position.z);

        _angle += _angleSpeed;
	}

    public void SetParams(float init_angle, float height, float angle_speed)
    {
        _angle = init_angle;
        _altura = height;
        _angleSpeed = angle_speed;
    }
}
