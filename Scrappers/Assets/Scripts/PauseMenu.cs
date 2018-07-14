using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

	public GameObject player;

	public void QuitGame (){
		Application.Quit();
	}
	public void ResumeGame (){
		player.SetActive (true);
		this.gameObject.SetActive (false);
	}

}
