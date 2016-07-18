using UnityEngine;
using System.IO;

public class MakeScreenshot : MonoBehaviour
{
    public RenderTexture _renderTexture;

    private bool _capture = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            _capture = true;
        }
    }

	void OnPostRender()
    {
	    if (_capture)
        {
            Texture2D tex = new Texture2D(_renderTexture.width, _renderTexture.height);
            tex.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            tex.Apply();

            byte[] data = tex.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + "/screenshot.png", data);
            _capture = false;
        }
	}
}
