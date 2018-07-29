using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public enum SpawnState {SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public float waveDelay = 5f;
    public int spawnDistance = 25;

    private int waveMultiplier;
    private int nextWave = 0;
    private float waveCountdown;
    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.COUNTING;

    private void Start()
    {
        waveCountdown = waveDelay;

    }

    private void Update()
    {
        if (GameMaster.gm.gameStarted)
        {
            if (state == SpawnState.WAITING)
            {
                if (!EnemyIsAlive())
                {
                    WaveCompleted();
                }
                else
                {
                    return;
                }
            }

            if (waveCountdown <= 0)
            {
                if (state != SpawnState.SPAWNING)
                {
                    // start spawning
                    StartCoroutine(SpawnWave(waves[nextWave]));
                }

            }
            else
            {
                waveCountdown -= Time.deltaTime;
            }
        }
    }

    void WaveCompleted(){
        state = SpawnState.COUNTING;
        waveCountdown = waveDelay;

        if (nextWave + 1 > waves.Length - 1)
        {
            //go back to first wave
            waveMultiplier++;
            nextWave = 0;
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0) { 
            searchCountdown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;

        for (int i = 0; i < (_wave.count * waveMultiplier); i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1 / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(Transform _enemy){
        GameObject target = GameObject.FindGameObjectWithTag("Player");
        int chosenNumber = 1;
        if (Random.value < 0.5f)
            chosenNumber = -1;
        if (target == null)
            return;
        // Spawn enemy
        Vector3 spawnLocation = new Vector3(target.transform.position.x + (spawnDistance * chosenNumber) + Random.Range(-5f, 5f), target.transform.position.y + Random.Range(0, 15f), target.transform.position.z);
        Collider[] hitColliders = Physics.OverlapSphere(spawnLocation, 2);
        int tryCount = 0;
        while (hitColliders.Length > 0){
            tryCount++;
            if (tryCount > 10)
                return;
            spawnLocation = new Vector3(target.transform.position.x + (spawnDistance * chosenNumber)+ Random.Range(-5f, 5f), target.transform.position.y + Random.Range(0, 15f), target.transform.position.z);
            hitColliders = Physics.OverlapSphere(spawnLocation, 2);
        }
        Instantiate(_enemy, spawnLocation, new Quaternion(0, 0, 0, 0));

    }
}
