using UnityEngine;
using System.Collections;

public class ButtonVibration : MonoBehaviour {

    public bool _isVibrating = true;
    public bool _isSmoothing = false;
    private Vector3 _pivotPos;
    private Vector3 _pivotPosSmoothing;

    private Vector3 _randomSmoothPos;
    private Vector3 _velocity;
    private float _speed = 3f;

	// Use this for initialization
	void Start () {
        _pivotPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (_isVibrating)
        {
            transform.position = _pivotPos + Vector3.right * Random.Range(-5f, 5f) + Vector3.up * Random.Range(-5f, 5f);
        }

        if (_isSmoothing)
        {
            if (Vector3.Distance(transform.position, _randomSmoothPos) <= 0.1f)
            {
                _randomSmoothPos = _pivotPosSmoothing + Vector3.right * Random.Range(-10f, 10f) + Vector3.up * Random.Range(-10f, 10f);
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, _randomSmoothPos, ref _velocity, _speed / 3f, _speed, Time.deltaTime);
            }
            //transform.localPosition = _pivotPosSmoothing + Vector3.right * Random.Range(-5f, 5f) + Vector3.up * Random.Range(-5f, 5f);
        }
	}

    public void StartSmoothing()
    {
        _isSmoothing = true;
        _isVibrating = false;
        _pivotPosSmoothing = transform.position;
        _randomSmoothPos = _pivotPosSmoothing + Vector3.right * Random.Range(-10f, 10f) + Vector3.up * Random.Range(-10f, 10f);
    }

    
}
