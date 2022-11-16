using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFoundation : MonoBehaviour
{
    private float buildRange = 3f;
    public GameObject player;
    public GameObject turret;
    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the player is close enough to the foundation to build...
        if (distanceToPlayer <= buildRange)
        {
            // SAM, MENUS CAN GO HERE

            // If E is pressed + no turret on that spot
            if (Input.GetKeyDown(KeyCode.E) && transform.childCount == 0)
            {
                var builtTurret = Instantiate(turret, transform.position, transform.rotation);
                builtTurret.transform.SetParent(transform);
            }
        }
    }

    // Draws turret foundation's interactable distance in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, buildRange);
    }
}
