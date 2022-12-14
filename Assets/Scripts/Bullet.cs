using UnityEngine;

public class Bullet : MonoBehaviour {

	private Transform target;

	public float speed = 70f;

	public int damage = 50;

	public float explosionRadius = 0f;
    public GameObject impactEffect;

    public void Seek (Transform _target)
	{
		target = _target;
	}

	// Update is called once per frame
	void Update () {

		if (target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = new Vector3(target.position.x, target.position.y + 3, target.position.z) - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if (dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);

	}

	void HitTarget ()
	{
		GameObject effectIns = Instantiate(impactEffect, new Vector3(target.position.x, target.position.y + 3,
			target.position.z), transform.rotation);
		effectIns.transform.SetParent(target);
		Destroy(effectIns, 5f);

		if (explosionRadius > 0f)
		{
			Explode();
		}
        else
		{
			Damage(target);
		}
        
		Destroy(gameObject);
	}

	void Explode ()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in colliders)
		{
			if (collider.tag == "Enemy")
			{
				Damage(collider.transform);
			}
			else if(collider.tag == "Arch")
			{
				Damage(collider.transform);
			}
		}
	}

	void Damage (Transform enemy)
	{
		Enemy e = enemy.GetComponent<Enemy>();
		Arch a = enemy.GetComponent<Arch>();
		if (e != null)
		{
			e.TakeDamage(damage);
		}
		if(a != null)
		{
			a.TakeDamage(damage);
		}
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
