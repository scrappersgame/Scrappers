using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Transform KillZone;
    private bool shuttingDown = false;
    public AudioClip hitSound;
    public AudioClip destroySound;
    public Transform destroyParticals;
    public Transform[] itemDrops;
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
    private void OnApplicationQuit()
    {
        shuttingDown = true;
    }
    private void OnDestroy()
    {
        if (!shuttingDown)
        {
            if (itemDrops.Length > 0){
                int _maxDrops = itemDrops.Length;
                int _numberDrops = Random.Range(0, _maxDrops);
                for (int i = 0; i < _numberDrops; i++){
                    Transform _droppedItem = Instantiate(itemDrops[Random.Range(0, _maxDrops)], transform.position, transform.rotation);
                }
            }
            if (destroyParticals != null)
            {
                Transform particles = Instantiate(destroyParticals, transform.position, transform.rotation);
                Destroy(particles.gameObject, 2f);
            }
            if (destroySound != null)
            {
                float masterVolume = GameMaster.gm.masterVolume;
                AudioSource.PlayClipAtPoint(destroySound, transform.position, masterVolume);
            }
        }
    }
}
