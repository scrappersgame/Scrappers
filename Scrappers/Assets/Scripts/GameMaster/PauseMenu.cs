using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject player;

	public void QuitGame (){
		Application.Quit();
	}
    private void Update()
    {
        bool pauseButton = Input.GetKeyDown(KeyCode.Escape);
        if (pauseButton)
        {
            Debug.Log("unpausing");
            pauseButton = false;
            GameMaster.gm.ResumeGame();
        }
    }
}
