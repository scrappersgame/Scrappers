using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public float fireRate = 0;
	public float Damage = 10;
	public float Range = 60;
	public float EffectSpawnRate = 10;
	public float volume = 0.5f;
	public LayerMask Targets;
	public Transform BulletTrailPrefab;
	public Transform MuzzleFlashPrefab;
	public Color TrailColor;
	public AudioClip gunShot;
	private AudioSource audioSource;
	private Vector2 firePointPosition;
	private Gradient gradient;

	float timeToSpawnEffect = 0;
	float timeToFire = 0;
	Transform firePoint;
	// Use this for initialization
	void Awake () {
		audioSource = GameObject.FindGameObjectWithTag ("Sounds").GetComponent<AudioSource>();
		// select the point to create the bullets
		firePoint = transform.Find("Muzzle");
		if (firePoint == null){
			Debug.LogError("Your gun doesn't have a muzzle.");
		}
		gradient = new Gradient();

	}

	// Update is called once per frame
	void Update () {
		if (fireRate == 0){
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
				audioSource.clip = gunShot;
				AudioSource.PlayClipAtPoint(gunShot, transform.position, volume);
			} 
		} else {
			if (Input.GetButton("Fire1") && Time.time > timeToFire){
				timeToFire = Time.time + 1/fireRate;
				Shoot ();
				audioSource.clip = gunShot;
				audioSource.PlayOneShot(gunShot, volume);
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
		gradient.SetKeys(
			new GradientColorKey[] { new GradientColorKey(TrailColor, 0.0f), new GradientColorKey(TrailColor, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(0, 1.0f) }
		);
		int RotationOffset = 0;
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float xDifference = mouseWorldPosition.x - transform.position.x;
		if (xDifference < 0){
			RotationOffset = 180;
		}
		Transform bulletInstance = Instantiate (BulletTrailPrefab, firePointPosition, firePoint.rotation) as Transform;
		bulletInstance.Rotate (Vector3.forward * RotationOffset);
		bulletInstance.GetComponent<TrailRenderer> ().colorGradient = gradient;
		Transform muzzleFlashInstance = Instantiate (MuzzleFlashPrefab, firePointPosition, firePoint.rotation) as Transform;
		muzzleFlashInstance.parent = firePoint;
		muzzleFlashInstance.GetComponent<SpriteRenderer> ().color = TrailColor;
		float size = Random.Range (0.6f, 0.9f);
		muzzleFlashInstance.localScale = new Vector3 (size, size, size);
		Destroy (muzzleFlashInstance.gameObject, 0.02f);
	}
}
