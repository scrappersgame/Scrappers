using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsBack : MonoBehaviour {

	public GameObject pauseMenu; 
	public GameObject mainMenu; 
	public bool pause;

	public void SetPause (bool pauseBool){
		pause = pauseBool;
	}
	public void GoBack (){
		if (pause) {
			pauseMenu.SetActive (true);
		} else {
			mainMenu.SetActive (true);
		}
	}

}
