using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour {

	private Transform target;
	public GameObject waypoints;
	private int wavepointIndex = 0;

	private Enemy enemy;
	private bool reachedEnd = false;

	void Start()
	{
		enemy = GetComponent<Enemy>();
		target = waypoints.GetComponent<Waypoints>().points[0];
	}

	void Update()
	{
		if (gameObject.tag != "Dead" && !reachedEnd)
		{
			Vector3 dir = target.position - transform.position;
			transform.Translate(dir.normalized * enemy.speed * Time.deltaTime, Space.World);

			if (Vector3.Distance(transform.position, target.position) <= 0.4f)
			{
				GetNextWaypoint();
			}

			transform.rotation = Quaternion.LookRotation (dir);
			enemy.speed = enemy.startSpeed;
		}

		else if (!reachedEnd)
		{
			transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
		}
	}

	void GetNextWaypoint()
	{
		if (wavepointIndex >= waypoints.GetComponent<Waypoints>().points.Length - 1)
		{
			EndPath();
			return;
		}

		wavepointIndex++;
		target = waypoints.GetComponent<Waypoints>().points[wavepointIndex];
	}

	void EndPath()
	{
		reachedEnd = true;
		PlayerStats.Lives--;
		WaveSpawner.EnemiesAlive--;

		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

		if (transform.Find("Bomb"))
		{
			GameObject effect = Instantiate(GetComponent<Enemy>().deathEffect, transform.position, Quaternion.identity);
			Destroy(effect, 5f);
			Destroy(gameObject);
		}
		else
		{
			GetComponent<Animator>().SetTrigger("attack");	
			InvokeRepeating("DamageArch", 0f, 1f);
		}
	}

	void DamageArch()
	{
		enemy.GetComponent<Enemy>().DamageArch();
	}
}
