using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public GameObject follower;
    public GameObject UI;
	public GameObject title;

	public void QuitGame (){
		Application.Quit();
	}
	public void PlayGame (){
		Vector3 followerp = follower.transform.position;
        follower.GetComponent<IntroMovement>().enabled = false;
		follower.transform.position = new Vector3 (followerp.x, 0, followerp.z);
        GameMaster.gm.spawnPoint = follower;
        GameMaster.gm.SpawnPlayer (0);
        GameMaster.gm.paused = false;
        this.gameObject.SetActive (false);
		title.SetActive (false);
        UI.SetActive(true);
	}
    private void Update()
    {
        bool pauseButton = Input.GetKey(KeyCode.Escape);
        if (pauseButton)
        {
            pauseButton = false;
            QuitGame();
        }
    }
		
}
