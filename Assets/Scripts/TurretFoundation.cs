// using System.Diagnostics;
// using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TurretFoundation : MonoBehaviour
{
    private float buildRange = 6f;
    public GameObject player;
    public GameObject minigunTurret;
    public GameObject flamethrowerTurret;
    public GameObject slowdownTurret;
    public GameObject sniperTurret;

    public TextMeshProUGUI sellText;
    public TextMeshProUGUI buyText;
    public TextMeshProUGUI upgradeText;

    // How much the user has spent on this turret to upgrade it
    // Used to calculate sellback price
    private int turretValue = 0;

    // So that while in a menu, the prompts dont show up
    private bool inMenu = false;

    // prices of each upgrade will scale off of the base price 
    // private int miniGunPrice = 0;
    // private int flameThrowerPrice = 0;
    // private int miniGunPrice = 0;
    // private int slowDownPrice = 0;
    // private int sniperPrice = 0;



    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        // If the player is close enough to the foundation to build...
        if (distanceToPlayer <= buildRange)
        {

            // check if there is a turret placed
            isTurretBuilt();
            // SAM, MENUS CAN GO HERE

            if (!inMenu)
            {
                // Is there already a turret there?
                if (!isTurretBuilt()) 
                {
                    Debug.Log("THE PROMPT SHOULKD FUCJING SHOW FUCK YOU");
                    buyText.gameObject.SetActive(true);
                }
                // Occupied, prompt to sell or upgrade
                else
                {
                    Debug.Log("THE PROMPT SHOULKD FUCJING SHOW FUCK YOU");
                    int sellPrice = 1000;
                    sellText.SetText("Press Q To Sell  +" + sellPrice);

                    int upgradePrice = 3000;
                    upgradeText.SetText("Press E To Upgrade -" + upgradePrice);
                    
                    upgradeText.gameObject.SetActive(true);
                    sellText.gameObject.SetActive(true);

                }
            }




            // Debug.Log("ABLE TO BUILD");
            // Build minigun or upgrade when E is pressed
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Minigun");
                // Build a minigun turret if not there
                if (!isTurretBuilt()) {
                    BuildMiniGun();
                }

                // Else upgrade to the next version
                else if (transform.GetChild(1).GetComponent<Turret>().nextStage != null)
                {
                    var turretUpgrade = transform.GetChild(1).GetComponent<Turret>().nextStage;
                    // Keeps the turret head facing the direction it's currently facing
                    var turretHeadRotation = transform.GetChild(1).GetChild(0).rotation;
                    Destroy(transform.GetChild(1).gameObject);
                    var builtTurret = Instantiate(turretUpgrade, transform.position, transform.rotation);
                    builtTurret.transform.GetChild(0).rotation = turretHeadRotation;
                    builtTurret.transform.SetParent(transform);
                    Debug.Log("upgraded turret with the parent as " + transform);
                    builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
                }
            }

            // Build flamethrower when R is pressed
            else if (Input.GetKeyDown(KeyCode.R))
            {
                // Build a flamethrower turret if not there
                if (!isTurretBuilt())
                {
                    BuildFlameThrower();
                }
            }

            // Build slowdown when T is pressed
            else if (Input.GetKeyDown(KeyCode.T))
            {
                // Build a slowdown turret if not there
                if (!isTurretBuilt())
                {
                    BuildSlowDown();
                }
            }

            // Build sniper when Y is pressed
            else if (Input.GetKeyDown(KeyCode.Y))
            {
                // Build a sniper turret if not there
                if (!isTurretBuilt())
                {
                    BuildSniper();
                }
            }

            // Delete turret when Q is pressed
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                // Deletes turret if there
                if (isTurretBuilt())
                {
                    SellTurret();
                }
            }
        } else {
            // When they're not in range the prompts should go away
            buyText.gameObject.SetActive(false);
            upgradeText.gameObject.SetActive(false);
            sellText.gameObject.SetActive(false);

        }
    }

    bool isTurretBuilt()
    {
        foreach (Transform tr in transform)
        {
            if (tr.tag == "Turret")
            {
                // Debug.Log("THE WHORE TURRET IS BEING DETECTED");
                return true;
            }
        }
        // Debug.Log("THE WHORE TURRET IS not BEING DETECTED");
        return false;
    }

    void SellTurret()
    {
        // Play a noise?

        // Remove the turret
        Destroy(transform.GetChild(1).gameObject);

        // ADD THE FUNDS BACK TO THE PLAYERS SHEKELS

        //Replace the glowy
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void BuildMiniGun()
    {
        //Remove the glowy
        transform.GetChild(0).gameObject.SetActive(false);

        Debug.Log("Minigun");
        // Build a minigun turret if not there
        var builtTurret = Instantiate(minigunTurret, transform.position, transform.rotation);
        builtTurret.transform.SetParent(transform);
        builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

    }

    void BuildFlameThrower()
    {
        //Remove the glowy
        transform.GetChild(0).gameObject.SetActive(false);

        var builtTurret = Instantiate(flamethrowerTurret, transform.position,  transform.rotation);
        builtTurret.transform.SetParent(transform);
        builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    void BuildSlowDown()
    {
        //Remove the glowy
        transform.GetChild(0).gameObject.SetActive(false);

        var builtTurret = Instantiate(slowdownTurret, transform.position,  transform.rotation);
        builtTurret.transform.SetParent(transform);
        builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    void BuildSniper()
    {
        //Remove the glowy
        transform.GetChild(0).gameObject.SetActive(false);

        var builtTurret = Instantiate(sniperTurret, transform.position, transform.rotation);
        builtTurret.transform.SetParent(transform);
        builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    // Draws turret foundation's interactable distance in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, buildRange);
    }
}
