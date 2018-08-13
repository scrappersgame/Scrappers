using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {



	[System.Serializable]
	public class PlayerStats {
        public int maxScrap = 1000;
        public int currentScrap = 0;

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

    private StatusIndicator statusIndicator;
    private Transform KillZone;
    private Transform gunHand;
    private GameObject currentGun;

	void Awake (){
        statusIndicator = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<StatusIndicator>();

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
        if (KillZone == null)
            KillZone = GameObject.FindGameObjectWithTag("KZ").transform;
		if (transform.position.y <= KillZone.position.y){
            DamagePlayer (stats.currentHealth);
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
    public void SwitchGuns (GameObject _gunPrefab){
        if (gunHand == null)
            gunHand = GameObject.FindGameObjectWithTag("GunHand").transform;
        if (currentGun != null)
            Destroy(currentGun);
        GameObject _newGun = Instantiate(_gunPrefab, gunHand.position, gunHand.rotation);
        _newGun.transform.SetParent(gunHand.parent);
        _newGun.transform.localScale = _newGun.transform.parent.localScale;
        currentGun = _newGun;
        
    }
    public void AddScrap(int value){
        stats.currentScrap += value;
        statusIndicator.SetScrap(stats.currentScrap, stats.maxScrap);

    }
}
