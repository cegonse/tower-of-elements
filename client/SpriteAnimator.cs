using UnityEngine;
using System.Collections.Generic;

public struct AnimationFrame
{
	public float timeToNext;
	public Sprite sprite;
	public bool interval;
}

public class SpriteAnimator : MonoBehaviour
{
    private Dictionary<string, List<AnimationFrame>> _animations;
    private string _activeAnimation;
    private SpriteRenderer _renderer;
    private int _animationIndex = -1;
    private float _animationTimer = 0f;
    private float _animationTimerScaler = 1f;

    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        if (_animationIndex != -1)
        {
            if (_animations[_activeAnimation][_animationIndex].timeToNext > _animationTimer * _animationTimerScaler)
            {
                _animationTimer += Time.deltaTime;
            }
            else
            {
                if (_animationIndex < _animations[_activeAnimation].Count - 1)
                {
                    _animationIndex++;
                }
                else
                {
                    _animationIndex = 0;
                }

                _renderer.sprite = _animations[_activeAnimation][_animationIndex].sprite;
                _animationTimer = 0f;
            }
        }

    }

    public void AddAnimation(string animation_name, List<AnimationFrame> animation, int init_sprite = 0)
    {
        //Set the dictionary if not
        if (_animations == null)
        {
            _animations = new Dictionary<string, List<AnimationFrame>>();
        }
        //Set the renderer if not
        if (_renderer == null)
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (_renderer == null)
            {
                _renderer = gameObject.AddComponent<SpriteRenderer>();
            }
        }
        //Add the animation to the dictionary and set the activeAnimation
        _animations.Add(animation_name, animation);
        if (string.IsNullOrEmpty(_activeAnimation))
        {
            SetActiveAnimation(animation_name, init_sprite);
        }
        
    }

    public void SetActiveAnimation(string animation_name, int animationIndex = 0)
    {
        _activeAnimation = animation_name;
        _animationIndex = animationIndex;
        _renderer.sprite = _animations[_activeAnimation][_animationIndex].sprite;
    }

    public void SetAnimationIndex(int animationIndex)
    {
        _animationIndex = animationIndex;
    }

    public int GetAnimationIndex()
    {
       return  _animationIndex;
    }

    public void SetAnimationTimerScaler(float scaler)
    {
        _animationTimerScaler = scaler;
    }

    public float GetAnimationTimerScaler()
    {
        return _animationTimerScaler;
    }

    public bool IsTheLastFrame()
    {
        return _animationIndex == _animations[_activeAnimation].Count - 1;

    }
}
