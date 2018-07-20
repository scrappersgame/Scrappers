using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Transform KillZone;
	public static bool playerPaused = true;

	[System.Serializable]
	public class PlayerStats {
        public int maxHealth = 100;
        private int _curHealth;
        public int currentHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            currentHealth = maxHealth;
        }
	}
    public PlayerStats stats = new PlayerStats();

    [Header("Optional")]
    [SerializeField]
    private StatusIndicator statusIndicator;

	void Awake (){
		KillZone = GameObject.FindGameObjectWithTag ("KZ").transform;
	}
    private void Start()
    {
        stats.Init();
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }
    void Update () {
		if (transform.position.y <= KillZone.position.y){
            DamagePlayer (stats.currentHealth);
        }else if(transform.position.y <-.5){
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
	public void DamagePlayer (int damage) {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0){
			GameMaster.KillPlayer(this);
		}
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
	}
}
