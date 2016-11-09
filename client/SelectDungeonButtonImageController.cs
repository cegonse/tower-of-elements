using UnityEngine;
using UnityEngine.UI;

public class SelectDungeonButtonImageController : MonoBehaviour
{
	void Start ()
    {
        if (name.Contains("Wind"))
        {
            if (!SaveGameController.instance.HasPower(SaveGameController.UnlockablePowers.Wind))
            {
                Texture2D disabledTex = Resources.Load("Textures/GUI/WindLevelOff") as Texture2D;
                transform.GetChild(1).GetComponent<Image>().sprite =
                    Sprite.Create(disabledTex, new Rect(0, 0, disabledTex.width, disabledTex.height), Vector2.one * 0.5f);
                transform.GetChild(1).GetComponent<Button>().enabled = false;

                transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        else if (name.Contains("Ice"))
        {
            if (!SaveGameController.instance.HasPower(SaveGameController.UnlockablePowers.Ice))
            {
                Texture2D disabledTex = Resources.Load("Textures/GUI/WaterLevelOff") as Texture2D;
                transform.GetChild(1).GetComponent<Image>().sprite =
                    Sprite.Create(disabledTex, new Rect(0, 0, disabledTex.width, disabledTex.height), Vector2.one * 0.5f);
                transform.GetChild(1).GetComponent<Button>().enabled = false;

                transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        else if (name.Contains("Fire"))
        {
            if (!SaveGameController.instance.HasPower(SaveGameController.UnlockablePowers.Fire))
            {
                Texture2D disabledTex = Resources.Load("Textures/GUI/FireLevelOff") as Texture2D;
                transform.GetChild(1).GetComponent<Image>().sprite =
                    Sprite.Create(disabledTex, new Rect(0, 0, disabledTex.width, disabledTex.height), Vector2.one * 0.5f);
                transform.GetChild(1).GetComponent<Button>().enabled = false;

                transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        else if (name.Contains("Earth"))
        {
            if (!SaveGameController.instance.HasPower(SaveGameController.UnlockablePowers.Earth))
            {
                Texture2D disabledTex = Resources.Load("Textures/GUI/EarthLevelOff") as Texture2D;
                transform.GetChild(1).GetComponent<Image>().sprite =
                    Sprite.Create(disabledTex, new Rect(0, 0, disabledTex.width, disabledTex.height), Vector2.one * 0.5f);
                transform.GetChild(1).GetComponent<Button>().enabled = false;

                transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        else if (name.Contains("All"))
        {
            if (!SaveGameController.instance.HasPower(SaveGameController.UnlockablePowers.All))
            {
                Texture2D disabledTex = Resources.Load("Textures/GUI/DungeonLevelOff") as Texture2D;
                transform.GetChild(1).GetComponent<Image>().sprite =
                    Sprite.Create(disabledTex, new Rect(0, 0, disabledTex.width, disabledTex.height), Vector2.one * 0.5f);
                transform.GetChild(1).GetComponent<Button>().enabled = false;

                transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }
}
