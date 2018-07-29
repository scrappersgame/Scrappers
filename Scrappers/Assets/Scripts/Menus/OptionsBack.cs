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
    private void Update()
    {
        bool pauseButton = Input.GetKeyDown(KeyCode.Escape);
        if (pauseButton)
        {
            pauseButton = false;
            GoBack();
        }
    }
	public void GoBack (){
		if (pause) {
			pauseMenu.SetActive (true);
		} else {
			mainMenu.SetActive (true);
		}
        this.transform.parent.gameObject.SetActive(false);
	}

}
