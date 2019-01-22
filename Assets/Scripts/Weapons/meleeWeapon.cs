using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeWeapon : MonoBehaviour
{

    public int damageAmount = 5;
    public float swingRate = 3f;
    private ArmRotation armRotation;
    private bool swinging = false;

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
        if (swinging)
        {
            float masterVolume = GameMaster.gm.masterVolume;
            Enemy _enemy = collision.collider.GetComponent<Enemy>();
            if (_enemy != null)
            {
                _enemy.DamageEnemy(damageAmount);
                AudioSource.PlayClipAtPoint(_enemy.hitSound, collision.collider.transform.position, masterVolume);
            }
            Vector2 hitPoint = collision.GetContact(0).point;
            Vector3 hitPosition = collision.collider.bounds.center;
            Destructible destructible = collision.collider.GetComponent<Destructible>();
            if (destructible != null)
            {
                destructible.DamageTile(damageAmount, hitPoint);
                AudioSource.PlayClipAtPoint(destructible.hitSound, hitPosition, masterVolume);
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
    }
    void Swing()
    {
        if (armRotation == null)
        {
            armRotation = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ArmRotation>();
        }
        armRotation.SwingArm();
        swinging = true;
        StartCoroutine(StopSwinging(1f / swingRate));
    }
    IEnumerator StopSwinging(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        swinging = false;

    }
}
