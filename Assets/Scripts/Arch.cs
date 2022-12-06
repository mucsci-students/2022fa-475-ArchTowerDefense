using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Arch : MonoBehaviour
{
    [HideInInspector]
    public float startHealth = 10;
    [Header("Health")]
    private float health;
    public GameObject healthBar;

    [Header("Statuses")]
    private bool isDead = false;

    public AudioClip protectLogo;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBar != null)
            healthBar.GetComponent<Slider>().value = health / startHealth;
    }

    public float GetHealth()
    {
        return health;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0 && !isDead)
        {
            healthBar.GetComponent<Slider>().value = 0;
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}
