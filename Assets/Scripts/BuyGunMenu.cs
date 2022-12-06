using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyGunMenu : MonoBehaviour
{

    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController player;
    public GameObject GunMenu;
    private Transform WeaponHolder;
    private Currency moneyBag;
    private GameObject spawnSystem;
    private SpawnSystem ss;

    private int activeGun = 0;
    void Start()
    {
        spawnSystem = GameObject.Find("SpawnSystem");
        ss = spawnSystem.GetComponent<SpawnSystem>();
        
        WeaponHolder = player.transform.GetChild(0).GetChild(0);
        moneyBag = WeaponHolder.transform.parent.parent.GetComponent<Currency>();
    }

    // Update is called once per frame
    void Update()
    {
        GetActiveWeapon();

        // ALSO CHECK WE R IN BUILD PHASE
        // Open the menu to buy
        if (Input.GetKeyDown(KeyCode.G) && !player.m_MouseLook.inMenu && ss.inBuildPeriod)
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
        if (activeGun != 1 && moneyBag.shekels >= 2500)
        {
            moneyBag.purchase(2500);
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(1).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }

    public void BuySMG()
    {
        if (activeGun != 2 && moneyBag.shekels >= 1000)
        {
            moneyBag.purchase(1000);
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(2).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }

    public void BuyProcrastinator()
    {
        if (activeGun != 3 && moneyBag.shekels >= 3000)
        {
            moneyBag.purchase(3000);
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(3).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }

    public void BuyAK()
    {
        if (activeGun != 4 && moneyBag.shekels >= 2500)
        {
            moneyBag.purchase(2500);
            WeaponHolder.GetChild(activeGun).gameObject.SetActive(false);
            WeaponHolder.GetChild(4).gameObject.SetActive(true);
            GunMenu.gameObject.SetActive(false);
        }

    }


}
