using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TurretBuySystem : MonoBehaviour
{
    public GameObject buyTurretMenu;
    private GameObject turretPlatform;

    public void ShowBuyTurretMenu(GameObject platform)
    {
        turretPlatform = platform;
        Cursor.visible = true;

        Cursor.lockState = CursorLockMode.None;

        buyTurretMenu.gameObject.SetActive(true);
    }

    public void HideBuyTurretMenu()
    {

        Cursor.visible = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;

        buyTurretMenu.gameObject.SetActive(false);

    }
    public void buyMini()
    {
        turretPlatform.GetComponent<TurretFoundation>().BuildMiniGun();
        HideBuyTurretMenu();
    }

    public void buyFlame()
    {
        turretPlatform.GetComponent<TurretFoundation>().BuildFlameThrower();
        HideBuyTurretMenu();
    }

    public void buySlow()
    {
        turretPlatform.GetComponent<TurretFoundation>().BuildSlowDown();
        HideBuyTurretMenu();
    }

    public void buySnipe()
    {
        turretPlatform.GetComponent<TurretFoundation>().BuildSniper();
        HideBuyTurretMenu();
    }
}
