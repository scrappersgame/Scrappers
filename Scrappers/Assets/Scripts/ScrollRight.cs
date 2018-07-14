using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollRight : MonoBehaviour {
	public int moveSpeed = 5;
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);

	}
}
