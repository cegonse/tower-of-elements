using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public enum TransitionType : int
{
    Init,
    Finish
}

public enum UIState : int
{
    Intro,
    MainMenu,
    OptionsMenu,
    SelectDungeonMenu,
    WindLevelsMenu,
    FireLevelsMenu,
    EarthLevelsMenu,
    IceLevelsMenu,
    AllLevelsMenu,
    FirstLevelsMenu,
    SelectionLevelMenu,
    PlayState,
    TutorialState
}

public class MainGUIController : MonoBehaviour
{

    // Custom serializable class where to store the GameObjects of each interface
    [System.Serializable]
    public class UI_Interface
    {
        public GameObject[] Intro;
        public GameObject[] MainMenu;
        public GameObject[] OptionsMenu;
        public GameObject[] SelectDungeonMenu;
        public GameObject[] WindLevelsMenu;
        public GameObject[] FireLevelsMenu;
        public GameObject[] EarthLevelsMenu;
        public GameObject[] IceLevelsMenu;
        public GameObject[] AllLevelsMenu;
        public GameObject[] FirstLevelsMenu;
        public GameObject[] SelectionLevelMenu;
    }

    //Singleton
    public static MainGUIController instance;

    //*** UI GameObjects ***
    public UI_Interface _interfaces;

    public AudioClip _music;
    public AudioClip _clickSound;
    public AudioClip _totemVibrateSound;
    public AudioClip _totemMoveSound;

    //*** Transition variables ***
    //Varible to store when a transition is being done
    private bool _onTransition = false;
    //The type of the transition (INIT, FINISH)
    private TransitionType _transitionType;
    //This is the UIState used to know what transition method has to be called on Update()
    private UIState _transitionState;
    //Make _UIActualState be the first UI that we want to show
    private UIState _UIActualState = UIState.Intro, _UINextState, _UIPreviousState = UIState.Intro;
    // This constant indicates to the MakeUITransition method that should
    // make the INIT transition of the _UINextState
    private const bool INIT_NEXT_TRANSITION = true;


    //*** Variables used on interface transitions ***

    private const float MAIN_MENU_INTRO_DURATION = 1f;
    private const float MAIN_MENU_OUTRO_DURATION = 0.5f;

    private const float OPTIONS_MENU_DURATION = 0.2f;

    private const float SELECT_DUNGEON_MENU_INTRO_DURATION = 1f;
    private const float SELECT_DUNGEON_MENU_OUTRO_DURATION = 0.5f;

    private const float LEVELS_MENU_INTRO_DURATION = 1f;
    private const float LEVELS_MENU_OUTRO_DURATION = 0.5f;
    private const int LEVELS_MENU_BUTTON_OFFSET = 3;

    private const float SELECTION_LEVEL_MENU_INTRO_DURATION = 0.8f;
    private const float SELECTION_LEVEL_MENU_OUTRO_DURATION = 0.5f;

    private float _transitionCounterTime;

    //MainMenu

    private const float _mainMenuBlackImageStartTime = 0f;
    private const float _mainMenuBlackImageEndTime = 0.5f * MAIN_MENU_INTRO_DURATION;

    private const float _mainMenuLogoStartTime = 0.2f * MAIN_MENU_INTRO_DURATION;
    private const float _mainMenuLogoEndTime = 0.7f * MAIN_MENU_INTRO_DURATION;

    private const float _mainMenuButtonsStartTime = 0.5f * MAIN_MENU_INTRO_DURATION;
    private const float _mainMenuButtonsEndTime = MAIN_MENU_INTRO_DURATION;

    private const float _mainMenuBlackImageOutroStartTime = 0f;
    private const float _mainMenuBlackImageOutroEndTime = 0.5f;

    //OptionsMenu

    //SelectDungeonMenu
    private const float _selectDungeonMenuBlackImageEndTime = 0.6f * SELECT_DUNGEON_MENU_INTRO_DURATION;

    private const float _selectDungeonMenuButtonWindStartTime = 0.5f * SELECT_DUNGEON_MENU_INTRO_DURATION;
    private const float _selectDungeonMenuButtonEarthStartTime = 0.6f * SELECT_DUNGEON_MENU_INTRO_DURATION;
    private const float _selectDungeonMenuButtonFireStartTime = 0.7f * SELECT_DUNGEON_MENU_INTRO_DURATION;
    private const float _selectDungeonMenuButtonIceStartTime = 0.8f * SELECT_DUNGEON_MENU_INTRO_DURATION;
    private const float _selectDungeonMenuButtonAllStartTime = 0.9f * SELECT_DUNGEON_MENU_INTRO_DURATION;

    private const float _selectDungeonMenuButtonsDurationTime = 0.1f * SELECT_DUNGEON_MENU_INTRO_DURATION;

    private float transition_value = 0f;
    private const float buttonTime = (1f / _selectDungeonMenuButtonsDurationTime);
    private const float windEndTime = (_selectDungeonMenuButtonWindStartTime + _selectDungeonMenuButtonsDurationTime);
    private const float earthEndTime = (_selectDungeonMenuButtonEarthStartTime + _selectDungeonMenuButtonsDurationTime);
    private const float fireEndTime = (_selectDungeonMenuButtonFireStartTime + _selectDungeonMenuButtonsDurationTime);
    private const float iceEndTime = (_selectDungeonMenuButtonIceStartTime + _selectDungeonMenuButtonsDurationTime);
    private const float allEndTime = (_selectDungeonMenuButtonAllStartTime + _selectDungeonMenuButtonsDurationTime);

    private bool _windMoveStarted = true, _windMoveEnded = true;
    private bool _earthMoveStarted = true, _earthMoveEnded = true;
    private bool _iceMoveStarted = true, _iceMoveEnded = true;
    private bool _fireMoveStarted = true, _fireMoveEnded = true;
    private bool _allMoveStarted = true, _allMoveEnded = true;

    private Vector3 _dungeonButtonWindPos, _dungeonButtonEarthPos, _dungeonButtonFirePos, _dungeonButtonIcePos, _dungeonButtonAllPos;

    private static float _screenFactor = Screen.height / 325f;//Screen.width/500f;
    private Vector3 _dungeonButtonWindPosOffset = new Vector3(-80, +40, 1) * _screenFactor,
        _dungeonButtonEarthPosOffset = new Vector3(-65, -40, 1) * _screenFactor,
        _dungeonButtonFirePosOffset = new Vector3(60, -50, 1) * _screenFactor,
        _dungeonButtonIcePosOffset = new Vector3(85, 50, 1) * _screenFactor,
        _dungeonButtonAllPosOffset = new Vector3(0, 80, 1) * _screenFactor;


    //Common LevelsMenu values

    private const float _levelsMenuBlackImageEndTime = 0.5f * LEVELS_MENU_INTRO_DURATION;
    private const float _levelsMenuBackButtonStartTime = 0.9f * LEVELS_MENU_INTRO_DURATION;
    private const float _levelsMenuBackButtonEndTime = 1f * LEVELS_MENU_INTRO_DURATION;

    //Level buttons start time
    private float[] _levelMenuButtonsStartTime = {    0.45f * LEVELS_MENU_INTRO_DURATION,
                                                      0.5f * LEVELS_MENU_INTRO_DURATION,
                                                      0.55f * LEVELS_MENU_INTRO_DURATION,
                                                      0.6f * LEVELS_MENU_INTRO_DURATION,
                                                      0.65f * LEVELS_MENU_INTRO_DURATION,
                                                      0.7f * LEVELS_MENU_INTRO_DURATION,
                                                      0.75f * LEVELS_MENU_INTRO_DURATION,
                                                      0.8f * LEVELS_MENU_INTRO_DURATION,
                                                      0.85f * LEVELS_MENU_INTRO_DURATION,
                                                      0.9f * LEVELS_MENU_INTRO_DURATION
                                                 };
    //Level buttons end time
    private float[] _levelMenuButtonsEndTime = {      0.55f * LEVELS_MENU_INTRO_DURATION,
                                                      0.6f * LEVELS_MENU_INTRO_DURATION,
                                                      0.65f * LEVELS_MENU_INTRO_DURATION,
                                                      0.7f * LEVELS_MENU_INTRO_DURATION,
                                                      0.75f * LEVELS_MENU_INTRO_DURATION,
                                                      0.8f * LEVELS_MENU_INTRO_DURATION,
                                                      0.85f * LEVELS_MENU_INTRO_DURATION,
                                                      0.9f * LEVELS_MENU_INTRO_DURATION,
                                                      0.95f * LEVELS_MENU_INTRO_DURATION,
                                                      1f * LEVELS_MENU_INTRO_DURATION
                                                 };



