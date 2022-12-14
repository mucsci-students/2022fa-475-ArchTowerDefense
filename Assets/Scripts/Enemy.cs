using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Camera cam;
	private Animator anim;
	private GameManager gameManager;
	public float startSpeed = 10f;
	public float burnRecover = 1f;

	[HideInInspector]
	public float speed;

	public float startHealth = 100;
	public float attack = 1;
	private float health;
	private float burning = 0;

	public int worth = 50;

	public GameObject deathEffect;

	[Header("Unity Stuff")]
    public GameObject healthBar;
    public Vector3 healthOffset;

	private GameObject player;
	private Currency moneyBag;

    private bool isDead = false;

	void Start ()
	{
        cam = Camera.main;

		GameObject[] player = GameObject.FindGameObjectsWithTag("Player");
		moneyBag = player[0].transform.GetComponent<Currency>();

		gameManager = FindObjectOfType<GameManager>();
		anim = GetComponent<Animator>();

		gameObject.layer = LayerMask.NameToLayer("Enemies");
		speed = startSpeed;
		health = startHealth;
	}

    void Update()
    {
        // Hides health bar if enemy is behind the player
       if (!isDead)
	   {
			Vector3 visiblePos = cam.WorldToViewportPoint(transform.position);
			if (visiblePos.z >= 0)
			{
				healthBar.SetActive(true);
			}
			else
			{
				healthBar.SetActive(false);
			}

			if (healthBar.activeSelf)
			{
				Vector3 pos = cam.WorldToScreenPoint(transform.position + healthOffset);
				healthBar.transform.position = pos;

				healthBar.GetComponent<Slider>().value = health / startHealth;
			}
	   }
    }

    public void TakeDamage (float amount)
	{
		health -= amount;

		//healthBar.fillAmount = health / startHealth;

		if (health <= 0 && !isDead)
		{
			Die();
		}
	}

	public void Burning (float heat)
	{
		burning += heat;
		TakeDamage(burning);
		StartCoroutine(TimeFromBurn(burning));
	}

	IEnumerator TimeFromBurn(float burnVal) {
		yield return new WaitForSeconds(burnRecover);

		// Reset burning if not being burnt
		if (burnVal == burning)
			burning = 0;
	}

	public void Slow (float pct)
	{
		if (speed > 0.5f)
			speed = startSpeed * (1f - pct);
	}

	void Die ()
	{
		moneyBag.addShekels(worth);
		
		isDead = true;
		healthBar.SetActive(false);
		gameObject.tag = "Dead";
		GetComponent<CapsuleCollider>().isTrigger = true;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

		PlayerStats.Money += worth;

		anim.SetTrigger("dead");

		if (transform.Find("Bomb"))
		{
			GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
			Destroy(effect, 5f);

			Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);

			foreach (Collider col in hitColliders)
			{
				if (col.gameObject.CompareTag("Arch"))
				{
					col.gameObject.GetComponent<Arch>().TakeDamage(10);
				}
				else if (col.gameObject.CompareTag("Enemy"))
				{
					col.gameObject.GetComponent<Enemy>().TakeDamage(10);
				}
			}

			Destroy(gameObject);
			GetComponent<AudioSource>().Play();
		}
		else
			StartCoroutine(Disintegrate());
	}

	IEnumerator Disintegrate()
	{
		yield return new WaitForSeconds(1f);
		GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(effect, 5f);
		Destroy(gameObject);
	}

	public void DamageArch()
	{
		if (gameManager.arch != null)
			gameManager.arch.GetComponent<Arch>().TakeDamage(attack);
	}

    //private void OnBecameVisible()
    //{
    //    print("can see!");
    //    healthBar.SetActive(true);
    //}

    //private void OnBecameInvisible()
    //{
    //    print("cant see!");
    //    healthBar.SetActive(false);
    //}
}
