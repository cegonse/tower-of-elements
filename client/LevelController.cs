using UnityEngine;
using System.Collections.Generic;

public class LevelController
{
	private GameController _gameController;
	private Dictionary<string, Level> _levels;
	private Level _activeLevel;
    
	public LevelController(GameController gameController)
	{
		_gameController = gameController;
		_levels = new Dictionary<string, Level>();
		
		// Level loading
        string levelList = (Resources.Load("level_list") as TextAsset).text;
        JSONObject json = new JSONObject(levelList);

        if (json)
        {
            List<JSONObject> jsonLev = json.list;

            for (int i = 0; i < jsonLev.Count; i++)
            {
                CreateLevel(jsonLev[i].str);
            }
        }

        _gameController.GetAudioController().PlayChannel(0);
        _gameController.GetAudioController().PauseChannel(0);
	}
	
    
	public bool CreateLevel(string name)
    {
        Level lv = new Level(this, name);

        if (lv == null)
        {
            return false;
        }
        else
        {
            _levels.Add(name, lv);
            return true;
        }
    }
	
	public void MarshallLevel(string levelData)
	{
		
	}
	
	public void CreateTestLevel()
	{
		Level lv = new Level(this, "test");
		_levels.Add("test", lv);
		_activeLevel = lv;
		
		for (int i = 0; i < 50; i++)
		{
			int posx = Random.Range(-10, 11);
			int posy = Random.Range(-5, 6);
			GameObject go = lv.CreateBlock((BlockType) Random.Range(0, 2),
				posx, posy, "block" + i.ToString());
				
			lv.AddEntity(go, go.name);
		}
	}
	
	public string SerializeLevel()
	{
		return "";
	}
	
	public void OnUpdate()
	{
		if(_activeLevel != null)
		{
			_activeLevel.OnUpdate();
		}
	}
	
	//Gets and Sets
	
	public GameController GetGameController()
	{
		return _gameController;
	}
	
	public Level GetActiveLevel()
	{
		return _activeLevel;
	}
	
	public Dictionary<string, Level> GetLevelList()
	{
		return _levels;
	}
	
	public void SetActiveLevel(string level)
	{
        if(_activeLevel != null)
		    _activeLevel.ClearLevel();
			
		if (GameController.IS_DEBUG_MODE)
		{
			GameObject.Find("LevelName").GetComponent<UnityEngine.UI.Text>().text = level;
		}
        
		_activeLevel = _levels[level];

        _gameController.GetAudioController().PauseChannel(0);
        _gameController.GetAudioController().StopChannel(1, true);

		_activeLevel.LoadLevel();
	}
	
}