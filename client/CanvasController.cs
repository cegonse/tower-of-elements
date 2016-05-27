using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


public class CanvasController : MonoBehaviour {

    enum CanvasState : int
    {
        MainDialog,
        OptionsDialog,
        GoToLevel,
        SelectElemDialog,
        LevelsDialog,
        GoToLevelDialog
    }

    enum LevelSelection : int
    {
        Normal,
        Wind,
        Ice,
        Fire,
        Earth,
        All
    }

    private CanvasState _actualState = CanvasState.MainDialog;
    private CanvasState _nextState = CanvasState.MainDialog;

    public GameObject[] dialogs;

    private string _levelToGo = "";
    private GameObject _levelDialog;

    private bool _showingLevelButtons = false;
    private bool _showingLevelDialog = false;
    private bool _hidingLevelDialog = false;

    private TransformTweener[] _levelButtons_tt;
    private int _levelButtons_iterator = 0;
    private int _levelDialogElements_iterator = 0;
    private int _levelDialogMaxElements = 7;

    private TransformTweener levelsBackButton_tt;

    public Vector3 _backButtonsPosition;
    public float tweenTimeDefault = 0.5f;
    public GameObject _blackImage;

    public GameObject[] stars;
    private int _starsOnLevel = 3;
    private int _starsIterator = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //############################################################
    // OnTweenFinished
    //############################################################

    public void OnTweenFinished(string control)
    {
        if (_showingLevelButtons)
        {
            _levelButtons_iterator++;
            if (_levelButtons_iterator < _levelButtons_tt.Length)
            {
                _levelButtons_tt[_levelButtons_iterator].DoTween(gameObject);
            }
            else
            {
                _showingLevelButtons = false;
                levelsBackButton_tt.DoTween(gameObject);
            }
        }
        else
        {
            if (_showingLevelDialog)
            {
                ShowLevelDialogProgressive();
            }
            else if (_hidingLevelDialog)
            {
                foreach (GameObject star in stars)
                {
                    star.SetActive(false);
                }
                _hidingLevelDialog = false;
            }
            else
            {
                if (_actualState != _nextState)
                {
                    ShowHideDialog(_nextState, true);
                    _actualState = _nextState;
                }
            }
            
        }
        
        
    }

    //############################################################
    //Callbacks
    //############################################################

    public void OnDungeonButton(GameObject canvasToShow)
    {
        _levelDialog = canvasToShow;
        ChangeDialog(CanvasState.LevelsDialog);
    }

    public void OnLevelButton(GameObject button)
    {
        string levelName = button.GetComponentInChildren<Text>().text;
        _starsOnLevel = 0;

        if (!_showingLevelButtons)
        {
            SaveGameController.instance.SetTargetLevel(levelName);
            List<SaveGameController.LevelProgressData> pd = SaveGameController.instance.GetLevelProgress();

            GameObject timeLabel = GameObject.Find("TimeImage/Text");
            int min = 0, s1 = 0, s2 = 0;
            bool found = false;

            foreach (SaveGameController.LevelProgressData data in pd)
            {
                Debug.Log(data.Id);

                if (data.Id.Contains(levelName))
                {
                    _starsOnLevel = SaveGameController.instance.GetStarCount(data.Score, levelName);

                    min = (int)(data.Score / 60f);
                    s1 = (int)(data.Score % 60 / 10);
                    s2 = (int)(data.Score % 60 % 10);

                    if (timeLabel != null)
                    {
                        timeLabel.GetComponent<Text>().text = min + ":" + s1 + s2;
                    }

                    found = true;
                }
            }

            if (!found)
            {
                timeLabel.GetComponent<Text>().text = "0:00";
            }

            ShowHideDialog(CanvasState.GoToLevelDialog, true);
        }
            

    }

    public void OnLevelSelectedBackButton()
    {
        if (!_showingLevelDialog)
            ShowHideDialog(CanvasState.GoToLevelDialog, false);
    }

