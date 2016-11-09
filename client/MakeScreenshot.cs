using UnityEngine;
using System.IO;

public class MakeScreenshot : MonoBehaviour
{
    private bool _capture = false;
    private RenderTexture _renderTexture;
    private Main _main;

    public void DoScreenshot(RenderTexture rt, Main main)
    {
        _capture = true;
        _renderTexture = rt;
        _main = main;
    }

	void OnPostRender()
    {
	    if (_capture)
        {
            GetComponent<Camera>().targetTexture = _renderTexture;
            _main.GetCanvas().SetActive(false);

            Texture2D tex = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            tex.Apply();

            byte[] data = tex.EncodeToPNG();
            System.DateTime time = System.DateTime.UtcNow;
            string date = time.Day.ToString() + "_" + time.Month.ToString() + "_" + time.Year.ToString() + "_" +
                            time.Hour.ToString() + "_" + time.Minute.ToString() + "_" + time.Second.ToString();
            string name = Application.persistentDataPath + "/" + date + "_Screenshot.png";

            File.WriteAllBytes(name, data);

            Texture2D.Destroy(tex);
            GetComponent<Camera>().targetTexture = null;
            _main.GetCanvas().SetActive(true);

            _capture = false;
        }
	}
}
