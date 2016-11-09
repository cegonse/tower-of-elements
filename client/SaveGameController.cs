using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class SaveGameController : MonoBehaviour
{
    public struct LevelProgressData
    {
        public string Id;
        public float Score;
        public float ThreeStarsTime;
    }

    public static SaveGameController instance = null;

    private const bool ENCRYPT_SAVE = false;
    private const int LEVELS_BEFORE_UNLOCK = 7;
    private const int TUTORIAL_BEFORE_UNLOCK = 3;

    private string _path;
    private string _pathThreeStars = "static_values";
    
    private const string _saveKey = "Tdcc6LpsMD2uPxt97Cwtx2C6eWZkDD9hSHGEv7KNewtwbR9hkNMgNXhJCGPyYChX";

    private SystemLanguage _language = SystemLanguage.English;
    private bool _musicOn = true;
    private bool _sfxOn = true;

    private List<LevelProgressData> _levels;
    private Dictionary<string, float> _threeStarTimes;
    private Dictionary<string, float> _twoStarTimes;

    private bool _hasWindPower = false;
    private bool _hasIcePower = false;
    private bool _hasFirePower = false;
    private bool _hasEarthPower = false;
    private bool _hasAllPowers = false;

    private string _targetLevel = "0_02";
    private UIState _targetMainMenu = UIState.Intro;

    public enum UnlockablePowers
    {
        Wind,
        Ice,
        Fire,
        Earth,
        All
    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }

        _path = Application.persistentDataPath + Path.DirectorySeparatorChar + "save.json";
        _levels = new List<LevelProgressData>();

        if (Application.systemLanguage == SystemLanguage.Spanish ||
            Application.systemLanguage == SystemLanguage.Catalan ||
            Application.systemLanguage == SystemLanguage.Japanese)
        {
            _language = Application.systemLanguage;
        }

        Load();

        //Load all the static values needed
        LoadStaticValues();
    }

    public void SetTargetMenu(UIState st)
    {
        _targetMainMenu = st;
    }

    public UIState GetTargetMenu()
    {
        return _targetMainMenu;
    }

    public int GetStarCount(float time, string lvName)
    {
        int stars = 3;
        float threeTime = GetThreeStarsTime(lvName);
        float twoTime = GetTwoStarsTime(lvName);

        if (time <= threeTime)
        {
            stars = 3;
        }
        else
        {
            if (time > threeTime && time <= twoTime)
            {
                stars = 2;
            }
            else
            {
                stars = 1;
            }
        }

        return stars;
    }

    public bool IsMusicOn()
    {
        return _musicOn;
    }

    public bool IsSfxOn()
    {
        return _sfxOn;
    }

    public SystemLanguage GetActiveLanguage()
    {
        return _language;
    }

    public List<LevelProgressData> GetLevelProgress()
    {
        return _levels;
    }

    public void ToggleMusic(bool m)
    {
        _musicOn = m;
        Save();
    }

    public void ToggleSfx(bool s)
    {
        _sfxOn = s;
        Save();
    }

    public void SetActiveLanguage(SystemLanguage l)
    {
        _language = l;
        Save();
    }

    public void SetLevelProgress(LevelProgressData lvp)
    {
        bool found = false;

        for (int i = 0; i < _levels.Count && !found; i++)
        {
            if (lvp.Id == _levels[i].Id)
            {
                _levels[i] = lvp;
                found = true;
            }
        }

        if (!found)
        {
            _levels.Add(lvp);
        }

        // Check if a new power was unlocked
        int tutorialLevelCount = 0,
            windLevelCount = 0,
            iceLevelCount = 0,
            fireLevelCount = 0,
            earthLevelCount = 0;

        for (int i = 0; i < _levels.Count; ++i)
        {
            if (_levels[i].Id.Contains("0_"))
            {
                ++tutorialLevelCount;
            }

            if (_levels[i].Id.Contains("1_"))
            {
                ++windLevelCount;
            }

            if (_levels[i].Id.Contains("2_"))
            {
                ++iceLevelCount;
            }

            if (_levels[i].Id.Contains("3_"))
            {
                ++fireLevelCount;
            }

            if (_levels[i].Id.Contains("4_"))
            {
                ++earthLevelCount;
            }
        }

        if (tutorialLevelCount >= TUTORIAL_BEFORE_UNLOCK)
        {
            UnlockPower(UnlockablePowers.Wind);
        }

        if (windLevelCount >= LEVELS_BEFORE_UNLOCK)
        {
            UnlockPower(UnlockablePowers.Ice);
        }

        if (iceLevelCount >= LEVELS_BEFORE_UNLOCK)
        {
            UnlockPower(UnlockablePowers.Fire);
        }

        if (fireLevelCount >= LEVELS_BEFORE_UNLOCK)
        {
            UnlockPower(UnlockablePowers.Earth);
        }

        if (earthLevelCount >= LEVELS_BEFORE_UNLOCK)
        {
            UnlockPower(UnlockablePowers.All);
        }

        Save();
    }
	
	public bool HasPower(UnlockablePowers p)
    {
        bool result = false;

        switch (p)
        {
            case UnlockablePowers.Earth:
                result = _hasEarthPower;
                break;

            case UnlockablePowers.Fire:
                result = _hasFirePower;
                break;

            case UnlockablePowers.Ice:
                result = _hasIcePower;
                break;

            case UnlockablePowers.Wind:
                result = _hasWindPower;
                break;

            case UnlockablePowers.All:
                result = _hasAllPowers;
                break;
        }

        return result;
    }

    public void UnlockPower(UnlockablePowers p)
    {
        switch (p)
        {
            case UnlockablePowers.Earth:
                _hasEarthPower = true;
                break;

            case UnlockablePowers.Fire:
                _hasFirePower = true;
                break;

            case UnlockablePowers.Ice:
                _hasIcePower = true;
                break;

            case UnlockablePowers.Wind:
                _hasWindPower = true;
                break;

            case UnlockablePowers.All:
                _hasAllPowers = true;
                break;
        }

        Save();
    }

    public void SetTargetLevel(string tg)
    {
        _targetLevel = tg;
        Save();
    }

    public string GetTargetLevel()
    {
        return _targetLevel;
    }

    public void Save()
    {
        JSONObject json = new JSONObject(JSONObject.Type.OBJECT);

        json.AddField("language", (int)_language);
        json.AddField("music", _musicOn);
        json.AddField("sfx", _sfxOn);
		
		json.AddField("has_wind", _hasWindPower);
        json.AddField("has_ice", _hasIcePower);
        json.AddField("has_fire", _hasFirePower);
        json.AddField("has_earth", _hasEarthPower);
        json.AddField("has_all", _hasAllPowers);

        json.AddField("target_level", _targetLevel);

        JSONObject jlist = new JSONObject(JSONObject.Type.ARRAY);

        foreach (LevelProgressData lvp in _levels)
        {
            JSONObject jfield = new JSONObject(JSONObject.Type.OBJECT);

            jfield.AddField("id", lvp.Id);
            jfield.AddField("score", lvp.Score);

            jlist.Add(jfield);
        }

        json.AddField("levels", jlist);

        string saveFileData = json.Print(true);

        if (ENCRYPT_SAVE)
        {
            saveFileData = Cypher(saveFileData);
        }

        File.WriteAllText(_path, saveFileData);
    }

    public void Load()
    {
        string jsonSave = "";
        JSONObject save = null;

        if (File.Exists(_path))
        {
            jsonSave = File.ReadAllText(_path);

            if (ENCRYPT_SAVE)
            {
                jsonSave = Cypher(jsonSave);
            }

            if (!string.IsNullOrEmpty(jsonSave))
            {
                save = new JSONObject(jsonSave);
                
                if (save != null)
                {
                    _language = (SystemLanguage)save["language"].n;
                    _musicOn = save["music"].b;
                    _sfxOn = save["sfx"].b;

                    _hasWindPower = save["has_wind"].b;
                    _hasIcePower = save["has_ice"].b;
                    _hasFirePower = save["has_fire"].b;
                    _hasEarthPower = save["has_earth"].b;
                    _hasAllPowers = save["has_all"].b;

                    _targetLevel = save["target_level"].str;

                    if (save["levels"] != null)
                    {
                        List<JSONObject> lv = save["levels"].list;

                        if (lv != null)
                        {
                            _levels.Clear();

                            for (int i = 0; i < lv.Count; i++)
                            {
                                LevelProgressData lvp = new LevelProgressData();

                                lvp.Id = lv[i]["id"].str;
                                lvp.Score = lv[i]["score"].n;
                                lvp.ThreeStarsTime = Random.Range(0f, 50f);
                                _levels.Add(lvp);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Save();
        }
    }

    private string Cypher(string data)
    {
        StringBuilder result = new StringBuilder(data);

        for (int i = 0, k = 0; i < data.Length; i++)
        {
            result[i] ^= _saveKey[k++];

            if (k >= _saveKey.Length)
            {
                k = 0;
            }
        }

        return result.ToString();
    }

    private void LoadStaticValues()
    {
        //Load the threeStarsTimes dictionary
        _threeStarTimes = new Dictionary<string, float>();
        _twoStarTimes = new Dictionary<string, float>();

        string threeStarsTextString = (Resources.Load(_pathThreeStars) as TextAsset).text;

        if (!string.IsNullOrEmpty(threeStarsTextString))
        {
            JSONObject threeStarsJSON = new JSONObject(threeStarsTextString);

            if (threeStarsJSON["level_values"] != null)
            {
                List<JSONObject> staticValues = threeStarsJSON["level_values"].list;

                for (int i = 0; i < staticValues.Count; i++ )
                {
                    _threeStarTimes.Add(staticValues[i]["name"].str, staticValues[i]["threeStarsTime"].n);
                    _twoStarTimes.Add(staticValues[i]["name"].str, staticValues[i]["twoStarsTime"].n);
                }
            }
        }
        else
        {
            Debug.LogError("The static values json is not found!");
        }
    }

    public float GetThreeStarsTime(string lvl)
    {
        if (_threeStarTimes.ContainsKey(lvl))
        {
            return _threeStarTimes[lvl];
        }
        else
        {
            Debug.LogError("The level " + lvl + " is not in the _threeStarsTimes dictionary.");
            return 100f;
        }
    }

    public float GetTwoStarsTime(string lvl)
    {
        if (_twoStarTimes.ContainsKey(lvl))
        {
            return _twoStarTimes[lvl];
        }
        else
        {
            Debug.LogError("The level " + lvl + " is not in the _twoStarTimes dictionary.");
            return 100f;
        }
    }
}
