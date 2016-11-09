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
    private UnityEngine.UI.Image _image;
    private float _alphaDelta;

    public void StartParticle(Vector3 direction, bool rock = false, float decay = 0.8f, float speed = 3f, bool star = false)
    {
        float offset = Random.Range(-0.8f, 0.8f);

        if (rock)
        {
            offset = 0f;
        }

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
        _speed = Random.Range(1f, speed);

        if (star)
        {
            _speed = speed;
        }

        _decayTime = Random.Range(0.5f, decay);

        if (star)
        {
            _decayTime = decay;
        }

        _rend = GetComponent<SpriteRenderer>();

        if (_rend == null)
        {
            _image = GetComponent<UnityEngine.UI.Image>();
        }

        _alphaDelta = 1f / _decayTime;

        _state = State.Moving;
    }

	void Update ()
    {
	    if (_state == State.Moving)
        {
            if (_rend != null)
            {
                Color c = _rend.color;
                c.a -= _alphaDelta * Time.deltaTime * _speed;
                _rend.color = c;
            }
            else
            {
                Color c = _image.color;
                c.a -= _alphaDelta * Time.deltaTime;
                _image.color = c;
            }

            transform.position += _direction * _speed * Time.deltaTime;
            _timer += Time.deltaTime;

            if (_timer > _decayTime)
            {
                GameObject.Destroy(gameObject);
            }
        }
	}
}
