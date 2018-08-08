using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    public enum SpawnState {SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave {
        public string name;                             // What is this wave called?
        public Transform enemy;                         // What is this wave spawning?
        public int count;                               // How many is it spawning?
        public float rate;                              // How fast is it spawning?
    }

    public Wave[] waves;                                // list of waves
    public float waveDelay = 5f;                        // delay between waves
    public int spawnDistance = 25;                      // distance away from player enemies spawn
    public int repetitions = 0;                         // amount of times the waves will repeat (0 = infinite)

    private int waveMultiplier;                         // add more when we repeat
    private int nextWave = 0;                           // number of the wave after the current wave
    private float waveCountdown;                        // countdown to next wave start
    private float searchDelay = 1f;                     // delay between checking for enemies
    private SpawnState state = SpawnState.COUNTING;     // 

    private void Start()
    {
        waveCountdown = waveDelay;

    }

    private void Update()
    {
        if (GameMaster.gm.gameStarted)
        {
            // have we stopped spawning?
            if (state == SpawnState.WAITING)
            {
                // wait for all the enemies to be dead before moving on
                if (!EnemyIsAlive())
                {
                    WaveCompleted();
                }
                else
                {
                    return;
                }
            }
            //
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
        // are we at the end of the waves?
        if (nextWave + 1 > waves.Length - 1)
        {
            // should we repeat?
            if (repetitions == 0 || repetitions > waveMultiplier)
            {
                //go back to first wave
                waveMultiplier++;
                nextWave = 0;
            }else{
                
                //stop spawning
                this.enabled = false;
            }
        }
        else
        {
            // keep em coming!
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchDelay -= Time.deltaTime;
        if (searchDelay <= 0) { 
            searchDelay = 1f;
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
