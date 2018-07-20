using UnityEngine.SceneManagement;
using UnityEngine;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture; //screen overlay
	public float fadeSpeed;

	private int drawDepth = -1000; //over 
	private float alpha = 1.0f;

	private int fadeDir = -1;
	void OnGUI (){
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);
		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);
	}
	public float BeginFade (int direction){
		fadeDir = direction;
		return (fadeSpeed);
	}

	void OnEnable() {
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		BeginFade (-1);
	}
}