    //SelectionLevelMenu
    private const float _selectionLevelBackImageEndTime = 0.5f * SELECTION_LEVEL_MENU_INTRO_DURATION;
    private const float _selectionLevelStarsDuration = 0.2f;
    private float SELECTION_LEVEL_MENU_INTRO_MAX_DURATION;

    private GameObject _levelButton = null;
    private int _starsOnLevel = 0;
    private float _timeOnLevel = 0f;

    private int _windUses = 0;
    private int _iceUses = 0;
    private int _fireUses = 0;
    private int _earthUses = 0;

    public float TWO_STARS_MULTIPLIER = 2f;
    public float ONE_STAR_MULTIPLIER = 3f;

    private AudioSource _musicSource;
    private AudioSource _sfxSource;

    // Use this for initialization
    void Start()
    {

        //Singleton
        if (instance == null)
        {
            instance = this;
        }

        //Dungeon buttons positions
        _dungeonButtonWindPos = _interfaces.SelectDungeonMenu[2].transform.position;
        _dungeonButtonEarthPos = _interfaces.SelectDungeonMenu[3].transform.position;
        _dungeonButtonIcePos = _interfaces.SelectDungeonMenu[4].transform.position;
        _dungeonButtonFirePos = _interfaces.SelectDungeonMenu[5].transform.position;
        _dungeonButtonAllPos = _interfaces.SelectDungeonMenu[6].transform.position;

        //*********************************
        // UI transition

        //Set the next UI state
        if (SaveGameController.instance.GetTargetMenu() == UIState.Intro)
        {
            _UINextState = UIState.MainMenu;
        }
        else if (SaveGameController.instance.GetTargetMenu() == UIState.SelectDungeonMenu)
        {
            _UINextState = UIState.SelectDungeonMenu;
        }

        //Start the transition
        MakeUITransition(INIT_NEXT_TRANSITION);

        _musicSource = transform.GetChild(0).GetComponent<AudioSource>();
        _sfxSource = transform.GetChild(1).GetComponent<AudioSource>();

        _musicSource.clip = _music;
        _musicSource.loop = true;
        _musicSource.Play();

        Resources.UnloadUnusedAssets();
    }

    // Update is called once per frame
    void Update()
    {
        //*************************************
        // UI over time transitions
        // This methods are called when the GUI
        // is doing a transition.
        if (_onTransition)
        {
            switch (_transitionState)
            {
                case UIState.Intro:
                    IntroTransition();
                    break;
                case UIState.MainMenu:
                    MainMenuTransition();
                    break;

                case UIState.OptionsMenu:
                    OptionsMenuTransition();
                    break;

                case UIState.SelectDungeonMenu:
                    SelectDungeonMenuTransition();
                    break;

                case UIState.WindLevelsMenu:
                    WindLevelsMenuTransition();
                    break;

                case UIState.EarthLevelsMenu:
                    EarthLevelsMenuTransition();
                    break;

                case UIState.IceLevelsMenu:
                    IceLevelsMenuTransition();
                    break;

                case UIState.FireLevelsMenu:
                    FireLevelsMenuTransition();
                    break;

                case UIState.AllLevelsMenu:
                    AllLevelsMenuTransition();
                    break;

                case UIState.FirstLevelsMenu:
                    FirstLevelsMenuTransition();
                    break;

                case UIState.SelectionLevelMenu:
                    SelectionLevelMenuTransition();
                    break;
            }
        }
        //- END - UI over time transitions
        //*************************************
    }

    //**********************************************************************************************************************
    //      BUTTON METHODS
    //**********************************************************************************************************************

    public void OnPlayButton()
    {
        //Set the next UI state
        _UINextState = UIState.SelectDungeonMenu;

        //*********************************
        // UI transition
        //Start the transition
        MakeUITransition();

        _sfxSource.clip = _clickSound;
        _sfxSource.Play();
    }

    public void OnOptionsButton()
    {
        //*********************************
        // UI transition

        //Set the next UI state
        _UINextState = UIState.OptionsMenu;
        //Start the transition
        MakeUITransition();

        _sfxSource.clip = _clickSound;
        _sfxSource.Play();
    }

    public void BackToMainMenu()
    {
        //*********************************
        // UI transition

        //Set the next UI state
        _UINextState = UIState.MainMenu;
        //Start the transition
        MakeUITransition();

        _sfxSource.clip = _clickSound;
        _sfxSource.Play();
    }

    public void OnSelectDungeonElementButton(GameObject button)
    {
        //*********************************
        // UI transition

        //Set the next UI state
        if (button.name.Contains("Wind"))
        {
            _UINextState = UIState.WindLevelsMenu;
        }
        else if (button.name.Contains("Ice"))
        {
            _UINextState = UIState.IceLevelsMenu;
        }
        else if (button.name.Contains("Earth"))
        {
            _UINextState = UIState.EarthLevelsMenu;
        }
        else if (button.name.Contains("Fire"))
        {
            _UINextState = UIState.FireLevelsMenu;
        }
        else if (button.name.Contains("All"))
        {
            _UINextState = UIState.AllLevelsMenu;
        }

        //Start the transition
        MakeUITransition();

        _sfxSource.clip = _clickSound;
        _sfxSource.Play();
    }

    public void OnLevelButton(GameObject button)
    {
        _levelButton = button;

        if (_levelButton != null)
        {
            // Get the star count
            List<SaveGameController.LevelProgressData> levelsData = SaveGameController.instance.GetLevelProgress();
            bool found = false;

            for (int i = 0; i < levelsData.Count && !found; i++)
            {
                if (_levelButton.name == levelsData[i].Id)
                {
                    _timeOnLevel = levelsData[i].Score;

                    float threeStarsTime = SaveGameController.instance.GetThreeStarsTime(levelsData[i].Id);
                    float twoStarsTime = SaveGameController.instance.GetTwoStarsTime(levelsData[i].Id);

                    if (_timeOnLevel <= threeStarsTime)
                    {
                        _starsOnLevel = 3;
                    }
                    else
                    {
                        if (_timeOnLevel > threeStarsTime && _timeOnLevel <= twoStarsTime)
                        {
                            _starsOnLevel = 2;
                        }
                        else
                        {
                            if (_timeOnLevel > twoStarsTime)
                            {
                                _starsOnLevel = 1;
                            }
                            else
                            {
                                _starsOnLevel = 0;
                            }
                        }
                    }

                    found = true;
                }
            }

            if (!found)
            {
#if UNITY_EDITOR
                Debug.LogError("There is no level with the name: " + _levelButton.name + "!");
                Debug.LogError("Stars on level: " + _starsOnLevel);
#endif
                _starsOnLevel = 0;
                _timeOnLevel = 0f;
            }

            SaveGameController.instance.SetTargetLevel(_levelButton.name);

            // Get the use count
            TextAsset txLevel = Resources.Load("Levels/" + _levelButton.name) as TextAsset;
            JSONObject jsLevel = new JSONObject(txLevel.text);

            _windUses = (int)jsLevel["player"]["wind"].n;
            _iceUses = (int)jsLevel["player"]["ice"].n;
            _fireUses = (int)jsLevel["player"]["fire"].n;
            _earthUses = (int)jsLevel["player"]["earth"].n;

            Resources.UnloadAsset(txLevel);

            //*********************************
            // UI transition
            // Set the next UI state
            _UINextState = UIState.SelectionLevelMenu;

            // Start the transition
            MakeUITransition();
        }

        _sfxSource.clip = _clickSound;
        _sfxSource.Play();
    }

    public void BackToSelectDungeonMenu()
    {
        //*********************************
        // UI transition

        //Set the next UI state
        _UINextState = UIState.SelectDungeonMenu;
        //Start the transition
        MakeUITransition();

        _sfxSource.clip = _clickSound;
        _sfxSource.Play();
    }

