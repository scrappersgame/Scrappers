using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour {
	public int RotationOffset = 0;
	// Update is called once per frame
	void Update () {
        if (Time.timeScale > 0)
        {
    		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
    		float xDifference = mouseWorldPosition.x - transform.parent.position.x;
    		if (xDifference < 0){
    			RotationOffset = 180;
    		}else{
    			RotationOffset = 0;
    		}
    		// caculate difference between the mouse and the arm
    		Vector3 difference = mouseWorldPosition - transform.position;
    		difference.Normalize (); // make sure the difference points add up to 1

    		float rotZ = Mathf.Atan2 (difference.y, difference.x) * Mathf.Rad2Deg; // find the angle in degrees
    		transform.rotation = Quaternion.Euler (0f, 0f, rotZ + RotationOffset); // rotate the arm
        }
	}
}
