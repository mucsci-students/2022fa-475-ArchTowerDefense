using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Arch : MonoBehaviour
{
    private Camera cam;
    [HideInInspector]
    public float startHealth = 100;
    [Header("Health")]
    private float health;
    public GameObject healthBar;
    public Vector3 healthOffset;

    [Header("Statuses")]
    private bool isDead = false;
    private bool underAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        health = startHealth;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 visiblePos = cam.WorldToViewportPoint(transform.position);
        
        if(visiblePos.z >= 0)
        {
            healthBar.SetActive(true);
        }
        else
        {
            healthBar.SetActive(false);
        }

        if(healthBar.activeSelf)
        {
            Vector3 pos = cam.WorldToScreenPoint(transform.position + healthOffset);
            healthBar.transform.position = pos;
            healthBar.GetComponent<Slider>().value = health / startHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        Destroy(gameObject);
    }
}
