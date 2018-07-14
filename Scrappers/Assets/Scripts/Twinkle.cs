using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkle : MonoBehaviour {
    public float speed = 1.0f;
    public float amount = 1.0f;

	// Update is called once per frame
	void Update () {
        Vector3 curPos = transform.position;
        float newX = curPos.x + (Mathf.Sin(Time.time * speed) * amount/1000);
        transform.position = new Vector3(newX, curPos.y, curPos.z);
		
	}
}
