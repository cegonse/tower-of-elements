using UnityEngine;
using System.Collections;

public class LocalizedText : MonoBehaviour
{
    public string _englishText;
    public string _spanishText;

    private UnityEngine.UI.Text _text;

    void Start()
    {
        _text = GetComponent<UnityEngine.UI.Text>();

        if (SaveGameController.instance.GetActiveLanguage() == SystemLanguage.Spanish ||
            SaveGameController.instance.GetActiveLanguage() == SystemLanguage.Catalan)
        {
            _text.text = _spanishText;
        }
        else
        {
            _text.text = _englishText;
        }
    }
}
