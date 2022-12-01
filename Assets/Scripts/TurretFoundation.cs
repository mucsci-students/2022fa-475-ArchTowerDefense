// using System.Diagnostics;
// using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretFoundation : MonoBehaviour
{
    private float buildRange = 3f;
    public GameObject player;
    public GameObject minigunTurret;
    public GameObject flamethrowerTurret;
    public GameObject slowdownTurret;
    public GameObject sniperTurret;
    
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If the player is close enough to the foundation to build...
        if (distanceToPlayer <= buildRange)
        {
            // SAM, MENUS CAN GO HERE
            Debug.Log("ABLE TO BUILD");
            // Build minigun or upgrade when E is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Minigun");
                // Build a minigun turret if not there
                if (transform.childCount == 0) {
                    var builtTurret = Instantiate(minigunTurret, transform.position,  transform.rotation);
                    builtTurret.transform.SetParent(transform);
                    builtTurret.transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
                }

                // Else upgrade to the next version
                else if (transform.GetChild(0).GetComponent<Turret>().nextStage != null)
                {
                    var turretUpgrade = transform.GetChild(0).GetComponent<Turret>().nextStage;
                    // Keeps the turret head facing the direction it's currently facing
                    var turretHeadRotation = transform.GetChild(0).GetChild(0).rotation;
                    Destroy(transform.GetChild(0).gameObject);
                    var builtTurret = Instantiate(turretUpgrade, transform.position, transform.rotation);
                    builtTurret.transform.GetChild(0).rotation = turretHeadRotation;
                    builtTurret.transform.SetParent(transform);
                    builtTurret.transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
                }
            }

            // Build flamethrower when R is pressed
            else if (Input.GetKeyDown(KeyCode.R))
            {
                // Build a flamethrower turret if not there
                if (transform.childCount == 0) {
                    var builtTurret = Instantiate(flamethrowerTurret, transform.position,  transform.rotation);
                    builtTurret.transform.SetParent(transform);
                    builtTurret.transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
                }
            }

            // Build slowdown when T is pressed
            else if (Input.GetKeyDown(KeyCode.T))
            {
                // Build a slowdown turret if not there
                if (transform.childCount == 0) {
                    var builtTurret = Instantiate(slowdownTurret, transform.position,  transform.rotation);
                    builtTurret.transform.SetParent(transform);
                    builtTurret.transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
                }
            }

            // Build sniper when Y is pressed
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                // Build a sniper turret if not there
                if (transform.childCount == 0)
                {
                    var builtTurret = Instantiate(sniperTurret, transform.position, transform.rotation);
                    builtTurret.transform.SetParent(transform);
                    builtTurret.transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
                }
            }

            // Delete turret when Q is pressed
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                // Deletes turret if there
                if (transform.childCount != 0)
                {
                    Destroy(transform.GetChild(0).gameObject);
                }
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
