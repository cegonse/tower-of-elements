using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

    private LeverDoor _leverDoor;
    private Sprite _left, _right;
    private Level _level;


	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetLevel (Level lv)
    {
        _level = lv;
    }
}
