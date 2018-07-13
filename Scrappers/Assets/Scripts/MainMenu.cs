using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject gm;

	public void QuitGame (){
		Application.Quit();
	}
	public void PlayGame (){
		StartCoroutine (StartGame());
	}

	IEnumerator StartGame(){
		float fadeTime = gm.GetComponent<Fading>().BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}
