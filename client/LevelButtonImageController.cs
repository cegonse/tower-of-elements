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

        string texName = "";

        if (_levelState == LevelState.On || _levelState == LevelState.Off)
        {
            texName = "Textures/GUI/" + _elementType.ToString() + "Level" + _levelState;
        }
        else
        {
            texName = "Textures/GUI/" + _elementType.ToString() + "Level_" + _levelState;
        }

        _buttonImage = transform.GetChild(1).GetComponent<Image>();
        Texture2D buttonTex = Resources.Load(texName) as Texture2D;
        _buttonImage.sprite = Sprite.Create(buttonTex, new Rect(0, 0, buttonTex.width, buttonTex.height), Vector2.one * 0.5f);

        // Disable glow and button usability
        if (_levelState == LevelState.Off)
        {
            transform.GetChild(0).GetComponent<Image>().enabled = false;
            transform.GetChild(1).GetComponent<Button>().enabled = false;
        }

        // Disable smoothing
        if (_levelState != LevelState.On)
        {
            GetComponent<ButtonVibration>().enabled = false;
        }
        else
        {
            GetComponent<ButtonVibration>().StartSmoothing();
        }
    }

    void CalculateButtonLevelState()
    {
        List<SaveGameController.LevelProgressData> levelsData = SaveGameController.instance.GetLevelProgress();
        bool found = false;
        int dungeonIndex = 0, levelIndex = 0;

        // Split the level identifier into its dungeon
        // and level index.
        dungeonIndex = int.Parse(name.Split('_')[0]);
        levelIndex = int.Parse(name.Split('_')[1]);

        for (int i = 0; i < levelsData.Count && !found; i++)
        {
            // Check if the level has been previously completed and
            // the star count has to be shown
            if (gameObject.name == levelsData[i].Id)
            {
                float _timeOnLevel = levelsData[i].Score;
                float threeStarsTime = SaveGameController.instance.GetThreeStarsTime(levelsData[i].Id);
                float twoStarsTime = SaveGameController.instance.GetTwoStarsTime(levelsData[i].Id);

                if (_timeOnLevel <= threeStarsTime)
                {
                    _levelState = LevelState.ThreeStars;
                }
                else
                {
                    if (_timeOnLevel > twoStarsTime && _timeOnLevel <= twoStarsTime)
                    {
                        _levelState = LevelState.TwoStars;
                    }
                    else
                    {
                        if (_timeOnLevel > twoStarsTime)
                        {
                            _levelState = LevelState.OneStar;
                        }
                    }
                }

                // Disable the level glow
                transform.GetChild(0).GetComponent<Image>().enabled = false;

                found = true;
            }
        }

        if (!found)
        {
            for (int i = 0; i < levelsData.Count && !found; i++)
            {
                // Split the current iteration level
                int thisDungeon = 0, thisIndex = 0;
                thisDungeon = int.Parse(levelsData[i].Id.Split('_')[0]);
                thisIndex = int.Parse(levelsData[i].Id.Split('_')[1]);

                // Check if the level hasn't been completed but
                // should be active to play

                // The first level should be active if the last level
                // of the tutorial was completed
                if (thisDungeon == dungeonIndex - 1 && thisIndex == 4 && levelIndex == 1)
                {
                    _levelState = LevelState.On;
                    found = true;
                }

                // The following levels should be active if the previous
                // one has been completed
                if (thisDungeon == dungeonIndex && thisIndex == levelIndex - 1)
                {
                    _levelState = LevelState.On;
                    found = true;
                }
            }

            if (!found)
            {
                _levelState = LevelState.Off;
            }
        }
    }
}