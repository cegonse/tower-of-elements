using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class TextureController
{
    private Dictionary<string, Texture> _textures;
    private Dictionary<string, float> _textureSizes;
	private Dictionary<string, List<AnimationFrame> > _animations;
	
	private GameController _gameController;
    private string _rcPath;
    private int _memoryUsage = 0;
	
	public TextureController(GameController game)
	{
		_gameController = game;
		_textures = new Dictionary<string, Texture>();
        _textureSizes = new Dictionary<string, float>();
		_animations = new Dictionary<string, List<AnimationFrame> >();
		
        // Get the Resources folder path
        if (GameController.IS_EDITOR_RUNTIME)
        {
            string rcFilePath = Path.GetFullPath(".") +
                Path.DirectorySeparatorChar + "data" + Path.DirectorySeparatorChar + "bin" + Path.DirectorySeparatorChar + "rcpath.txt";

            _rcPath = File.ReadAllText(rcFilePath);
        }

		// Texture loading
		string textureList = "";
		
		if (GameController.IS_EDITOR_RUNTIME)
		{
			string texListPath = _rcPath + Path.DirectorySeparatorChar + "texture_list.txt";	
			textureList = File.ReadAllText(texListPath);
		}
		else
		{
			textureList = (Resources.Load("texture_list") as TextAsset).text;
		}
		
		JSONObject json = new JSONObject(textureList);
		
		if (json)
		{
			List<JSONObject> jsonTex = json.list;
			
			for (int i = 0; i < jsonTex.Count; i++)
			{
                _textures.Add(jsonTex[i]["id"].str, null);
                _textureSizes.Add(jsonTex[i]["id"].str, jsonTex[i]["size"].n);

                if (_textureSizes[jsonTex[i]["id"].str] == 128 && !jsonTex[i]["id"].str.Contains("Shade"))
                {
                    _textureSizes[jsonTex[i]["id"].str] -= 4;
                }
			}
		}
		
		// Animation loading
		string animationList = "";
		
		if (GameController.IS_EDITOR_RUNTIME)
		{
			string animListPath = _rcPath + Path.DirectorySeparatorChar + "animation_list.txt";	
			animationList = File.ReadAllText(animListPath);
		}
		else
		{
			animationList = (Resources.Load("animation_list") as TextAsset).text;
		}

        // Override texture list loading
        if (!GameController.IS_EDITOR_RUNTIME)
        {
            JSONObject overrideList = new JSONObject((Resources.Load("overwrite_texture_list") as TextAsset).text);

            for (int i = 0; i < overrideList.Count; i++)
            {
                if (_textureSizes.ContainsKey(overrideList[i]["id"].str))
                {
                    _textureSizes[overrideList[i]["id"].str] = (int)overrideList[i]["size"].n;
                }
            }
        }

        // Load animations
        JSONObject jsonAnim = new JSONObject(animationList);
		
		if (jsonAnim)
		{
			List<JSONObject> jAnim = jsonAnim.list;
			
			for (int i = 0; i < jAnim.Count; i++)
			{
				LoadAnimation(jAnim[i].str);
            }
		}
	}
	
	public Texture GetTexture(string name)
	{
		if (_textures.ContainsKey(name))
		{
            if (_textures[name] == null)
            {
                _textures.Remove(name);
                LoadTexture(name);
                return _textures[name];
            }
            else
            {
                return _textures[name];
            }
		}
		else
		{
			return null;
		}
	}

    public List<string> GetTextureList()
    {
        return _textures.Keys.ToList();
    }

    public float GetTextureSize(string name)
    {
        if (_textureSizes.ContainsKey(name))
        {
            return _textureSizes[name];
        }
        else
        {
            return 256.0f;
        }
    }
	
	public bool LoadTexture(string name)
	{
		Texture tex = null;
		
		if (GameController.IS_EDITOR_RUNTIME)
		{
            string texPath = _rcPath + Path.DirectorySeparatorChar + "Textures" + Path.DirectorySeparatorChar + name + ".png";
				
			byte[] texData = File.ReadAllBytes(texPath);
			Texture2D tx = new Texture2D(256, 256);
			tx.LoadImage(texData);
			tex = (Texture)tx;
		}
		else
		{
			tex = Resources.Load("Textures/" + name) as Texture;
        }

        #if UNITY_EDITOR
        _memoryUsage += (tex.width * tex.height * 4) / 1000000;
        Debug.Log("VRAM Usage: " + _memoryUsage + " MB / " + SystemInfo.graphicsMemorySize + " MB");
        #endif

        if (tex == null)
		{
			Debug.LogError("FAILED TO LOAD TEXTURE " + name + "!!");
			return false;
		}
		else
		{
			_textures.Add(name, tex);
			return true;
		}
    }
	
	public void LoadAnimation(string name)
	{
		string animData = "";
		
		if (GameController.IS_EDITOR_RUNTIME)
		{
            string animPath = _rcPath + Path.DirectorySeparatorChar + "Textures" + Path.DirectorySeparatorChar + name + ".txt";

            try
            {
                animData = File.ReadAllText(animPath);
            }
            catch (System.IO.IsolatedStorage.IsolatedStorageException e)
            {
                Debug.LogError("Exception handled trying to open animation " + name);
                Debug.LogError(e.StackTrace);
            }
		}
		else
		{
			animData = (Resources.Load("Textures/" + name) as TextAsset).text;
		}

        JSONObject jsonAnim = null;

        if (!string.IsNullOrEmpty(animData))
        {
            jsonAnim = new JSONObject(animData);
        }
		
		if (jsonAnim)
		{
			List<JSONObject> jsonFrames = jsonAnim["animation"].list;
			List<AnimationFrame> animation = new List<AnimationFrame>();
			
			for (int i = 0; i < jsonFrames.Count; i++)
			{
				AnimationFrame f = new AnimationFrame();
				
				string ttnStr = jsonFrames[i]["time"].str.Replace(',','.');
                string texName = jsonFrames[i]["texture"].str;

                f.timeToNext = float.Parse(ttnStr);
				
				Texture2D tex = (Texture2D) GetTexture(texName);

                f.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), _textureSizes[texName]);

                animation.Add(f);
			}
			
			if (jsonAnim["interval"])
			{
				AnimationFrame t = animation[0];
				t.interval = ((int)jsonAnim["interval"].n) == 1 ? true : false;
				animation[0] = t;
			}
			else
			{
				Debug.Log("WARNING! Badly formed animation file: " + name);
			}
			
			_animations.Add(name, animation);
		}
	}
	
	public List<AnimationFrame> GetAnimation(string name)
	{
		if (_animations.ContainsKey(name))
		{
			return _animations[name];
		}
		else
		{
			return null;
		}
	}
	
	public void UnloadTexture(string name)
	{
		if (_textures.ContainsKey(name))
		{
			Resources.UnloadAsset(_textures[name]);
            _textures[name] = null;
		}
	}
}