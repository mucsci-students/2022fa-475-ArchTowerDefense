using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TurretBuySystem : MonoBehaviour
{
    public GameObject buyTurretMenu;
    private GameObject turretPlatform;

    public GameObject player;
    private Currency moneyBag;

    void Update()
    {
        moneyBag = player.GetComponent<Currency>();
    }
    public void ShowBuyTurretMenu(GameObject platform)
    {
        turretPlatform = platform;
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;

        buyTurretMenu.gameObject.SetActive(true);
        turretPlatform.GetComponent<TurretFoundation>().inMenu = true;
    }

    public void HideBuyTurretMenu()
    {
        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;

        buyTurretMenu.gameObject.SetActive(false);
        turretPlatform.GetComponent<TurretFoundation>().inMenu = false;

    }
    public void buyMini()
    {
        if (moneyBag.shekels >= 500)
        {
            moneyBag.purchase(500);
            turretPlatform.GetComponent<TurretFoundation>().BuildMiniGun();
            HideBuyTurretMenu();
        }
    }

    public void buyFlame()
    {
        if (moneyBag.shekels >= 1000)
        {
            moneyBag.purchase(1000);
            turretPlatform.GetComponent<TurretFoundation>().BuildFlameThrower();
            HideBuyTurretMenu();
        }
    }

    public void buySlow()
    {
        if (moneyBag.shekels >= 750)
        {
            moneyBag.purchase(750);
            turretPlatform.GetComponent<TurretFoundation>().BuildSlowDown();
            HideBuyTurretMenu();
        }
    }

    public void buySnipe()
    {
        if (moneyBag.shekels >= 1500)
        {
            moneyBag.purchase(1500);
            turretPlatform.GetComponent<TurretFoundation>().BuildSniper();
            HideBuyTurretMenu();
        }
    }
}
