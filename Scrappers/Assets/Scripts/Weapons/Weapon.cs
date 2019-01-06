using UnityEngine;

public class Weapon : MonoBehaviour {
	public int fireRate = 0;
    public int Damage = 10;
    public int BulletCost = 1;
    public float Force = 5f;
	public float Range = 15f;
	public float EffectSpawnRate = 10f;
    public float EffectVolume = 0.5f;
    public float FlareSize = 0.4f;
    public float CamShakeAmount = 0.1f;
    public float CamShakeLength = 0.1f;
	public LayerMask Targets;
    public Transform BulletTrailPrefab;
    public Transform HitPrefab;
	public Transform MuzzleFlashPrefab;
	public Color TrailColor;
    public AudioClip gunShot;
    public AudioClip gunClick;
    private Vector2 firePointPosition;
    private Gradient gradient;
    private AudioSource sounds;

    CameraShake camShake;

	float timeToSpawnEffect = 0;
	float timeToFire = 0;
	Transform firePoint;
	// Use this for initialization
	void Awake () {
        sounds = GameObject.FindGameObjectWithTag("Sounds").GetComponent<AudioSource>();
		// select the point to create the bullets
		firePoint = transform.Find("Muzzle");
		if (firePoint == null){
			Debug.LogError("Your gun doesn't have a muzzle.");
		}
		gradient = new Gradient();
    }

    private void Start()
    {
        camShake = GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null)
            Debug.LogError("No camera shake script on GM object.");
    }

    // Update is called once per frame
    void Update () {
        if (!GameMaster.gm.paused && !GameMaster.gm.speaking){
            if (fireRate == 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetButton("Fire1") && Time.time > timeToFire)
                {
                    timeToFire = Time.time + (1f / fireRate);
                    Shoot();
                }
            }

        }
	}

	void Shoot () {
        if (PlayerMaster.stats.currentScrap > 0)
        {
            GameMaster.gm.playerObj.GetComponent<Player>().RemoveScrap(BulletCost);
            float masterVolume = GameMaster.gm.masterVolume;
            sounds.clip = gunShot;
            sounds.volume = EffectVolume * masterVolume;
            sounds.loop = false;
            sounds.Play();
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
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Damage); //Damage enemy
                    //create sound at hitpoint
                    AudioSource.PlayClipAtPoint(enemy.hitSound, hit.collider.transform.position, masterVolume);
                }
                Destructible destructible = hit.collider.GetComponent<Destructible>();
                if (destructible != null)
                {
                    float hitPointDist = (firePointPosition - hit.point).magnitude;
                    Vector2 tilehitPoint = firePoint.position + hitAngle * (hitPointDist + .1f);
                    destructible.DamageTile(Damage, tilehitPoint);
                    Vector3 hitPosition = new Vector3(tilehitPoint.x, tilehitPoint.y, 0);
                    AudioSource.PlayClipAtPoint(destructible.hitSound, hitPosition, masterVolume);
                }
                else if (hit.transform.parent != null)
                {
                    enemy = hit.transform.parent.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.DamageEnemy(Damage); //Damage enemy
                                                   //create sound at hitpoint
                        AudioSource.PlayClipAtPoint(enemy.hitSound, hit.collider.transform.position, masterVolume);
                    }
                }
    		}
        }
        else
        {
            float masterVolume = GameMaster.gm.masterVolume;
            sounds.clip = gunClick;
            sounds.volume = EffectVolume * masterVolume;
            sounds.loop = false;
            sounds.Play();
        }
    }

    void Effect (Vector2 hitPoint, int RotationOffset, Vector3 hitNormal){
        //setting bullet colors
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(TrailColor, 0.0f), new GradientColorKey(TrailColor, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0.0f), new GradientAlphaKey(0, 1.0f) }
        );
        if (hitNormal != new Vector3(999,999,999)){
            
            //create hit particles
            Transform hit = Instantiate(HitPrefab, hitPoint, Quaternion.FromToRotation(Vector3.right, hitNormal));
            ParticleSystem.ColorOverLifetimeModule settings = hit.GetComponent<ParticleSystem>().colorOverLifetime;
            settings.color = gradient;
            Destroy(hit.gameObject, 0.5f);
        }

        //create bullet trail
		Transform trail = Instantiate (BulletTrailPrefab, firePointPosition, firePoint.rotation) as Transform;
        trail.Rotate (Vector3.forward * RotationOffset);
        trail.GetComponent<TrailRenderer> ().colorGradient = gradient;
        trail.GetComponent<MoveTrail>().endPoint = hitPoint;

        //create muzzle flash
        Transform muzzle = Instantiate (MuzzleFlashPrefab, firePointPosition, firePoint.rotation) as Transform;
        muzzle.parent = firePoint;
        muzzle.GetComponent<SpriteRenderer> ().color = TrailColor;
        float size = Random.Range (FlareSize - 0.1f, FlareSize + 0.1f);
        muzzle.localScale = new Vector3 (size, size, size);
        Destroy (muzzle.gameObject, 0.02f);

        //shake camera
        camShake.Shake(CamShakeAmount, CamShakeLength);
	}
}
