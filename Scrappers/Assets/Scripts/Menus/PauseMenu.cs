using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
	public void QuitGame (){
		Application.Quit();
	}
    private void Update()
    {
        bool pauseButton = Input.GetKeyDown(KeyCode.Escape);
        if (pauseButton)
        {
            pauseButton = false;
            GameMaster.gm.ResumeGame();
        }
    }
}
