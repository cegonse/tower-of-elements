﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class SaveGameController : MonoBehaviour
{
    public struct LevelProgressData
    {
        public string Id;
        public float Score;
    }

    public static SaveGameController instance = null;

    private string _path;
    private const string _saveKey = "Tdcc6LpsMD2uPxt97Cwtx2C6eWZkDD9hSHGEv7KNewtwbR9hkNMgNXhJCGPyYChX";

    private SystemLanguage _language = SystemLanguage.English;
    private bool _musicOn = true;
    private bool _sfxOn = true;

    private List<LevelProgressData> _levels;
	
	private bool _hasWindPower = false;
    private bool _hasIcePower = false;
    private bool _hasFirePower = false;
    private bool _hasEarthPower = false;

    private string _targetLevel = "0_01";

    public enum UnlockablePowers
    {
        Wind,
        Ice,
        Fire,
        Earth
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

        for (int i = 0; i < _levels.Count; i++)
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

        json.AddField("target_level", _targetLevel);

        JSONObject jlist = new JSONObject(JSONObject.Type.ARRAY);

        foreach (LevelProgressData lvp in _levels)
        {
            JSONObject jfield = new JSONObject(JSONObject.Type.OBJECT);

            jfield.AddField("id", lvp.Id);
            jfield.AddField("score", lvp.Score);

            jlist.Add(jfield);
        }

        Debug.Log(json.Print());

        File.WriteAllText(_path, Cypher(json.Print()));
        
        #if UNITY_WEBGL
        Application.ExternalCall("FS.syncfs", false);
        #endif
    }

    public void Load()
    {
        string jsonSave = "";
        JSONObject save = null;

        if (File.Exists(_path))
        {
            jsonSave = File.ReadAllText(_path);

            if (!string.IsNullOrEmpty(jsonSave))
            {
                save = new JSONObject(Cypher(jsonSave));

                if (save != null)
                {
                    _language = (SystemLanguage)save["language"].n;
                    _musicOn = save["music"].b;
                    _sfxOn = save["sfx"].b;

                    _hasWindPower = save["has_wind"].b;
                    _hasIcePower = save["has_ice"].b;
                    _hasFirePower = save["has_fire"].b;
                    _hasEarthPower = save["has_earth"].b;

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
}