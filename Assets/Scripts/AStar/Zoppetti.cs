using System.Runtime.ExceptionServices;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zoppetti : MonoBehaviour
{

    private Transform target;
    private Enemy targetEnemy;

    [Header("Zooop")]
    public Transform player;
    private NavMeshAgent agent;
    public Transform firePoint;

    [Header("General")]
    public string enemyTag = "Enemy";
    public float range = 15f;

    [Header("Attack")]
    public float fireRate = 1f;
    private float fireCountdown = 0f;
    public GameObject bulletPrefab;


    // [Header("References")]
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;
        RaycastHit hit;
		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
                if (Physics.Linecast(firePoint.transform.position, enemy.transform.position, out hit)) 
                {
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
		} 
        else
		{
			target = null;
		}
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Respec");
            agent.destination = player.position;
          
            gameObject.GetComponent<Animator>().Play("Zoop.Run");
            
        }
        // gameObject.GetComponent<Animator>().Play("Zoop.Idle");
        if(agent.transform.position == agent.destination)
        {
            Debug.Log("Arrived");
            gameObject.GetComponent<Animator>().Play("Zoop.Idle");
        }

        if(fireCountdown <= 0f)
        {
            Shoot(firePoint);
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    void Shoot(Transform firePoint)
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if(bullet != null)
            bullet.Seek(target);
    }
}
