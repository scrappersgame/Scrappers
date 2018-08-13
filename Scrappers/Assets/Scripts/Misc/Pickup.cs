using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public int ScrapValue = 1;
    public float speed = 10f;
    public ForceMode2D fMode;
    private Transform target;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            target = coll.transform.Find("Arm").transform;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<Player>().AddScrap(ScrapValue);
            Destroy(gameObject);
        }
    }
    void FixedUpdate(){
        if(target != null){
            Debug.Log("Moving...");
            Vector3 dir = (target.position - transform.position).normalized;
            dir *= 10f * Time.fixedDeltaTime;

            //move the item
            rb.AddForce(dir, fMode);
        }
    }
}