    public void BackToLevelsSelectionMenu()
    {
        //*********************************
        // UI transition
        // Set the next UI state
        _UINextState = _UIPreviousState;
        // Start the transition
        MakeUITransition();

        _sfxSource.clip = _clickSound;
        _sfxSource.Play();
    }

    public void GoToPlayScene()
    {
        //*********************************
        // UI transition
        // Set the next UI state
        _UINextState = UIState.PlayState;
        // Start the transition
        MakeUITransition();
    }

    //**********************************************************************************************************************
    //      TRANSITION METHODS
    //**********************************************************************************************************************

    /* MakeUITransition()
     * 
     * - Call this method to change between two interfaces
     * 
     * @param initNextTransition
     *     Indicates if the transition to be shown is
     *     the INIT transition of the next UIState.
     *      
     */
    private void MakeUITransition(bool initNextTransition = false)
    {

        //Check if this method was called when a transition ended and we want to initiate a transition to the _UINextState
        //or if it was the call to start a whole transition between two UIstates
        if (initNextTransition)
        {
            _transitionType = TransitionType.Init;
            _UIPreviousState = _UIActualState;
            _UIActualState = _UINextState;
            _transitionState = _UINextState;
        }
        else
        {
            _transitionType = TransitionType.Finish;
            _transitionState = _UIActualState;
        }

        switch (_UIActualState)
        {
            case UIState.Intro:
                IntroTransition();
                break;

            case UIState.MainMenu:
                MainMenuTransition();
                break;

            case UIState.OptionsMenu:
                OptionsMenuTransition();
                break;

            case UIState.SelectDungeonMenu:
                SelectDungeonMenuTransition();
                break;

            case UIState.WindLevelsMenu:
                WindLevelsMenuTransition();
                break;

            case UIState.EarthLevelsMenu:
                EarthLevelsMenuTransition();
                break;

            case UIState.IceLevelsMenu:
                IceLevelsMenuTransition();
                break;

            case UIState.FireLevelsMenu:
                FireLevelsMenuTransition();
                break;

            case UIState.AllLevelsMenu:
                AllLevelsMenuTransition();
                break;

            case UIState.FirstLevelsMenu:
                FirstLevelsMenuTransition();
                break;

            case UIState.SelectionLevelMenu:
                SelectionLevelMenuTransition();
                break;

            case UIState.TutorialState:
                TutorialStateTransition();
                break;
        }
    }
    //END MakeUITransition()

    //****************************************************************************
    // The following methods control the INIT/FINISH transitions of each interface

    /* IntroTransition()
     * 
     * - The method that controls the Intro UIState transitions
     * 
     * (In that case, this methods do nothing because the Intro
     *  interface is an auxiliar one)
     *      
     */

