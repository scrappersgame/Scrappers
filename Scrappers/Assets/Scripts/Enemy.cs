using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private Transform KillZone;
    public static bool playerPaused = true;

    [System.Serializable]
    public class EnemyStats
    {
        public int Health = 100;
    }

    public EnemyStats stats = new EnemyStats();
    void Awake()
    {
        KillZone = GameObject.FindGameObjectWithTag("KZ").transform;
    }
    void Update()
    {
        if (transform.position.y <= KillZone.position.y)
        {
            DamageEnemy(stats.Health);
        }
    }
    public void DamageEnemy(int damage)
    {
        stats.Health -= damage;
        if (stats.Health <= 0)
        {
            GameMaster.KillEnemy(this);
        }
    }
}
