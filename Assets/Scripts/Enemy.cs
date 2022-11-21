using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Camera cam;
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
    public GameObject healthBar;
    public Vector3 healthOffset;

    private bool isDead = false;

	void Start ()
	{
        cam = Camera.main;

		gameObject.layer = LayerMask.NameToLayer("Enemies");
		speed = startSpeed;
		health = startHealth;
	}

    void Update()
    {
        // Hides health bar if enemy is behind the player
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
		isDead = true;

		PlayerStats.Money += worth;

		//GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		//Destroy(effect, 5f);

		--WaveSpawner.EnemiesAlive;

		Destroy(gameObject);
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
