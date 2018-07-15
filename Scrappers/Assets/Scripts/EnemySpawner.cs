using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner spawner;
    public GameObject spawnPrefab;
    public GameObject target;
    public float spawnRate = 0.5f;
    public int maxSpawn = 5;
    public int spawnDistance = 25;
    public int enemiesSpawned = 0;
    public bool spawning = true;

	void Start () {
        if (spawner == null)
        {
            spawner = GameObject.FindGameObjectWithTag("GM").GetComponent<EnemySpawner>();
        }
        spawner.StartCoroutine(SpawnEnemies());
	}

    private void Update()
    {
        if(!spawner.spawning && (spawner.enemiesSpawned < spawner.maxSpawn)){
            spawner.spawning = true;
            StartSpawningEnemies();
        }
    }
    public static void StartSpawningEnemies(){
        spawner.StartCoroutine(SpawnEnemies());
    }
    public static IEnumerator SpawnEnemies()
    {
        spawner.target = GameObject.FindGameObjectWithTag("Player");
        if (spawner.target == null)
            yield break;
        int chosenNumber = 1;
        if (Random.value < 0.5f)
            chosenNumber = -1;
        Vector3 spawnLocation = new Vector3(spawner.target.transform.position.x + (spawner.spawnDistance * chosenNumber), spawner.target.transform.position.y + Random.Range(0.0f, 10.0f), spawner.target.transform.position.z);

        GameObject enemyObject = Instantiate(spawner.spawnPrefab, spawnLocation, new Quaternion(0, 0, 0, 0));
        enemyObject.GetComponent<EnemyAI>().target = spawner.target.transform;
        yield return new WaitForSeconds(1f / spawner.spawnRate);
        if (spawner.enemiesSpawned<spawner.maxSpawn)
        {
            spawner.StartCoroutine(SpawnEnemies());
            spawner.enemiesSpawned++;
        }else{
            spawner.spawning = false;
        }
    }

    public static void RemoveEnemy(){
        spawner.enemiesSpawned--;
    }

}