    public void OnLevelSelectedGoToLevel()
    {
        GameObject overBlackTexture = GameObject.Find("OverBlackTexture");
        AlphaUITweener ovBlTx_at = overBlackTexture.GetComponent<AlphaUITweener>();

        ovBlTx_at.StartAlpha = 0f;
        ovBlTx_at.EndAlpha = 1f;
        ovBlTx_at.TweenTime = 0.2f;
        ovBlTx_at.DoTween();

        ChangeDialog(CanvasState.GoToLevel);
    }

    public void ToSelectElem()
    {
        _showingLevelButtons = false;
        ChangeDialog(CanvasState.SelectElemDialog);
    }

    public void ToOptions()
    {
        ChangeDialog(CanvasState.OptionsDialog);
    }

    public void ToMainDialog()
    {
        ChangeDialog(CanvasState.MainDialog);
    }


    //############################################################
    // Change/Show/Hide methods
    //############################################################

    private void ChangeDialog(CanvasState state)
    {
        ShowHideDialog(_actualState, false);
        _nextState = state;
    }

    private void ShowHideDialog(CanvasState state, bool show)
    {

        float scale1, scale2;
        if (show)
        {
            scale1 = 0f;
            scale2 = 1f;
        }
        else
        {
            scale1 = 1f;
            scale2 = 0f;
        }

        switch (state)
        {
            case CanvasState.MainDialog:

                //MainDialog on 0
                GameObject mainDialog = dialogs[0];
                mainDialog.SetActive(true);

                TransformTweener mainDialog_tt = mainDialog.GetComponent<TransformTweener>();

                mainDialog_tt.TweenTime = tweenTimeDefault;

                mainDialog_tt.Position0 = new Vector3(0, 0, 0);
                mainDialog_tt.PositionF = new Vector3(0, 0, 0);

                mainDialog_tt.Rotation0 = 0f;
                mainDialog_tt.RotationF = 0f;

                mainDialog_tt.Scale0 = new Vector3(scale1, scale1, scale1);
                mainDialog_tt.ScaleF = new Vector3(scale2, scale2, scale2);

                mainDialog_tt.DoTween(gameObject);

                break;

            case CanvasState.OptionsDialog:

                //OptionButtonsContainer on 1
                GameObject optionDialog = dialogs[1];
                optionDialog.SetActive(true);

                TransformTweener optionDialog_tt = optionDialog.GetComponent<TransformTweener>();

                optionDialog_tt.TweenTime = tweenTimeDefault;

                optionDialog_tt.Position0 = new Vector3(0, 0, 0);
                optionDialog_tt.PositionF = new Vector3(0, 0, 0);

                optionDialog_tt.Rotation0 = 0f;
                optionDialog_tt.RotationF = 0f;

                optionDialog_tt.Scale0 = new Vector3(scale1, scale1, scale1);
                optionDialog_tt.ScaleF = new Vector3(scale2, scale2, scale2);

                optionDialog_tt.DoTween();

                //OptionBackButton on 2
                GameObject optionDialog2 = dialogs[2];
                optionDialog2.SetActive(true);

                TransformTweener optionDialog2_tt = optionDialog2.GetComponent<TransformTweener>();

                optionDialog2_tt.TweenTime = tweenTimeDefault;

                optionDialog2_tt.Position0 = _backButtonsPosition;
                optionDialog2_tt.PositionF = _backButtonsPosition;

                optionDialog2_tt.Rotation0 = 0f;
                optionDialog2_tt.RotationF = 0f;

                optionDialog2_tt.Scale0 = new Vector3(scale1, scale1, scale1);
                optionDialog2_tt.ScaleF = new Vector3(scale2, scale2, scale2);

                optionDialog2_tt.DoTween(gameObject);

                break;

            case CanvasState.SelectElemDialog:

                //LevelButtonsContainer on 3
                GameObject selecElemDialog = dialogs[3];
                selecElemDialog.SetActive(true);

                TransformTweener selecElemDialog_tt = selecElemDialog.GetComponent<TransformTweener>();

                selecElemDialog_tt.TweenTime = tweenTimeDefault;

                selecElemDialog_tt.Position0 = new Vector3(0, 0, 0);
                selecElemDialog_tt.PositionF = new Vector3(0, 0, 0);

                selecElemDialog_tt.Rotation0 = 0f;
                selecElemDialog_tt.RotationF = 0f;

                selecElemDialog_tt.Scale0 = new Vector3(scale1, scale1, scale1);
                selecElemDialog_tt.ScaleF = new Vector3(scale2, scale2, scale2);

                selecElemDialog_tt.DoTween();

                //SelectElemBackButton on 4
                GameObject selecElemDialogBack = dialogs[4];
                selecElemDialogBack.SetActive(true);

                TransformTweener selecElemDialogBack_tt = selecElemDialogBack.GetComponent<TransformTweener>();

                selecElemDialogBack_tt.TweenTime = tweenTimeDefault;

                selecElemDialogBack_tt.Position0 = _backButtonsPosition;
                selecElemDialogBack_tt.PositionF = _backButtonsPosition;

                selecElemDialogBack_tt.Rotation0 = 0f;
                selecElemDialogBack_tt.RotationF = 0f;

                selecElemDialogBack_tt.Scale0 = new Vector3(scale1, scale1, scale1);
                selecElemDialogBack_tt.ScaleF = new Vector3(scale2, scale2, scale2);

                selecElemDialogBack_tt.DoTween(gameObject);

                break;

            case CanvasState.LevelsDialog:

                //LevelsBackButton
                GameObject levelsBackButton = _levelDialog.transform.GetChild(0).gameObject;
                levelsBackButton.SetActive(true);

                levelsBackButton_tt = levelsBackButton.GetComponent<TransformTweener>();

                levelsBackButton_tt.TweenTime = tweenTimeDefault;

                levelsBackButton_tt.Position0 = _backButtonsPosition;
                levelsBackButton_tt.PositionF = _backButtonsPosition;

                levelsBackButton_tt.Rotation0 = 0f;
                levelsBackButton_tt.RotationF = 0f;

                levelsBackButton_tt.Scale0 = new Vector3(scale1, scale1, scale1);
                levelsBackButton_tt.ScaleF = new Vector3(scale2, scale2, scale2);

                if (!show)
                {
                    levelsBackButton_tt.DoTween(gameObject);
                }

                //LevelsButtons

                GameObject levelsContainer = _levelDialog.transform.GetChild(1).gameObject;
                
                _levelButtons_tt = levelsContainer.transform.GetComponentsInChildren<TransformTweener>();

                if (show)
                {
                    ShowLevelButtons();
                }
                else
                {
                    HideLevelButtons();
                }
                
                break;
                
            case CanvasState.GoToLevelDialog:

                if (show)
                {
                    _levelDialogElements_iterator = 0;
                    _starsIterator = 0;
                    _showingLevelDialog = true;

                    //Show the dark_alpha_image
                    //_blackImage.GetComponent<AlphaTweenerUI>().Show();
                    //_blackImage.GetComponent<Image>().color = Color.Lerp(new Color(0f, 0f, 0f, scale1), new Color(0f, 0f, 0f, scale2), tweenTimeDefault);
                    AlphaUITweener _blackImage_at = _blackImage.GetComponent<AlphaUITweener>();

                    _blackImage_at.StartAlpha = 0f;
                    _blackImage_at.EndAlpha = 0.6f;
                    _blackImage_at.TweenTime = 0.2f;
                    _blackImage_at.DoTween();
                    _blackImage.SetActive(true);

                    //Show the back image
                    GameObject levelBackImage = GameObject.Find("LevelBackImage");

                    TransformTweener levelBackImage_tt = levelBackImage.GetComponent<TransformTweener>();

                    levelBackImage_tt.TweenTime = tweenTimeDefault;

                    levelBackImage_tt.Position0 = new Vector3(0f, 0f, 0f); ;
                    levelBackImage_tt.PositionF = new Vector3(0f, 0f, 0f); ;

                    levelBackImage_tt.Rotation0 = 0f;
                    levelBackImage_tt.RotationF = 0f;

                    levelBackImage_tt.Scale0 = new Vector3(scale1, scale1, scale1);
                    levelBackImage_tt.ScaleF = new Vector3(scale2, scale2, scale2);

                    levelBackImage_tt.DoTween(gameObject);
                }
                else
                {
                    _hidingLevelDialog = true;

                    switch (_starsOnLevel)
                    {
                        case 0:
                            foreach (GameObject star in stars)
                            {
                                star.SetActive(false);
                            }
                            break;
                        case 1:
                            stars[0].SetActive(false);
                            stars[2].SetActive(false);
                            stars[3].SetActive(false);
                            stars[4].SetActive(false);
                            break;

                        case 2:
                            stars[0].SetActive(false);
                            stars[1].SetActive(false);
                            stars[2].SetActive(false);
                            break;

                        case 3:
                            stars[3].SetActive(false);
                            stars[4].SetActive(false);
                            break;

                    }

                    GameObject selectingLevelDialog = GameObject.Find("SelectingLevelDialog");

                    TransformTweener[] selectingLevelDialog_tt = selectingLevelDialog.GetComponentsInChildren<TransformTweener>();

                    foreach(TransformTweener tt in selectingLevelDialog_tt)
                    {
                        if (!tt.gameObject.name.Contains("LevelBackImage"))
                        {
                            tt.TweenTime = 0.1f;

                            tt.PositionF.x = tt.Position0.x = tt.gameObject.transform.localPosition.x;
                            tt.PositionF.y = tt.Position0.y = tt.gameObject.transform.localPosition.y;

                            tt.Rotation0 = 0f;
                            tt.RotationF = 0f;

                            tt.Scale0 = new Vector3(1f, 1f, 1f);
                            tt.ScaleF = new Vector3(0f, 0f, 0f);

                            tt.DoTween();
                        }

                    }
                    AlphaUITweener _blackImage_at = _blackImage.GetComponent<AlphaUITweener>();

                    _blackImage_at.StartAlpha = 0.6f;
                    _blackImage_at.EndAlpha = 0f;
                    _blackImage_at.TweenTime = 0.2f;
                    _blackImage_at.DoTween();
                    _blackImage.SetActive(false);

                    //Show the back image
                    GameObject levelBackImage = GameObject.Find("LevelBackImage");

                    TransformTweener levelBackImage_tt = levelBackImage.GetComponent<TransformTweener>();

                    levelBackImage_tt.TweenTime = tweenTimeDefault;

                    levelBackImage_tt.Position0 = new Vector3(0f, 0f, 0f); ;
                    levelBackImage_tt.PositionF = new Vector3(0f, 0f, 0f); ;

                    levelBackImage_tt.Rotation0 = 0f;
                    levelBackImage_tt.RotationF = 0f;

                    levelBackImage_tt.Scale0 = new Vector3(scale1, scale1, scale1);
                    levelBackImage_tt.ScaleF = new Vector3(scale2, scale2, scale2);

                    levelBackImage_tt.DoTween(gameObject);
                }

                break;

            case CanvasState.GoToLevel:

                Application.LoadLevel("game");

                break;
        }
    }

