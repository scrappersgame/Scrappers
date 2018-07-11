using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameMaster : MonoBehaviour {
	public static GameMaster gm;
	public GameObject CamController;

	void Start (){
		if (gm == null) {
			gm = GameObject.FindGameObjectWithTag ("GM").GetComponent<GameMaster>();
			gm.StartCoroutine(gm.SpawnPlayer (0));
		}
	}
	public Transform playerPrefab;
	public Transform spawnPoint;

	public IEnumerator SpawnPlayer (int spawnDelay){
		Debug.Log ("Add respawn options");
		yield return new WaitForSeconds (spawnDelay);
		Transform playerObj = Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		CamController.GetComponent<CinemachineVirtualCamera>().m_Follow = playerObj;

	}
	public static void KillPlayer (Player player){
		Destroy (player.gameObject);
		gm.StartCoroutine(gm.SpawnPlayer (2));
	}
}
