using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Arch : MonoBehaviour
{
    [HideInInspector]
    public float startHealth = 100;
    [Header("Health")]
    private float health;
    public GameObject healthBar;

    [Header("Statuses")]
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.GetComponent<Slider>().value = health / startHealth;
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
