using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public int ScrapValue = 1;
    public float speed = 10f;
    public ForceMode2D fMode;
    public GameObject itemPrefab;
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
            Player _player = coll.gameObject.GetComponent<Player>();
            _player.AddScrap(ScrapValue);
            if (itemPrefab != null)
            {
                _player.AddItem(itemPrefab, this.gameObject);
            }
            Destroy(gameObject);
        }
    }
    void FixedUpdate(){
        if(target != null){
            Vector3 dir = (target.position - transform.position).normalized;
            dir *= 100f * Time.fixedDeltaTime * rb.mass;

            //move the item
            rb.AddForce(dir, fMode);
        }
    }
}
