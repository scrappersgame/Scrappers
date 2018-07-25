using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMovement : MonoBehaviour {
	public float moveSpeed = 4f;    // how fast are we going?
    private Vector3 startPoint;     // where did we start?
	void Awake(){
        // where we started
        startPoint = transform.position;
    }
    void Update(){
        Vector3 curPos = transform.position;
        if (curPos.x < startPoint.x + 10) // Take 10 steps to the right
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        } else if (curPos.x < startPoint.x + 20) // then 10 to the left and 10 more to the right
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        } else {
            if (curPos.y < 520) // then first star to your left and straight on till morning.
            {
                transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
                if (curPos.y > 507){
                    moveSpeed = Mathf.Lerp(moveSpeed, 0, .01f); // slow it down baybee
                }
            }
        }

	}
}
