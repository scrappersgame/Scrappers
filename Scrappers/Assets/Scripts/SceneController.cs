using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public static SceneController sceneController;

	bool gameStart;

	void Awake (){
		if (!gameStart) {
			sceneController = this; 									// so the script can call itself
			SceneManager.LoadSceneAsync (1, LoadSceneMode.Additive); 	// load first scene after the game manager 
			gameStart = true; 											// only do it once
		}
	}
	public void UnloadScene (int scene) {
		StartCoroutine (Unload (scene));
	}

	IEnumerator Unload(int scene){
		yield return null;
		SceneManager.UnloadSceneAsync (scene);
	}
}
