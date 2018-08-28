using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeWeapon : MonoBehaviour {

    public int damageAmount = 5;
    public float swingRate = 3f;
    private ArmRotation armRotation;

    private void Update()
    {
        float timeToSwing = 0f;
        if (!GameMaster.gm.paused && !GameMaster.gm.speaking)
        {
            if (Input.GetButton("Fire1") && Time.time > timeToSwing)
            {
                timeToSwing = Time.time + (1f / swingRate);
                Swing();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        float masterVolume = GameMaster.gm.masterVolume;
        Enemy _enemy = collision.collider.GetComponent<Enemy>();
        if (_enemy != null)
        {
            _enemy.DamageEnemy(damageAmount);
            AudioSource.PlayClipAtPoint(_enemy.hitSound, collision.collider.transform.position, masterVolume);
        }
        else if (collision.transform.parent != null)
        {
            _enemy = collision.transform.parent.GetComponent<Enemy>();
            if (_enemy != null)
            {
                _enemy.DamageEnemy(damageAmount); //Damage enemy
                                           //create sound at hitpoint
                AudioSource.PlayClipAtPoint(_enemy.hitSound, collision.collider.transform.position, masterVolume);
            }
        }
    }
    void Swing(){
        if(armRotation == null){
            armRotation = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ArmRotation>();
        }
        armRotation.SwingArm();
    }
}
