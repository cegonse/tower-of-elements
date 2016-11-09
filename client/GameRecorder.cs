using UnityEngine;
using System.Collections.Generic;

public class GameRecorder : MonoBehaviour
{
    public string _server = "http://cesar.jumbledevs.net:8011/";

    public enum GameCommand
    {
        MoveRight,
        MoveLeft,
        StopMove,
        Ice,
        Fire,
        Earth,
        Wind,
        Eye,
        Reset,
        Death
    }

    private enum State
    {
        Idle,
        Uploading
    }

    public struct GameEvent
    {
        public float Time;
        public GameCommand Command;

        public GameEvent(float t, GameCommand c)
        {
            Time = t;
            Command = c;
        }
    }

    private WWW _socket;
    private List<GameEvent> _events;
    private State _state = State.Idle;
    private string _currentLevel = "";

    private static GameRecorder _instance;

    public void OnGameEvent(GameEvent ev)
    {
        _events.Add(ev);
    }

    public static GameRecorder GetInstance()
    {
        return _instance;
    }

    public void SetLevel(string lv)
    {
        _currentLevel = lv;
    }

    public void Upload()
    {
        if (_state == State.Idle)
        {
            JSONObject json = new JSONObject(JSONObject.Type.OBJECT);
            JSONObject jsonList = new JSONObject(JSONObject.Type.ARRAY);

            for (int i = 0; i < _events.Count; ++i)
            {
                JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);

                obj.AddField("command", _events[i].Command.ToString());
                obj.AddField("time", _events[i].Time);

                jsonList.Add(obj);
            }

            json.AddField("events", jsonList);
            json.AddField("level", _currentLevel);

            _socket = new WWW(_server, System.Text.Encoding.ASCII.GetBytes(json.Print()));
            _state = State.Uploading;
        }
    }

	void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        _events = new List<GameEvent>();
	}
	
	void Update()
    {
	    if (_state == State.Uploading)
        {
            if (_socket.isDone)
            {
                _events.Clear();
                _state = State.Idle;
                Debug.Log(_socket.error);
            }
        }
	}
}
