using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Transform KillZone;
    private bool shuttingDown = false;
    public AudioClip hitSound;
    public AudioClip destroySound;
    public Transform destroyParticals;

    [System.Serializable]
    public class EnemyStats
    {
        public int maxHealth = 100;
        public int damageAmount = 1;
        public bool continuousDMG = true;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!stats.continuousDMG)
        {
            Player _player = collision.collider.GetComponent<Player>();
            if (_player != null)
            {
                _player.DamagePlayer(stats.damageAmount);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (stats.continuousDMG)
        {
            Player _player = collision.collider.GetComponent<Player>();
            if (_player != null)
            {
                _player.DamagePlayer(stats.damageAmount);
            }
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
            if (destroyParticals != null)
            {
                Transform particles = Instantiate(destroyParticals, transform.position, transform.rotation);
                Destroy(particles.gameObject, 2f);
            }
            if (destroySound != null)
            {
                AudioSource.PlayClipAtPoint(destroySound, transform.position, 1f);
            }
        }
    }
}
