using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameMaster : MonoBehaviour {
	public static GameMaster gm;
	public GameObject CamController;
	public GameObject spawnPoint;
	public GameObject pauseMenu;
    public GameObject player;
	public AudioClip spawnClip;
	private AudioSource audioSource;

	void Awake (){
		audioSource = GameObject.FindGameObjectWithTag ("Sounds").GetComponent<AudioSource>();
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
		}
	}
	void LateUpdate(){
		bool pause = Input.GetKey (KeyCode.Escape);
		if (pause && player != null && player.activeSelf == true) {
			PauseGame();
		}
        pause = false;
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
	public void PauseGame(){
		pauseMenu.SetActive (true);
        Time.timeScale = 0;
	}
}
