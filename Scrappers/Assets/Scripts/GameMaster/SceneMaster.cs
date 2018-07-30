using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class SceneMaster : MonoBehaviour {
    public AudioClip sceneMusic;
    public TextAsset sceneInkJSON;
	// Use this for initialization
	void Start () {
        if (sceneMusic != null)
            AudioSourceCrossfade.cf.Play(sceneMusic);
        if(sceneInkJSON != null)
            StoryMaster.sm.StartStory(sceneInkJSON);
	}
	
}
