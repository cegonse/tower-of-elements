using UnityEngine;
using System.Collections;

public class DustParticle : MonoBehaviour
{
    private enum State
    {
        Waiting,
        Moving
    }

    private State _state = State.Waiting;
    private float _decayTime;
    private float _speed;
    private Vector3 _direction;
    private float _timer = 0f;
    private SpriteRenderer _rend;
    private float _alphaDelta;

    public void StartParticle(Vector3 direction)
    {
        float offset = Random.Range(-0.8f, 0.8f);
        _direction = direction;

        if (direction == Vector3.up)
        {
            _direction.x += offset;
        }
        else
        {
            _direction.y += offset;
        }

        _direction.Normalize();
        _speed = Random.Range(1f, 3f);
        _decayTime = Random.Range(0.5f, 0.8f);
        _rend = GetComponent<SpriteRenderer>();
        _alphaDelta = 1f / _decayTime;

        _state = State.Moving;
    }

	void Update ()
    {
	    if (_state == State.Moving)
        {
            Color c = _rend.color;
            c.a -= _alphaDelta * Time.deltaTime * _speed;
            _rend.color = c;

            transform.position += _direction * _speed * Time.deltaTime;
            _timer += Time.deltaTime;

            if (_timer > _decayTime)
            {
                GameObject.Destroy(gameObject);
            }
        }
	}
}
