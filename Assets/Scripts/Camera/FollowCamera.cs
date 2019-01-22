using System;
using UnityEngine;

public class FollowCamera: MonoBehaviour {

	private Transform cam;
	void Awake () {
		cam = Camera.main.transform;
	}

	void Update () {
		Vector3 newPos = new Vector3 (cam.position.x, transform.position.y, transform.position.z);
		transform.position = newPos;
	}
}