    private void ShowLevelButtons()
    {
        TransformTweener tt;
        _showingLevelButtons = true;

        for ( int i = 0; i < _levelButtons_tt.Length; i++ )
        {
            tt = _levelButtons_tt[i];
            tt.gameObject.SetActive(true);

            tt.TweenTime = 0.1f + ((i+1) / _levelButtons_tt.Length) * (0.3f);

            tt.PositionF.x = tt.Position0.x = -180 + (i % 6)*72;
            tt.PositionF.y = tt.Position0.y = 87 - (i / 6) * 72;

            tt.Rotation0 = 0f;
            tt.RotationF = 0f;

            tt.Scale0 = new Vector3(0f, 0f, 0f);
            tt.ScaleF = new Vector3(1f, 1f, 1f);
        }
        //Comenzar el tween desde el primer elemento
        _levelButtons_iterator = 0;
        _levelButtons_tt[_levelButtons_iterator].DoTween(gameObject);
    }

    private void HideLevelButtons()
    {
        TransformTweener tt;

        for (int i = 0; i < _levelButtons_tt.Length; i++)
        {
            tt = _levelButtons_tt[i];
            tt.gameObject.SetActive(true);

            tt.TweenTime = tweenTimeDefault;

            tt.PositionF.x = tt.Position0.x = -180 + (i % 6) * 72;
            tt.PositionF.y = tt.Position0.y = 87 - (i / 6) * 72;

            tt.Rotation0 = 0f;
            tt.RotationF = 0f;

            tt.Scale0 = new Vector3(1f, 1f, 1f);
            tt.ScaleF = new Vector3(0f, 0f, 0f);

            tt.DoTween();
        }
    }

