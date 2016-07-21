using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class LevelButtonImageController : MonoBehaviour
{

    public enum ElementType : int
    {
        None,
        Wind,
        Water,
        Fire,
        Earth,
        Dungeon
    }

    public enum LevelState : int
    {
        On,
        OneStar,
        TwoStars,
        ThreeStars,
        Off
    }

    private ElementType _elementType;
    private LevelState _levelState = LevelState.OneStar;
    private Image _buttonImage;

    // Use this for initialization
    void Start()
    {

        //Get the element of the level
        int elemType = Int32.Parse(gameObject.name.Substring(0, 1));
        _elementType = (ElementType)(elemType);

        CalculateButtonLevelState();

        _buttonImage = GetComponent<Image>();
        Texture2D buttonTex = Resources.Load("Textures/GUI/" + _elementType.ToString() + "Level" + _levelState) as Texture2D;
        _buttonImage.sprite = Sprite.Create(buttonTex, new Rect(0, 0, buttonTex.width, buttonTex.height), Vector2.one * 0.5f);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void CalculateButtonLevelState()
    {
        List<SaveGameController.LevelProgressData> levelsData = SaveGameController.instance.GetLevelProgress();

        bool found = false;

        for (int i = 0; i < levelsData.Count && !found; i++)
        {
            if (gameObject.name == levelsData[i].Id)
            {
                float _timeOnLevel = levelsData[i].Score;
                float threeStarsTime = SaveGameController.instance.GetThreeStarsTime(levelsData[i].Id);

                if (_timeOnLevel <= threeStarsTime)
                {
                    _levelState = LevelState.ThreeStars;
                }
                else
                {
                    if (_timeOnLevel <= threeStarsTime * MainGUIController.instance.TWO_STARS_MULTIPLIER)
                    {
                        _levelState = LevelState.TwoStars;
                    }
                    else
                    {
                        if (_timeOnLevel <= threeStarsTime * MainGUIController.instance.ONE_STAR_MULTIPLIER)
                        {
                            _levelState = LevelState.OneStar;
                        }
                        else
                        {
                            _levelState = LevelState.On;
                        }
                    }
                }
                found = true;
            }
        }

        if (!found)
        {
            Debug.Log("There is no level with the name: " + gameObject.name + "!");
            _levelState = LevelState.Off;
        }
    }
}