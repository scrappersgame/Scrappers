using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Transform KillZone;
    public AudioClip hitSound;
    public bool playerPaused = true;

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        private int _curHealth;
        public int currentHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init(){
            currentHealth = maxHealth;
        }
    }

    [Header("Optional")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    public EnemyStats stats = new EnemyStats();
    void Awake()
    {
        KillZone = GameObject.FindGameObjectWithTag("KZ").transform;
    }
    private void Start()
    {
        stats.Init();
        if(statusIndicator != null){
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }
    void Update()
    {
        if (transform.position.y <= KillZone.position.y)
        {
            DamageEnemy(stats.currentHealth);
        }
    }
    public void DamageEnemy(int damage)
    {
        stats.currentHealth -= damage;
        if (stats.currentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }
        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }
}
