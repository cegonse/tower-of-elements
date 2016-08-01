using UnityEngine;
using System.Collections;

public class AudioController
{
    private GameController _game;
    private GameObject[] _channels;
    private bool[] _pausedChannels;
    private GameObject _followGo = null;

    public AudioController(GameController gc, int channels = 1)
    {
        _game = gc;
        _channels = new GameObject[channels];
        _pausedChannels = new bool[channels];

        for (int i = 0; i < channels; i++)
        {
            _channels[i] = new GameObject();
            _channels[i].name = "Audio channel " + i;
            _channels[i].AddComponent<AudioSource>();
            _pausedChannels[i] = false;
        }
    }

    public void SetFollowGameObject(GameObject go)
    {
        _followGo = go;
    }

    public void OnUpdate()
    {
        if (_followGo != null)
        {
            foreach (GameObject go in _channels)
            {
                go.transform.position = _followGo.transform.position;
            }
        }
    }

    public int GetChannelCount()
    {
        return _channels.Length;
    }

    public void SetLoopChannel(int ch, bool lp)
    {
        _channels[ch].GetComponent<AudioSource>().loop = lp;
    }

    public void SetClipToChannel(int ch, AudioClip clip)
    {
        _channels[ch].GetComponent<AudioSource>().clip = clip;
    }

    public void SetChannelVolume(int ch, float vol)
    {
        _channels[ch].GetComponent<AudioSource>().volume = vol;
    }

    public float GetChannelVolume(int ch)
    {
        return _channels[ch].GetComponent<AudioSource>().volume;
    }

    public void PlayChannel(int ch)
    {
        if (!_channels[ch].GetComponent<AudioSource>().isPlaying)
        {
            _channels[ch].GetComponent<AudioSource>().Play();
        }
    }

    public void StopChannel(int ch, bool all = false)
    {
        if (all)
        {
            for (int i = ch; i < _channels.Length; i++)
            {
                _channels[i].GetComponent<AudioSource>().Stop();
            }
        }
        else
        {
            _channels[ch].GetComponent<AudioSource>().Stop();
        }
    }

    public void PauseChannel(int ch)
    {
        if (_pausedChannels[ch])
        {
            _channels[ch].GetComponent<AudioSource>().UnPause();
            _pausedChannels[ch] = false;
        }
        else
        {
            _channels[ch].GetComponent<AudioSource>().Pause();
            _pausedChannels[ch] = true;
        }
    }
}
