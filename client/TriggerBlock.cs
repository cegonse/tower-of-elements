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

    private TriggerBlockType _type;
    
    private RaycastHit2D _hit;
    private Vector2 _lineOrigin;
    private Vector2 _lineEnd;
    private GameObject _label;
    private bool _triggered = false;

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
	}

    public void StartTrigger()
    {
        string tx = "publisher senpai pls buy my game\nthis is so cool ajajajaja\nadrian come pollas";
        _triggered = true;

        switch (_type)
        {
            case TriggerBlockType.Wind:
                break;

            case TriggerBlockType.Ice:
                break;

            case TriggerBlockType.Fire:
                break;

            case TriggerBlockType.Earth:
                break;
        }

        _label.GetComponent<InGameTextController>().ShowText(tx, _label.GetComponent<UnityEngine.UI.Text>());
    }
}