    public void ShowLevelDialogProgressive()
    {
        
        switch (_levelDialogElements_iterator)
        {
            case 0:

                //Go button
                GameObject go_button = GameObject.Find("go_button");
                TransformTweener go_button_tt = go_button.GetComponent<TransformTweener>();

                go_button_tt.TweenTime = 0.1f;

                go_button_tt.PositionF.x = go_button_tt.Position0.x = go_button.transform.localPosition.x;
                go_button_tt.PositionF.y = go_button_tt.Position0.y = go_button.transform.localPosition.y;

                go_button_tt.Rotation0 = 0f;
                go_button_tt.RotationF = 0f;

                go_button_tt.Scale0 = new Vector3(0f, 0f, 0f);
                go_button_tt.ScaleF = new Vector3(1f, 1f, 1f);

                go_button_tt.DoTween();

                //Back button
                GameObject back_button = GameObject.Find("back_button");
                TransformTweener back_button_tt = back_button.GetComponent<TransformTweener>();

                back_button_tt.TweenTime = 0.1f;

                back_button_tt.PositionF.x = back_button_tt.Position0.x = back_button.transform.localPosition.x;
                back_button_tt.PositionF.y = back_button_tt.Position0.y = back_button.transform.localPosition.y;

                back_button_tt.Rotation0 = 0f;
                back_button_tt.RotationF = 0f;

                back_button_tt.Scale0 = new Vector3(0f, 0f, 0f);
                back_button_tt.ScaleF = new Vector3(1f, 1f, 1f);

                back_button_tt.DoTween(gameObject);

                _levelDialogElements_iterator++;
                break;

            case 1:

                //Wind uses
                GameObject wind_image = GameObject.Find("WindImage");
                TransformTweener wind_image_tt = wind_image.GetComponent<TransformTweener>();

                wind_image_tt.TweenTime = 0.1f;

                wind_image_tt.PositionF.x = wind_image_tt.Position0.x = wind_image.transform.localPosition.x;
                wind_image_tt.PositionF.y = wind_image_tt.Position0.y = wind_image.transform.localPosition.y;

                wind_image_tt.Rotation0 = 0f;
                wind_image_tt.RotationF = 0f;

                wind_image_tt.Scale0 = new Vector3(0f, 0f, 0f);
                wind_image_tt.ScaleF = new Vector3(1f, 1f, 1f);

                wind_image_tt.DoTween(gameObject);

                _levelDialogElements_iterator++;
                break;

            case 2:

                //Ice uses
                GameObject ice_image = GameObject.Find("IceImage");
                TransformTweener ice_image_tt = ice_image.GetComponent<TransformTweener>();

                ice_image_tt.TweenTime = 0.1f;

                ice_image_tt.PositionF.x = ice_image_tt.Position0.x = ice_image.transform.localPosition.x;
                ice_image_tt.PositionF.y = ice_image_tt.Position0.y = ice_image.transform.localPosition.y;

                ice_image_tt.Rotation0 = 0f;
                ice_image_tt.RotationF = 0f;

                ice_image_tt.Scale0 = new Vector3(0f, 0f, 0f);
                ice_image_tt.ScaleF = new Vector3(1f, 1f, 1f);

                ice_image_tt.DoTween(gameObject);

                _levelDialogElements_iterator++;
                break;

            case 3:

                //Fire uses
                GameObject fire_image = GameObject.Find("FireImage");
                TransformTweener fire_image_tt = fire_image.GetComponent<TransformTweener>();

                fire_image_tt.TweenTime = 0.1f;

                fire_image_tt.PositionF.x = fire_image_tt.Position0.x = fire_image.transform.localPosition.x;
                fire_image_tt.PositionF.y = fire_image_tt.Position0.y = fire_image.transform.localPosition.y;

                fire_image_tt.Rotation0 = 0f;
                fire_image_tt.RotationF = 0f;

                fire_image_tt.Scale0 = new Vector3(0f, 0f, 0f);
                fire_image_tt.ScaleF = new Vector3(1f, 1f, 1f);

                fire_image_tt.DoTween(gameObject);

                _levelDialogElements_iterator++;
                break;

            case 4:

                //Earth uses
                GameObject earth_image = GameObject.Find("EarthImage");
                TransformTweener earth_image_tt = earth_image.GetComponent<TransformTweener>();

                earth_image_tt.TweenTime = 0.1f;

                earth_image_tt.PositionF.x = earth_image_tt.Position0.x = earth_image.transform.localPosition.x;
                earth_image_tt.PositionF.y = earth_image_tt.Position0.y = earth_image.transform.localPosition.y;

                earth_image_tt.Rotation0 = 0f;
                earth_image_tt.RotationF = 0f;

                earth_image_tt.Scale0 = new Vector3(0f, 0f, 0f);
                earth_image_tt.ScaleF = new Vector3(1f, 1f, 1f);

                earth_image_tt.DoTween(gameObject);

                _levelDialogElements_iterator++;
                break;

            case 5:

                //Clock
                GameObject time_image = GameObject.Find("TimeImage");
                TransformTweener time_image_tt = time_image.GetComponent<TransformTweener>();

                time_image_tt.TweenTime = 0.1f;

                time_image_tt.PositionF.x = time_image_tt.Position0.x = time_image.transform.localPosition.x;
                time_image_tt.PositionF.y = time_image_tt.Position0.y = time_image.transform.localPosition.y;

                time_image_tt.Rotation0 = 0f;
                time_image_tt.RotationF = 0f;

                time_image_tt.Scale0 = new Vector3(0f, 0f, 0f);
                time_image_tt.ScaleF = new Vector3(1f, 1f, 1f);

                time_image_tt.DoTween(gameObject);

                _levelDialogElements_iterator++;
                break;

            case 6:

                switch (_starsOnLevel)
                {
                    case 0:

                        _showingLevelDialog = false;
                        break;

                    case 1:

                        GameObject star1 = stars[1];
                        star1.SetActive(true);

                        TransformTweener star1_tt = star1.GetComponent<TransformTweener>();
                        star1_tt.TweenTime = 0.1f;

                        star1_tt.PositionF.x = star1_tt.Position0.x = star1.transform.localPosition.x;
                        star1_tt.PositionF.y = star1_tt.Position0.y = star1.transform.localPosition.y;

                        star1_tt.Rotation0 = 0f;
                        star1_tt.RotationF = 0f;

                        star1_tt.Scale0 = new Vector3(0f, 0f, 0f);
                        star1_tt.ScaleF = new Vector3(1f, 1f, 1f);

                        star1_tt.DoTween(gameObject);

                        _levelDialogElements_iterator++;
                        break;

                    case 2:

                        switch (_starsIterator)
                        {
                            case 0:

                                GameObject star2_1 = stars[3];
                                star2_1.SetActive(true);

                                TransformTweener star2_1_tt = star2_1.GetComponent<TransformTweener>();
                                star2_1_tt.TweenTime = 0.1f;

                                star2_1_tt.PositionF.x = star2_1_tt.Position0.x = star2_1.transform.localPosition.x;
                                star2_1_tt.PositionF.y = star2_1_tt.Position0.y = star2_1.transform.localPosition.y;

                                star2_1_tt.Rotation0 = 0f;
                                star2_1_tt.RotationF = 0f;

                                star2_1_tt.Scale0 = new Vector3(0f, 0f, 0f);
                                star2_1_tt.ScaleF = new Vector3(1f, 1f, 1f);

                                star2_1_tt.DoTween(gameObject);

                                _starsIterator++;
                                break;

                            case 1:
                                GameObject star2_2 = stars[4];
                                star2_2.SetActive(true);

                                TransformTweener star2_2_tt = star2_2.GetComponent<TransformTweener>();
                                star2_2_tt.TweenTime = 0.1f;

                                star2_2_tt.PositionF.x = star2_2_tt.Position0.x = star2_2.transform.localPosition.x;
                                star2_2_tt.PositionF.y = star2_2_tt.Position0.y = star2_2.transform.localPosition.y;

                                star2_2_tt.Rotation0 = 0f;
                                star2_2_tt.RotationF = 0f;

                                star2_2_tt.Scale0 = new Vector3(0f, 0f, 0f);
                                star2_2_tt.ScaleF = new Vector3(1f, 1f, 1f);

                                star2_2_tt.DoTween(gameObject);

                                _levelDialogElements_iterator++;
                                break;
                        }

                        break;

                    case 3:

                        switch (_starsIterator)
                        {
                            case 0:

                                GameObject star3_1 = stars[0];
                                star3_1.SetActive(true);

                                TransformTweener star3_1_tt = star3_1.GetComponent<TransformTweener>();
                                star3_1_tt.TweenTime = 0.1f;

                                star3_1_tt.PositionF.x = star3_1_tt.Position0.x = star3_1.transform.localPosition.x;
                                star3_1_tt.PositionF.y = star3_1_tt.Position0.y = star3_1.transform.localPosition.y;

                                star3_1_tt.Rotation0 = 0f;
                                star3_1_tt.RotationF = 0f;

                                star3_1_tt.Scale0 = new Vector3(0f, 0f, 0f);
                                star3_1_tt.ScaleF = new Vector3(1f, 1f, 1f);

                                star3_1_tt.DoTween(gameObject);

                                _starsIterator++;
                                break;

                            case 1:
                                GameObject star3_2 = stars[1];
                                star3_2.SetActive(true);

                                TransformTweener star3_2_tt = star3_2.GetComponent<TransformTweener>();
                                star3_2_tt.TweenTime = 0.1f;

                                star3_2_tt.PositionF.x = star3_2_tt.Position0.x = star3_2.transform.localPosition.x;
                                star3_2_tt.PositionF.y = star3_2_tt.Position0.y = star3_2.transform.localPosition.y;

                                star3_2_tt.Rotation0 = 0f;
                                star3_2_tt.RotationF = 0f;

                                star3_2_tt.Scale0 = new Vector3(0f, 0f, 0f);
                                star3_2_tt.ScaleF = new Vector3(1f, 1f, 1f);

                                star3_2_tt.DoTween(gameObject);

                                _starsIterator++;
                                break;

                            case 2:
                                GameObject star3_3 = stars[2];
                                star3_3.SetActive(true);

                                TransformTweener star3_3_tt = star3_3.GetComponent<TransformTweener>();
                                star3_3_tt.TweenTime = 0.1f;

                                star3_3_tt.PositionF.x = star3_3_tt.Position0.x = star3_3.transform.localPosition.x;
                                star3_3_tt.PositionF.y = star3_3_tt.Position0.y = star3_3.transform.localPosition.y;

                                star3_3_tt.Rotation0 = 0f;
                                star3_3_tt.RotationF = 0f;

                                star3_3_tt.Scale0 = new Vector3(0f, 0f, 0f);
                                star3_3_tt.ScaleF = new Vector3(1f, 1f, 1f);

                                star3_3_tt.DoTween(gameObject);

                                _levelDialogElements_iterator++;
                                break;
                        }

                        break;
                }

                break;

            case 7:

                _showingLevelDialog = false;
                break;
        }
    }

}
