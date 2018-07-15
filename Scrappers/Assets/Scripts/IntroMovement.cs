using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroMovement : MonoBehaviour {
	public float moveSpeed = 5f;
    private Vector3 startPoint;
	void Awake(){
        startPoint = transform.position;
    }
    void Update(){
        Vector3 curPos = transform.position;
        if (curPos.x < startPoint.x + 10)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        } else if (curPos.x < startPoint.x + 20)
        {
            transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
            transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
        } else {
            if (curPos.y < 100)
            {
                transform.Translate(Vector3.up * Time.deltaTime * moveSpeed);
                if (curPos.y > 80){
                    moveSpeed = Mathf.Lerp(moveSpeed, 0, .01f);
                }
            }
        }

	}
}
