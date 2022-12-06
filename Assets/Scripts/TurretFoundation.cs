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

    public GameObject turretBuyMenu;

    // How much the user has spent on this turret to upgrade it
    // Used to calculate sellback price
    public float turretValue = 0;
    private int turretLevel = 1;

    // So that while in a menu, the prompts dont show up
    // move this to FPS controller to acces from other things??
    public bool inMenu = false;
    private bool playerInRange = false;
    private Currency moneyBag;

    private float sellPrice = 0;
    private float upgradePrice = 0;

    // prices of each upgrade will scale off of the base price 
    // private int miniGunPrice = 0;
    // private int flameThrowerPrice = 0;
    // private int miniGunPrice = 0;
    // private int slowDownPrice = 0;
    // private int sniperPrice = 0;


    void Start()
    {
        moneyBag = player.GetComponent<Currency>();
    }


    void Update()
    {

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        isTurretBuilt();

        if (playerInRange)
        {
            // Update Prompts
            if (!inMenu)
            {
                if (!isTurretBuilt())
                {
                    PromptBuy();
                }
                // Occupied, prompt to sell or upgrade
                else
                {
                    PromptUpSell();
                }
            } else
            {
                // No prompts in menus
                PromptClear();
            }


            // Buy Turret Menu or Upgrade
            if (Input.GetKeyDown(KeyCode.E) && !inMenu)
            {
                if (!isTurretBuilt())
                {
                    // Make the buy menu active
                    turretBuyMenu.GetComponent<TurretBuySystem>().ShowBuyTurretMenu(this.gameObject);
                }
                // Else upgrade to the next version
                else if (transform.GetChild(1).GetComponent<Turret>().nextStage != null)
                {
                    if (moneyBag.shekels >= upgradePrice)
                    {
                        UpgradeTurret();
                    }
                }

                // Exiting the menu
            }
            else if (Input.GetKeyDown(KeyCode.E) && inMenu)
            {
                turretBuyMenu.GetComponent<TurretBuySystem>().HideBuyTurretMenu();
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
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            playerInRange = false;

            // When they're not in range the prompts should go away
            PromptClear();

        }
    }

    private void PromptBuy()
    {
        buyText.gameObject.SetActive(true);
        upgradeText.gameObject.SetActive(false);
        sellText.gameObject.SetActive(false);

    }

    private void PromptUpSell()
    {
        sellPrice = turretValue / 2;
        sellText.SetText("Press Q To Sell  +" + sellPrice);

        if (turretLevel == 1)
        {
            upgradePrice = turretValue + 250;
        }
        else if (turretLevel == 2)
        {
            upgradePrice = turretValue * 2;
        }
        upgradeText.SetText("Press E To Upgrade -" + upgradePrice);

        if (turretLevel == 3)
        {
            upgradeText.SetText("Max upgrade level reached");
        }

        upgradeText.gameObject.SetActive(true);
        sellText.gameObject.SetActive(true);
        buyText.gameObject.SetActive(false);

    }

    private void PromptClear()
    {
        buyText.gameObject.SetActive(false);
        upgradeText.gameObject.SetActive(false);
        sellText.gameObject.SetActive(false);
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
        // money
        moneyBag.addShekels(sellPrice);

        // Remove the turret
        Destroy(transform.GetChild(1).gameObject);

        // ADD THE FUNDS BACK TO THE PLAYERS SHEKELS

        //Replace the glowy
        transform.GetChild(0).gameObject.SetActive(true);

        turretLevel = 1;
    }

    public void BuildMiniGun()
    {
        //Remove the glowy
        transform.GetChild(0).gameObject.SetActive(false);

        Debug.Log("Minigun");
        // Build a minigun turret if not there
        var builtTurret = Instantiate(minigunTurret, transform.position, transform.rotation);
        builtTurret.transform.SetParent(transform);
        builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);

    }

    public void BuildFlameThrower()
    {
        //Remove the glowy
        transform.GetChild(0).gameObject.SetActive(false);

        var builtTurret = Instantiate(flamethrowerTurret, transform.position, transform.rotation);
        builtTurret.transform.SetParent(transform);
        builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    public void BuildSlowDown()
    {
        //Remove the glowy
        transform.GetChild(0).gameObject.SetActive(false);

        var builtTurret = Instantiate(slowdownTurret, transform.position, transform.rotation);
        builtTurret.transform.SetParent(transform);
        builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    public void BuildSniper()
    {
        //Remove the glowy
        transform.GetChild(0).gameObject.SetActive(false);

        var builtTurret = Instantiate(sniperTurret, transform.position, transform.rotation);
        builtTurret.transform.SetParent(transform);
        builtTurret.transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z);
    }

    void UpgradeTurret()
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
        turretLevel++;
        turretValue = upgradePrice;
    }

    // Draws turret foundation's interactable distance in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, buildRange);
    }
}
