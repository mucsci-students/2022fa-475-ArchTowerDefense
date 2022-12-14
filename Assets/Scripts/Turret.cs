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

    [Header("Rotating Barrel")]
    public bool rotating = false;
    public float barrelSpeedMax = 300f;
    public float barrelAcc = 5f;
    private float barrelSpeed = 0f;
    public Transform turretBarrel;

	[Header("Use Fire")]
	public bool useFire = false;
	public float burnVal = 1;
	public GameObject flameEffect;
	public Transform firePointLeft;
	public Transform firePointRight;
	private GameObject leftFire;
	private GameObject rightFire;

	[Header("Use Laser")]
	public bool useLaser = false;

	public int laserDamage = 0;
	public float slowAmount = .5f;

	public LineRenderer lineRenderer;
	public GameObject laserEffect;
	private GameObject laser;

    [Header("Is Sniper")]
    public bool isSniper = false;
    public Transform firePointSub;
    public Transform firePointSide1;
    public Transform firePointSide2;

    [Header("Unity Setup Fields")]

	public string enemyTag = "Enemy";
	public Transform partToRotate;
    public GameObject nextStage;
    public float turnSpeed = 10f;
	public Transform firePoint;
	private bool audioPlaying = false;

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

		if (useLaser)
		{
			laser = Instantiate(laserEffect, firePoint.position, transform.rotation);
        	laser.transform.SetParent(firePoint);
		}
    }
	
	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
        RaycastHit hit;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, new Vector3(enemy.transform.position.x,
				enemy.transform.position.y, enemy.transform.position.z));
			if (distanceToEnemy < shortestDistance)
			{
                if (Physics.Linecast(firePoint.transform.position, new Vector3(enemy.transform.position.x,
					enemy.transform.position.y + 3, enemy.transform.position.z), out hit)) {
                    if (hit.transform.CompareTag("Ground"))
                    {
                        continue;
                    }
                }
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
            else if (useLaser)
			{
				// if (lineRenderer.enabled)
				// {
				// 	lineRenderer.enabled = false;
				//	laser.GetComponent<ParticleSystem>().Stop();
				// }
				laser.GetComponent<ParticleSystem>().Stop();
			}
			else if (rotating)
			{
				turretBarrel.Rotate(0f, 0f, Time.deltaTime * fireRate * barrelSpeed);
            	DecreaseBarrelSpeed();
			}

			if (audioPlaying)
			{
				audioPlaying = false;
				GetComponent<AudioSource>().Stop();
			}
			return;
		}

		LockOnTarget();

		if (!audioPlaying)
		{
			audioPlaying = true;
			GetComponent<AudioSource>().Play();
		}

		if (useFire)
		{
			Fire();
		}
        else if (useLaser)
		{
			Laser();
		}
		else
		{
            if (fireCountdown <= 0f)
            {
                Shoot(firePoint);
                fireCountdown = 1f / fireRate;

                if (isSniper)
                {
                    // Sub sniper gun
                    if (Vector3.Distance(target.position, transform.position) <= (range / 3.0f * 2.0f) && firePointSub != null)
                    {
                        StartCoroutine(DelayShot(firePointSub, fireCountdown / 4.0f));

                        // Side sniper guns
                        if (Vector3.Distance(target.position, transform.position) <= (range / 3.0f) && firePointSide1 != null)
                        {
                            StartCoroutine(DelayShot(firePointSide1, fireCountdown / 2.0f));
                            StartCoroutine(DelayShot(firePointSide2, fireCountdown / 4.0f * 3.0f));
                        }
                    }
                }
            }

			fireCountdown -= Time.deltaTime;

            if (rotating)
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
		leftFire.transform.rotation = transform.GetChild(0).rotation;
		rightFire.transform.rotation = transform.GetChild(0).rotation;
		leftFire.GetComponent<ParticleSystem>().Play();
		rightFire.GetComponent<ParticleSystem>().Play();
	}

    void Laser ()
	{
		targetEnemy.TakeDamage(laserDamage * Time.deltaTime);
		targetEnemy.Slow(slowAmount);

		// if (!lineRenderer.enabled)
		// {
		// 	lineRenderer.enabled = true;
		// 	impactEffect.Play();
		// 	impactLight.enabled = true;
		// }

		// lineRenderer.SetPosition(0, firePoint.position);
		// lineRenderer.SetPosition(1, target.position);

		// Vector3 dir = firePoint.position - target.position;

		// laser.transform.position = target.position + dir.normalized;
		// laser.transform.rotation = Quaternion.LookRotation(dir);
		laser.transform.rotation = transform.GetChild(0).rotation;
		laser.GetComponent<ParticleSystem>().Play();
	}

	void Shoot (Transform firePoint)
	{
		GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
		Bullet bullet = bulletGO.GetComponent<Bullet>();

        GameObject effectIns = Instantiate(barrelSmoke, firePoint.position, transform.rotation);
        effectIns.transform.SetParent(transform.GetChild(0));
        Destroy(effectIns, 5f);

        if (bullet != null)
			bullet.Seek(target);
	}

    IEnumerator DelayShot(Transform firePoint, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Shoot(firePoint);
    }

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);

        if (isSniper)
        {
            Gizmos.DrawWireSphere(transform.position, range / 3.0f * 2.0f);
            Gizmos.DrawWireSphere(transform.position, range / 3.0f);
        }
	}
}
