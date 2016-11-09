using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private GameObject _player;

    private float _boundX;
    private float _boundY;
    private float _boundH;
    private float _boundW;

    private bool _paused;

    private Vector3 _cameraVelocity;
    private float _cameraDampeningTime = 0.166f;

    public void SetPlayer(GameObject pl)
    {
        _player = pl;
    }

    public void SetLevelBounds(float x, float y, float w, float h)
    {
        _boundH = h;
        _boundW = w;
        _boundX = x;
        _boundY = y;
    }

    public void OnPause()
    {
        _paused = !_paused;
    }


    /*void OnPreCull()
    {
        if (!_paused && _player != null)
        {
            Vector3 camPos = transform.position;
            Vector3 offset = _player.transform.position - Vector3.forward;

            float height = 3f;
            float width = height * ((float)Screen.width / (float)Screen.height);
            float maxH = _boundH - height;
            float minH = _boundY + height;
            float maxW = _boundW - width;
            float minW = _boundX + width;

            if (offset.y > maxH)
            {
                offset.y = maxH;
            }
            else if (offset.y < minH)
            {
                offset.y = minH;
            }

            if (offset.x < minW)
            {
                offset.x = minW;
            }
            else if (offset.x > maxW)
            {
                offset.x = maxW;
            }

            camPos = Vector3.SmoothDamp(camPos, offset, ref _cameraVelocity, _cameraDampeningTime);

            if (_doCameraScreenShake)
            {
                camPos += Random.onUnitSphere * _screenShakeIntensity;
                _cameraScreenShakeTimer += Time.deltaTime;

                if (_cameraScreenShakeTimer > _cameraScreenShakeMaxTime)
                {
                    _doCameraScreenShake = false;
                }
            }

            transform.position = camPos;
        }
    }*/
}
