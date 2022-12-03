using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting,
        spread,
        range,
        reloadTime,
        timeBetweenShots;
    public int magazineSize,
        bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft,
        bulletsShot;

    bool shooting,
        readyToShoot,
        reloading;

    //Reference
    public Camera cam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public GameObject Gun;

    //  Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash;
    public GameObject groundImpact;
    public GameObject enemyImpact;
    public GameObject turretImpact;
    // public GameObject archImpact;
    public GameObject defaultImpact;
    [HideInInspector]
    private GameObject impactEffect;

    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        if(bulletsLeft < (.25 * magazineSize))
        {
            text.color = new Color(.698f, .133f, .133f, 1.0f);
        }
        else
        {
            text.color = new Color(0.1960784f, 0.1960784f, 0.1960f, 1.0f);
        }
        // update bullet num
        text.SetText(bulletsLeft + " / " + magazineSize);

    }

    private void MyInput()
    {
        // Are they able to shoot by holding down or only tapping?
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calc direction with spread
        Vector3 direction = cam.transform.forward + new Vector3(x, y, 0);

        // Ray Cast
        if (Physics.Raycast(cam.transform.position, direction, out rayHit, range))
        {
            Debug.Log(rayHit.collider.name);
            Debug.Log(rayHit.point);

            switch(rayHit.collider.name)
            {
                case "Ground":
                    impactEffect = groundImpact;
                    break;
                case "Zombie":
                case "Skeleton":
                case "RedSkeleton":
                case "BombSkeleton":
                    impactEffect = enemyImpact;
                    break;
                case "Turret":
                    impactEffect = turretImpact;
                    break;
                default:
                    impactEffect = defaultImpact;
                    break;
            }

            if (rayHit.collider.CompareTag("Enemy"))
                // Make sure enemies have the tag "Enemy" and have the TakeDamage function
                rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);

            if(rayHit.collider.CompareTag("Arch"))
                rayHit.collider.GetComponent<Arch>().TakeDamage(damage);
        }


        // Graphics
        GameObject muzzFlash = (GameObject)Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        muzzFlash.transform.parent = Gun.transform;
        
        GameObject impactGO = Instantiate(impactEffect, rayHit.point, cam.transform.rotation);
        Destroy(impactGO, 2f);

        // Recoil
        StartCoroutine(StartRecoil());

        bulletsLeft--;
        bulletsShot--;

        // Calling the function with some delay - the time between shooting
        Invoke("ResetShot", timeBetweenShooting);

        // How many times to shoot for each tap?
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    IEnumerator StartRecoil()
    {
        Debug.Log(Gun.name + " recoil");
        if(Gun.name == "Pistol")
        {
            Gun.GetComponent<Animator>().Play("Pistol.Shoot");
            Gun.GetComponent<Animator>().Play("PistolRecoil.SlideBack");
            yield return new WaitForSeconds(0.10f);
            Gun.GetComponent<Animator>().Play("Pistol.New State");
            Gun.GetComponent<Animator>().Play("PistolRecoil.New State");
        }
        else if(Gun.name == "AssaultRifle")
        {
            Gun.GetComponent<Animator>().Play("AssaultRifle.Shoot");
            yield return new WaitForSeconds(0.50f);
            Gun.GetComponent<Animator>().Play("AssaultRifle.New State");
        }
        else if(Gun.name == "SMG")
        {
            Gun.GetComponent<Animator>().Play("SMG.Shoot");
            yield return new WaitForSeconds(0.10f);
            Gun.GetComponent<Animator>().Play("SMG.New State");
        }
        else if(Gun.name == "procrastinator")
        {
            // Gun.GetComponent<Animator>().Play("Recoil");
            Gun.GetComponent<Animator>().Play("Procrastinator.Shoot");
            // yield return new WaitForSeconds(0.20f);
            yield return new WaitForSeconds(1.50f);
            Gun.GetComponent<Animator>().Play("Procrastinator.New State");
        }
        else if(Gun.name == "AK47")
        {
            Gun.GetComponent<Animator>().Play("AK47.Shoot");
            yield return new WaitForSeconds(0.10f);
            Gun.GetComponent<Animator>().Play("AK47.New State");
        }
    }
}
