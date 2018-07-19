using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public int fireRate = 0;
    public int Damage = 10;
    public float Force = 5f;
	public float Range = 15;
	public float EffectSpawnRate = 10;
    public float EffectVolume = 0.5f;
    public float FlareSize= 0.4f;
	public LayerMask Targets;
    public Transform BulletTrailPrefab;
    public Transform HitPrefab;
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
        if (Time.timeScale > 0){
            if (fireRate == 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                    audioSource.clip = gunShot;
                    AudioSource.PlayClipAtPoint(gunShot, transform.position, EffectVolume);
                }
            }
            else
            {
                if (Input.GetButton("Fire1") && Time.time > timeToFire)
                {
                    timeToFire = Time.time + (1f / fireRate);
                    Shoot();
                    audioSource.clip = gunShot;
                    AudioSource.PlayClipAtPoint(gunShot, transform.position, EffectVolume);
                }
            }
        }
	}

	void Shoot () {
		firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
        int RotationOffset = 0;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float xDifference = mouseWorldPosition.x - transform.parent.parent.position.x;
            if (xDifference < 0)
            {
                RotationOffset = 180;
            }            
        float degrees = firePoint.rotation.eulerAngles.z + RotationOffset;
        float radians = degrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float y = Mathf.Sin(radians);
        Vector3 hitAngle = new Vector3(x, y, 0);
        Vector2 hitPoint = firePoint.position + hitAngle * Range;
        Vector3 hitNormal = new Vector3(999,999,999);
        RaycastHit2D hit = Physics2D.Raycast (firePointPosition, hitPoint - firePointPosition, Range, Targets);
		if (Time.time >= timeToSpawnEffect){
            if (hit.collider != null)
            {
                float hitPointDist = (firePointPosition - hit.point).magnitude;
                hitPoint = firePoint.position + hitAngle * hitPointDist;
                hitNormal = hit.normal;
            }
            Effect (hitPoint, RotationOffset, hitNormal);
			timeToSpawnEffect = Time.time + 1/EffectSpawnRate;
		}
		if (hit.collider != null){
            hit.collider.transform.GetComponent<Rigidbody2D>().AddForce(hit.normal * -Force);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null){
                enemy.DamageEnemy(Damage);
                AudioSource.PlayClipAtPoint(enemy.hitSound, hit.collider.transform.position, 1f);
            }
		}
	}

    void Effect (Vector2 hitPoint, int RotationOffset, Vector3 hitNormal){
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(TrailColor, 0.0f), new GradientColorKey(TrailColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(0, 1.0f) }
        );
        if (hitNormal != new Vector3(999,999,999)){
            Transform hit = Instantiate(HitPrefab, hitPoint, Quaternion.FromToRotation(Vector3.right, hitNormal));
            ParticleSystem.ColorOverLifetimeModule settings = hit.GetComponent<ParticleSystem>().colorOverLifetime;
            settings.color = gradient;
            Destroy(hit.gameObject, 0.5f);
        }

		Transform trail = Instantiate (BulletTrailPrefab, firePointPosition, firePoint.rotation) as Transform;
        trail.Rotate (Vector3.forward * RotationOffset);
        trail.GetComponent<TrailRenderer> ().colorGradient = gradient;
        trail.GetComponent<MoveTrail>().endPoint = hitPoint;
        Transform muzzle = Instantiate (MuzzleFlashPrefab, firePointPosition, firePoint.rotation) as Transform;
        muzzle.parent = firePoint;
        muzzle.GetComponent<SpriteRenderer> ().color = TrailColor;
        float size = Random.Range (FlareSize - 0.1f, FlareSize + 0.1f);
        muzzle.localScale = new Vector3 (size, size, size);
        Destroy (muzzle.gameObject, 0.02f);
	}
}
