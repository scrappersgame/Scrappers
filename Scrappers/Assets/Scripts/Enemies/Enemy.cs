using UnityEngine;

public class Enemy : MonoBehaviour {

    public AudioClip hitSound;
    public AudioClip destroySound;
    public Transform destroyParticals;
    public StatusIndicator statusIndicator;
    public Transform[] itemDrops;
    private bool shuttingDown = false;
    private Transform target;
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

    public EnemyStats stats = new EnemyStats();
    private void Start()
    {
        stats.Init();
        if(statusIndicator != null){
            statusIndicator.SetHealth(stats.currentHealth, stats.maxHealth);
        }
    }
    void Update()
    {
        if (transform.position.y <= -10f)
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
            target = this.gameObject.GetComponent<EnemyAI>().target;
            if (itemDrops.Length > 0){
                int _maxDrops = itemDrops.Length;
                int _numberDrops = Random.Range(1, _maxDrops);
                for (int i = 0; i < _numberDrops; i++){
                    Transform _droppedItem = Instantiate(itemDrops[Random.Range(0, _maxDrops)], transform.position, transform.rotation);
                    Vector3 _rotDirection = Vector3.left;
                    if( Random.Range(0, 2) == 1 )
                        _rotDirection = Vector3.right;
                    _droppedItem.Rotate(_rotDirection * Random.Range(1, 10));
                    Vector3 _moveDirection = (transform.position - target.position).normalized * Random.Range(.1f, .7f);
                    _droppedItem.gameObject.GetComponent<Rigidbody2D>().AddForce(_moveDirection, ForceMode2D.Impulse);
                    _droppedItem.gameObject.GetComponent<Rigidbody2D>().AddTorque(.7f);
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
