using UnityEngine;
using System.Collections;

public class WinTimerRollController : MonoBehaviour
{
    private enum TimerState
    {
        Idle,
        Rolling
    }

    private TimerState _state = TimerState.Idle;
    private float _timer = 0f;
    private UnityEngine.UI.Text _label;

    void Start()
    {
        _label = GetComponent<UnityEngine.UI.Text>();
    }

    public void StartRoll()
    {
        _state = TimerState.Rolling;
        _timer = 0f;
    }

    public void ShowTime(float time)
    {
        _state = TimerState.Idle;

        int min = (int)(time / 60f);
        int s1 = (int)(time % 60 / 10);
        int s2 = (int)(time % 60 % 10);

        _label.text = min + ":" + s1 + s2;
    }
	
	void Update ()
    {
        if (_state == TimerState.Rolling)
        {
            _timer += Time.deltaTime;

            if (_timer > 0.05f)
            {
                int min = Random.Range(0, 9);
                int s1 = Random.Range(0, 9);
                int s2 = Random.Range(0, 9);

                _label.text = min + ":" + s1 + s2;
                _timer = 0f;
            }
        }
	}
}
