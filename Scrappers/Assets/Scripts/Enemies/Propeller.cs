using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour {
    
    public GameObject Blade1;
    public GameObject Blade2;
    public float spinRate = 20f;
    public int damageAmount = 1;
    private bool left = true;

    float timeToSwitch = 0;
	void Update () {
        if (Time.time > timeToSwitch){
            if (left)
            {
                Blade1.SetActive(false);
                Blade2.SetActive(true);
                left = false;
            }
            else
            {
                Blade1.SetActive(true);
                Blade2.SetActive(false);
                left = true;
            }
            timeToSwitch = Time.time + (1f / spinRate);
        }
	}
    private void OnCollisionStay2D(Collision2D collision)
    {
        Player _player = collision.collider.GetComponent<Player>();
        if (_player != null)
        {
            _player.DamagePlayer(damageAmount);
        }
    }
}
