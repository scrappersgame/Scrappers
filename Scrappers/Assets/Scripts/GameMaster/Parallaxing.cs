using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;		// Array of all the moving bits
	public float smoothing = 1f;			// How smooth are you? must be > 0

	private float[] plaxScales;  			// proportion of movement
	private int bg_length;					// number of backgrounds
	private Transform cam;						// reference to main camera
	private Vector3 previousCamPos;		// stores old cam data

	// called before Start() good for setting up references
	void Awake () {
		cam = Camera.main.transform;
		bg_length = backgrounds.Length;
	}
	// Use this for initialization
	void Start () {
		previousCamPos = cam.position;

		// assigning movement speed
		plaxScales = new float[bg_length];
		for (int i = 0; i < bg_length; i++){
			plaxScales[i] = backgrounds[i].position.z * -1;
		}
	}

	// Update is called once per frame
	void Update () {
		float camChange = (previousCamPos.x - cam.position.x); // camera movement
		for (int i = 0; i < bg_length; i++){
			// background position
			Vector3 bg_position = backgrounds[i].position;

			// parallax is inverse of cam movement
			float parallax = camChange * plaxScales[i];

			// calculate x change
			float bgTargetPosX = bg_position.x + parallax;

			// set up full position
			Vector3 bgTargetPos = new Vector3 (bgTargetPosX, bg_position.y, bg_position.z);

			// slowly move toward target
			backgrounds[i].position = Vector3.Lerp(bg_position, bgTargetPos, smoothing * Time.deltaTime);
		}

		// update old cam position
		previousCamPos = cam.position;
	}
}
