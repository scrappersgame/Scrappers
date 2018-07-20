using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkle : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        Vector3 curPos = transform.position;
        Quaternion curRot = transform.rotation;
        transform.rotation = new Quaternion(curRot.x,curRot.y,curRot.z+.0005f,curRot.w);
		
	}
}
