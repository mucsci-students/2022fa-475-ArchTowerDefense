using UnityEngine;
using System.Collections;

public class Turret : MonoBehaviour {

	private Transform target;
	private Enemy targetEnemy;

	[Header("General")]

	public float range = 15f;

	[Header("Use Bullets (default)")]
	public GameObject bulletPrefab;
    public GameObject barrelSmoke;
	public float fireRate = 1f;
	private float fireCountdown = 0f;
    public float barrelSpeedMax = 300f;
    public float barrelAcc = 5f;
    private float barrelSpeed = 0f;
	public Transform firePoint;
    public Transform turretBarrel;

	[Header("Use Fire")]
	public bool useFire = false;
	public float burnVal = 1;
	//public LineRenderer lineRenderer;
	public GameObject flameEffect;
	public Light impactLight;
	public Transform firePointLeft;
	public Transform firePointRight;
	private GameObject leftFire;
	private GameObject rightFire;

	// [Header("Use Laser")]
	// public bool useLaser = false;

	// public int damageOverTime = 30;
	// public float slowAmount = .5f;

	// public LineRenderer lineRenderer;
	// public ParticleSystem impactEffect;
	// public Light impactLight;

	[Header("Unity Setup Fields")]

	public string enemyTag = "Enemy";
	public Transform partToRotate;
    public GameObject nextStage;
    public float turnSpeed = 10f;

	// Use this for initialization
	void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);

		if (useFire)
		{
			leftFire = Instantiate(flameEffect, firePointLeft.position, transform.rotation);
        	leftFire.transform.SetParent(firePointLeft);

			rightFire = Instantiate(flameEffect, firePointRight.position, transform.rotation);
        	rightFire.transform.SetParent(firePointRight);
		}
    }
	
	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if (nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
			targetEnemy = nearestEnemy.GetComponent<Enemy>();
		} else
		{
			target = null;
		}
	}

	// Update is called once per frame
	void Update ()
    {
        if (target == null)
        {
			if (useFire)
			{
				leftFire.GetComponent<ParticleSystem>().Stop();
				rightFire.GetComponent<ParticleSystem>().Stop();
			}
            // else if (useLaser)
			// {
			// 	if (lineRenderer.enabled)
			// 	{
			// 		lineRenderer.enabled = false;
			// 		impactEffect.Stop();
			// 		impactLight.enabled = false;
			// 	}
			// }
			else
			{
				turretBarrel.Rotate(0f, 0f, Time.deltaTime * fireRate * barrelSpeed);
            	DecreaseBarrelSpeed();
			}

			return;
		}

		LockOnTarget();

		if (useFire)
		{
			Fire();
		}
        // else if (useLaser)
		// {
		// 	Laser();
		// }
		else
		{
			if (fireCountdown <= 0f)
			{
				Shoot();
				fireCountdown = 1f / fireRate;
			}

			fireCountdown -= Time.deltaTime;
        	IncreaseBarrelSpeed();
		}

	}

	void LockOnTarget ()
	{
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void IncreaseBarrelSpeed()
    {
        if (barrelSpeed < barrelSpeedMax)
        {
            barrelSpeed += barrelAcc;
        };
    }

    void DecreaseBarrelSpeed()
    {
        if (barrelSpeed > 0)
        {
            barrelSpeed -= barrelAcc;
        }
    }

	void Fire ()
	{
		targetEnemy.Burning(burnVal * Time.deltaTime * .1f);
		leftFire.GetComponent<ParticleSystem>().Play();
		rightFire.GetComponent<ParticleSystem>().Play();
	}

    // void Laser ()
	// {
	// 	targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
	// 	targetEnemy.Slow(slowAmount);

	// 	if (!lineRenderer.enabled)
	// 	{
	// 		lineRenderer.enabled = true;
	// 		impactEffect.Play();
	// 		impactLight.enabled = true;
	// 	}

	// 	lineRenderer.SetPosition(0, firePoint.position);
	// 	lineRenderer.SetPosition(1, target.position);

	// 	Vector3 dir = firePoint.position - target.position;

	// 	impactEffect.transform.position = target.position + dir.normalized;

	// 	impactEffect.transform.rotation = Quaternion.LookRotation(dir);
	// }

	void Shoot ()
	{
		GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

        GameObject effectIns = Instantiate(barrelSmoke, firePoint.position, transform.rotation);
        effectIns.transform.SetParent(transform.GetChild(0));
        Destroy(effectIns, 5f);

        if (bullet != null)
			bullet.Seek(target);
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}
