using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float startSpeed = 10f;
	public float burnRecover = 1f;

	[HideInInspector]
	public float speed;

	public float startHealth = 100;
	private float health;
	private float burning = 0;

	public int worth = 50;

	//public GameObject deathEffect;

	[Header("Unity Stuff")]
	//public Image healthBar;

	private bool isDead = false;

	void Start ()
	{
		speed = startSpeed;
		health = startHealth;
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
		print("BURN DAMAGE AT " + Time.deltaTime + ": " + burning);
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
		speed = startSpeed * (1f - pct);
	}

	void Die ()
	{
		isDead = true;

		PlayerStats.Money += worth;

		//GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		//Destroy(effect, 5f);

		--WaveSpawner.EnemiesAlive;

		Destroy(gameObject);
	}

}
