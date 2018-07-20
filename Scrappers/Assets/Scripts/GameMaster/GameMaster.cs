using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameMaster : MonoBehaviour {
	public static GameMaster gm;
	public GameObject CamController;
	public GameObject spawnPoint;
    public GameObject pauseMenu;
    public GameObject pauseBG;
    public GameObject player;
	public AudioClip spawnClip;
    private AudioSource audioSource;
    public bool paused = true;

	void Awake (){
		audioSource = GameObject.FindGameObjectWithTag ("Sounds").GetComponent<AudioSource>();
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}
	void LateUpdate(){
        bool pauseButton = Input.GetKeyDown(KeyCode.Escape);
        if (pauseButton && !paused) {
            Debug.Log("pausing");
            pauseButton = false;
            paused = true;
			PauseGame();
        }
        int playerCount = GameObject.FindGameObjectsWithTag("Player").Length;
        if (playerCount >1){
            for (int i = 1; i < playerCount; i++){
                Destroy(GameObject.FindGameObjectsWithTag("Player")[i]);
            }
        }

	}
	public Transform playerPrefab;
	public Transform spawnPrefab;

	public void SpawnPlayer(int spawnDelay){
		gm.StartCoroutine(gm.SpawnPlayerRoutine (spawnDelay));
	}
	public IEnumerator SpawnPlayerRoutine (int spawnDelay){
		audioSource.clip = spawnClip;
		yield return new WaitForSeconds (spawnDelay);
		audioSource.Play ();
		Vector3 spawnPosition = spawnPoint.transform.position;
		Quaternion spawnRotation = spawnPoint.transform.rotation;
		Transform playerObj = Instantiate (playerPrefab, spawnPosition, spawnRotation);
		player = playerObj.gameObject;
		pauseMenu.GetComponent<PauseMenu> ().player = player;
		Transform spawnObj = Instantiate (spawnPrefab, spawnPosition, spawnRotation);
		Destroy (spawnObj.gameObject, 3);
		CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = playerObj;
        EnemySpawner.StartSpawningEnemies();

	}
	public static void KillPlayer (Player player){
		Destroy (player.gameObject);
		gm.SpawnPlayer (2);
	}
    public static void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
        EnemySpawner.RemoveEnemy();
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        pauseBG.SetActive(true);
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        pauseBG.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(Unpause(.01f));
    }
    IEnumerator Unpause(float delay){
        yield return delay;
        paused = false;
    }
}
