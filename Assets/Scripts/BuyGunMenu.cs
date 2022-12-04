using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGunMenu : MonoBehaviour
{

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    public GameObject GunMenu;
    private Transform WeaponHolder;

    private int activeGun = 0;
    void Start()
    {
        WeaponHolder = player.transform.GetChild(0).GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        GetActiveWeapon();

        // ALSO CHECK WE R IN BUILD PHASE
        // Open the menu to buy
        if (Input.GetKeyDown(KeyCode.G) && !player.m_MouseLook.inMenu)
        {
            Debug.Log(player.m_MouseLook.inMenu);
            GunMenu.gameObject.SetActive(true);

        }
        // Close the menu without buying
        else if (Input.GetKeyDown(KeyCode.G) && player.m_MouseLook.inMenu)
        {
            GunMenu.gameObject.SetActive(false);
        }

    }

    private void GetActiveWeapon()
    {
        foreach (Transform child in WeaponHolder)
        {
            if (child.gameObject.activeSelf)
            {
                activeGun = child.GetSiblingIndex();
            }
        }
    }

    public void ChoosePistol()
    {
        if (activeGun != 0)
        {
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(0).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }

    public void BuyAR()
    {
        if (activeGun != 1)
        {
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(1).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }

    public void BuySMG()
    {
        if (activeGun != 2)
        {
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(2).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }

    public void BuyProcrastinator()
    {
        if (activeGun != 3)
        {
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(3).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }

    public void BuyAK()
    {
        if (activeGun != 4)
        {
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(4).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }


}
