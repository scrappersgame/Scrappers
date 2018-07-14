using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

	public GameObject gm;
	public GameObject follower;
	public GameObject title;

	public void QuitGame (){
		Application.Quit();
	}
	public void PlayGame (){
		Vector3 followerp = follower.transform.position;
		follower.GetComponent<ScrollRight>().enabled = false;
		follower.transform.position = new Vector3 (followerp.x, followerp.y - 3, followerp.z);
		gm.GetComponent<GameMaster>().spawnPoint = follower;
		gm.GetComponent<GameMaster>().SpawnPlayer (0);
		this.gameObject.SetActive (false);
		title.SetActive (false);
	}
		
}
