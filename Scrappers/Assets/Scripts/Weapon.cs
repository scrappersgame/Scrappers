using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public float fireRate = 0;
	public float Damage = 10;
	public float Range = 60;
	public float EffectSpawnRate = 10;
	public LayerMask Targets;
	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	private Vector2 firePointPosition;

	float timeToSpawnEffect = 0;
	float timeToFire = 0;
	Transform firePoint;
	// Use this for initialization
	void Awake () {
		// select the point to create the bullets
		firePoint = transform.Find("Muzzle");
		if (firePoint == null){
			Debug.LogError("Your gun doesn't have a muzzle.");
		}

	}

	// Update is called once per frame
	void Update () {
		if (fireRate == 0){
			if (Input.GetButtonDown("Fire1")){
				Shoot();
			}
		} else {
			if (Input.GetButton("Fire1") && Time.time > timeToFire){
				timeToFire = Time.time + 1/fireRate;
				Shoot ();
			}
		}
	}

	void Shoot () {
		Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, Range, Targets);
		if (Time.time >= timeToSpawnEffect){
			Effect ();
			timeToSpawnEffect = Time.time + 1/EffectSpawnRate;
		}
		Debug.DrawLine (firePointPosition, mousePosition , Color.cyan);
		if (hit.collider != null){
			Debug.DrawLine (firePointPosition, hit.point, Color.red);
		}
	}

	void Effect (){
		int RotationOffset = 0;
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float xDifference = mouseWorldPosition.x - transform.position.x;
		if (xDifference < 0){
			RotationOffset = 180;
		}
		Transform bulletInstance = Instantiate (BulletTrailPrefab, firePointPosition, firePoint.rotation) as Transform;
		bulletInstance.Rotate (Vector3.forward * RotationOffset);
		Transform muzzleFlashInstance = Instantiate (MuzzleFlashPrefab, firePointPosition, firePoint.rotation) as Transform;
		muzzleFlashInstance.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		muzzleFlashInstance.localScale = new Vector3 (size, size, size);
		Destroy (muzzleFlashInstance.gameObject, 0.02f);
	}
}
