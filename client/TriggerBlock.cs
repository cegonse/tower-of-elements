using UnityEngine;
using System.Collections;

public class TriggerBlock : MonoBehaviour
{
    public enum TriggerBlockType
    {
        Wind,
        Ice,
        Fire,
        Earth
    }

    public enum TriggerState
    {
        Idle,
        ShowFlash,
        FadeFlash,
        ShowText,
        Finished
    }

    private TriggerBlockType _type;
    private TriggerState _state;
    
    private RaycastHit2D _hit;
    private Vector2 _lineOrigin;
    private Vector2 _lineEnd;
    private GameObject _label;
    private bool _triggered = false;
    private string _triggerText = "test text";
    private UnityEngine.UI.Image _whiteFlash;
    private GameController _game;

    public void SetTriggerData(TriggerBlockType type, GameObject label)
    {
        _type = type;
        _lineOrigin = new Vector2(transform.position.x - 0.4f, transform.position.y);
        _lineEnd = new Vector2(transform.position.x + 0.4f, transform.position.y);
        _label = label;
    }

	void Update ()
    {
        if (!_triggered)
        {
            _hit = Physics2D.Linecast(_lineOrigin, _lineEnd);

            if (_hit.collider.gameObject != null)
            {
                if (_hit.collider.gameObject.GetComponent<Player>() != null)
                {
                    StartTrigger();
                }
            }
        }

        RunTrigger();
	}

    public void SetGameController(GameController gc)
    {
        _game = gc;
    }

    private void RunTrigger()
    {
        if (_state == TriggerState.ShowFlash)
        {
            _whiteFlash = GameObject.Find("WhiteFlash").GetComponent<UnityEngine.UI.Image>();
            _whiteFlash.color = Color.white;
            _game.SetGamePaused(true);
            _state = TriggerState.FadeFlash;
        }
        else if (_state == TriggerState.FadeFlash)
        {
            Color c = _whiteFlash.color;
            c.a -= Time.deltaTime * 0.5f;
            _whiteFlash.color = c;

            if (c.a <= 0f)
            {
                _state = TriggerState.ShowText;
            }
        }
        else if (_state == TriggerState.ShowText)
        {
            _label.GetComponent<InGameTextController>().ShowText(_triggerText, _label.GetComponent<UnityEngine.UI.Text>());
            _state = TriggerState.Idle;
        }
    }

    public void StartTrigger()
    {
        if (_state == TriggerState.Idle)
        {
            _triggered = true;

            switch (_type)
            {
                case TriggerBlockType.Wind:
                    _triggerText = "You got the power of wind!\nNow you can move ice blocks.";
                    break;

                case TriggerBlockType.Ice:
                    _triggerText = "You got the power of ice!\nNow you can place ice blocks.";
                    break;

                case TriggerBlockType.Fire:
                    _triggerText = "You got the power of fire!\nNow you can melt ice blocks.";
                    break;

                case TriggerBlockType.Earth:
                    _triggerText = "You got the power of earth!\nNow you can place rocks.";
                    break;
            }

            _state = TriggerState.ShowFlash;
        }
    }
}
