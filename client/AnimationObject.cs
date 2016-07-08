using UnityEngine;
using System.Collections;

public class AnimationObject : MonoBehaviour {

    private string _animation;
    private SpriteAnimator _spriteAnimator;
    private Level _level;
	// Use this for initialization
	void Start () {
        _spriteAnimator = gameObject.AddComponent<SpriteAnimator>();
        _spriteAnimator.SetActiveLevel(_level);

        if (_level.GetLevelController().GetGameController().GetTextureController().GetAnimation(_animation) != null)
        {
            _spriteAnimator.AddAnimation("WHATEVER", _level.GetLevelController().GetGameController().GetTextureController().GetAnimation(_animation));
        }
        else
        {
            Debug.LogError("ERROR in AnimationObject, no animation is defined. Destroying object.");
            GameObject.Destroy(gameObject);
        }

	}
	
	// Update is called once per frame
	void Update () {
        if (_spriteAnimator.IsTheLastFrame())
        {
            GameObject.Destroy(gameObject);
        }
	}

    public void SetParams(Level lvl, string anim)
    {
        _level = lvl;
        _animation = anim;
    }
}
