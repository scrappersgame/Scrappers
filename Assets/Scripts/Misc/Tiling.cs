﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {
	public int offsetX = 2;                 // distance between edge of camera and new buddy
	public bool reverseScale = true;        // used if the sprite is not repeatable

    private bool hasLeftBuddy = false;      // Clowns to the left of me
    private bool hasRightBuddy = false;     // Jokers to the right
	private float spriteWidth = 0f;         // width of texture
	private Camera cam;                     // how you see dat?

	void Awake(){
		cam = Camera.main;
	}


	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x * transform.parent.localScale.x;
	}

	// Update is called once per frame
	void Update () {
		if (cam == null) {
			cam = Camera.main;
		}
		if (hasLeftBuddy == false || hasRightBuddy == false){
			// calculate how far the camera extends
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;

			// calculate how far the sprite extends beyond the camera
            float edgeVisiblePositionRight = (transform.position.x + spriteWidth/2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (transform.position.x - spriteWidth/2) + camHorizontalExtend;

			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasRightBuddy == false){
				MakeNewBuddy(1);
				hasRightBuddy = true;
			}
			else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasLeftBuddy == false){
				MakeNewBuddy(-1);
				hasLeftBuddy = true;
			}
				
		}
	}
	// fuction to create buddy, requires side indicator (-1 or 1)
	void MakeNewBuddy (int rightOrLeft) {
		// caculate buddy position
        Vector3 newPosition = new Vector3 (transform.position.x + spriteWidth * rightOrLeft, transform.position.y, transform.position.z);
		// make new buddy
        Transform newBuddy = Instantiate (transform, newPosition, transform.rotation) as Transform;

		if(reverseScale == true){
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x*-1,newBuddy.localScale.y,newBuddy.localScale.z);
		}

        newBuddy.parent = transform.parent;
		if (rightOrLeft > 0){
			newBuddy.GetComponent<Tiling>().hasLeftBuddy = true;
		} else {
			newBuddy.GetComponent<Tiling>().hasRightBuddy = true;
		}
	}

}