    private void IntroTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    // In this case do nothing because is an auxiliar interface
                    // and has nothing.

                }
                else //During _onTransition calls
                     // This part is called during the transition
                     // Use it to move, fade, etc. the interface objects
                     // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //In this case nothing is needed
                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                    // here to ensure the INIT transition of the UINextState is called
                    MakeUITransition(INIT_NEXT_TRANSITION);
                }
                else //During _onTransition calls
                     // This part is called during the transition
                     // Use it to move, fade, etc. the interface objects
                     // NOTE: Must detect the end of the transition and set _onTransition variable to false
                     // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                     // to Init the next transition if needed
                {

                }

                break;
        }
    }
    // END IntroTransition()

    /* MainMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void MainMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //MainMenu
                    _interfaces.MainMenu[0].SetActive(true);

                    //Black Image
                    _interfaces.MainMenu[4].SetActive(true);
                    _interfaces.MainMenu[4].GetComponent<Image>().color = Color.black * (_UIPreviousState != UIState.OptionsMenu ? 1f : 0.5f);


                    if (_UIPreviousState != UIState.OptionsMenu && _UIPreviousState != UIState.SelectDungeonMenu)
                    {
                        //Logo
                        _interfaces.MainMenu[1].transform.localScale = Vector3.zero;

                        //Buttons
                        _interfaces.MainMenu[2].transform.localScale = Vector3.zero;
                        _interfaces.MainMenu[3].transform.localScale = Vector3.zero;
                    }
                    else
                    {
                        //Logo
                        _interfaces.MainMenu[1].transform.localScale = Vector3.one;

                        //Buttons
                        _interfaces.MainMenu[2].transform.localScale = Vector3.one;
                        _interfaces.MainMenu[3].transform.localScale = Vector3.one;
                    }

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Black Image
                    if (_transitionCounterTime + _mainMenuBlackImageStartTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + _mainMenuBlackImageEndTime >= Time.realtimeSinceStartup)
                    {
                        float alpha = (_UIPreviousState != UIState.OptionsMenu ? 1f : 0.5f) - (Time.realtimeSinceStartup - _transitionCounterTime - _mainMenuBlackImageStartTime) * ((_UIPreviousState != UIState.OptionsMenu ? 1f : 0.5f) / (_mainMenuBlackImageEndTime - _mainMenuBlackImageStartTime));
                        _interfaces.MainMenu[4].GetComponent<Image>().color = Color.black * alpha;
                    }

                    if (_UIPreviousState != UIState.OptionsMenu && _UIPreviousState != UIState.SelectDungeonMenu)
                    {
                        //Logo
                        if (_transitionCounterTime + _mainMenuLogoStartTime < Time.realtimeSinceStartup
                            && _transitionCounterTime + _mainMenuLogoEndTime >= Time.realtimeSinceStartup)
                        {
                            float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _mainMenuLogoStartTime) * (1f / (_mainMenuLogoEndTime - _mainMenuLogoStartTime));
                            _interfaces.MainMenu[1].transform.localScale = Vector3.one * scale;
                        }

                        //Buttons
                        if (_transitionCounterTime + _mainMenuButtonsStartTime < Time.realtimeSinceStartup
                            && _transitionCounterTime + _mainMenuButtonsEndTime >= Time.realtimeSinceStartup)
                        {
                            float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _mainMenuButtonsStartTime) * (1f / (_mainMenuButtonsEndTime - _mainMenuButtonsStartTime));
                            _interfaces.MainMenu[2].transform.localScale = Vector3.one * scale;
                            _interfaces.MainMenu[3].transform.localScale = Vector3.one * scale;
                        }
                    }

                    //End of transition
                    if (_transitionCounterTime + (_UIPreviousState != UIState.OptionsMenu ? MAIN_MENU_INTRO_DURATION : MAIN_MENU_OUTRO_DURATION) <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Black Image
                        _interfaces.MainMenu[4].SetActive(false);
                        _interfaces.MainMenu[4].GetComponent<Image>().color = Color.clear;

                        //Logo
                        _interfaces.MainMenu[1].transform.localScale = Vector3.one;

                        //Buttons
                        _interfaces.MainMenu[2].transform.localScale = Vector3.one;
                        _interfaces.MainMenu[3].transform.localScale = Vector3.one;


                    }
                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Black Image
                    _interfaces.MainMenu[4].SetActive(true);
                    _interfaces.MainMenu[4].GetComponent<Image>().color = Color.clear;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {
                    //Black Image
                    if (_transitionCounterTime + _mainMenuBlackImageOutroStartTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + _mainMenuBlackImageOutroEndTime >= Time.realtimeSinceStartup)
                    {
                        float alpha = (Time.realtimeSinceStartup - _transitionCounterTime - _mainMenuBlackImageOutroStartTime) * ((_UINextState != UIState.OptionsMenu ? 1f : 0.5f) / (_mainMenuBlackImageOutroEndTime - _mainMenuBlackImageOutroStartTime));
                        _interfaces.MainMenu[4].GetComponent<Image>().color = Color.black * alpha;
                    }

                    //End of transition
                    if (_transitionCounterTime + MAIN_MENU_OUTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Black Image
                        _interfaces.MainMenu[4].GetComponent<Image>().color = Color.black * (_UINextState != UIState.OptionsMenu ? 1f : 0.5f);

                        //Main Menu
                        if (_UINextState != UIState.OptionsMenu)
                        {
                            _interfaces.MainMenu[0].SetActive(false);

                            // Check if it is the first play. If it is not,
                            // proceed to the totem. If it is, fade to black
                            // and go to the tutorial scene.
                            List<SaveGameController.LevelProgressData> levelsData = SaveGameController.instance.GetLevelProgress();

                            if (levelsData.Count < 3)
                            {
                                #if UNITY_EDITOR
                                Debug.Log("Loading tutorial...");
                                #endif

                                SaveGameController.instance.SetTargetLevel("0_02");
                                UnityEngine.SceneManagement.SceneManager.LoadScene("game");
                            }
                        }

                        // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                        // here to ensure the INIT transition of the UINextState is called
                        MakeUITransition(INIT_NEXT_TRANSITION);
                    }
                }

                break;
        }
    }
    // END MainMenuTransition()

    /* OptionsMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void OptionsMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Options Menu
                    _interfaces.OptionsMenu[0].SetActive(true);
                    _interfaces.OptionsMenu[0].transform.localScale = Vector3.zero;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Dialog scale
                    if (_transitionCounterTime + OPTIONS_MENU_DURATION >= Time.realtimeSinceStartup)
                    {
                        float scale = (Time.realtimeSinceStartup - _transitionCounterTime) * (1f / (OPTIONS_MENU_DURATION));
                        _interfaces.OptionsMenu[0].transform.localScale = Vector3.one * scale;
                    }

                    //End of transition
                    if (_transitionCounterTime + OPTIONS_MENU_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Options Menu
                        _interfaces.OptionsMenu[0].transform.localScale = Vector3.one;

                    }
                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Options Menu
                    _interfaces.OptionsMenu[0].SetActive(true);
                    _interfaces.OptionsMenu[0].transform.localScale = Vector3.one;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {
                    //Dialog scale
                    if (_transitionCounterTime + OPTIONS_MENU_DURATION >= Time.realtimeSinceStartup)
                    {
                        float scale = 1f - (Time.realtimeSinceStartup - _transitionCounterTime) * (1f / (OPTIONS_MENU_DURATION));
                        _interfaces.OptionsMenu[0].transform.localScale = Vector3.one * scale;
                    }

                    //End of transition
                    if (_transitionCounterTime + OPTIONS_MENU_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Options Menu
                        _interfaces.OptionsMenu[0].transform.localScale = Vector3.zero;
                        _interfaces.OptionsMenu[0].SetActive(false);

                        // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                        // here to ensure the INIT transition of the UINextState is called
                        MakeUITransition(INIT_NEXT_TRANSITION);
                    }
                }

                break;
        }
    }
    // END OptionsMenuTransition()

    /* SelectDungeonMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void SelectDungeonMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //MainMenu
                    _interfaces.SelectDungeonMenu[0].SetActive(true);

                    //Black Image
                    _interfaces.SelectDungeonMenu[1].SetActive(true);
                    _interfaces.SelectDungeonMenu[1].GetComponent<Image>().color = Color.black;

                    //Buttons
                    _interfaces.SelectDungeonMenu[2].transform.position = _dungeonButtonWindPos;
                    _interfaces.SelectDungeonMenu[3].transform.position = _dungeonButtonEarthPos;
                    _interfaces.SelectDungeonMenu[4].transform.position = _dungeonButtonFirePos;
                    _interfaces.SelectDungeonMenu[5].transform.position = _dungeonButtonIcePos;
                    _interfaces.SelectDungeonMenu[6].transform.position = _dungeonButtonAllPos;

                    _interfaces.SelectDungeonMenu[2].GetComponent<ButtonVibration>()._isVibrating = true;
                    _interfaces.SelectDungeonMenu[3].GetComponent<ButtonVibration>()._isVibrating = true;
                    _interfaces.SelectDungeonMenu[4].GetComponent<ButtonVibration>()._isVibrating = true;
                    _interfaces.SelectDungeonMenu[5].GetComponent<ButtonVibration>()._isVibrating = true;
                    _interfaces.SelectDungeonMenu[6].GetComponent<ButtonVibration>()._isVibrating = true;

                    _windMoveStarted = true; _windMoveEnded = true;
                    _earthMoveStarted = true; _earthMoveEnded = true;
                    _iceMoveStarted = true; _iceMoveEnded = true;
                    _fireMoveStarted = true; _fireMoveEnded = true;
                    _allMoveStarted = true; _allMoveEnded = true;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Black Image
                    float alpha;
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + _selectDungeonMenuBlackImageEndTime >= Time.realtimeSinceStartup)
                    {
                        alpha = _selectDungeonMenuBlackImageEndTime - Time.realtimeSinceStartup + _transitionCounterTime;
                        _interfaces.SelectDungeonMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    //Wind Button
                    if (_transitionCounterTime + _selectDungeonMenuButtonWindStartTime < Time.realtimeSinceStartup)
                    {
                        if (_transitionCounterTime + windEndTime >= Time.realtimeSinceStartup)
                        {
                            if (_windMoveStarted)
                            {
                                _interfaces.SelectDungeonMenu[2].GetComponent<ButtonVibration>()._isVibrating = false;
                                _windMoveStarted = false;
                            }
                            transition_value = (Time.realtimeSinceStartup - _transitionCounterTime - _selectDungeonMenuButtonWindStartTime) * buttonTime;
                            _interfaces.SelectDungeonMenu[2].transform.position = _dungeonButtonWindPos + _dungeonButtonWindPosOffset * transition_value;
                        }
                        else if (_windMoveEnded)
                        {
                            _interfaces.SelectDungeonMenu[2].transform.position = _dungeonButtonWindPos + _dungeonButtonWindPosOffset;
                            _interfaces.SelectDungeonMenu[2].GetComponent<ButtonVibration>().StartSmoothing();
                            _windMoveEnded = false;
                        }
                    }

                    //Earth Button                                        
                    if (_transitionCounterTime + _selectDungeonMenuButtonEarthStartTime < Time.realtimeSinceStartup)
                    {
                        if (_transitionCounterTime + earthEndTime >= Time.realtimeSinceStartup)
                        {
                            if (_earthMoveStarted)
                            {
                                _interfaces.SelectDungeonMenu[3].GetComponent<ButtonVibration>()._isVibrating = false;
                                _earthMoveStarted = false;
                            }
                            transition_value = (Time.realtimeSinceStartup - _transitionCounterTime - _selectDungeonMenuButtonEarthStartTime) * buttonTime;
                            _interfaces.SelectDungeonMenu[3].transform.position = _dungeonButtonEarthPos + _dungeonButtonEarthPosOffset * transition_value;
                        }
                        else if (_earthMoveEnded)
                        {
                            _interfaces.SelectDungeonMenu[3].transform.position = _dungeonButtonEarthPos + _dungeonButtonEarthPosOffset;
                            _interfaces.SelectDungeonMenu[3].GetComponent<ButtonVibration>().StartSmoothing();
                            _earthMoveEnded = false;
                        }
                    }

                    //Fire Button                    
                    if (_transitionCounterTime + _selectDungeonMenuButtonFireStartTime < Time.realtimeSinceStartup)
                    {
                        if (_transitionCounterTime + fireEndTime >= Time.realtimeSinceStartup)
                        {
                            if (_fireMoveStarted)
                            {
                                _interfaces.SelectDungeonMenu[5].GetComponent<ButtonVibration>()._isVibrating = false;
                                _fireMoveStarted = false;
                            }
                            transition_value = (Time.realtimeSinceStartup - _transitionCounterTime - _selectDungeonMenuButtonFireStartTime) * buttonTime;
                            _interfaces.SelectDungeonMenu[5].transform.position = _dungeonButtonFirePos + _dungeonButtonFirePosOffset * transition_value;
                        }
                        else if (_fireMoveEnded)
                        {
                            _interfaces.SelectDungeonMenu[5].transform.position = _dungeonButtonFirePos + _dungeonButtonFirePosOffset;
                            _interfaces.SelectDungeonMenu[5].GetComponent<ButtonVibration>().StartSmoothing();
                            _fireMoveEnded = false;
                        }
                    }

                    //Ice Button                    
                    if (_transitionCounterTime + _selectDungeonMenuButtonIceStartTime < Time.realtimeSinceStartup)
                    {
                        if (_transitionCounterTime + iceEndTime >= Time.realtimeSinceStartup)
                        {
                            if (_iceMoveStarted)
                            {
                                _interfaces.SelectDungeonMenu[4].GetComponent<ButtonVibration>()._isVibrating = false;
                                _iceMoveStarted = false;
                            }
                            transition_value = (Time.realtimeSinceStartup - _transitionCounterTime - _selectDungeonMenuButtonIceStartTime) * buttonTime;
                            _interfaces.SelectDungeonMenu[4].transform.position = _dungeonButtonIcePos + _dungeonButtonIcePosOffset * transition_value;
                        }
                        else if (_iceMoveEnded)
                        {
                            _interfaces.SelectDungeonMenu[4].transform.position = _dungeonButtonIcePos + _dungeonButtonIcePosOffset;
                            _interfaces.SelectDungeonMenu[4].GetComponent<ButtonVibration>().StartSmoothing();
                            _iceMoveEnded = false;
                        }
                    }

                    //All Button
                    if (_transitionCounterTime + _selectDungeonMenuButtonAllStartTime < Time.realtimeSinceStartup)
                    {
                        if (_transitionCounterTime + allEndTime >= Time.realtimeSinceStartup)
                        {
                            if (_allMoveStarted)
                            {
                                _interfaces.SelectDungeonMenu[6].GetComponent<ButtonVibration>()._isVibrating = false;
                                _allMoveStarted = false;
                            }
                            _interfaces.SelectDungeonMenu[6].GetComponent<ButtonVibration>()._isVibrating = false;
                            transition_value = (Time.realtimeSinceStartup - _transitionCounterTime - _selectDungeonMenuButtonAllStartTime) * buttonTime;
                            _interfaces.SelectDungeonMenu[6].transform.position = _dungeonButtonAllPos + _dungeonButtonAllPosOffset * transition_value;
                        }
                        else if (_allMoveEnded)
                        {
                            _interfaces.SelectDungeonMenu[6].transform.position = _dungeonButtonAllPos + _dungeonButtonAllPosOffset;
                            _interfaces.SelectDungeonMenu[6].GetComponent<ButtonVibration>().StartSmoothing();
                            _allMoveEnded = false;
                        }
                    }

                    //End of transition
                    if (_transitionCounterTime + SELECT_DUNGEON_MENU_INTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //BlackImage
                        _interfaces.SelectDungeonMenu[1].SetActive(false);
                    }
                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {

                    //Black Image
                    _interfaces.SelectDungeonMenu[1].SetActive(true);
                    _interfaces.SelectDungeonMenu[1].GetComponent<Image>().color = Color.clear;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {

                    //Black Image
                    float alpha;
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + SELECT_DUNGEON_MENU_OUTRO_DURATION >= Time.realtimeSinceStartup)
                    {
                        alpha = SELECT_DUNGEON_MENU_OUTRO_DURATION - Time.realtimeSinceStartup + _transitionCounterTime;
                        _interfaces.SelectDungeonMenu[1].GetComponent<Image>().color = Color.black * (1f - alpha);
                    }

                    //End of transition
                    if (_transitionCounterTime + SELECT_DUNGEON_MENU_OUTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //SelectDungeonMenu
                        _interfaces.SelectDungeonMenu[0].SetActive(false);

                        // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                        // here to ensure the INIT transition of the UINextState is called
                        MakeUITransition(INIT_NEXT_TRANSITION);
                    }
                }

                break;
        }
    }
    // END SelectDungeonMenuTransition()

    /* WindLevelsMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void WindLevelsMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //MainMenu
                    _interfaces.WindLevelsMenu[0].SetActive(true);

                    //Black Image
                    _interfaces.WindLevelsMenu[1].SetActive(true);
                    _interfaces.WindLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UIPreviousState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                    //Buttons
                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        for (int i = 2; i < _interfaces.WindLevelsMenu.Length; i++)
                        {
                            _interfaces.WindLevelsMenu[i].transform.localScale = Vector3.zero;
                        }
                    }
                    else
                    {
                        for (int i = 2; i < _interfaces.WindLevelsMenu.Length; i++)
                        {
                            _interfaces.WindLevelsMenu[i].transform.localScale = Vector3.one;
                        }
                    }

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Black Image
                    float alpha;
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + _levelsMenuBlackImageEndTime >= Time.realtimeSinceStartup)
                    {
                        alpha = _levelsMenuBlackImageEndTime - Time.realtimeSinceStartup + _transitionCounterTime;
                        _interfaces.WindLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        //Buttons
                        for (int i = 0; i < _interfaces.WindLevelsMenu.Length - LEVELS_MENU_BUTTON_OFFSET; i++)
                        {
                            if (_transitionCounterTime + _levelMenuButtonsStartTime[i] < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + _levelMenuButtonsEndTime[i] >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelMenuButtonsStartTime[i]) * (1f / (_levelMenuButtonsEndTime[i] - _levelMenuButtonsStartTime[i]));
                                    _interfaces.WindLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.WindLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one;
                                }
                            }

                        }

                        //BackButton
                        if (_transitionCounterTime + _levelsMenuBackButtonStartTime < Time.realtimeSinceStartup
                            && _transitionCounterTime + _levelsMenuBackButtonEndTime >= Time.realtimeSinceStartup)
                        {
                            float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelsMenuBackButtonStartTime) * (1f / (_levelsMenuBackButtonEndTime - _levelsMenuBackButtonStartTime));
                            _interfaces.WindLevelsMenu[2].transform.localScale = Vector3.one * scale;
                        }
                    }


                    //End of transition
                    if (_transitionCounterTime + SELECT_DUNGEON_MENU_INTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //BlackImage
                        _interfaces.WindLevelsMenu[1].SetActive(false);

                        //Back Button
                        if (_UIPreviousState != UIState.SelectionLevelMenu)
                        {
                            _interfaces.WindLevelsMenu[2].transform.localScale = Vector3.one;
                        }

                    }

                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Black Image
                    _interfaces.WindLevelsMenu[1].SetActive(true);
                    _interfaces.WindLevelsMenu[1].GetComponent<Image>().color = Color.clear;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {
                    //Black Image
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + LEVELS_MENU_OUTRO_DURATION >= Time.realtimeSinceStartup)
                    {
                        float alpha = (Time.realtimeSinceStartup - _transitionCounterTime) * ((_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f) / (LEVELS_MENU_OUTRO_DURATION));
                        _interfaces.WindLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    //End of transition
                    if (_transitionCounterTime + LEVELS_MENU_OUTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Black Image
                        _interfaces.WindLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                        //WindLevelsMenu
                        if (_UINextState != UIState.SelectionLevelMenu)
                            _interfaces.WindLevelsMenu[0].SetActive(false);

                        // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                        // here to ensure the INIT transition of the UINextState is called
                        MakeUITransition(INIT_NEXT_TRANSITION);
                    }
                }

                break;
        }
    }
    // END WindLevelsMenuTransition()

    /* EarthLevelsMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void EarthLevelsMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //MainMenu
                    _interfaces.EarthLevelsMenu[0].SetActive(true);

                    //Black Image
                    _interfaces.EarthLevelsMenu[1].SetActive(true);
                    _interfaces.EarthLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UIPreviousState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                    //Buttons
                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        for (int i = 2; i < _interfaces.EarthLevelsMenu.Length; i++)
                        {
                            _interfaces.EarthLevelsMenu[i].transform.localScale = Vector3.zero;
                        }
                    }
                    else
                    {
                        for (int i = 2; i < _interfaces.EarthLevelsMenu.Length; i++)
                        {
                            _interfaces.EarthLevelsMenu[i].transform.localScale = Vector3.one;
                        }
                    }

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Black Image
                    float alpha;
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + _levelsMenuBlackImageEndTime >= Time.realtimeSinceStartup)
                    {
                        alpha = _levelsMenuBlackImageEndTime - Time.realtimeSinceStartup + _transitionCounterTime;
                        _interfaces.EarthLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        //Buttons
                        for (int i = 0; i < _interfaces.EarthLevelsMenu.Length - LEVELS_MENU_BUTTON_OFFSET; i++)
                        {
                            if (_transitionCounterTime + _levelMenuButtonsStartTime[i] < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + _levelMenuButtonsEndTime[i] >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelMenuButtonsStartTime[i]) * (1f / (_levelMenuButtonsEndTime[i] - _levelMenuButtonsStartTime[i]));
                                    _interfaces.EarthLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.EarthLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one;
                                }
                            }

                        }

                        //BackButton
                        if (_transitionCounterTime + _levelsMenuBackButtonStartTime < Time.realtimeSinceStartup
                            && _transitionCounterTime + _levelsMenuBackButtonEndTime >= Time.realtimeSinceStartup)
                        {
                            float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelsMenuBackButtonStartTime) * (1f / (_levelsMenuBackButtonEndTime - _levelsMenuBackButtonStartTime));
                            _interfaces.EarthLevelsMenu[2].transform.localScale = Vector3.one * scale;
                        }
                    }


                    //End of transition
                    if (_transitionCounterTime + SELECT_DUNGEON_MENU_INTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //BlackImage
                        _interfaces.EarthLevelsMenu[1].SetActive(false);

                        //Back Button
                        if (_UIPreviousState != UIState.SelectionLevelMenu)
                        {
                            _interfaces.EarthLevelsMenu[2].transform.localScale = Vector3.one;
                        }

                    }

                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Black Image
                    _interfaces.EarthLevelsMenu[1].SetActive(true);
                    _interfaces.EarthLevelsMenu[1].GetComponent<Image>().color = Color.clear;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {
                    //Black Image
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + LEVELS_MENU_OUTRO_DURATION >= Time.realtimeSinceStartup)
                    {
                        float alpha = (Time.realtimeSinceStartup - _transitionCounterTime) * ((_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f) / (LEVELS_MENU_OUTRO_DURATION));
                        _interfaces.EarthLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    //End of transition
                    if (_transitionCounterTime + LEVELS_MENU_OUTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Black Image
                        _interfaces.EarthLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                        //WindLevelsMenu
                        if (_UINextState != UIState.SelectionLevelMenu)
                            _interfaces.EarthLevelsMenu[0].SetActive(false);

                        // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                        // here to ensure the INIT transition of the UINextState is called
                        MakeUITransition(INIT_NEXT_TRANSITION);
                    }
                }

                break;
        }
    }
    // END EarthLevelsMenuTransition()

    /* IceLevelsMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void IceLevelsMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //MainMenu
                    _interfaces.IceLevelsMenu[0].SetActive(true);

                    //Black Image
                    _interfaces.IceLevelsMenu[1].SetActive(true);
                    _interfaces.IceLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UIPreviousState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                    //Buttons
                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        for (int i = 2; i < _interfaces.IceLevelsMenu.Length; i++)
                        {
                            _interfaces.IceLevelsMenu[i].transform.localScale = Vector3.zero;
                        }
                    }
                    else
                    {
                        for (int i = 2; i < _interfaces.IceLevelsMenu.Length; i++)
                        {
                            _interfaces.IceLevelsMenu[i].transform.localScale = Vector3.one;
                        }
                    }

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Black Image
                    float alpha;
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + _levelsMenuBlackImageEndTime >= Time.realtimeSinceStartup)
                    {
                        alpha = _levelsMenuBlackImageEndTime - Time.realtimeSinceStartup + _transitionCounterTime;
                        _interfaces.IceLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        //Buttons
                        for (int i = 0; i < _interfaces.IceLevelsMenu.Length - LEVELS_MENU_BUTTON_OFFSET; i++)
                        {
                            if (_transitionCounterTime + _levelMenuButtonsStartTime[i] < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + _levelMenuButtonsEndTime[i] >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelMenuButtonsStartTime[i]) * (1f / (_levelMenuButtonsEndTime[i] - _levelMenuButtonsStartTime[i]));
                                    _interfaces.IceLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.IceLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one;
                                }
                            }

                        }

                        //BackButton
                        if (_transitionCounterTime + _levelsMenuBackButtonStartTime < Time.realtimeSinceStartup
                            && _transitionCounterTime + _levelsMenuBackButtonEndTime >= Time.realtimeSinceStartup)
                        {
                            float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelsMenuBackButtonStartTime) * (1f / (_levelsMenuBackButtonEndTime - _levelsMenuBackButtonStartTime));
                            _interfaces.IceLevelsMenu[2].transform.localScale = Vector3.one * scale;
                        }
                    }


                    //End of transition
                    if (_transitionCounterTime + SELECT_DUNGEON_MENU_INTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //BlackImage
                        _interfaces.IceLevelsMenu[1].SetActive(false);

                        //Back Button
                        if (_UIPreviousState != UIState.SelectionLevelMenu)
                        {
                            _interfaces.IceLevelsMenu[2].transform.localScale = Vector3.one;
                        }

                    }

                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Black Image
                    _interfaces.IceLevelsMenu[1].SetActive(true);
                    _interfaces.IceLevelsMenu[1].GetComponent<Image>().color = Color.clear;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {
                    //Black Image
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + LEVELS_MENU_OUTRO_DURATION >= Time.realtimeSinceStartup)
                    {
                        float alpha = (Time.realtimeSinceStartup - _transitionCounterTime) * ((_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f) / (LEVELS_MENU_OUTRO_DURATION));
                        _interfaces.IceLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    //End of transition
                    if (_transitionCounterTime + LEVELS_MENU_OUTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Black Image
                        _interfaces.IceLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                        //WindLevelsMenu
                        if (_UINextState != UIState.SelectionLevelMenu)
                            _interfaces.IceLevelsMenu[0].SetActive(false);

                        // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                        // here to ensure the INIT transition of the UINextState is called
                        MakeUITransition(INIT_NEXT_TRANSITION);
                    }
                }

                break;
        }
    }
    // END IceLevelsMenuTransition()

    /* FireLevelsMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void FireLevelsMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //MainMenu
                    _interfaces.FireLevelsMenu[0].SetActive(true);

                    //Black Image
                    _interfaces.FireLevelsMenu[1].SetActive(true);
                    _interfaces.FireLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UIPreviousState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                    //Buttons
                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        for (int i = 2; i < _interfaces.FireLevelsMenu.Length; i++)
                        {
                            _interfaces.FireLevelsMenu[i].transform.localScale = Vector3.zero;
                        }
                    }
                    else
                    {
                        for (int i = 2; i < _interfaces.FireLevelsMenu.Length; i++)
                        {
                            _interfaces.FireLevelsMenu[i].transform.localScale = Vector3.one;
                        }
                    }

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Black Image
                    float alpha;
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + _levelsMenuBlackImageEndTime >= Time.realtimeSinceStartup)
                    {
                        alpha = _levelsMenuBlackImageEndTime - Time.realtimeSinceStartup + _transitionCounterTime;
                        _interfaces.FireLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        //Buttons
                        for (int i = 0; i < _interfaces.FireLevelsMenu.Length - LEVELS_MENU_BUTTON_OFFSET; i++)
                        {
                            if (_transitionCounterTime + _levelMenuButtonsStartTime[i] < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + _levelMenuButtonsEndTime[i] >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelMenuButtonsStartTime[i]) * (1f / (_levelMenuButtonsEndTime[i] - _levelMenuButtonsStartTime[i]));
                                    _interfaces.FireLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.FireLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one;
                                }
                            }

                        }

                        //BackButton
                        if (_transitionCounterTime + _levelsMenuBackButtonStartTime < Time.realtimeSinceStartup
                            && _transitionCounterTime + _levelsMenuBackButtonEndTime >= Time.realtimeSinceStartup)
                        {
                            float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelsMenuBackButtonStartTime) * (1f / (_levelsMenuBackButtonEndTime - _levelsMenuBackButtonStartTime));
                            _interfaces.FireLevelsMenu[2].transform.localScale = Vector3.one * scale;
                        }
                    }


                    //End of transition
                    if (_transitionCounterTime + SELECT_DUNGEON_MENU_INTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //BlackImage
                        _interfaces.FireLevelsMenu[1].SetActive(false);

                        //Back Button
                        if (_UIPreviousState != UIState.SelectionLevelMenu)
                        {
                            _interfaces.FireLevelsMenu[2].transform.localScale = Vector3.one;
                        }

                    }

                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Black Image
                    _interfaces.FireLevelsMenu[1].SetActive(true);
                    _interfaces.FireLevelsMenu[1].GetComponent<Image>().color = Color.clear;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {
                    //Black Image
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + LEVELS_MENU_OUTRO_DURATION >= Time.realtimeSinceStartup)
                    {
                        float alpha = (Time.realtimeSinceStartup - _transitionCounterTime) * ((_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f) / (LEVELS_MENU_OUTRO_DURATION));
                        _interfaces.FireLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    //End of transition
                    if (_transitionCounterTime + LEVELS_MENU_OUTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Black Image
                        _interfaces.FireLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                        //WindLevelsMenu
                        if (_UINextState != UIState.SelectionLevelMenu)
                            _interfaces.FireLevelsMenu[0].SetActive(false);

                        // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                        // here to ensure the INIT transition of the UINextState is called
                        MakeUITransition(INIT_NEXT_TRANSITION);
                    }
                }

                break;
        }
    }
    // END FireLevelsMenuTransition()

    /* AllLevelsMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void AllLevelsMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //MainMenu
                    _interfaces.AllLevelsMenu[0].SetActive(true);

                    //Black Image
                    _interfaces.AllLevelsMenu[1].SetActive(true);
                    _interfaces.AllLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UIPreviousState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                    //Buttons
                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        for (int i = 2; i < _interfaces.AllLevelsMenu.Length; i++)
                        {
                            _interfaces.AllLevelsMenu[i].transform.localScale = Vector3.zero;
                        }
                    }
                    else
                    {
                        for (int i = 2; i < _interfaces.AllLevelsMenu.Length; i++)
                        {
                            _interfaces.AllLevelsMenu[i].transform.localScale = Vector3.one;
                        }
                    }

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Black Image
                    float alpha;
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + _levelsMenuBlackImageEndTime >= Time.realtimeSinceStartup)
                    {
                        alpha = _levelsMenuBlackImageEndTime - Time.realtimeSinceStartup + _transitionCounterTime;
                        _interfaces.AllLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    if (_UIPreviousState != UIState.SelectionLevelMenu)
                    {
                        //Buttons
                        for (int i = 0; i < _interfaces.AllLevelsMenu.Length - LEVELS_MENU_BUTTON_OFFSET; i++)
                        {
                            if (_transitionCounterTime + _levelMenuButtonsStartTime[i] < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + _levelMenuButtonsEndTime[i] >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelMenuButtonsStartTime[i]) * (1f / (_levelMenuButtonsEndTime[i] - _levelMenuButtonsStartTime[i]));
                                    _interfaces.AllLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.AllLevelsMenu[i + LEVELS_MENU_BUTTON_OFFSET].transform.localScale = Vector3.one;
                                }
                            }

                        }

                        //BackButton
                        if (_transitionCounterTime + _levelsMenuBackButtonStartTime < Time.realtimeSinceStartup
                            && _transitionCounterTime + _levelsMenuBackButtonEndTime >= Time.realtimeSinceStartup)
                        {
                            float scale = (Time.realtimeSinceStartup - _transitionCounterTime - _levelsMenuBackButtonStartTime) * (1f / (_levelsMenuBackButtonEndTime - _levelsMenuBackButtonStartTime));
                            _interfaces.AllLevelsMenu[2].transform.localScale = Vector3.one * scale;
                        }
                    }


                    //End of transition
                    if (_transitionCounterTime + SELECT_DUNGEON_MENU_INTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //BlackImage
                        _interfaces.AllLevelsMenu[1].SetActive(false);

                        //Back Button
                        if (_UIPreviousState != UIState.SelectionLevelMenu)
                        {
                            _interfaces.AllLevelsMenu[2].transform.localScale = Vector3.one;
                        }

                    }

                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Black Image
                    _interfaces.AllLevelsMenu[1].SetActive(true);
                    _interfaces.AllLevelsMenu[1].GetComponent<Image>().color = Color.clear;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {
                    //Black Image
                    if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + LEVELS_MENU_OUTRO_DURATION >= Time.realtimeSinceStartup)
                    {
                        float alpha = (Time.realtimeSinceStartup - _transitionCounterTime) * ((_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f) / (LEVELS_MENU_OUTRO_DURATION));
                        _interfaces.AllLevelsMenu[1].GetComponent<Image>().color = Color.black * alpha;
                    }

                    //End of transition
                    if (_transitionCounterTime + LEVELS_MENU_OUTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //Black Image
                        _interfaces.AllLevelsMenu[1].GetComponent<Image>().color = Color.black * (_UINextState != UIState.SelectionLevelMenu ? 1f : 0.5f);

                        //WindLevelsMenu
                        if (_UINextState != UIState.SelectionLevelMenu)
                            _interfaces.AllLevelsMenu[0].SetActive(false);

                        // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                        // here to ensure the INIT transition of the UINextState is called
                        MakeUITransition(INIT_NEXT_TRANSITION);
                    }
                }

                break;
        }
    }
    // END AllLevelsMenuTransition()

    /* FirstLevelsMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void FirstLevelsMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    // In this case do nothing because is an auxiliar interface
                    // and has nothing.

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //In this case nothing is needed
                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                    // here to ensure the INIT transition of the UINextState is called
                    MakeUITransition(INIT_NEXT_TRANSITION);
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {

                }

                break;
        }
    }
    // END FirstLevelsMenuTransition()

    /* SelectionLevelMenuTransition()
     * 
     * - The method that controls the Intro UIState transitions
     *      
     */

    private void SelectionLevelMenuTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Options Menu
                    _interfaces.SelectionLevelMenu[0].SetActive(true);
                    _interfaces.SelectionLevelMenu[0].transform.localScale = Vector3.zero;

                    //Set the time on the hourglass
                    _interfaces.SelectionLevelMenu[1].GetComponent<Text>().text = GetTimeText(_timeOnLevel);

                    //Set the use count for each element
                    _interfaces.SelectionLevelMenu[11].GetComponent<Text>().text = _windUses.ToString();
                    _interfaces.SelectionLevelMenu[12].GetComponent<Text>().text = _iceUses.ToString();
                    _interfaces.SelectionLevelMenu[13].GetComponent<Text>().text = _fireUses.ToString();
                    _interfaces.SelectionLevelMenu[14].GetComponent<Text>().text = _earthUses.ToString();

                    //Stars
                    switch (_starsOnLevel)
                    {
                        case 0:
                            _interfaces.SelectionLevelMenu[2].SetActive(false);
                            _interfaces.SelectionLevelMenu[3].SetActive(false);
                            _interfaces.SelectionLevelMenu[4].SetActive(false);
                            break;

                        case 1:
                            _interfaces.SelectionLevelMenu[2].SetActive(true);
                            _interfaces.SelectionLevelMenu[3].SetActive(false);
                            _interfaces.SelectionLevelMenu[4].SetActive(false);

                            //Size
                            _interfaces.SelectionLevelMenu[2].transform.localScale = Vector3.zero;
                            break;

                        case 2:
                            _interfaces.SelectionLevelMenu[2].SetActive(false);
                            _interfaces.SelectionLevelMenu[3].SetActive(true);
                            _interfaces.SelectionLevelMenu[4].SetActive(false);

                            //Size
                            _interfaces.SelectionLevelMenu[5].transform.localScale = Vector3.zero;
                            _interfaces.SelectionLevelMenu[6].transform.localScale = Vector3.zero;
                            break;

                        case 3:

                            _interfaces.SelectionLevelMenu[2].SetActive(false);
                            _interfaces.SelectionLevelMenu[3].SetActive(false);
                            _interfaces.SelectionLevelMenu[4].SetActive(true);

                            //Size
                            _interfaces.SelectionLevelMenu[7].transform.localScale = Vector3.zero;
                            _interfaces.SelectionLevelMenu[8].transform.localScale = Vector3.zero;
                            _interfaces.SelectionLevelMenu[9].transform.localScale = Vector3.zero;
                            break;

                    }
                    //END TIME
                    SELECTION_LEVEL_MENU_INTRO_MAX_DURATION = _selectionLevelBackImageEndTime + _selectionLevelStarsDuration * _starsOnLevel;

                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //Dialog scale
                    if (_transitionCounterTime < Time.realtimeSinceStartup)
                    {
                        if (_transitionCounterTime + _selectionLevelBackImageEndTime >= Time.realtimeSinceStartup)
                        {
                            float scale = (Time.realtimeSinceStartup - _transitionCounterTime) * (1f / _selectionLevelBackImageEndTime);
                            _interfaces.SelectionLevelMenu[0].transform.localScale = Vector3.one * scale;
                        }
                        else
                        {
                            _interfaces.SelectionLevelMenu[0].transform.localScale = Vector3.one;
                        }
                    }

                    //Stars
                    switch (_starsOnLevel)
                    {
                        case 0:
                            //Do nothing
                            break;

                        case 1:
                            //Star 1 scale
                            float star1StartTime = _selectionLevelBackImageEndTime;
                            float star1EndTime = _selectionLevelBackImageEndTime + _selectionLevelStarsDuration;
                            if (_transitionCounterTime + star1StartTime < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + star1EndTime >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - star1StartTime) * (1f / _selectionLevelStarsDuration);
                                    _interfaces.SelectionLevelMenu[2].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.SelectionLevelMenu[2].transform.localScale = Vector3.one;
                                }
                            }
                            break;

                        case 2:

                            //Star 2_1 scale
                            float star2_1StartTime = _selectionLevelBackImageEndTime;
                            float star2_1EndTime = _selectionLevelBackImageEndTime + _selectionLevelStarsDuration;
                            if (_transitionCounterTime + star2_1StartTime < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + star2_1EndTime >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - star2_1StartTime) * (1f / _selectionLevelStarsDuration);
                                    _interfaces.SelectionLevelMenu[5].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.SelectionLevelMenu[5].transform.localScale = Vector3.one;
                                }
                            }
                            //Star 2_2 scale
                            float star2_2EndTime = star2_1EndTime + _selectionLevelStarsDuration;
                            if (_transitionCounterTime + star2_1EndTime < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + star2_2EndTime >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - star2_1EndTime) * (1f / _selectionLevelStarsDuration);
                                    _interfaces.SelectionLevelMenu[6].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.SelectionLevelMenu[6].transform.localScale = Vector3.one;
                                }
                            }

                            break;

                        case 3:

                            //Star 3_1 scale
                            float star3_1StartTime = _selectionLevelBackImageEndTime;
                            float star3_1EndTime = _selectionLevelBackImageEndTime + _selectionLevelStarsDuration;
                            if (_transitionCounterTime + star3_1StartTime < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + star3_1EndTime >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - star3_1StartTime) * (1f / _selectionLevelStarsDuration);
                                    _interfaces.SelectionLevelMenu[7].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.SelectionLevelMenu[7].transform.localScale = Vector3.one;
                                }
                            }
                            //Star 3_2 scale
                            float star3_2EndTime = star3_1EndTime + _selectionLevelStarsDuration;
                            if (_transitionCounterTime + star3_1EndTime < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + star3_2EndTime >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - star3_1EndTime) * (1f / _selectionLevelStarsDuration);
                                    _interfaces.SelectionLevelMenu[8].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.SelectionLevelMenu[8].transform.localScale = Vector3.one;
                                }
                            }

                            //Star 3_3 scale
                            float star3_3EndTime = star3_2EndTime + _selectionLevelStarsDuration;
                            if (_transitionCounterTime + star3_2EndTime < Time.realtimeSinceStartup)
                            {
                                if (_transitionCounterTime + star3_3EndTime >= Time.realtimeSinceStartup)
                                {
                                    float scale = (Time.realtimeSinceStartup - _transitionCounterTime - star3_2EndTime) * (1f / _selectionLevelStarsDuration);
                                    _interfaces.SelectionLevelMenu[9].transform.localScale = Vector3.one * scale;
                                }
                                else
                                {
                                    _interfaces.SelectionLevelMenu[9].transform.localScale = Vector3.one;
                                }
                            }

                            break;

                    }

                    //End of transition
                    if (_transitionCounterTime + SELECTION_LEVEL_MENU_INTRO_MAX_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        //SelectionLevel Menu
                        _interfaces.SelectionLevelMenu[0].transform.localScale = Vector3.one;

                    }
                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //Selection Level Menu
                    _interfaces.SelectionLevelMenu[0].SetActive(true);
                    _interfaces.SelectionLevelMenu[0].transform.localScale = Vector3.one;

                    //Black Image
                    if (_UINextState == UIState.PlayState)
                    {
                        _interfaces.SelectionLevelMenu[10].SetActive(true);
                        _interfaces.SelectionLevelMenu[10].GetComponent<Image>().color = Color.clear;

                    }


                    //Save the time
                    _transitionCounterTime = Time.realtimeSinceStartup;

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {


                    //Black Image
                    if (_UINextState == UIState.PlayState)
                    {
                        if (_transitionCounterTime < Time.realtimeSinceStartup
                        && _transitionCounterTime + SELECTION_LEVEL_MENU_OUTRO_DURATION >= Time.realtimeSinceStartup)
                        {
                            float alpha = (Time.realtimeSinceStartup - _transitionCounterTime) * (1f / (SELECTION_LEVEL_MENU_OUTRO_DURATION));
                            _interfaces.SelectionLevelMenu[10].GetComponent<Image>().color = Color.black * alpha;
                        }

                    }
                    else
                    {
                        //Dialog scale
                        if (_transitionCounterTime + SELECTION_LEVEL_MENU_OUTRO_DURATION >= Time.realtimeSinceStartup)
                        {
                            float scale = 1f - (Time.realtimeSinceStartup - _transitionCounterTime) * (1f / (SELECTION_LEVEL_MENU_OUTRO_DURATION));
                            _interfaces.SelectionLevelMenu[0].transform.localScale = Vector3.one * scale;
                        }
                    }

                    //End of transition
                    if (_transitionCounterTime + SELECTION_LEVEL_MENU_OUTRO_DURATION <= Time.realtimeSinceStartup)
                    {
                        _onTransition = false;

                        if (_UINextState == UIState.PlayState)
                        {
                            _interfaces.SelectionLevelMenu[10].GetComponent<Image>().color = Color.black;
                            //Go to the play scene
                            UnityEngine.SceneManagement.SceneManager.LoadScene("game");
                        }
                        else
                        {
                            //Selection Level Menu
                            _interfaces.SelectionLevelMenu[0].transform.localScale = Vector3.zero;
                            _interfaces.SelectionLevelMenu[0].SetActive(false);

                            // Call the MakeUITransition function whit INIT_NEXT_TRANSITION
                            // here to ensure the INIT transition of the UINextState is called
                            MakeUITransition(INIT_NEXT_TRANSITION);
                        }

                    }
                }

                break;
        }
    }
    // END SelectionLevelMenuTransition()

    void TutorialStateTransition()
    {
        switch (_transitionType)
        {
            case TransitionType.Init: //Init transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //////////////////////
                    // Initialization code
                    //////////////////////

                    //Set onTransition to true!
                    _onTransition = true;

                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                {
                    //////////////////////
                    // Transition code
                    //////////////////////

                    //End of transition
                    if (/* Transition finished condition */true)
                    {
                        _onTransition = false;

                        /////////////////////////
                        // Transition finish code
                        /////////////////////////
                    }
                }

                break;

            case TransitionType.Finish: //Finish transition

                // The first time the transition begins
                // Use this part to initialize all the
                // things needed to make the transition.
                if (!_onTransition)
                {
                    //////////////////////
                    // Initialization code
                    //////////////////////

                    //Set onTransition to true!
                    _onTransition = true;
                }
                else //During _onTransition calls
                // This part is called during the transition
                // Use it to move, fade, etc. the interface objects
                // NOTE: Must detect the end of the transition and set _onTransition variable to false
                // Usually you must call here the MakeUITransition with INIT_NEXT_TRANSITION method
                // to Init the next transition if needed
                {
                    //////////////////////
                    // Transition code
                    //////////////////////

                    //End of transition
                    if (/* Transition finished condition */true)
                    {
                        _onTransition = false;

                        /////////////////////////
                        // Transition finish code
                        /////////////////////////
                    }
                }
                break;
        }
    }


    //Auxiliar functions

    public string GetTimeText(float time)
    {

        int min = (int)(time / 60f);
        int s1 = (int)(time % 60 / 10);
        int s2 = (int)(time % 60 % 10);

        return min + ":" + s1 + s2;
    }
}