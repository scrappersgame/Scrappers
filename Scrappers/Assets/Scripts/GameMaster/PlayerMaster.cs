using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaster : MonoBehaviour {

    [System.Serializable]
    public class PlayerStats
    {
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
    public static PlayerStats stats = new PlayerStats();
}
