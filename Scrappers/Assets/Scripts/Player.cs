using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Transform KillZone;
	public static bool playerPaused = true;

	[System.Serializable]
	public class PlayerStats {
		public int Health = 100;
	}

	public PlayerStats playerStats = new PlayerStats();
	void Awake (){
		KillZone = GameObject.FindGameObjectWithTag ("KZ").transform;
	}
	void Update () {
		if (transform.position.y <= KillZone.position.y){
			DamagePlayer (playerStats.Health);
		}
	}
	public void DamagePlayer (int damage) {
		playerStats.Health -= damage;
		if (playerStats.Health <= 0){
			GameMaster.KillPlayer(this);
		}
	}
}
