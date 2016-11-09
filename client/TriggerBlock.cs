using UnityEngine;
using System.Collections;

public class TriggerBlock : MonoBehaviour
{
    public enum TriggerBlockType
    {
        Wind,
        Ice,
        Fire,
        Earth,
        TotemEye,
        TotemWind,
        TotemFire,
        TotemEarth,
        TotemIce
    }

    public enum TriggerState
    {
        Idle,
        ShowFlash,
        FadeFlash,
        ShowText,
        Finished,

        FadingInImage,
        ShowingImage,
        FadingOutImage
    }

    private TriggerBlockType _type;
    private TriggerState _state = TriggerState.Idle;
    
    private RaycastHit2D[] _hit;
    private Vector2 _lineOrigin;
    private Vector2 _lineEnd;

    private GameObject _label;
    private GameObject _other;
    private bool _triggered = false;
    private string _triggerText = "test text";
    private UnityEngine.UI.Image _whiteFlash;
    private GameController _game;
    private float _targetImageAlpha = 0.75f;

    public void SetTriggerData(TriggerBlockType type, GameObject label, GameObject other = null)
    {
        _type = type;
        _lineOrigin = new Vector2(transform.position.x - 0.5f, transform.position.y);
        _lineEnd = new Vector2(transform.position.x + 0.5f, transform.position.y);
        _label = label;

        if (_type == TriggerBlockType.TotemEye || _type == TriggerBlockType.TotemIce ||
            _type == TriggerBlockType.TotemWind || _type == TriggerBlockType.TotemFire ||
            _type == TriggerBlockType.TotemEarth)
        {
            _label.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 0f);
            _other = other;
        }
    }

	void Update ()
    {
        _hit = Physics2D.LinecastAll(_lineOrigin, _lineEnd);

        foreach (RaycastHit2D h in _hit)
        {
            if (h.collider.gameObject != null)
            {
                if (h.collider.gameObject.GetComponent<Player>() != null)
                {
                    StartTrigger();
                }
            }
        }

        if (_hit.Length == 1)
        {
            if (_state == TriggerState.ShowingImage)
            {
                _state = TriggerState.FadingOutImage;
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

        if (_state == TriggerState.FadingInImage)
        {
            Color c = _label.GetComponent<UnityEngine.UI.Image>().color;
            c.a += Time.deltaTime * 4f;
            _label.GetComponent<UnityEngine.UI.Image>().color = c;
            //_other.GetComponent<UnityEngine.UI.Image>().color = c;

            if (c.a > _targetImageAlpha)
            {
                _label.GetComponent<UnityEngine.UI.Image>().color = new Color(1f,1f,1f, _targetImageAlpha);
                //_other.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, _targetImageAlpha);
                _state = TriggerState.ShowingImage;
            }
        }
        else if (_state == TriggerState.FadingOutImage)
        {
            Color c = _label.GetComponent<UnityEngine.UI.Image>().color;
            c.a -= Time.deltaTime * 4f;
            _label.GetComponent<UnityEngine.UI.Image>().color = c;
            //_other.GetComponent<UnityEngine.UI.Image>().color = c;

            if (c.a < 0f)
            {
                _state = TriggerState.Idle;
            }
        }
        else if (_state == TriggerState.ShowingImage)
        {
            if (_game.GetLevelController().GetActiveLevel().GetGuiCallbacks().IsOnEye())
            {
                _state = TriggerState.FadingOutImage;
            }
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
                    _state = TriggerState.ShowFlash;
                    break;

                case TriggerBlockType.Ice:
                    _triggerText = "You got the power of ice!\nNow you can place ice blocks.";
                    _state = TriggerState.ShowFlash;
                    break;

                case TriggerBlockType.Fire:
                    _triggerText = "You got the power of fire!\nNow you can melt ice blocks.";
                    _state = TriggerState.ShowFlash;
                    break;

                case TriggerBlockType.Earth:
                    _triggerText = "You got the power of earth!\nNow you can place rocks.";
                    _state = TriggerState.ShowFlash;
                    break;

                case TriggerBlockType.TotemEye:
                case TriggerBlockType.TotemEarth:
                case TriggerBlockType.TotemFire:
                case TriggerBlockType.TotemWind:
                case TriggerBlockType.TotemIce:
                    if (!_game.GetLevelController().GetActiveLevel().GetGuiCallbacks().IsOnEye())
                    {
                        _state = TriggerState.FadingInImage;
                    }
                    break;
            }
        }
    }
}
