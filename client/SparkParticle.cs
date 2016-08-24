using UnityEngine;
using System.Collections;

public class SparkParticle : MonoBehaviour
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
    private float _accel = 0.1f;

    public void StartParticle(Vector3 direction)
    {
        _direction = direction * 0.3f;
        _direction.y = Random.Range(0.5f, 1f);
        _direction.Normalize();

        _speed = Random.Range(2f, 5f);
        _decayTime = Random.Range(0.8f, 1.2f);
        _rend = GetComponent<SpriteRenderer>();
        _alphaDelta = 1f / _decayTime;
        transform.localRotation = Quaternion.AngleAxis(Random.Range(0f, 359f), Vector3.forward);

        _state = State.Moving;
    }

    void Update()
    {
        if (_state == State.Moving)
        {
            Color c = _rend.color;
            c.a -= _alphaDelta * Time.deltaTime * _speed;
            _rend.color = c;
            _accel += Time.deltaTime * 2f;

            transform.position += _direction * _speed * Time.deltaTime;
            transform.position += Vector3.down * Time.deltaTime * _accel;
            _timer += Time.deltaTime;

            if (_timer > _decayTime)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
