using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {

    public int moveSpeed = 100;
    public Vector3 endPoint;

    // Update is called once per frame
    void Update () {
        transform.position = Vector3.Lerp(transform.position, endPoint, 1/Time.deltaTime * moveSpeed);
        float dist = Vector3.Distance(endPoint, transform.position);
        if (dist < .001 && dist > -.001){
            Destroy(this.gameObject,1);
        }
    }
	void OnCollisionEnter (Collision col){
		Debug.Log ("Hit");
		Destroy (gameObject);
	}
}
