using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMaster : MonoBehaviour {
    public AudioClip sceneMusic;
	// Use this for initialization
	void Start () {
        if (sceneMusic != null)
            AudioSourceCrossfade.cf.Play(sceneMusic);
	}
	
}
